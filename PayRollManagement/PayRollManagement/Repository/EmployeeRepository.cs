using Microsoft.EntityFrameworkCore;
using PayRollManagement.DTO;
using PayRollManagement.Interface;
using PayRollManagement.Models;

namespace PayRollManagement.Repository
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly PayMasterDbContext _context;
        public EmployeeRepository(PayMasterDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddEmployee(EmployeeDto dto)
        {
            var employee = new Employee
            {
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                Designation = dto.Designation,
                Department = dto.Department,
                DateOfJoining = dto.DateOfJoining ?? DateTime.Now,
                ManagerId = dto.ManagerId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee.EmployeeId;
        }

        public async Task<List<EmployeeDto>> GetAllEmployees()
        {
            return await _context.Employees
                .Select(emp => new EmployeeDto
                {
                    EmployeeId = emp.EmployeeId,
                    UserId = emp.UserId,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Email = emp.Email,
                    Phone = emp.Phone,
                    Address = emp.Address,
                    Designation = emp.Designation,
                    Department = emp.Department,
                    DateOfJoining = emp.DateOfJoining,
                    ManagerId = emp.ManagerId
                })
                .ToListAsync();
        }

        public async Task<EmployeeDto> GetEmployeeById(int employeeId)
        {
            var emp = await _context.Employees.FindAsync(employeeId);
            if (emp == null) return null;

            return new EmployeeDto
            {
                EmployeeId = emp.EmployeeId,
                UserId = emp.UserId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Email = emp.Email,
                Phone = emp.Phone,
                Address = emp.Address,
                Designation = emp.Designation,
                Department = emp.Department,
                DateOfJoining = emp.DateOfJoining,
                ManagerId = emp.ManagerId
            };
        }

        public async Task<bool> UpdateEmployee(int employeeId, EmployeeDto dto)
        {
            var emp = await _context.Employees.FindAsync(employeeId);
            if(emp == null) return false;

            emp.FirstName = dto.FirstName;
            emp.LastName = dto.LastName;
            emp.Email = dto.Email;
            emp.Phone = dto.Phone;
            emp.Address = dto.Address;
            emp.Designation = dto.Designation;
            emp.Department = dto.Department;
            emp.DateOfJoining = dto.DateOfJoining ?? emp.DateOfJoining;
            emp.ManagerId = dto.ManagerId;

            _context.Employees.Update(emp);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
