using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class SubmitLeaveViewModel
    {
        [Display(Name = "Start Date")]
        public DateTime StartDateTime { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDateTime { get; set; }
        public string Justification { get; set; }
        [Display(Name = "Manager")]
        public int ManagerId { get; set; }
        public List<SelectListItem> Managers { get; set; } // Additional property for dropdown
        public List<LeaveApplication> LeaveApplications { get; set; } // Property to store leave applications
    }
}