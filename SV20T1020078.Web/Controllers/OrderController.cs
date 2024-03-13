using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SV20T1020078.Web.Controllers
{
    [Authorize] //Có nghĩa khi truy cập Action Trong Controller , Kiểm tra xem đã đăng nhập , hay chưa đăng nhập
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id = 0)
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult EditDetail(int id = 0 , int products = 0)
        {
            return View();
        }
        public  IActionResult Shipping(int id = 0)
        {
            return View();
        }

    }
}
