IF NOT EXISTS (SELECT 1 FROM Lookup_Department WHERE Id IN (1,2))
BEGIN
	INSERT INTO Lookup_Department VALUES ('Finance');
	INSERT INTO Lookup_Department VALUES ('IT');
END
GO

IF NOT EXISTS (SELECT 1 FROM Lookup_Leave_Status WHERE Id IN (1,2,3))
BEGIN
	INSERT INTO Lookup_Leave_Status VALUES ('Pending');
	INSERT INTO Lookup_Leave_Status VALUES ('Approved');
	INSERT INTO Lookup_Leave_Status VALUES ('Rejected');
END
GO

IF NOT EXISTS (SELECT 1 FROM Employees WHERE Id IN (1,2,3))
BEGIN
	INSERT INTO Employees VALUES ('Ayaha', 'mikagiaya@hotmail.co.jp', '1', '0');
	INSERT INTO Employees VALUES ('Soh Giok Hian', 'mikagiaya@gmail.com', '1', '1');
	INSERT INTO Employees VALUES ('Ceres', 'ceres.soh@sgsupport.com', '2', '0');
END
GO

