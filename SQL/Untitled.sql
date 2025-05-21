CREATE TABLE [Roles] (
  [RoleId] integer PRIMARY KEY,
  [RoleName] varchar(30) UNIQUE
)
GO

CREATE TABLE [Users] (
  [UserId] integer PRIMARY KEY,
  [Username] varchar(50),
  [PasswordHash] varchar(255),
  [RoleId] integer,
  [IsActive] boolean
)
GO

CREATE TABLE [Employees] (
  [EmployeeId] integer PRIMARY KEY,
  [UserId] integer,
  [FirstName] varchar(50),
  [LastName] varchar(50),
  [Email] varchar(100),
  [Phone] varchar(20),
  [Address] varchar(max),
  [Designation] varchar(50),
  [Department] varchar(50),
  [DateOfJoining] date,
  [ManagerId] integer
)
GO

CREATE TABLE [Salaries] (
  [SalaryId] integer PRIMARY KEY,
  [EmployeeId] integer,
  [BasicPay] decimal(10,2),
  [HRA] decimal(10,2),
  [Allowances] decimal(10,2),
  [Deductions] decimal(10,2),
  [EffectiveFrom] date
)
GO

CREATE TABLE [Payroll] (
  [PayrollId] integer PRIMARY KEY,
  [EmployeeId] integer,
  [Month] integer,
  [Year] integer,
  [GrossPay] decimal(10,2),
  [NetPay] decimal(10,2),
  [ProcessedBy] integer,
  [ProcessedDate] datetime
)
GO

CREATE TABLE [LeaveRequests] (
  [LeaveId] integer PRIMARY KEY,
  [EmployeeId] integer,
  [StartDate] date,
  [EndDate] date,
  [Reason] varchar(max),
  [Status] varchar(20),
  [ApprovedBy] integer,
  [AppliedDate] datetime
)
GO

CREATE TABLE [AuditLogs] (
  [LogId] integer PRIMARY KEY,
  [UserId] integer,
  [Action] varchar(255),
  [Timestamp] datetime,
  [Description] varchar(max)
)
GO

ALTER TABLE [Users] ADD FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([RoleId])
GO

ALTER TABLE [Employees] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO

ALTER TABLE [Employees] ADD FOREIGN KEY ([ManagerId]) REFERENCES [Employees] ([EmployeeId])
GO

ALTER TABLE [Salaries] ADD FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([EmployeeId])
GO

ALTER TABLE [Payroll] ADD FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([EmployeeId])
GO

ALTER TABLE [Payroll] ADD FOREIGN KEY ([ProcessedBy]) REFERENCES [Users] ([UserId])
GO

ALTER TABLE [LeaveRequests] ADD FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([EmployeeId])
GO

ALTER TABLE [LeaveRequests] ADD FOREIGN KEY ([ApprovedBy]) REFERENCES [Employees] ([EmployeeId])
GO

ALTER TABLE [AuditLogs] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO
