using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Infrastructure.EntityConfiguration
{
    public class OfficeReadModelConfiguration : IEntityTypeConfiguration<OfficeReadModel>
    {
        public void Configure(EntityTypeBuilder<OfficeReadModel> resourceConfiguration)
        {
            resourceConfiguration.Property(e => e.Location)
              .HasConversion(
                v => v.ToString(),
                v => (Location)Enum.Parse(typeof(Location), v))
              .HasColumnType("nvarchar(100)");
        }
    }
}