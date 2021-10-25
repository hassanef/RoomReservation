﻿using Microsoft.EntityFrameworkCore;
using Reservation.Application.Queries.Contract;
using Reservation.Domain.ReadModels;
using Reservation.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Application.Queries
{
    public class ResourceReservationQuery : IResourceReservationQuery
    {
        private readonly ReservationDbContextReadOnly _contextReadOnly;

        public ResourceReservationQuery(ReservationDbContextReadOnly contextReadOnly)
        {
            _contextReadOnly = contextReadOnly;
        }
        public async Task<List<ResourceReservationReadModel>> GetResourceReservations()
        {
            return await _contextReadOnly.ResourceReservations.Include(x => x.RoomReservation).ToListAsync();
        }
    }
}
