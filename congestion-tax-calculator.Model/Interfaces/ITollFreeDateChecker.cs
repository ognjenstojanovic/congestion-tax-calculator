using System;
using System.Threading.Tasks;

namespace congestion_tax_calculator.Model.Interfaces
{
    public interface ITollFreeDateChecker
    {
        Task UpdateTollFreeDays(int year);
        
        bool IsTollFreeDate(DateTime dateTime);
    }
}