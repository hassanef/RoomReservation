using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.ReadModels;
using System;

namespace Reservation.Infrastructure.EntityConfiguration
{
    public class OfficeReadModelConfiguration : IEntityTypeConfiguration<OfficeDto>
    {
        public void Configure(EntityTypeBuilder<OfficeDto> resourceConfiguration)
        {
            resourceConfiguration.Property(e => e.Location)
              .HasConversion(
                v => v.ToString(),
                v => (Location)Enum.Parse(typeof(Location), v))
              .HasColumnType("nvarchar(100)");
        }
    }
}