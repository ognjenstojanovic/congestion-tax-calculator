using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using congestion_tax_calculator.Model.Configuration;
using congestion_tax_calculator.Model.Enum;
using congestion_tax_calculator.Model.Interfaces;

namespace congestion_tax_calculator_net_core
{
    public class CongestionTaxCalculator : ICongestionTaxCalculator
    {
        private readonly ICongestionTaxPriceGetter _congestionTaxPriceGetter;
        private readonly ITollFreeDateChecker _tollFreeDateChecker;

        public CongestionTaxCalculator(
            ICongestionTaxPriceGetter congestionTaxPriceGetter,
            ITollFreeDateChecker tollFreeDateChecker)
        {
            _congestionTaxPriceGetter = congestionTaxPriceGetter;
            _tollFreeDateChecker = tollFreeDateChecker;
        }

        public async Task<decimal> Calculate(VehicleType vehicleType, IList<DateTime> passes)
        {
            decimal totalTax = 0;
            
            await _congestionTaxPriceGetter.UpdateTaxRules();
            await _tollFreeDateChecker.UpdateTollFreeDays(passes.First().Year);
            
            while (passes.Count != 0)
            {
                var firstPassInInterval = passes.First();
                
                var passesInOneHour = passes.Where(pass => firstPassInInterval.AddHours(1) >= pass).ToList();
                
                totalTax += passesInOneHour.Select(pass => GetTollFee(pass, vehicleType)).Max();
                
                passes = passes.Except(passesInOneHour).ToList();
            }
            
            return totalTax > 60 ? 60 : totalTax;
        }

        private bool IsTollFreeVehicle(VehicleType vehicleType)
        {
            return TollFreeVehiclesConfiguration.TollFreeVehicles.Contains(vehicleType);
        }

        public decimal GetTollFee(DateTime date, VehicleType vehicleType)
        {
            var result = 0m;

            if (!_tollFreeDateChecker.IsTollFreeDate(date) &&
                !IsTollFreeVehicle(vehicleType))
            {
                result = _congestionTaxPriceGetter.GetPrice(date);   
            }

            return result;
        }

    }
}