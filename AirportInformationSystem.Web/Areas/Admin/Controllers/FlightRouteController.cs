using AirportInformationSystem.Services.Interfaces;
using AirportInformationSystem.Web.ViewModels.FlightRoute.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirportInformationSystem.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FlightRouteController : Controller
    {
        private readonly IFlightRouteService _flightRouteService;
        private readonly ICityService _cityService;
        private readonly ICompanyService _companyService;
        private readonly IAirplaneTypeService _airplaneTypeService;

        public FlightRouteController(IFlightRouteService flightRouteService, ICityService cityService, ICompanyService companyService, IAirplaneTypeService airplaneTypeService)
        {
            _flightRouteService = flightRouteService;
            _cityService = cityService;
            _companyService = companyService;
            _airplaneTypeService = airplaneTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> ManageFlightRoutes(string? departureCityName, string? arrivalCityName)
        {
            var viewModel = new FlightRouteAdminFilterViewModel
            {
                DepartureCityName = departureCityName ?? "",
                ArrivalCityName = arrivalCityName ?? "",
                Cities = await _cityService.GetCityNameDropdownAsync()
            };

            if (!string.IsNullOrWhiteSpace(departureCityName) && !string.IsNullOrWhiteSpace(arrivalCityName))
            {
                TryValidateModel(viewModel);

                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var filteredRoutes = await _flightRouteService.GetFilteredFlightRoutesAsync(viewModel);
                viewModel.FlightRoutes = filteredRoutes.ToList();
            }

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmToggleFlightRouteStatus(int id, string? departureCityName, string? arrivalCityName)
        {
            ConfirmToggleFlightRouteStatusViewModel? viewModel = await _flightRouteService.GetConfirmToggleFlightRouteStatusViewModelAsync(id);

            if (viewModel == null)
            {
                TempData["ErrorMessage"] = "Route not found.";
                return RedirectToAction(nameof(ManageFlightRoutes));
            }

            viewModel.FilterDepartureCityName = departureCityName;
            viewModel.FilterArrivalCityName = arrivalCityName;

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> ToggleFlightRouteStatus(int id, string? departureCityName, string? arrivalCityName)
        {
            await _flightRouteService.ToggleFlightRouteCancellationAsync(id);

            TempData["SuccessMessage"] = "Route status updated successfully.";

            return RedirectToAction(nameof(ManageFlightRoutes), new
            {
                departureCityName,
                arrivalCityName
            });
        }

        [HttpGet]
        public async Task<IActionResult> CreateFlightRoute()
        {
            var viewModel = new FlightRouteCreateViewModel
            {
                Cities = await _cityService.GetCityIdDropdownAsync(),
                Companies = await _companyService.GetCompanyIdDropdownAsync(),
                AirplaneTypes = await _airplaneTypeService.GetAirplaneTypeIdDropdownAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlightRoute(FlightRouteCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Cities = await _cityService.GetCityIdDropdownAsync();
                viewModel.Companies = await _companyService.GetCompanyIdDropdownAsync();
                viewModel.AirplaneTypes = await _airplaneTypeService.GetAirplaneTypeIdDropdownAsync();
                return View(viewModel);
            }

            await _flightRouteService.CreateFlightRouteAsync(viewModel);
            TempData["SuccessMessage"] = "Flight route created successfully.";

            return RedirectToAction(nameof(ManageFlightRoutes));
        }

        [HttpGet]
        public async Task<IActionResult> ChooseRouteForNewFlight(string? departureCityName, string? arrivalCityName, string? companyName)
        {
            var model = new ChooseRouteForNewFlightViewModel
            {
                DepartureCityName = departureCityName,
                ArrivalCityName = arrivalCityName,
                CompanyName = companyName,
                Routes = await _flightRouteService.SearchRoutesAsync(departureCityName, arrivalCityName, companyName),
                Cities = await _cityService.GetCityNameDropdownAsync(),
                Companies = await _companyService.GetCompanyNameDropdownAsync()
            };

            return View(model);
        }
    }
}
