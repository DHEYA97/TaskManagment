using Bookify.Web.Seeds;
using Microsoft.AspNetCore.Identity;
using TaskManagment.Core.Entities.Identity;
using TaskManagment.Core.Service;
using TaskManagment.Repository.Seeds;

namespace TaskManagment.ServiceAndFactore.Service
{
    public class SeedingData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) : ISeedingData
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task SeedDefaultDataAsync()
        {
            await SeedRoles.SeedAsync(_roleManager);
            await SeedUsers.SeedAsync(_userManager);
        }
    }
}
