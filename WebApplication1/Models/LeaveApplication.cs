using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    [Table("Leave_Applications")]
    public class LeaveApplication
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public int ManagerId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Justification { get; set; }
        public int Status { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? AppRejDate { get; set; }

        [ForeignKey("Status")]
        public virtual LookupLeaveStatus LeaveStatus { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Employee Manager { get; set; }
    }
}
