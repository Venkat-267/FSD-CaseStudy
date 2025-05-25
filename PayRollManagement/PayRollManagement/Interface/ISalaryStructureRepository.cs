using PayRollManagement.DTO;

namespace PayRollManagement.Interface
{
    public interface ISalaryStructureRepository
    {
        Task<int> AssignSalaryStructureAsync(SalaryStructureDto dto);
        Task<SalaryStructureDto> GetCurrentSalaryStructureAsync(int employeeId);
    }
}
