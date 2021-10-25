using Reservation.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Application.Queries.Contract
{
    public interface IRoomQuery
    {
        Task<List<RoomReadModel>> GetRooms(int officeId);
    }
}
