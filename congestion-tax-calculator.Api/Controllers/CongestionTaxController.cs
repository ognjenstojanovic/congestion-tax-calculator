using System;
using System.Linq;
using System.Threading.Tasks;
using congestion_tax_calculator.Api.Dto;
using congestion_tax_calculator.Model.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace congestion_tax_calculator.Api.Controllers
{
    /// <summary>
    /// Controller used to calculate Congestion tax information
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CongestionTaxController : ControllerBase
    {
        private readonly ICongestionTaxCalculator _taxCalculator;

        public CongestionTaxController(ICongestionTaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }

        /// <summary>
        /// POST method used to calculate Congestion tax information based on the vehicle type and toll both pass timestamps
        /// </summary>
        /// <param name="dto">The object containing the Vehicle type and timestamps</param>
        /// <returns></returns>
        [HttpPost]
        [Route("calculate")]
        public async Task<IActionResult> Post(TaxCallculationDto dto)
        {
            ValidateDto(dto);

            var tax = await _taxCalculator.Calculate(dto.VehicleType, dto.Dates.ToArray());
                
            return Ok(tax);
        }

        private void ValidateDto(TaxCallculationDto dto)
        {
            if (dto.Dates == null || !dto.Dates.Any())
            {
                throw new ArgumentException("You must specify dates for tax calculations.");
            }
            else
            {
                var firstDate = dto.Dates.First();

                if (dto.Dates.Any(d => d.Year != firstDate.Year ||
                                       d.Month != firstDate.Month ||
                                       d.Day != firstDate.Day))
                {
                    throw new ArgumentException("All specified dates must be on the same day.");
                }
            }
        }
    }
}
