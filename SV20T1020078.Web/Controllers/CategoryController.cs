using Microsoft.AspNetCore.Mvc;

namespace SV20T1020078.Web.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung loại hàng";
            return View("Edit");
        }

        public IActionResult Edit(string id)
        {
            ViewBag.Title = "Sửa loại hàng";
            return View();
        }

        public IActionResult Delete(string id)
        {
            return View();
        }
    }
}
