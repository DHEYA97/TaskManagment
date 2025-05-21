using Microsoft.AspNetCore.Identity;
using TaskManagment.Core.Abstractions;
using TaskManagment.Core.Entities.Identity;
namespace Bookify.Web.Seeds
{
    public static class SeedUsers
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser admin = new()
            {
                UserName = DefaultUsers.AdminUserName,
                Email = DefaultUsers.AdminEmail,
                FullName = DefaultUsers.AdminFullName,
                EmailConfirmed = true
            };

            var adminUser = await userManager.FindByEmailAsync(admin.Email);
            if (adminUser is null)
            {
                await userManager.CreateAsync(admin, DefaultUsers.AdminPassword);
                await userManager.AddToRoleAsync(admin, DefaultRoles.Admin);
            }

            ApplicationUser manager = new()
            {
                UserName = DefaultUsers.ManagerUserName,
                Email = DefaultUsers.ManagerEmail,
                FullName = DefaultUsers.ManagerFullName,
                EmailConfirmed = true
            };

            var memberUser = await userManager.FindByEmailAsync(manager.Email);
            if (memberUser is null)
            {
                await userManager.CreateAsync(manager, DefaultUsers.ManagerPassword);
                await userManager.AddToRoleAsync(manager, DefaultRoles.Manager);
            }

            ApplicationUser taskManager = new()
            {
                UserName = DefaultUsers.TaskManagerUserName,
                Email = DefaultUsers.TaskManagerEmail,
                FullName = DefaultUsers.TaskManagerFullName,
                EmailConfirmed = true
            };

            var registerUser = await userManager.FindByEmailAsync(taskManager.Email);
            if (registerUser is null)
            {
                await userManager.CreateAsync(taskManager, DefaultUsers.TaskManagerPassword);
                await userManager.AddToRoleAsync(taskManager, DefaultRoles.TaskManager);
            }
        }
    }
}