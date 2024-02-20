using Microsoft.AspNetCore.Mvc;

namespace SV20T1020078.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";
            return View("Edit");
        }
        public IActionResult Edit(string id)
        {
            ViewBag.Title = "Sửa thông tin nhân viên";
            return View();
        }

        public IActionResult Delete(string id)
        {
            return View();
        }
    }
}
