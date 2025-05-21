using Hangfire;
using HangfireBasicAuthenticationFilter;
using TaskManagment.Mvc.Extensions;
using TaskManagment.Mvc.Middlewares;
using TaskManagment.Repository.Data;
using Serilog;
using Serilog.Context;
using System.Security.Claims;
namespace TaskManagment
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var cultures = new[] { AppCultures.English, AppCultures.Arabic };


            builder.Host.UseSerilog((context, configration) =>
            {
                configration.ReadFrom.Configuration(context.Configuration);
            });

            builder.Services.AddDependency(builder.Configuration,cultures);

            builder.Services.AddControllersWithViews();

            //Add Serilog
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
            builder.Host.UseSerilog();

            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "Deny");

                await next();
            });

            // Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseExceptionHandler("/Home/Error");

            //app.UseStatusCodePages(async statusCodeContext =>
            //{
            //	// using static System.Net.Mime.MediaTypeNames;
            //	statusCodeContext.HttpContext.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Plain;

            //	await statusCodeContext.HttpContext.Response.WriteAsync(
            //		$"Status Code Page: {statusCodeContext.HttpContext.Response.StatusCode}");
            //});

            //app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
            //app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            //app.UseCookiePolicy(new CookiePolicyOptions
            //{
            //    Secure = CookieSecurePolicy.Always
            //});

            


            app.UseRouting();

            //Localization
            var localizationOptions = new RequestLocalizationOptions()
            .AddSupportedCultures(cultures)
            .AddSupportedUICultures(cultures);
            app.UseRequestLocalization(localizationOptions);
            app.UseRequestCulture();
            app.Use(async (context, next) =>
            {
                var cultureInfo = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;

                cultureInfo!.DateTimeFormat = CultureInfo.GetCultureInfo(AppCultures.English).DateTimeFormat;
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;

                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var seedingData = scope.ServiceProvider.GetRequiredService<ISeedingData>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                await seedingData.SeedDefaultDataAsync();
                Log.Information("🚀 Data Seeding Successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "❌ Data Seeding Failed");
            }
            try
            {
                await context.Database.MigrateAsync();
                Log.Information("🚀 Context Migrate Successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "❌ Context Migrate Failed");
            }



            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = app.Configuration.GetValue<string>("HangfireSettings:Username"),
                        Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
                    }
                ],
                DashboardTitle = "Majal Job Dashboard",
            });

            //Serilog
            app.Use(async (context, next) =>
            {
                LogContext.PushProperty("UserId", context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                LogContext.PushProperty("UserName", context.User.FindFirst(ClaimTypes.Name)?.Value);

                await next();
            });
            app.UseSerilogRequestLogging();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
