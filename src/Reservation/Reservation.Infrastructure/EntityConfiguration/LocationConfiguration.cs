using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.AggregatesModel;

namespace Reservation.Infrastructure.EntityConfiguration
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> resourceConfiguration)
        {
            resourceConfiguration.HasKey(o => o.Id);
            resourceConfiguration.Property(r => r.Id).ValueGeneratedNever();

            resourceConfiguration.Property(r => r.Title).IsRequired().HasMaxLength(200);
        }
    }
}