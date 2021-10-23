using Reservation.Application.Commands;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.Exceptions;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Reservation.Api.IntegrationTests.ApiTests
{
    [Collection("Integration")]
    public class RoomReservationTests
    {
        private readonly HttpClient _client;

        public RoomReservationTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRoomReservationCommand_ThenGetReturnSuccessStatusCode()
        {
            DateTime nowDate = DateTime.Now;
            DateTime startDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day + 1,
                08, 01, 0);

            var postRentalRequest = new RoomReservationCommand
            {
                RoomId = 1,
                UserId = 2,
                Location = Location.Amsterdam,
                StartDate = startDate,
                EndDate = startDate.AddHours(2)
            };

            using var result = await _client.PostAsJsonAsync($"/api/v1/roomreservation", postRentalRequest);
            Assert.True(result.IsSuccessStatusCode);
        }
        [Fact]
        public async Task GivenRequest_WithInvalidRoomId_WhenPostRoomReservationCommand_ThenThrowException()
        {
            DateTime nowDate = DateTime.Now;
            DateTime startDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day + 1,
                08, 01, 0);

            var roomReservationRequest = new RoomReservationCommand
            {
                RoomId = 0,
                UserId = 2,
                Location = Location.Amsterdam,
                StartDate = startDate,
                EndDate = startDate.AddHours(2)
            };

            await Assert.ThrowsAsync<ReservationException>(async () =>
            {
                using var result = await _client.PostAsJsonAsync($"/api/v1/roomreservation", roomReservationRequest);
            });
        }
        [Fact]
        public async Task GivenRequest_WithInvalidStartDate_WhenPostRoomReservationCommand_ThenThrowException()
        {
            DateTime nowDate = DateTime.Now;

            var roomReservationRequest = new RoomReservationCommand
            {
                RoomId = 0,
                UserId = 2,
                Location = Location.Amsterdam,
                StartDate = DateTime.Now.AddHours(-1),
                EndDate = DateTime.Now.AddHours(2)
            };

            await Assert.ThrowsAsync<ReservationException>(async () =>
            {
                using var result = await _client.PostAsJsonAsync($"/api/v1/roomreservation", roomReservationRequest);
            });
        }
    }
}
