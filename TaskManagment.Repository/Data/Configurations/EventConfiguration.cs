using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagment.Core.Entities;

namespace TaskManagment.Repository.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasMany(e => e.Images)
                .WithOne(etm => etm.Event)
                .HasForeignKey(etm => etm.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}