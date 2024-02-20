using Microsoft.AspNetCore.Mvc;

namespace SV20T1020078.Web.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung khách hàng";
            // dùng chung giao diện với Edit
            return View("Edit");
        }

        public IActionResult Edit(string id)
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            return View();
        }

        public IActionResult Delete(string id)
        {
            return View();
        }
    }
}
