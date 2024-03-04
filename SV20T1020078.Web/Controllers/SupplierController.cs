using Microsoft.AspNetCore.Mvc;
using SV20T1020078.BusinessLayers;

namespace SV20T1020078.Web.Controllers
{
    public class SupplierController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1 , string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfSuppliers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            var model = new Models.SupplierSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data

            };
            return View(model);
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
