using Microsoft.EntityFrameworkCore;

namespace PayRollManagement.Models
{
    public class PayMasterDbContext: DbContext
    {
        public PayMasterDbContext(DbContextOptions<PayMasterDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             // optionsBuilder.UseSqlServer("");

            //base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SalaryStructure>().HasKey(s => s.SalaryId);
            modelBuilder.Entity<LeaveRequest>().HasKey(l => l.LeaveId);
            modelBuilder.Entity<AuditLog>().HasKey(a => a.LogId);
            // One-to-many: Role -> Users
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);

            // One-to-one: User -> Employee
            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<Employee>(e => e.UserId);

            // Self-reference: Employee -> Manager
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.Subordinates)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many: Employee -> LeaveRequests
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.LeaveRequests)
                .WithOne(l => l.Employee)
                .HasForeignKey(l => l.EmployeeId);

            // One-to-many: User (Approver) -> LeaveRequests
            modelBuilder.Entity<User>()
                .HasMany(u => u.ApprovedLeaveRequests)
                .WithOne(l => l.Approver)
                .HasForeignKey(l => l.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many: Employee -> Payrolls
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Payrolls)
                .WithOne(p => p.Employee)
                .HasForeignKey(p => p.EmployeeId);

            // One-to-many: User (Processor) -> Payrolls
            modelBuilder.Entity<User>()
                .HasMany(u => u.ProcessedPayrolls)
                .WithOne(p => p.Processor)
                .HasForeignKey(p => p.ProcessedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many: Employee -> SalaryStructures
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.SalaryStructures)
                .WithOne(s => s.Employee)
                .HasForeignKey(s => s.EmployeeId);

            // One-to-many: User -> AuditLogs
            modelBuilder.Entity<User>()
                .HasMany(u => u.AuditLogs)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<SalaryStructure> SalaryStructures { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
