using Microsoft.AspNetCore.Mvc;
using SV20T1020078.BusinessLayers;
using SV20T1020078.DomainModels;

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
            var model = new Customer()
            {
                CustomerID = 0
            };
            // dùng chung giao diện với Edit
            return View("Edit" , model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            var model = CommonDataService.GetCustomer(id);
            if(model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
        [HttpPost] //Attribute (chỉ nhận dữ liệu gửi lên dưới dạng là POST)
        public IActionResult Save(Customer model)  // viết tường minh :  int customerID , string custormerName ,....
        {
            if(model.CustomerID == 0)
            {
                int id = CommonDataService.AddCustomer(model);
            }
            else
            {
                bool result = CommonDataService.UpdateCustomer(model);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {   
            if(Request.Method== "POST")
            {
                bool result = CommonDataService.DeleteCustomer(id);
                return RedirectToAction("Index");
            }   
            var model = CommonDataService.GetCustomer(id);
            if(model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
