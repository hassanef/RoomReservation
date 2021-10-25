using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Infrastructure.EntityConfiguration
{
    public class RoomReservationConfiguration : IEntityTypeConfiguration<RoomReservation>
    {
        public void Configure(EntityTypeBuilder<RoomReservation> roomReservationConfiguration)
        {
            roomReservationConfiguration.HasKey(r => r.Id);
            roomReservationConfiguration.Property(r => r.Id).ValueGeneratedOnAdd();

            roomReservationConfiguration.Property(r => r.UserId).IsRequired();
            roomReservationConfiguration.Property(r => r.RoomId).IsRequired();
            
            roomReservationConfiguration.OwnsOne(o => o.Period);
            roomReservationConfiguration.OwnsMany<ResourceReservation>("ResourceReservations", x =>
            {
                x.HasKey("Id");
                x.Property(e => e.Id).ValueGeneratedOnAdd();
                x.HasOne<Resource>()
                    .WithMany()
                    .IsRequired()
                    .HasForeignKey(x => x.ResourceId);
            });

            roomReservationConfiguration.HasOne<Room>()
              .WithMany()
              .IsRequired()
              .HasForeignKey(x => x.RoomId);
        }
    }
}