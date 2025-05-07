using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static AirportInformationSystem.Common.EntityValidationConstants.Flight;

namespace AirportInformationSystem.Data.Models
{
    public class Flight
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(FlightCodeMaxLength)]
        public string FlightCode { get; set; } = null!;

        [Required]
        public int FlightRouteId { get; set; }

        [ForeignKey(nameof(FlightRouteId))]
        public FlightRoute FlightRoute { get; set; } = null!;

        [Required]
        [Column(TypeName = "date")]
        public DateTime FlightDate { get; set; }

        [Required]
        public bool IsCancelled { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
