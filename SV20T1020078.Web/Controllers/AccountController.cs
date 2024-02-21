using Microsoft.AspNetCore.Mvc;

namespace SV20T1020078.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            if(Request.Method == "POST")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
