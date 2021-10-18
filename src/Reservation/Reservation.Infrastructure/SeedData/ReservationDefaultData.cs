using Reservation.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Infrastructure.SeedData
{
    public static class ReservationDefaultData
    {
        public static List<Room> CreateRoomsPredefine()
        {
            var rooms = new List<Room>()
            {
                new Room("Room100", 8, false, Location.Amsterdam),
                new Room("Room200", 15, true, Location.Amsterdam),
                new Room("Room300", 30, true, Location.Amsterdam),
                new Room("Room400", 11, true, Location.Amsterdam),
                new Room("Room100", 18, true, Location.Berlin),
                new Room("Room200", 7, false, Location.Amsterdam),
                new Room("Room300", 13, true, Location.Amsterdam)
            };

            return rooms;
        }
        public static List<Resource> CreateResourcesPredefine()
        {
            var resources = new List<Resource>()
            {
                new Resource("WhiteBoard", ResourceType.Fixed),
                new Resource("Beamer", ResourceType.Fixed),
                new Resource("TelevisionWithWheels", ResourceType.Movable)
            };

            return resources;
        }
    }
}
