using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayRollManagement.DTO;
using PayRollManagement.Interface;

namespace PayRollManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeSheetController : ControllerBase
    {
        private readonly ITimeSheetRepository _repo;
        public TimeSheetController(ITimeSheetRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("submit")]
        public async Task<IActionResult> Submit(TimeSheetDto dto)
        {
            var id = await _repo.SubmitTimeSheetAsync(dto);
            return Ok(new { Message = "Submitted", TimeSheetId = id });
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("my/{employeeId}")]
        public async Task<IActionResult> GetMine(int employeeId)
        {
            var data = await _repo.GetByEmployeeAsync(employeeId);
            return Ok(data);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet("pending/{managerId}")]
        public async Task<IActionResult> GetPending(int managerId)
        {
            var data = await _repo.GetPendingApprovalsAsync(managerId);
            return Ok(data);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost("approve")]
        public async Task<IActionResult> Approve([FromQuery] int id, [FromQuery] int approverId)
        {
            var result = await _repo.ApproveTimeSheetAsync(id, approverId);
            return result ? Ok("Approved") : BadRequest("Already approved or not found");
        }
    }
}
