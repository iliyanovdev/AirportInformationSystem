using System.ComponentModel.DataAnnotations;

namespace AirportInformationSystem.Web.ViewModels.Flight.Admin
{
    public class CreateFlightInputModel
    {
        public int FlightRouteId { get; set; }

        [Required]
        public DateTime FlightDate { get; set; }
    }

}
