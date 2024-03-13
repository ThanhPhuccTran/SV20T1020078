using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020078.BusinessLayers;
using SV20T1020078.DomainModels;
using SV20T1020078.Web.Models;

namespace SV20T1020078.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")] //Có nghĩa khi truy cập Action Trong Controller , Kiểm tra xem đã đăng nhập , hay chưa đăng nhập
    public class SupplierController : Controller
    {
        const int PAGE_SIZE = 20;
        const string SUPPLIER_SEARCH = "customer_search";
        public IActionResult Index(int page = 1 , string searchValue = "")
        {
            Models.PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SUPPLIER_SEARCH);
            if (input == null)
            {
                input = new PaginationSearchInput
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                };

            }
            return View(input);
        }
        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfSuppliers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new SupplierSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiênn tim kiếm
            ApplicationContext.SetSessionData(SUPPLIER_SEARCH, input);
            return View(model);
        }
        public IActionResult Create() 
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
            var model = new Supplier()
            {
                SupplierID = 0
            };
            // dùng chung giao diện với Edit
            return View("Edit", model);
            
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Sửa thông tin nhà cung cấp";
            var model = CommonDataService.GetSupplier(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
        [HttpPost] //Attribute (chỉ nhận dữ liệu gửi lên dưới dạng là POST)
        public IActionResult Save(Supplier model)  // viết tường minh :  int customerID , string custormerName ,....
        {
            if (string.IsNullOrWhiteSpace(model.SupplierName))
            {
                ModelState.AddModelError(nameof(model.SupplierName), "Tên nhà cung cấp không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.ContactName))
            {
                ModelState.AddModelError(nameof(model.ContactName), "Tên giao dịch không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), "Email không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Province))
            {
                ModelState.AddModelError(nameof(model.Province), "Vui lòng chọn tỉnh/thành ");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.SupplierID == 0 ? "Bổ sung Nhà cung cấp" : "Cập nhật thông tin Nhà cung cấp";
                return View("Edit", model);
            }
            if (model.SupplierID == 0)
            {
                int id = CommonDataService.AddSupplier(model);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(model.Email), "Email bị trùng ");
                    ViewBag.Title = "Bổ sung Nhà cung cấp";
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateSupplier(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được Nhà cung cấp . Có thể email bị trùng");
                    ViewBag.Title = "Cập nhật Nhà cung cấp";
                    return View("Edit", model);
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteSupplier(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetSupplier(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

    }
}
