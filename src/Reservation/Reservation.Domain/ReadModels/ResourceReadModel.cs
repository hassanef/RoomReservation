﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Reservation.Domain.AggregatesModel;

namespace Reservation.Domain.ReadModels
{
    public class ResourceReadModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public ResourceType Type { get; set; }
    }
}