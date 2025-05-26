using Microsoft.EntityFrameworkCore;
using PayRollManagement.DTO;
using PayRollManagement.Interface;
using PayRollManagement.Models;

namespace PayRollManagement.Repository
{
    public class PayrollRepository : IPayrollRepository
    {
        private readonly PayMasterDbContext _context;

        public PayrollRepository(PayMasterDbContext context)
        {
            _context = context;
        }

        public async Task<PayrollDto> GeneratePayrollAsync(int employeeId, int month, int year, int processedBy)
        {
            // Avoid duplication
            var existing = await _context.Payrolls.FirstOrDefaultAsync(p =>
                p.EmployeeId == employeeId && p.Month == month && p.Year == year);

            if (existing != null)
                throw new Exception("Payroll already generated for this period.");

            // Get latest salary structure
            var salary = await _context.SalaryStructures
                .Where(s => s.EmployeeId == employeeId)
                .OrderByDescending(s => s.EffectiveFrom)
                .FirstOrDefaultAsync();

            if (salary == null)
                throw new Exception("No salary structure found for this employee.");

            var policy = await _context.PayrollPolicies
                .OrderByDescending(p => p.EffectiveFrom)
                .FirstOrDefaultAsync();

            // ✅ Get all benefits for this employee (optional: filter by month/year if you want)
            var benefits = await _context.Benefits
                .Where(b => b.EmployeeId == employeeId)
                .ToListAsync();

            decimal benefitTotal = benefits.Sum(b => b.Amount);

            // Base + HRA + Allowances + Benefits
            decimal gross = salary.BasicPay + (salary.HRA ?? 0) + (salary.Allowances ?? 0) + benefitTotal;
            decimal pfRate = (salary.PFPercentage ?? policy?.DefaultPFPercent ?? 12) / 100;
            decimal professionalTax = policy?.ProfessionalTax ?? 0;
            decimal employeePF = salary.BasicPay * pfRate;
            decimal employerPF = salary.BasicPay * pfRate;
            decimal netPay = gross - employeePF - professionalTax;

            var payroll = new Payroll
            {
                EmployeeId = employeeId,
                Month = month,
                Year = year,
                GrossPay = gross,
                EmployeePF = employeePF,
                EmployerPF = employerPF,
                ProfessionalTax = professionalTax,
                NetPay = netPay,
                ProcessedBy = processedBy,
                ProcessedDate = DateTime.Now
            };

            _context.Payrolls.Add(payroll);
            await _context.SaveChangesAsync();

            return new PayrollDto
            {
                PayrollId = payroll.PayRollId,
                EmployeeId = payroll.EmployeeId,
                Month = payroll.Month,
                Year = payroll.Year,
                GrossPay = payroll.GrossPay,
                EmployeePF = payroll.EmployeePF,
                EmployerPF = payroll.EmployerPF,
                ProfessionalTax = payroll.ProfessionalTax,
                NetPay = payroll.NetPay,
                ProcessedBy = payroll.ProcessedBy,
                ProcessedDate = payroll.ProcessedDate
            };
        }

        public async Task<PayrollDto> GetPayrollByEmployeeAndMonthAsync(int employeeId, int month, int year)
        {
            var payroll = await _context.Payrolls
                .FirstOrDefaultAsync(p => p.EmployeeId == employeeId && p.Month == month && p.Year == year);

            if (payroll == null) return null;

            return new PayrollDto
            {
                PayrollId = payroll.PayRollId,
                EmployeeId = payroll.EmployeeId,
                Month = payroll.Month,
                Year = payroll.Year,
                GrossPay = payroll.GrossPay,
                EmployeePF = payroll.EmployeePF,
                EmployerPF = payroll.EmployerPF,
                NetPay = payroll.NetPay,
                ProcessedBy = payroll.ProcessedBy,
                ProcessedDate = payroll.ProcessedDate
            };
        }

        public async Task<List<PayrollDto>> GetPayrollHistoryAsync(int employeeId)
        {
            return await _context.Payrolls
                .Where(p => p.EmployeeId == employeeId)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
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
    }
}
