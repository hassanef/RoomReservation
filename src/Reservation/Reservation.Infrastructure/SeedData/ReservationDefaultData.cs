using Reservation.Domain.AggregatesModel;
using System.Collections.Generic;

namespace Reservation.Infrastructure.SeedData
{
    public static class ReservationDefaultData
    {
        public static List<Office> CreateOfficesRoomsPredefine()
        {
            var offices = new List<Office>();
            var amsterdamOffice = new Office("Amsterdam Office", Location.Amsterdam);
            var berlinOffice = new Office("Berlin Office", Location.Berlin);

            amsterdamOffice.AddOffice("Room100", 8, false);
            amsterdamOffice.AddOffice("Room300", 15, true);
            amsterdamOffice.AddOffice("Room400", 30, true);
            amsterdamOffice.AddOffice("Room500", 11, true);
            berlinOffice.AddOffice("Room200", 7, true);
            berlinOffice.AddOffice("Room300", 13, false);

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
