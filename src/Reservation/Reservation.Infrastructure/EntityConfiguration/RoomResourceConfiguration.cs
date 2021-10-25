using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.AggregatesModel;
using System;

namespace Reservation.Infrastructure.EntityConfiguration
{
    public class RoomResourceConfiguration : IEntityTypeConfiguration<RoomResource>
    {
        public void Configure(EntityTypeBuilder<RoomResource> roomResourceConfiguration)
        {
            roomResourceConfiguration.HasKey(o => o.Id);
            roomResourceConfiguration.Property(x => x.Id).ValueGeneratedOnAdd();


            roomResourceConfiguration.HasOne<Resource>()
            .WithMany()
            .IsRequired()
            .HasForeignKey(x => x.ResourceId);
        }
    }
}