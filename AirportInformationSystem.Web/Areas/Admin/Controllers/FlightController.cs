using AirportInformationSystem.Services.Interfaces;
using AirportInformationSystem.Web.ViewModels.Flight.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirportInformationSystem.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FlightController : Controller
    {
        private readonly IFlightService _flightService;
        private readonly ICityService _cityService;
        private readonly ICompanyService _companyService;

        public FlightController(IFlightService flightService, ICityService cityService, ICompanyService companyService)
        {
            _flightService = flightService;
            _cityService = cityService;
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> ManageFlights(string? departureCityName, string? arrivalCityName, DateTime? flightDate)
        {
            var viewModel = new FlightAdminFilterViewModel
            {
                DepartureCityName = departureCityName ?? "",
                ArrivalCityName = arrivalCityName ?? "",
                FlightDate = flightDate,
                Cities = await _cityService.GetCityNameDropdownAsync()
            };

            bool isFiltering = !string.IsNullOrWhiteSpace(departureCityName)
                           && !string.IsNullOrWhiteSpace(arrivalCityName)
                           && flightDate.HasValue;

            if (isFiltering)
            {
                TryValidateModel(viewModel);

                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var filteredFlights = await _flightService.GetFilteredFlightsAsync(viewModel);
                viewModel.Flights = filteredFlights.ToList();
            }

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmToggleFlightStatus(Guid id, string? departureCityName, string? arrivalCityName, DateTime? flightDate)
        {
            ConfirmToggleFlightStatusViewModel? viewModel = await _flightService.GetConfirmToogleFlightStatusViewModelAsync(id);

            if (viewModel == null)
            {
                TempData["ErrorMessage"] = "Flight not found.";
                return RedirectToAction(nameof(ManageFlights));
            }

            viewModel.FilterDepartureCityName = departureCityName;
            viewModel.FilterArrivalCityName = arrivalCityName;
            viewModel.FilterFlightDate = flightDate;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFlightStatus(Guid id, string? departureCityName, string? arrivalCityName, DateTime? flightDate)
        {
            await _flightService.ToggleFlightCancellationAsync(id);

            TempData["SuccessMessage"] = "Flight status updated successfully.";

            return RedirectToAction(nameof(ManageFlights), new
            {
                departureCityName,
                arrivalCityName,
                flightDate
            });
        }

        [HttpGet("CreateFlight")]
        public async Task<IActionResult> CreateFlight(int routeId, string? returnUrl)
        {
            var viewModel = await _flightService.GetCreateFlightViewModelAsync(routeId);

            if (viewModel == null)
            {
                TempData["ErrorMessage"] = "Flight with such id not found";
                return RedirectToAction("ChooseRouteForNewFlight", "FlightRoute");
            }

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("ChooseRouteForNewFlight", "FlightRoute");

            return View(viewModel);
        }

        [HttpPost("CreateFlight")]
        public async Task<IActionResult> CreateFlight(CreateFlightInputModel inputModel, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                var model = await _flightService.GetCreateFlightViewModelAsync(inputModel.FlightRouteId);
                ViewBag.ReturnUrl = returnUrl ?? Url.Action("ChooseRouteForNewFlight", "FlightRoute");
                return View(model);
            }

            bool isSuccess = await _flightService.CreateFlightAsync(inputModel);

            if (isSuccess)
            {
                TempData["SuccessMessage"] = "Flight added successfully.";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction(nameof(ManageFlights));
            }
            else
            {
                ModelState.AddModelError("", "Flight for this date already exists or route not found.");
                var model = await _flightService.GetCreateFlightViewModelAsync(inputModel.FlightRouteId);
                ViewBag.ReturnUrl = returnUrl ?? Url.Action("ChooseRouteForNewFlight", "FlightRoute");
                return View(model);
            }
        }
    }
}
