using System.ComponentModel.DataAnnotations;

using static AirportInformationSystem.Common.EntityValidationConstants.Company;

namespace AirportInformationSystem.Data.Models
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(PrefixMaxLength)]
        public string Prefix { get; set; } = null!;

        public ICollection<FlightRoute> FlightRoutes { get; set; } = new HashSet<FlightRoute>();
    }
}
