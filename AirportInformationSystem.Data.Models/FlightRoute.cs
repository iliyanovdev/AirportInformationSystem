using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirportInformationSystem.Data.Models
{
    public class FlightRoute
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;

        [Required]
        public Guid AirplaneTypeId { get; set; }

        [ForeignKey(nameof(AirplaneTypeId))]
        public AirplaneType AirplaneType { get; set; } = null!;

        [Required]
        public Guid DepartureCityId { get; set; }

        [ForeignKey(nameof(DepartureCityId))]
        public City DepartureCity { get; set; } = null!;

        [Required]
        public Guid ArrivalCityId { get; set; }

        [ForeignKey(nameof(ArrivalCityId))]
        public City ArrivalCity { get; set; } = null!;

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan DepartureTime { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan FlightDuration { get; set; }

        [Required]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Price { get; set; }

        [Required]
        public bool IsCancelled { get; set; } = false;

        public ICollection<Flight> Flights { get; set; } = new HashSet<Flight>();
    }
}
