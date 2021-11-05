using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using congestion_tax_calculator.Model;
using congestion_tax_calculator.Model.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace congestion_tax_calculator_net_core
{
    public class CongestionTaxPriceGetter : ICongestionTaxPriceGetter
    {
        private readonly IConfiguration _configuration; 
        private IList<TaxRule> _taxRules;

        public CongestionTaxPriceGetter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task UpdateTaxRules()
        {
            var httpClient = new HttpClient();

            var taxRulesResponse =
                await httpClient.GetAsync($"{_configuration["DataStoreUrl"]}/GothenburgRules.json");
            
            var taxRulesString = await taxRulesResponse.Content.ReadAsStringAsync();

            _taxRules = JsonConvert.DeserializeObject<IList<TaxRule>>(taxRulesString);
        }
        
        public decimal GetPrice(DateTime date)
        {

            var correspondingRule = _taxRules
                .FirstOrDefault(r => date >= r.GetStartDate(date) && date <= r.GetEndDate(date));
            
            return correspondingRule?.Fee ?? 0;
        }
    }
}