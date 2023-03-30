using Microsoft.EntityFrameworkCore;
using Reservation.Domain.Exceptions;
using Reservation.Domain.ReadModels;
using Reservation.Infrastructure.EntityConfiguration;

namespace Reservation.Infrastructure.Context
{
    public class ReservationDbContextReadOnly: DbContext
    {
        //public DbSet<Resource> Resources { get; set; }
        public DbSet<RoomDto> Rooms { get; set; }
        public DbSet<OfficeDto> Offices { get; set; }
        public DbSet<RoomReservationDto> RoomReservations { get; set; }
        public DbSet<ResourceReservationDto> ResourceReservations { get; set; }
        public ReservationDbContextReadOnly(DbContextOptions<ReservationDbContextReadOnly> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OfficeReadModelConfiguration());

        }
        public override int SaveChanges()
        {
            throw new RoomReservationException("This context is read-only!");
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new RoomReservationException("This context is read-only!");
        }
    }
}
