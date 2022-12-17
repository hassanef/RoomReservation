using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Reservation.Domain.AggregatesModel;
using ResourceType = Reservation.Domain.AggregatesModel.ResourceType;

namespace Reservation.Domain.ReadModels
{
    public class ResourceDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public ResourceType Type { get; set; }
    }
}
