using Microsoft.EntityFrameworkCore;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.Exceptions;
using Reservation.Domain.ReadModels;
using Reservation.Infrastructure.EntityConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Infrastructure.Context
{
    public class ReservationDbContextReadOnly: DbContext
    {
        //public DbSet<Resource> Resources { get; set; }
        public DbSet<RoomReadModel> Rooms { get; set; }
        public DbSet<OfficeReadModel> Offices { get; set; }
        public DbSet<RoomReservationReadModel> RoomReservations { get; set; }
        public DbSet<ResourceReservationReadModel> ResourceReservations { get; set; }
        public ReservationDbContextReadOnly(DbContextOptions<ReservationDbContextReadOnly> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OfficeReadModelConfiguration());

        }
        public override int SaveChanges()
        {
            throw new ReservationException("This context is read-only!");
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new ReservationException("This context is read-only!");
        }
    }
}
