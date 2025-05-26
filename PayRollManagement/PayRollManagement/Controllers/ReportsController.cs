using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayRollManagement.Interface;
using System.Text;

namespace PayRollManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IAdminRepository _repo;
        public ReportsController(IAdminRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("payroll-summary")]
        public async Task<IActionResult> GetSummary([FromQuery] int month, [FromQuery] int year, [FromQuery] string? department)
        {
            var data = await _repo.GetPayrollSummaryAsync(month, year, department);
            return Ok(data);
        }

        [HttpGet("tax-statements")]
        public async Task<IActionResult> GetTaxStatements([FromQuery] int year)
        {
            var data = await _repo.GetTaxStatementsAsync(year);
            return Ok(data);
        }

        [HttpGet("payroll-summary/download")]
        public async Task<IActionResult> DownloadCsv([FromQuery] int month, [FromQuery] int year, [FromQuery] string? department)
        {
            var summary = await _repo.GetPayrollSummaryAsync(month, year, department);

            var csv = new StringBuilder();
            csv.AppendLine("EmployeeId,Name,Month,Year,GrossPay,PF,Tax,NetPay");

            foreach (var item in summary)
            {
                csv.AppendLine($"{item.EmployeeId},{item.EmployeeName},{item.Month},{item.Year},{item.GrossPay},{item.EmployeePF},{item.ProfessionalTax},{item.NetPay}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"PayrollSummary_{month}_{year}.csv");
        }

        [HttpGet("payroll-summary/download-pdf")]
        public async Task<IActionResult> DownloadPdf([FromQuery] int month, [FromQuery] int year, [FromQuery] string? department)
        {
            var summary = await _repo.GetPayrollSummaryAsync(month, year, department);

            if (summary == null || summary.Count == 0)
                return NotFound("No payroll data found for the given filter.");

            var pdfBytes = PdfReportGenerator.GeneratePayrollSummaryPdf(summary, month, year);

            string safeDept = string.IsNullOrEmpty(department) ? "All" : department.Replace(" ", "_");
            string fileName = $"PayrollSummary_{month}_{year}_{safeDept}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }


    }
}
