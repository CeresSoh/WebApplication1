using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Filter;
using System.Data.Entity;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = (int)Session["UserId"];
            var employee = _context.Employees.Find(userId);

            var model = new SubmitLeaveViewModel
            {
                Managers = _context.Employees
                                   .Where(e => e.IsManager && e.DepartmentId == employee.DepartmentId)
                                   .Select(e => new SelectListItem
                                   {
                                       Value = e.Id.ToString(),
                                       Text = e.Name
                                   }).ToList(),
                LeaveApplications = _context.LeaveApplications
                                            .Where(l => l.EmployeeId == userId)
                                            .Include(l => l.LeaveStatus) // Include related entity
                                            .Include(l => l.Employee)
                                            .Include(l => l.Manager)
                                            .ToList()
            };

            return View(model);
        }


        [HttpGet]
        public ActionResult SubmitLeave()
        {
            //ViewBag.Managers = _context.Employees.Where(e => e.IsManager).ToList();
            //return View();

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = (int)Session["UserId"];
            var employee = _context.Employees.Find(userId);

            var model = new SubmitLeaveViewModel
            {
                Managers = _context.Employees
                                   .Where(e => e.IsManager && e.DepartmentId == employee.DepartmentId)
                                   .Select(e => new SelectListItem
                                   {
                                       Value = e.Id.ToString(),
                                       Text = e.Name
                                   }).ToList(),
                LeaveApplications = _context.LeaveApplications
                                            .Where(l => l.EmployeeId == userId)
                                            .Include(l => l.LeaveStatus) // Include related entity
                                            .Include(l => l.Employee)
                                            .Include(l => l.Manager)
                                            .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitLeave(SubmitLeaveViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ManagerId == 0)
                {
                    ModelState.AddModelError("ManagerId", "Please select a manager for leave approval.");
                }

                if (model.EndDateTime < model.StartDateTime)
                {
                    ModelState.AddModelError("EndDateTime", "End Date must be later than or equal to Start Date.");
                }

                if (!ModelState.IsValid)
                {
                    // Repopulate the Managers dropdown and LeaveApplications for the view
                    int userId = (int)Session["UserId"];
                    var employee = _context.Employees.Find(userId);

                    model.Managers = _context.Employees
                        .Where(e => e.IsManager && e.DepartmentId == employee.DepartmentId)
                        .Select(e => new SelectListItem
                        {
                            Value = e.Id.ToString(),
                            Text = e.Name
                        }).ToList();

                    model.LeaveApplications = _context.LeaveApplications
                        .Where(l => l.EmployeeId == userId)
                        .Include(l => l.LeaveStatus)
                        .ToList();

                    return View("SubmitLeave", model);
                }

                var leaveApplication = new LeaveApplication
                {
                    EmployeeId = (int)Session["UserId"],
                    ManagerId = model.ManagerId,
                    StartDateTime = model.StartDateTime,
                    EndDateTime = model.EndDateTime,
                    Justification = model.Justification,
                    Status = _context.LookupLeaveStatuses.Single(s => s.Name == "Pending").Id,
                    SubmissionDate = DateTime.Now
                };

                _context.LeaveApplications.Add(leaveApplication);
                _context.SaveChanges();

                // Notify manager
                var manager = _context.Employees.Find(leaveApplication.ManagerId);
                SendEmail(manager.Email, "New Leave Application", "You have a new leave request pending your approval.");

                return RedirectToAction("SubmitLeave");
            }

            // Repopulate the Managers dropdown and LeaveApplications for the view
            int userIdForGet = (int)Session["UserId"];
            var employeeForGet = _context.Employees.Find(userIdForGet);

            model.Managers = _context.Employees
                .Where(e => e.IsManager && e.DepartmentId == employeeForGet.DepartmentId)
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name
                }).ToList();

            model.LeaveApplications = _context.LeaveApplications
                .Where(l => l.EmployeeId == userIdForGet)
                .Include(l => l.LeaveStatus)
                .ToList();

            return View("SubmitLeave", model);
        }

        [AuthFilter]
        public ActionResult Approval()
        {
            if ((bool)Session["IsManager"] == false)
            {
                return RedirectToAction("Index", "Home");
            }

            var pendingLeaves = _context.LeaveApplications
                .Where(l => l.Status == 1) // Pending
                .ToList();

            return View(pendingLeaves);
        }

        public ActionResult ApproveLeave(int leaveId)
        {
            var leave = _context.LeaveApplications.Find(leaveId);
            if (leave != null)
            {
                leave.Status = 2; // Assume "2" is the ID for "Approved"
                leave.AppRejDate = DateTime.Now;
                _context.SaveChanges();

                // Notify employee
                var employee = _context.Employees.Find(leave.EmployeeId);
                SendEmail(employee.Email, "Leave Application Approved", "Your leave request has been approved.");
            }
            return RedirectToAction("Approval");
        }

        public ActionResult RejectLeave(int leaveId)
        {
            var leave = _context.LeaveApplications.Find(leaveId);
            if (leave != null)
            {
                leave.Status = 3; // Assume "3" is the ID for "Rejected"
                leave.AppRejDate = DateTime.Now;
                _context.SaveChanges();

                // Notify employee
                var employee = _context.Employees.Find(leave.EmployeeId);
                SendEmail(employee.Email, "Leave Application Rejected", "Your leave request has been rejected.");
            }
            return RedirectToAction("Approval");
        }

        public ActionResult BackMenu()
        {
            return RedirectToAction("Index", "Home");
        }

        private void SendEmail(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io")
            {
                Port = 587,
                Credentials = new NetworkCredential("8527a7356ea9f5", "0a0da0782578fb"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("your-email@example.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);
            smtpClient.Send(mailMessage);
        }
    }
}
