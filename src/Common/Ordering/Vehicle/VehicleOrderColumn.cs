using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Common.Ordering.Vehicle
{
    public enum VehicleOrderColumn
    {
        [Description("Id")]
        Id,
        [Description("Manufacturer")]
        Manufacturer,
        [Description("Model")]
        Model,
        [Description("ProductionYear")]
        ProductionYear,
        [Description("EngineName")]
        EngineName,
        [Description("Power")]
        Power,
        [Description("Torque")]
        Torque,
        [Description("Cylinders")]
        Cylinders,
        [Description("Displacement")]
        Displacement,
        [Description("FuelType")]
        FuelType,
    }
}
