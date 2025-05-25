using PayRollManagement.DTO;

namespace PayRollManagement.Interface
{
    public interface IEmployeeRepository
    {
        Task<int> AddEmployee(EmployeeDto dto);
        Task<List<EmployeeDto>> GetAllEmployees();
        Task<EmployeeDto> GetEmployeeById(int employeeId);
        Task<bool> UpdateEmployee(int employeeId, EmployeeDto dto);
        Task<List<EmployeeDto>> SearchEmployeesAsync(string? name = null, string? department = null, string? designation = null, int? managerId = null);

    }
}
