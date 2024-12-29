using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Lookup_Leave_Status")] 
    public class LookupLeaveStatus
    {
        [Key]
        public int Id { get; set; } // Primary Key

        public string Name { get; set; }

        // Navigation property
        public virtual ICollection<LeaveApplication> LeaveApplications { get; set; }
    }
}
