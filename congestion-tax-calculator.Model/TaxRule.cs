using System;
using System.Linq;

namespace congestion_tax_calculator.Model
{
    public class TaxRule
    {
        public string StartTime { get; set; }
        
        public string EndTime { get; set; }
        
        public string Currency { get; set; }
        
        public decimal Fee { get; set; }

        public int StartHour => int.Parse(StartTime.Split(":")[0]);
        
        public int StartMinutes => int.Parse(StartTime.Split(":")[1]);
        
        public int EndHour => int.Parse(EndTime.Split(":")[0]);
        
        public int EndMinutes => int.Parse(EndTime.Split(":")[1]);
    }
}