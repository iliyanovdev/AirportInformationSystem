using System.ComponentModel.DataAnnotations;

using static AirportInformationSystem.Common.EntityValidationConstants.Country;

namespace AirportInformationSystem.Data.Models
{
    public class Country
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
        public ICollection<City> Cities { get; set; } = new HashSet<City>();
    }
}
