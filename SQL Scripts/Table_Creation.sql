IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Lookup_Department' AND SCHEMA_NAME(schema_id) = 'dbo')
BEGIN
	CREATE TABLE Lookup_Department(
		Id INT PRIMARY KEY IDENTITY,
		Name NVARCHAR(100),
	);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Lookup_Leave_Status' AND SCHEMA_NAME(schema_id) = 'dbo')
BEGIN
	CREATE TABLE Lookup_Leave_Status(
		Id INT PRIMARY KEY IDENTITY,
		Name NVARCHAR(100),
	);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Employees' AND SCHEMA_NAME(schema_id) = 'dbo')
BEGIN
	CREATE TABLE Employees (
		Id INT PRIMARY KEY IDENTITY,
		Name NVARCHAR(100),
		Email NVARCHAR(100),
		DepartmentId INT FOREIGN KEY REFERENCES Lookup_Department(Id),
		IsManager BIT
	);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Leave_Applications' AND SCHEMA_NAME(schema_id) = 'dbo')
BEGIN
	CREATE TABLE Leave_Applications (
		Id INT PRIMARY KEY IDENTITY,
		EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
		ManagerId INT FOREIGN KEY REFERENCES Employees(Id),
		StartDateTime DATETIME,
		EndDateTime DATETIME,
		Justification NVARCHAR(MAX),
		Status INT FOREIGN KEY REFERENCES Lookup_Leave_Status(Id),
		SubmissionDate DATETIME DEFAULT GETDATE(),
		AppRejDate DATETIME 
	);
END
GO

