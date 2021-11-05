using System;
using System.Collections.Generic;
using congestion_tax_calculator.Model.Enum;

namespace congestion_tax_calculator.Api.Dto
{
    public class TaxCallculationDto
    {
        public VehicleType VehicleType { get; set; }

        public IList<DateTime> Dates { get; set; }
    }
}