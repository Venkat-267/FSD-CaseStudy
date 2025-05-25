using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayRollManagement.DTO;
using PayRollManagement.Interface;

namespace PayRollManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepo;

        public AdminController(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        [HttpGet("team/payrolls/{managerId}")]
        public async Task<IActionResult> GetTeamPayrolls(int managerId)
        {
            var list = await _adminRepo.GetTeamPayrollsAsync(managerId);
            return Ok(list);
        }

        [HttpGet("team/leave-requests/{managerId}")]
        public async Task<IActionResult> GetTeamPendingLeaves(int managerId)
        {
            var list = await _adminRepo.GetTeamPendingLeavesAsync(managerId);
            return Ok(list);
        }

        [HttpPost("auditlog")]
        public async Task<IActionResult> LogAction(AuditLogDto dto)
        {
            var logId = await _adminRepo.GenerateAuditLogAsync(dto);
            return Ok(new { Message = "Audit log created", LogId = logId });
        }

        [HttpGet("auditlog/user/{userId}")]
        public async Task<IActionResult> GetUserLogs(int userId)
        {
            var logs = await _adminRepo.GetAuditLogsByUserAsync(userId);
            return Ok(logs);
        }

    }
}
