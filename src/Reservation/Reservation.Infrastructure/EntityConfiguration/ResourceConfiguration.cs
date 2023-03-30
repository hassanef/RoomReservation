
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.AggregatesModel;
using System;

namespace Reservation.Infrastructure.EntityConfiguration
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> resourceConfiguration)
        {
            resourceConfiguration.HasKey(o => o.Id);
            resourceConfiguration.Property(r => r.Id).ValueGeneratedOnAdd();

            resourceConfiguration.Property(e => e.Type)
              .HasConversion(
                v => v.ToString(),
                v => (ResourceType)Enum.Parse(typeof(ResourceType), v))
              .HasColumnType("nvarchar(100)");

        }
    }
}