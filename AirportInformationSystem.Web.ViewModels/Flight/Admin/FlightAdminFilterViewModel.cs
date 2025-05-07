using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using static AirportInformationSystem.Common.EntityValidationMessages.Flight;
using static AirportInformationSystem.Common.EntityValidationMessages.FlightRoute;

namespace AirportInformationSystem.Web.ViewModels.Flight.Admin
{
    public class FlightAdminFilterViewModel
    {
        [Required(ErrorMessage = FlightDateRequiredMessage)]
        [DisplayName("Flight Date")]
        [DataType(DataType.Date)]
        public DateTime? FlightDate { get; set; }

        [Required(ErrorMessage = DepartureCityRequiredMessage)]
        [DisplayName("From")]
        public string DepartureCityName { get; set; } = null!;

        [Required(ErrorMessage = ArrivalCityRequiredMessage)]
        [DisplayName("To")]
        public string ArrivalCityName { get; set; } = null!;

        public List<SelectListItem> Cities { get; set; } = new();

        public List<FlightAdminResultViewModel> Flights { get; set; } = new();
    }
}
