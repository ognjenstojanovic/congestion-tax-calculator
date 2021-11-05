using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using congestion_tax_calculator.Model.Enum;

namespace congestion_tax_calculator.Model.Interfaces
{
    public interface ICongestionTaxCalculator
    {
        /// <summary>
        /// Calculate the total toll fee for one day 
        /// </summary>
        /// <param name="vehicleType">Type of the vehicle that is being taxed</param>
        /// <param name="passes">Date and Time of all passes on one day</param>
        /// <returns>The total congestion tax for that day</returns>
        Task<decimal> Calculate(VehicleType vehicleType, IList<DateTime> passes);
    }
}