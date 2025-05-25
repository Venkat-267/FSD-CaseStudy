using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayRollManagement.DTO;
using PayRollManagement.Interface;

namespace PayRollManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryStructureController : ControllerBase
    {
        private readonly ISalaryStructureRepository _salaryRepo;

        public SalaryStructureController(ISalaryStructureRepository salaryRepo)
        {
            _salaryRepo = salaryRepo;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign(SalaryStructureDto dto)
        {
            var salaryId = await _salaryRepo.AssignSalaryStructureAsync(dto);
            return Ok(new { Message = "Salary structure assigned", SalaryId = salaryId });
        }

        [HttpGet("current/{employeeId}")]
        public async Task<IActionResult> GetCurrent(int employeeId)
        {
            var salary = await _salaryRepo.GetCurrentSalaryStructureAsync(employeeId);
            if (salary == null) return NotFound(new { Error = "No salary structure found" });

            return Ok(salary);
        }
    }
}
