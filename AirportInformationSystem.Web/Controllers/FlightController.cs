using Microsoft.AspNetCore.Mvc;
using AirportInformationSystem.Services.Interfaces;
using AirportInformationSystem.Web.ViewModels.Flight;
using Microsoft.AspNetCore.Identity;
using AirportInformationSystem.Data.Models;

namespace AirportInformationSystem.Web.Controllers
{
    public class FlightController : Controller
    {
        private readonly IFlightService _flightService;
        private readonly ICityService _cityService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FlightController(IFlightService flightService, ICityService cityService, UserManager<ApplicationUser> userManager)
        {
            _flightService = flightService;
            _cityService = cityService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Search(
            string? DepartureCityName,
            string? ArrivalCityName,
            DateTime? DepartureDate,
            bool IsRoundTrip = false,
            DateTime? ReturnDate = null)
        {
            var model = new FlightSearchViewModel
            {
                DepartureCityName = DepartureCityName ?? "",
                ArrivalCityName = ArrivalCityName ?? "",
                DepartureDate = DepartureDate,
                IsRoundTrip = IsRoundTrip,
                ReturnDate = ReturnDate,
                Cities = await _cityService.GetCityNameDropdownAsync()
            };

            if (IsRoundTrip)
            {
                if (!ReturnDate.HasValue)
                {
                    ModelState.AddModelError(nameof(model.ReturnDate), "Return date is required for round-trip.");
                }
                else if (ReturnDate < DepartureDate)
                {
                    ModelState.AddModelError(nameof(model.ReturnDate), "Return date cannot be before departure date.");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.DepartureCityName) &&
                model.DepartureCityName == model.ArrivalCityName)
            {
                ModelState.AddModelError(nameof(model.ArrivalCityName), "Departure and Arrival cities must be different.");
            }

            if (!string.IsNullOrWhiteSpace(model.DepartureCityName) &&
                !string.IsNullOrWhiteSpace(model.ArrivalCityName) &&
                model.DepartureDate.HasValue &&
                ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                model = await _flightService.SearchFlightsAsync(model, user?.Id);
            }

            return View(model);
        }
    }
}
