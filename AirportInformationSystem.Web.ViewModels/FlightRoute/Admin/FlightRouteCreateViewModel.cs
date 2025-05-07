using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirportInformationSystem.Web.ViewModels.FlightRoute.Admin
{
    public class FlightRouteCreateViewModel
    {
        [Required]
        [Display(Name = "Company")]
        public Guid CompanyId { get; set; }

        [Required]
        [Display(Name = "Airplane Type")]
        public Guid AirplaneTypeId { get; set; }

        [Required]
        [Display(Name = "Departure City")]
        public Guid DepartureCityId { get; set; }

        [Required]
        [Display(Name = "Arrival City")]
        public Guid ArrivalCityId { get; set; }

        [Required]
        [Display(Name = "Departure Time")]
        [DataType(DataType.Time)]
        public TimeSpan DepartureTime { get; set; }

        [Required]
        [Display(Name = "Flight Duration")]
        [DataType(DataType.Time)]
        public TimeSpan FlightDuration { get; set; }

        [Required]
        [Range(0.00, 99999.99)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        public List<SelectListItem> Companies { get; set; } = new();

        public List<SelectListItem> AirplaneTypes { get; set; } = new();

        public List<SelectListItem> Cities { get; set; } = new();
    }
}
