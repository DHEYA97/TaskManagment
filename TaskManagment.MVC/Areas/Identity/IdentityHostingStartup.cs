//using Microsoft.AspNetCore.Identity;
//using TaskManagment.Repository.Data;

//[assembly: HostingStartup(typeof(TaskManagment.Mvc.Areas.Identity.IdentityHostingStartup))]
//namespace TaskManagment.Mvc.Areas.Identity
//{
//    public class IdentityHostingStartup : IHostingStartup
//    {
//        public void Configure(IWebHostBuilder builder)
//        {
//            builder.ConfigureServices((context, services) =>
//            {
//                services.AddIdentity<ApplicationUser, IdentityRole>(config =>
//                {
//                    config.SignIn.RequireConfirmedEmail = true;
//                    config.Password.RequiredLength = 6;
//                    config.Password.RequireNonAlphanumeric = false;
//                    config.Password.RequireUppercase = true;
//                })
//                .AddRoleManager<RoleManager<IdentityRole>>()
//                .AddDefaultUI()
//                .AddDefaultTokenProviders()
//                .AddEntityFrameworkStores<ApplicationDbContext>();
//            });
//        }
//    }
//}