using Reservation.Domain.Utils;
using System;

namespace Reservation.Domain.UnitTests.Stubs
{
    public class StubClock : IClock
    {
        private DateTime _now;

        public StubClock(DateTime now)
        {
            _now = now;
        }
        public static StubClock CreateClockWhichSetsNowAs(DateTime now)
        {
            return new StubClock(now);
        }
        public  void TimeTravelTo(DateTime datetime)
        {
            this._now = datetime;
        }
        public DateTime Now()
        {
            return _now;
        }
    }
}
