-- CREATE DATABASE EasypayPayrollManagementSystem;

USE EasypayPayrollManagementSystem;

-- Create Roles Table
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName VARCHAR(30) NOT NULL UNIQUE
);

-- Create Users Table: username and Password
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    RoleId INT NOT NULL FOREIGN KEY (RoleId) REFERENCES Roles(RoleId),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Create Employees Table
CREATE TABLE Employees (
    EmployeeId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL UNIQUE FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100),
    Phone VARCHAR(20),
    Address VARCHAR(MAX),
    Designation VARCHAR(50),
    Department VARCHAR(50),
    DateOfJoining DATE,
	ManagerId INT NULL FOREIGN KEY (ManagerId) REFERENCES Employees(EmployeeId)
);

-- Create Salaries Table
CREATE TABLE SalaryStructure (
    SalaryId INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId INT NOT NULL FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId),
    BasicPay DECIMAL(10,2) NOT NULL,
    HRA DECIMAL(10,2),
    Allowances DECIMAL(10,2),
    Deductions DECIMAL(10,2),
    EffectiveFrom DATE NOT NULL
);

-- Create Payroll table
CREATE TABLE Payroll (
    PayrollId INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId INT NOT NULL FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId),
    Month INT NOT NULL,
    Year INT NOT NULL,
    GrossPay DECIMAL(10,2),
    NetPay DECIMAL(10,2),
    ProcessedBy INT FOREIGN KEY (ProcessedBy) REFERENCES Users(UserId),
    ProcessedDate DATETIME DEFAULT GETDATE()
);

-- Create leave Requests
CREATE TABLE LeaveRequests (
    LeaveId INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId INT NOT NULL FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId),
    LeaveType VARCHAR(50),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Reason VARCHAR(MAX),
    Status VARCHAR(20) DEFAULT 'Pending',
    ApprovedBy INT FOREIGN KEY (ApprovedBy) REFERENCES Users(UserId),
    AppliedDate DATETIME DEFAULT GETDATE(),
    ApprovedDate DATETIME
);

-- Create AuditLogs
CREATE TABLE AuditLogs (
    LogId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL FOREIGN KEY (UserId) REFERENCES Users(UserId),
    Action VARCHAR(255),
    Timestamp DATETIME DEFAULT GETDATE(),
    Description VARCHAR(MAX)
);
