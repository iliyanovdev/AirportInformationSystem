using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AirportInformationSystem.Common.EntityValidationConstants.City;

namespace AirportInformationSystem.Data.Models
{
    public class City
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public Guid CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; } = null!;

        public ICollection<FlightRoute> DepartureRoutes { get; set; } = new List<FlightRoute>();
        public ICollection<FlightRoute> ArrivalRoutes { get; set; } = new List<FlightRoute>();

    }
}
