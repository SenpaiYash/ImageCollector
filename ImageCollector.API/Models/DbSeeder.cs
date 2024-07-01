using ImageCollector.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ImageCollector.API.Models
{
    public class DbSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DbSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedData()
        {
            await SeedUsers(_userManager);
        }

        public static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@admin.com",
                    UserName = "test@admin.com"
                };

               var result = await userManager.CreateAsync(user, "P@ssword123");
            }
        }
    }


}
