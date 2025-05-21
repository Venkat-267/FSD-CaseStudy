namespace PayRollManagement.Models
{
    public class Payroll
    {
        public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public decimal? GrossPay { get; set; }
        public decimal? EmployeePF { get; set; }
        public decimal? EmployerPF { get; set; }
        public decimal? NetPay { get; set; }

        public int? ProcessedBy { get; set; }
        public DateTime ProcessedDate { get; set; }

        public Employee Employee { get; set; }
        public User Processor { get; set; }
    }
}
