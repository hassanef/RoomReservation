using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.AggregatesModel;

namespace Reservation.Infrastructure.EntityConfiguration
{
    public class OfficeConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> resourceConfiguration)
        {
            resourceConfiguration.HasKey(o => o.Id);
            resourceConfiguration.Property(r => r.Id).ValueGeneratedOnAdd();

            resourceConfiguration.Property(r => r.Title).IsRequired().HasMaxLength(200);

            resourceConfiguration.HasOne<Location>()
             .WithMany()
             .IsRequired()
             .HasForeignKey(x => x.LocationId);
        }
    }
}