namespace PayRollManagement.DTO
{
    public class PayrollDto
    {
        public int? PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public decimal? GrossPay { get; set; }
        public decimal? EmployeePF { get; set; }
        public decimal? EmployerPF { get; set; }
        public decimal? ProfessionalTax { get; set; }
        public decimal? NetPay { get; set; }

        public int? ProcessedBy { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }

    public class PayrollSummaryDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal GrossPay { get; set; }
        public decimal EmployeePF { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal NetPay { get; set; }
    }

    public class TaxStatementDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal TotalEmployeePF { get; set; }
        public decimal TotalProfessionalTax { get; set; }
        public decimal TotalTax => TotalEmployeePF + TotalProfessionalTax;
    }

}
