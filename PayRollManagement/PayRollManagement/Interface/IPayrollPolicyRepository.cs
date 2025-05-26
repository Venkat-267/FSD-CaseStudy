using PayRollManagement.Models;

namespace PayRollManagement.Interface
{
    public interface IPayrollPolicyRepository
    {
        Task<int> SetPolicyAsync(PayrollPolicy policy);
        Task<PayrollPolicy?> GetLatestPolicyAsync(); 
    }
}
