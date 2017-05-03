﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelTracker.ApiModels.FuelReportApiModels
{
    public class PutUpdateFuelReport
    {
        public Guid Id { get; set; }
        public float Distance { get; set; }
        public float FuelBurned { get; set; }
        public float PricePerUnit { get; set; }
    }
}
