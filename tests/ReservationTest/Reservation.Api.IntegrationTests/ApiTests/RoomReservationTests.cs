using FluentValidation;
using Microsoft.AspNetCore.Http;
using Reservation.Application.Commands;
using System;
using System.Net;
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
        public async Task GivenRequest_WithInvalidRoomId_WhenPostRoomReservationCommand_ThenReturn400()
        {
            DateTime nowDate = DateTime.Now;
            DateTime startDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day + 1,
                08, 01, 0);

            var roomReservationRequest = new CreateRoomReservationCommand
            {
                RoomId = 0,
                LocationId = 1,
                StartDate = startDate,
                EndDate = startDate.AddHours(2)
            };

            using var result = await _client.PostAsJsonAsync($"/api/v1/roomreservation/createroomreservation", roomReservationRequest);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
        }
        [Fact]
        public async Task GivenRequest_WithInvalidStartDate_WhenPostRoomReservationCommand_ThenReturn400()
        {
            var roomReservationRequest = new CreateRoomReservationCommand
            {
                RoomId = 1,
                LocationId = 1,
                StartDate = DateTime.Now.AddHours(-1),
                EndDate = DateTime.Now.AddHours(2)
            };

            using var result = await _client.PostAsJsonAsync($"/api/v1/roomreservation/createroomreservation", roomReservationRequest);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
        }
        [Fact]
        public async Task GivenRequest_WithInvalidEndDate_WhenPostRoomReservationCommand_ThenReturn400()
        {
            var roomReservationRequest = new CreateRoomReservationCommand
            {
                RoomId = 1,
                LocationId = 1,
                StartDate = DateTime.Now.AddHours(1),
                EndDate = DateTime.Now.AddHours(-1)
            };

            using var result = await _client.PostAsJsonAsync($"/api/v1/roomreservation/createroomreservation", roomReservationRequest);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
        }
        [Fact]
        public async Task GivenRequest_WithInvalidLocationId_WhenPostRoomReservationCommand_ThenReturn400()
        {
            DateTime nowDate = DateTime.Now;
            DateTime startDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day + 2,
                08, 01, 0);

            var roomReservationRequest = new CreateRoomReservationCommand
            {
                RoomId = 1,
                LocationId = 3,
                StartDate = startDate,
                EndDate = startDate.AddHours(2)
            };

            using var result = await _client.PostAsJsonAsync($"/api/v1/roomreservation/createroomreservation", roomReservationRequest);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
        }
    }
}
