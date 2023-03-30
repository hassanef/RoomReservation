using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.AggregatesModel.OfficeAggregate;

namespace Reservation.Infrastructure.EntityConfiguration
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> roomConfiguration)
        {
            roomConfiguration.HasKey(o => o.Id);
            roomConfiguration.Property(x => x.Id).ValueGeneratedOnAdd();

            roomConfiguration.Property(e => e.PersonCapacity).IsRequired();
            roomConfiguration.Property(e => e.HasChair).IsRequired();
            roomConfiguration.Property(e => e.Title).IsRequired().HasMaxLength(200);
            roomConfiguration.OwnsMany<RoomResource>("RoomResources", x =>
            {
                x.HasKey("Id");
                x.Property(e => e.Id).ValueGeneratedOnAdd();
                x.HasOne<Resource>()
                  .WithMany()
                  .IsRequired()
                  .HasForeignKey(x => x.ResourceId);
            });

        }
    }
}