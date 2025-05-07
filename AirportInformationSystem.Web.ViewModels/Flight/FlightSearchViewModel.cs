using System.ComponentModel.DataAnnotations;

using static AirportInformationSystem.Common.EntityValidationMessages.FlightRoute;
using static AirportInformationSystem.Common.EntityValidationMessages.Flight;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirportInformationSystem.Web.ViewModels.Flight
{
    public class FlightSearchViewModel
    {
        [DisplayName("Round-trip")]
        public bool IsRoundTrip { get; set; }

        [Required(ErrorMessage = DepartureCityRequiredMessage)]
        [DisplayName("From")]
        public string DepartureCityName { get; set; } = null!;

        [Required(ErrorMessage = ArrivalCityRequiredMessage)]
        [DisplayName("To")]
        public string ArrivalCityName { get; set; } = null!;

        [Required(ErrorMessage = DepartureDateRequireMessage)]
        [DataType(DataType.Date)]
        [DisplayName("Departure Date")]
        public DateTime? DepartureDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Return Date")]
        public DateTime? ReturnDate { get; set; }
        public List<SelectListItem> Cities { get; set; } = new();

        public List<FlightResultViewModel> OutboundFlights { get; set; } = new();

        public List<FlightResultViewModel>? ReturnFlights { get; set; }
    }
}
