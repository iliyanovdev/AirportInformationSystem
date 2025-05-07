using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace AirportInformationSystem.Web.ViewModels.FlightRoute.Admin
{
    public class ChooseRouteForNewFlightViewModel
    {
        [DisplayName("From")]
        public string? DepartureCityName { get; set; }

        [DisplayName("To")]
        public string? ArrivalCityName { get; set; }

        [DisplayName("Company")]
        public string? CompanyName { get; set; }

        public List<SelectListItem> Cities { get; set; } = new();

        public List<SelectListItem> Companies { get; set; } = new();

        public List<FlightRouteListingViewModel> Routes { get; set; } = new();
    }
}
