using System;
using System.Threading.Tasks;

namespace congestion_tax_calculator.Model.Interfaces
{
    public interface ICongestionTaxPriceGetter
    {
        Task UpdateTaxRules();
        
        decimal GetPrice(DateTime date);
    }
}