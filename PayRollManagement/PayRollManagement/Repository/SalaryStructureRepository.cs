using Microsoft.EntityFrameworkCore;
using PayRollManagement.DTO;
using PayRollManagement.Interface;
using PayRollManagement.Models;

namespace PayRollManagement.Repository
{
    public class SalaryStructureRepository : ISalaryStructureRepository
    {
        private readonly PayMasterDbContext _context;

        public SalaryStructureRepository(PayMasterDbContext context)
        {
            _context = context;
        }

        public async Task<int> AssignSalaryStructureAsync(SalaryStructureDto dto)
        {
            var salary = new SalaryStructure
            {
                EmployeeId = dto.EmployeeId,
                BasicPay = dto.BasicPay,
                HRA = dto.HRA,
                Allowances = dto.Allowances,
                PFPercentage = dto.PFPercentage ?? 12.0m,
                EffectiveFrom = dto.EffectiveFrom
            };

            _context.SalaryStructures.Add(salary);
            await _context.SaveChangesAsync();
            return salary.SalaryId;
        }

        public async Task<SalaryStructureDto> GetCurrentSalaryStructureAsync(int employeeId)
        {
            var latest = await _context.SalaryStructures
                .Where(s => s.EmployeeId == employeeId)
                .OrderByDescending(s => s.EffectiveFrom)
                .FirstOrDefaultAsync();

            if (latest == null) return null;

            return new SalaryStructureDto
            {
                SalaryId = latest.SalaryId,
                EmployeeId = latest.EmployeeId,
                BasicPay = latest.BasicPay,
                HRA = latest.HRA,
                Allowances = latest.Allowances,
                PFPercentage = latest.PFPercentage,
                EffectiveFrom = latest.EffectiveFrom
            };
        }
    }
}
