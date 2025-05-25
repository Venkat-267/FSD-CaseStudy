using Microsoft.EntityFrameworkCore;
using PayRollManagement.DTO;
using PayRollManagement.Interface;
using PayRollManagement.Models;

namespace PayRollManagement.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly PayMasterDbContext _context;

        public AdminRepository(PayMasterDbContext context)
        {
            _context = context;
        }

        public async Task<List<PayrollDto>> GetTeamPayrollsAsync(int managerId)
        {
            var employeeIds = await _context.Employees
                .Where(e => e.ManagerId == managerId)
                .Select(e => e.EmployeeId)
                .ToListAsync();

            return await _context.Payrolls
                .Where(p => employeeIds.Contains(p.EmployeeId))
                .Select(p => new PayrollDto
                {
                    PayrollId = p.PayRollId,
                    EmployeeId = p.EmployeeId,
                    Month = p.Month,
                    Year = p.Year,
                    GrossPay = p.GrossPay,
                    EmployeePF = p.EmployeePF,
                    EmployerPF = p.EmployerPF,
                    NetPay = p.NetPay,
                    ProcessedBy = p.ProcessedBy,
                    ProcessedDate = p.ProcessedDate
                }).ToListAsync();
        }

        public async Task<List<LeaveRequestDto>> GetTeamPendingLeavesAsync(int managerId)
        {
            var employeeIds = await _context.Employees
                .Where(e => e.ManagerId == managerId)
                .Select(e => e.EmployeeId)
                .ToListAsync();

            return await _context.LeaveRequests
                .Where(l => employeeIds.Contains(l.EmployeeId) && l.Status == "Pending")
                .Select(l => new LeaveRequestDto
                {
                    LeaveId = l.LeaveId,
                    EmployeeId = l.EmployeeId,
                    LeaveType = l.LeaveType,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Reason = l.Reason,
                    Status = l.Status,
                    ApprovedBy = l.ApprovedBy,
                    AppliedDate = l.AppliedDate,
                    ApprovedDate = l.ApprovedDate
                }).ToListAsync();
        }

        public async Task<int> GenerateAuditLogAsync(AuditLogDto dto)
        {
            var log = new AuditLog
            {
                UserId = dto.UserId,
                Action = dto.Action,
                Description = dto.Description,
                Timestamp = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
            return log.LogId;
        }

        public async Task<List<AuditLogDto>> GetAuditLogsByUserAsync(int userId)
        {
            return await _context.AuditLogs
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Timestamp)
                .Select(a => new AuditLogDto
                {
                    LogId = a.LogId,
                    UserId = a.UserId,
                    Action = a.Action,
                    Description = a.Description,
                    Timestamp = a.Timestamp
                }).ToListAsync();
        }

    }
}
