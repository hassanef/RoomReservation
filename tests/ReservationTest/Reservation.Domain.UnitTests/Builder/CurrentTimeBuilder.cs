using Reservation.Domain.UnitTests.Stubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.UnitTests.Builder
{
    internal class CurrentTimeBuilder
    {
        private readonly StubClock _clock;

        public CurrentTimeBuilder()
        {
            _clock = new StubClock(DateTime.Now);
        }
        public void CurrentTimeIs(string datetime)
        {
            var date = DateTime.Parse(datetime);
            _clock.TimeTravelTo(date);
        }
    }
}
