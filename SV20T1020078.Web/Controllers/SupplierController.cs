using Microsoft.AspNetCore.Mvc;
using SV20T1020078.BusinessLayers;

namespace SV20T1020078.Web.Controllers
{
    public class SupplierController : Controller
    {
        public IActionResult Index(int page = 1 , string searchValue = "")
        {
            int pageSize = 20;
            int rowCount = 0;
            var data = CommonDataService.ListOfSuppliers(out rowCount , page, pageSize , searchValue);
            ViewBag.PageSize = page;
            ViewBag.RowCount = rowCount;
            int pageCount = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                pageCount += 1;
            ViewBag.PageCount = pageCount;
            return View(data);
        }

        public IActionResult Create() 
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
            return View("Edit");
        }

        public IActionResult Edit(string id)
        {
            ViewBag.Title = "Sửa thông tin nhà cung cấp";
            return View();
        }

        public IActionResult Delete(string id)
        {
            return View();
        }

    }
}
