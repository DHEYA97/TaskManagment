using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using TaskManagment.Core.Entities;
using TaskManagment.Core.Entities.Identity;
using TaskManagment.Core.Interfaces;
using System.Reflection;

namespace TaskManagment.Repository.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IServiceProvider serviceProvider) :
        IdentityDbContext<ApplicationUser, IdentityRole, string>(options)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public DbSet<EventFile> EventFiles { get; set; }
        public DbSet<Event> Events { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add Global Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Change OnDelete Behavior
            var cascadeFk = modelBuilder.Model
                                        .GetEntityTypes()
                                        .SelectMany(t => t.GetForeignKeys())
                                        .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFk)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var dateTimeService = _serviceProvider.GetRequiredService<IDateTimeService>();

            var Entities = ChangeTracker.Entries<BaseEntity>();
            foreach (var entity in Entities)
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Property(x => x.CreatedOn).CurrentValue = dateTimeService.GetNowKsa();
                }
                else if (entity.State == EntityState.Modified)
                {
                    entity.Property(x => x.UpdatedOn).CurrentValue = dateTimeService.GetNowKsa();
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //base.OnConfiguring(optionsBuilder);
        //optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        //}
    }
}
