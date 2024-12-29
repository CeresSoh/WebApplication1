using System.Data.Entity;
using WebApplication1.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<LeaveApplication> LeaveApplications { get; set; }
    public DbSet<LookupDepartment> LookupDepartments { get; set; }
    public DbSet<LookupLeaveStatus> LookupLeaveStatuses { get; set; }

    public ApplicationDbContext()
        : base("DefaultConnection")
    {
        Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        // Specify the table names for each entity
        modelBuilder.Entity<LeaveApplication>()
                    .ToTable("Leave_Applications");

        modelBuilder.Entity<LookupLeaveStatus>()
                    .ToTable("Lookup_Leave_Status");

        modelBuilder.Entity<LookupDepartment>()
                    .ToTable("Lookup_Department");

        // Configure relationships using Fluent API
        modelBuilder.Entity<LeaveApplication>()
                    .HasRequired(l => l.LeaveStatus)
                    .WithMany(s => s.LeaveApplications)
                    .HasForeignKey(l => l.Status);

        base.OnModelCreating(modelBuilder);
    }
}
