using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using static AirportInformationSystem.Common.EntityValidationMessages.Flight;

namespace AirportInformationSystem.Web.ViewModels.Flight.Admin
{
    public class CreateFlightViewModel
    {
        public int FlightRouteId { get; set; }

        public string DepartureCityName { get; set; } = null!;

        public string ArrivalCityName { get; set; } = null!;

        public string DepartureCountryName { get; set; } = null!;

        public string ArrivalCountryName { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public TimeSpan DepartureTime { get; set; }

        public TimeSpan FlightDuration { get; set; }

        public TimeSpan ArrivalTime { get; set; }

        public decimal Price { get; set; }

        [Required(ErrorMessage = FlightDateRequiredMessage)]
        [DataType(DataType.Date)]
        public DateTime FlightDate { get; set; }
    }

}
