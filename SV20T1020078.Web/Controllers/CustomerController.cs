using Microsoft.AspNetCore.Mvc;
using SV20T1020078.BusinessLayers;

namespace SV20T1020078.Web.Controllers
{
    public class CustomerController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1 , string searchValue = "")
        {
            /* int pageSize = 20;
             int rowCount = 0;

             var data = CommonDataService.ListOfCustomers(out rowCount , page , pageSize,searchValue);

             ViewBag.PageSize = page;
             ViewBag.RowCount = rowCount;
             int pageCount = rowCount / pageSize;
             if(rowCount % pageSize > 0)
                 pageCount += 1;
             ViewBag.PageCount = pageCount;*/

            int rowCount = 0;
            var data = CommonDataService.ListOfCustomers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            var model = new Models.CustomerSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data

            };
            return View(model); // dữ liệu truyền cho View có kiểu dữ liệu Model.CustomerSearchResult
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
