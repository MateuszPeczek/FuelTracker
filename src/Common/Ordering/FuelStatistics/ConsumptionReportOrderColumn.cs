using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Common.Ordering.FuelStatistics
{
    public enum ConsumptionReportOrderColumn
    {
        [Description("Id")]
        Id,
        [Description("Distance")]
        Distance,
        [Description("FuelBurned")]
        FuelBurned,
        [Description("FuelEfficiency")]
        FuelEfficiency,
        [Description("PricePerUnit")]
        PricePerUnit,
        [Description("DateCreated")]
        DateCreated
    }
}
