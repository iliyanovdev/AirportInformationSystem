using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirportInformationSystem.Data.Models
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public Guid FlightId { get; set; }

        [ForeignKey(nameof(FlightId))]
        public Flight Flight { get; set; } = null!;

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Price { get; set; }
    }
}
