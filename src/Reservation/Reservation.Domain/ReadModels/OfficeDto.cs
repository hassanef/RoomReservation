using Reservation.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.ReadModels
{
    public class OfficeDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Location Location { get; set; }
    }
}
