namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        DepartmentId = c.Int(nullable: false),
                        IsManager = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LookupDepartments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.LookupDepartments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeaveApplications",
                c => new
                    {
                        LeaveId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        ManagerId = c.Int(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        Justification = c.String(),
                        Status = c.Int(nullable: false),
                        SubmissionDate = c.DateTime(nullable: false),
                        AppRejDate = c.DateTime(),
                        LeaveStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.LeaveId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.LookupLeaveStatus", t => t.LeaveStatus_Id)
                .ForeignKey("dbo.Employees", t => t.ManagerId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.ManagerId)
                .Index(t => t.LeaveStatus_Id);
            
            CreateTable(
                "dbo.LookupLeaveStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeaveApplications", "ManagerId", "dbo.Employees");
            DropForeignKey("dbo.LeaveApplications", "LeaveStatus_Id", "dbo.LookupLeaveStatus");
            DropForeignKey("dbo.LeaveApplications", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "DepartmentId", "dbo.LookupDepartments");
            DropIndex("dbo.LeaveApplications", new[] { "LeaveStatus_Id" });
            DropIndex("dbo.LeaveApplications", new[] { "ManagerId" });
            DropIndex("dbo.LeaveApplications", new[] { "EmployeeId" });
            DropIndex("dbo.Employees", new[] { "DepartmentId" });
            DropTable("dbo.LookupLeaveStatus");
            DropTable("dbo.LeaveApplications");
            DropTable("dbo.LookupDepartments");
            DropTable("dbo.Employees");
        }
    }
}
