using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Common.Ordering.Engine
{
    public enum EngineOrderColumn
    {
        [Description("Id")]
        Id,
        [Description("Name")]
        Name,
        [Description("Power")]
        Power,
        [Description("Torque")]
        Torque,
        [Description("Cylinders")]
        Cylinders,
        [Description("Displacement")]
        Displacement,
        [Description("FuelType")]
        FuelType
    }
}
