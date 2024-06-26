using Microsoft.AspNetCore.Identity;

namespace ImageCollector.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Location> Locations { get; set; }
    }
}
