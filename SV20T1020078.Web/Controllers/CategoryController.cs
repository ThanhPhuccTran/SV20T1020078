using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020078.BusinessLayers;
using SV20T1020078.DomainModels;
using SV20T1020078.Web.Models;

namespace SV20T1020078.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")] //Có nghĩa khi truy cập Action Trong Controller , Kiểm tra xem đã đăng nhập , hay chưa đăng nhập
    public class CategoryController : Controller
    {
        const int PAGE_SIZE = 20;
        const string CATEGORY_SEARCH = "category_search";
        public IActionResult Index(int page = 1 , string searchValue="")
        {
            /*int rowCount = 0;
            var data = CommonDataService.ListOfCategories(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            var model = new Models.CategorySearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data

            };
            return View(model);*/
            Models.PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(CATEGORY_SEARCH);
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
            var data = CommonDataService.ListOfCategories(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new CategorySearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiênn tim kiếm
            ApplicationContext.SetSessionData(CATEGORY_SEARCH, input);
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung loại hàng";
            var model = new Category()
            {
                CategoryID = 0
            };
            return View("Edit",model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật loại hàng";
            var model = CommonDataService.GetCategory(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost] //Attribute (chỉ nhận dữ liệu gửi lên dưới dạng là POST)
        public IActionResult Save(Category model)  // viết tường minh :  int customerID , string custormerName ,....
        {
            if(string.IsNullOrWhiteSpace(model.CategoryName))
            {
                ModelState.AddModelError(nameof(model.CategoryName), "Tên loại không được để trống");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CategoryID == 0 ? "Bổ sung loại hàng" : "Cập nhật thông tin loại hàng";
                return View("Edit", model);
            }



            if (model.CategoryID == 0)
            {
                int id = CommonDataService.AddCategory(model);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(model.CategoryName), "Tên loại bị trùng ");
                    ViewBag.Title = "Bổ sung loại hàng";
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateCategory(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được loại hàng . Có thể tên loại bị trùng");
                    ViewBag.Title = "Cập nhật loại hàng";
                    return View("Edit", model);
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetCategory(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
