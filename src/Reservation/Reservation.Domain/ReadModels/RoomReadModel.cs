using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.ReadModels
{
    public class RoomReadModel
    {
        public int Id { get; set; }

        public int OfficeId { get;  set; }
        public string Title { get; set; }
        public byte PersonCapacity { get; set; }
        public bool HasChair { get; set; }
        public virtual OfficeReadModel Office { get; set; }
    }
}
