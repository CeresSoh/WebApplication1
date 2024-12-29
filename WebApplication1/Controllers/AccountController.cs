using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Login
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Employees.FirstOrDefault(e => e.Email == model.Email);

                if (user != null)
                {
                    // Store user information in session
                    Session["UserId"] = user.Id;
                    Session["UserEmail"] = user.Email;
                    Session["UserName"] = user.Name;
                    Session["IsManager"] = user.IsManager;

                    // Redirect based on role
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email address.");
                }
            }

            return View(model);
        }
    }
}
