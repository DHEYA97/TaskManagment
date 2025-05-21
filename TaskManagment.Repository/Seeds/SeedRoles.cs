using Microsoft.AspNetCore.Identity;
using TaskManagment.Core.Abstractions;

namespace TaskManagment.Repository.Seeds
{
    public static class SeedRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(DefaultRoles.Admin));
                await roleManager.CreateAsync(new IdentityRole(DefaultRoles.Manager));
                await roleManager.CreateAsync(new IdentityRole(DefaultRoles.TaskManager));
            }
        }
    }
}
