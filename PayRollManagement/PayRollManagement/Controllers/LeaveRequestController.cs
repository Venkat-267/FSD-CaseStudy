using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayRollManagement.DTO;
using PayRollManagement.Interface;

namespace PayRollManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestRepository _leaveRepo;
        private readonly IAdminRepository _adminRepo;
        public LeaveRequestController(ILeaveRequestRepository leaveRepo, IAdminRepository adminRepo)
        {
            _leaveRepo = leaveRepo;
            _adminRepo = adminRepo;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit(LeaveRequestDto dto)
        {
            var leaveId = await _leaveRepo.SubmitLeaveRequest(dto);
            return Ok(new { Message = "Leave Request Submitted", LeaveId = leaveId });
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            var list = await _leaveRepo.GetLeaveRequestsByEmployee(employeeId);
            return Ok(list);
        }

        [HttpPost("review/{leaveId}")]
        public async Task<IActionResult> Review(int leaveId, [FromQuery] int approverId, [FromQuery] string action)
        {
            var success = await _leaveRepo.ApproveOrDenyLeave(leaveId, approverId, action);
            if (!success)
            {
                return BadRequest(new { Error = "Invalid Leave or Action" });
            }
            await _adminRepo.GenerateAuditLogAsync(new AuditLogDto
            {
                UserId = approverId,
                Action = $"{action} Leave",
                Description = $"Leave #{leaveId} {action}ed by user {approverId}"
            });
            return Ok(new { Message = $"Leave {action.ToLower()}ed successfully!" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] int? employeeId,
        [FromQuery] string status,
        [FromQuery] string leaveType,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
        {
            var results = await _leaveRepo.SearchLeaveRequests(employeeId, status, leaveType, from, to);
            return Ok(results);
        }

    }
}
