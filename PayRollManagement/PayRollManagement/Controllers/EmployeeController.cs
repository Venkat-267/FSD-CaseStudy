using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayRollManagement.DTO;
using PayRollManagement.Interface;

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

        [HttpPut("update/{employeeId}")]
        public async Task<IActionResult> Update(int employeeId, EmployeeDto dto)
        {
            var success = await _employeeRepo.UpdateEmployee(employeeId, dto);
            if (!success) return NotFound(new { Error = "Employee not found" });
            return Ok(new { Message = "Employee updated" });
        }
    }
}
