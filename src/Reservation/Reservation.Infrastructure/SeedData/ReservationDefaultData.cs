using Reservation.Domain.AggregatesModel;
using System.Collections.Generic;

namespace Reservation.Infrastructure.SeedData
{
    public static class ReservationDefaultData
    {
        public static List<Location> CreateLocationsPredefine()
        {
            return new List<Location>() 
            {
                new Location(1, "Amsterdam", new System.TimeSpan(08, 00, 00), new System.TimeSpan(17, 00, 00)),
                new Location(2, "Berlin", new System.TimeSpan(08, 00, 00), new System.TimeSpan(20, 00, 00))
            };
        }
        public static List<Office> CreateOfficesRoomsPredefine()
        {
            var offices = new List<Office>();

            var amsterdamOffice = new Office("Amsterdam Office", 1);
            var berlinOffice = new Office("Berlin Office", 2);

            amsterdamOffice.AddRoom("Room100", 8, false, ResourceType.Fixed);
            amsterdamOffice.AddRoom("Room300", 15, true, ResourceType.Fixed);
            amsterdamOffice.AddRoom("Room400", 30, true, ResourceType.Fixed);
            amsterdamOffice.AddRoom("Room500", 11, true, ResourceType.Fixed);
            berlinOffice.AddRoom("Room200", 7, true, ResourceType.Fixed);
            berlinOffice.AddRoom("Room300", 13, false, ResourceType.Fixed);

            offices.Add(amsterdamOffice);
            offices.Add(berlinOffice);

            return offices;
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
