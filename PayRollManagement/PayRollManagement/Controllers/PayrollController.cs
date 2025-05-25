using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayRollManagement.Interface;

namespace PayRollManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollRepository _payrollRepo;

        public PayrollController(IPayrollRepository payrollRepo)
        {
            _payrollRepo = payrollRepo;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromQuery] int employeeId, [FromQuery] int month, [FromQuery] int year, [FromQuery] int processedBy)
        {
            try
            {
                var result = await _payrollRepo.GeneratePayrollAsync(employeeId, month, year, processedBy);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("{employeeId}/{month}/{year}")]
        public async Task<IActionResult> GetByMonth(int employeeId, int month, int year)
        {
            var result = await _payrollRepo.GetPayrollByEmployeeAndMonthAsync(employeeId, month, year);
            return result != null ? Ok(result) : NotFound(new { Error = "Payroll not found" });
        }

        [HttpGet("history/{employeeId}")]
        public async Task<IActionResult> GetHistory(int employeeId)
        {
            var result = await _payrollRepo.GetPayrollHistoryAsync(employeeId);
            return Ok(result);
        }
    }
}
