using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Reflection;
using TaskManagment.Core.Factore;
using TaskManagment.Core.Interfaces;
using TaskManagment.Core.Interfaces.Repositories;
using TaskManagment.Core.Settinges;
using TaskManagment.Core.UnitOfWork;
using TaskManagment.Core.Validations;
using TaskManagment.Mvc.Helpers;
using TaskManagment.Mvc.Localization;
using TaskManagment.Mvc.Mapping;
using TaskManagment.Repository.Data;
using TaskManagment.Repository.Repositories;
using TaskManagment.Repository.UnitOfWork;
using TaskManagment.ServiceAndFactore.Service;
using TaskManagment.ServiceAndFactory.Service;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;
using ViewToHTML.Extensions;

namespace TaskManagment.Mvc.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration, string[] cultures)
        {
            services.AddControllers();


            services
                    .AddDbContextConfig(configuration)
                    .AddMailConfig(configuration)
                    .AddHangFireConfig(configuration);

            services
                    .AddServicesConfig()
                    .AddIdentityConfig()
                    .AddMapsterConfig()
                    .AddFluentValidationConfig()
                    .AddHttpContextAccessorConfig()
                    .AddViewToHTML()
                    .AddDataProtectionConfig()
                    .AddExpressiveAnnotationsConfig()
                    .AddMigrationsEndPointConfig()
                    .AddLocalizationConf(cultures);



            return services;
        }
        private static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("DefaultConnection String Not Found");
            services.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(connectionString));
            return services;
        }

        private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {
            // Add Mapster Global Configration
            var mappConfig = TypeAdapterConfig.GlobalSettings;
            mappConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddScoped<MappingConfiguration>();
            return services;
        }
        private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(typeof(EventValidator).Assembly);
            return services;
        }
        private static IServiceCollection AddServicesConfig(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepositories<>), typeof(GenericRepositories<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IEmailSender, EmailService>();
            services.AddScoped<ISeedingData, SeedingData>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IDateTimeService, DateTimeService>();

            services.AddScoped<IEventModelFactory, EventModelFactory>();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            return services;
        }
        private static IServiceCollection AddIdentityConfig(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();

            services.Configure<SecurityStampValidatorOptions>(options =>
                options.ValidationInterval = TimeSpan.Zero
            );

            services.Configure<IdentityOptions>(Options =>
            {
                Options.Password.RequiredLength = 8;
                Options.SignIn.RequireConfirmedEmail = true;
                Options.User.RequireUniqueEmail = true;
                Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                Options.Lockout.MaxFailedAccessAttempts = 5;
            });
            return services;
        }
        private static IServiceCollection AddMailConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSetting>(configuration.GetSection(MailSetting.SectionName));
            return services;
        }
        private static IServiceCollection AddHangFireConfig(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"),new Hangfire.SqlServer.SqlServerStorageOptions
                    {
                        SchemaName = "dbo"
                    }));

            services.AddHangfireServer();
            return services;
        }
        private static IServiceCollection AddHttpContextAccessorConfig(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            return services;
        }
        private static IServiceCollection AddDataProtectionConfig(this IServiceCollection services)
        {
            services.AddDataProtection().SetApplicationName(nameof(TaskManagment));
            return services;
        }
        private static IServiceCollection AddExpressiveAnnotationsConfig(this IServiceCollection services)
        {
            services.AddExpressiveAnnotations();
            return services;
        }
        private static IServiceCollection AddMigrationsEndPointConfig(this IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();
            return services;
        }
        private static IServiceCollection AddLocalizationConf(
            this IServiceCollection services,
            string[] cultures
            )
        {
            services.AddLocalization();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(JsonStringLocalizerFactory));
                });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = cultures.Select(c => new CultureInfo(c)).ToArray();

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            return services;
        }
    }
}