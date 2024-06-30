using ImageCollector.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ImageCollector.API.Models
{
    public static class WebApplicationExtensions
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var dbSeeder = new DbSeeder(userManager);
            await dbSeeder.SeedData();
        }
    }


}
