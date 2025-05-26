namespace PayRollManagement.DTO
{
    public class TimeSheetDto
    {
        public int? TimeSheetId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime WorkDate { get; set; }
        public decimal HoursWorked { get; set; }
        public string TaskDescription { get; set; }
        public bool IsApproved { get; set; }
    }
}
