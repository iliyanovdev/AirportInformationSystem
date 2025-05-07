using System.ComponentModel.DataAnnotations;

using static AirportInformationSystem.Common.EntityValidationConstants.AirplaneType;

namespace AirportInformationSystem.Data.Models
{
    public class AirplaneType
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(ModelMaxLength)]
        public string Model { get; set; } = null!;

        [Required]
        public int Capacity { get; set; }

        public ICollection<FlightRoute> FlightRoutes { get; set; } = new HashSet<FlightRoute>();
    }
}
