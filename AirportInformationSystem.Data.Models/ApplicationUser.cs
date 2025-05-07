using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace AirportInformationSystem.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
