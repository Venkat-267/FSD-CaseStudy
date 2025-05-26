using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayRollManagement.DTO;
using PayRollManagement.Interface;
using System.Security.Claims;

namespace PayRollManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;
        public EmployeeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(EmployeeDto dto)
        {
            try
            {
                var empId = await _employeeRepo.AddEmployee(dto);
                return Ok(new { Message = "Employee added", EmployeeId = empId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeRepo.GetAllEmployees();
            return Ok(employees);
        }


        [HttpGet("{employeeId}")]
        public async Task<IActionResult> Get(int employeeId)
        {
            var emp = await _employeeRepo.GetEmployeeById(employeeId);
            if (emp == null) return NotFound(new { Error = "Employee not found" });
            return Ok(emp);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] string? department, [FromQuery] string? designation, [FromQuery] int? managerId)
        {
            var result = await _employeeRepo.SearchEmployeesAsync(name, department, designation, managerId);
            return Ok(result);
        }


        [HttpPut("update/{employeeId}")]
        public async Task<IActionResult> Update(int employeeId, EmployeeDto dto)
        {
            var success = await _employeeRepo.UpdateEmployee(employeeId, dto);
            if (!success) return NotFound(new { Error = "Employee not found" });
            return Ok(new { Message = "Employee updated" });
        }

        [HttpPut("update-personal")]
        public async Task<IActionResult> UpdatePersonalInfo([FromBody] UpdatePersonalInfoDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var success = await _employeeRepo.UpdatePersonalInfoAsync(userId, dto);
            if (!success)
                return NotFound(new { Error = "Employee not found" });

            return Ok(new { Message = "Personal information updated successfully." });
        }
    }
}
