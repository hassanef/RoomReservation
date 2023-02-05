using Reservation.Application.Commands;
using Reservation.Specs.Utils;
using System.Net;
using TechTalk.SpecFlow;

namespace Reservation.Specs
{
    [Binding]
    public class ReservationStepDefinitions
    {
        private int _roomId = 0;
        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Now;
        private readonly ISystemUnderTest _sut;
        public ReservationStepDefinitions(ISystemUnderTest sut)
        {
            _sut = sut;
        }

        [Given(@"I selected a speciefic room")]
        public void GivenISelectedASpecieficRoom()
        {
            _roomId = 1;
        }

        [Given(@"choosed the start datetime and end datetime in free times for reservation")]
        public void GivenChoosedTheStartDatetimeAndEndDatetimeInFreeTimesForReservation()
        {
            _startDate = DateTime.Today.AddDays(1).AddHours(9);
            _endDate = DateTime.Today.AddDays(1).AddHours(10);
        }

        [When(@"set the room with start datetime and end datetime")]
        public async Task WhenSetTheRoomWithStartDatetimeAndEndDatetime()
        {
            var roomReservationRequest = new CreateRoomReservationCommand
            {
                RoomId = 1,
                LocationId = 1,
                StartDate = _startDate,
                EndDate = _endDate
            };

             await _sut.PostAsync($"/api/v1/roomreservation/createroomreservation", roomReservationRequest);
        }

        [Then(@"room should be reserved in specific datetimes")]
        public void ThenRoomShouldBeReservedInSpecificDatetimes()
        {
            Assert.Equal(HttpStatusCode.OK, _sut.Response.StatusCode);
        }

        [Given(@"choosed the start datetime and end datetime in busy times for reservation")]
        public void GivenChoosedTheStartDatetimeAndEndDatetimeInBusyTimesForReservation()
        {
            _startDate = DateTime.Today.AddDays(1).AddHours(9);
            _endDate = DateTime.Today.AddDays(1).AddHours(10);
        }

        [Then(@"room could not be reserved in specific datetimes and retruned error")]
        public void ThenRoomCouldNotBeReservedInSpecificDatetimesAndRetrunedError()
        {
            Assert.Equal(HttpStatusCode.BadRequest, _sut.Response.StatusCode);
        }
    }
}
