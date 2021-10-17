using Microsoft.EntityFrameworkCore;
using Reservation.Domain.AggregatesModel;
using Reservation.Infrastructure.EntityConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Infrastructure.Context
{
    public class ReservationDbContext : DbContext
    {
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomResource> RoomResources { get; set; }
        public DbSet<RoomReservation> RoomReservations { get; set; }
        public DbSet<ResourceReservation> ResourceReservations { get; set; }


        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ResourceConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new RoomReservationConfiguration());
        }
    }
}
