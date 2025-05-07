using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using static AirportInformationSystem.Common.EntityValidationMessages.FlightRoute;

namespace AirportInformationSystem.Web.ViewModels.FlightRoute.Admin
{
    public class FlightRouteAdminFilterViewModel
    {
        [Required(ErrorMessage = DepartureCityRequiredMessage)]
        [DisplayName("From")]
        public string DepartureCityName { get; set; } = null!;

        [Required(ErrorMessage = ArrivalCityRequiredMessage)]
        [DisplayName("To")]
        public string ArrivalCityName { get; set; } = null!;

        public List<SelectListItem> Cities { get; set; } = new();

        public List<FlightRouteAdminResultViewModel> FlightRoutes { get; set; } = new();
    }
}
