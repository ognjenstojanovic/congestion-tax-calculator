using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using congestion_tax_calculator.Model.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace congestion_tax_calculator_net_core
{
    public class TollFreeDateChecker : ITollFreeDateChecker
    {
        private readonly IConfiguration _configuration;
        private IList<DateTime> _tollFreeDays;

        public TollFreeDateChecker(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task UpdateTollFreeDays(int year)
        {
            var httpClient = new HttpClient();

            var taxRulesResponse = await httpClient.GetAsync($"{_configuration["DataStoreUrl"]}/TollFreeDates{year}.json");
            
            var tollFreeDaysString = await taxRulesResponse.Content.ReadAsStringAsync();

            _tollFreeDays = JsonConvert.DeserializeObject<IList<DateTime>>(tollFreeDaysString);
        }
        
        public bool IsTollFreeDate(DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday ||
                   dateTime.DayOfWeek == DayOfWeek.Sunday ||
                   dateTime.Month == 7 ||
                   _tollFreeDays.Contains(dateTime.Date);
        }
    }
}