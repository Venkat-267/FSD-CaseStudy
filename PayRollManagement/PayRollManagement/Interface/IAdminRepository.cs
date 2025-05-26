using PayRollManagement.DTO;

namespace PayRollManagement.Interface
{
    public interface IAdminRepository
    {
        Task<List<PayrollDto>> GetTeamPayrollsAsync(int managerId);
        Task<List<LeaveRequestDto>> GetTeamPendingLeavesAsync(int managerId);
        Task<int> GenerateAuditLogAsync(AuditLogDto dto);
        Task<List<AuditLogDto>> GetAuditLogsByUserAsync(int userId);
        Task<List<PayrollSummaryDto>> GetPayrollSummaryAsync(int month, int year, string? department = null);
        Task<List<TaxStatementDto>> GetTaxStatementsAsync(int year);


    }
}
