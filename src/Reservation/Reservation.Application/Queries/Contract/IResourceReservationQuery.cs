using Reservation.Domain.ReadModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reservation.Application.Queries.Contract
{
    public interface IResourceReservationQuery
    {
        Task<List<ResourceReservationDto>> GetResourceReservations();
    }
}
