using System.Collections.Generic;
using congestion_tax_calculator.Model.Enum;

namespace congestion_tax_calculator.Model.Configuration
{
    public static class TollFreeVehiclesConfiguration
    {
        public static readonly List<VehicleType> TollFreeVehicles = new List<VehicleType>
        {
            VehicleType.Motorcycle,
            VehicleType.Tractor,
            VehicleType.Emergency,
            VehicleType.Diplomat,
            VehicleType.Foreign,
            VehicleType.Military
        };
    }
}