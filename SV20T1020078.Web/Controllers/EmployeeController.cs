using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SV20T1020078.BusinessLayers;
using SV20T1020078.DomainModels;
using SV20T1020078.Web.Models;
using System.Buffers;

namespace SV20T1020078.Web.Controllers
{
    [Authorize] //Có nghĩa khi truy cập Action Trong Controller , Kiểm tra xem đã đăng nhập , hay chưa đăng nhập
    public class EmployeeController : Controller
    {
        const int PAGE_SIZE = 20;
        const string EMPLOYEE_SEARCH = "employee_search";
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            /*int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            var model = new Models.EmployeeSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            return View(model);*/
            Models.PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(EMPLOYEE_SEARCH);
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
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";
            var model = new Employee()
            {
                EmployeeID = 0,
                Photo = "nophoto.png",
                BirthDate = new DateTime(1990, 1, 1),
                IsWorking=true,
            };
            return View("Edit", model);
        }
        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new EmployeeSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiênn tim kiếm
            ApplicationContext.SetSessionData(EMPLOYEE_SEARCH, input);
            return View(model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhân viên";
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrWhiteSpace(model.Photo))
            {
                model.Photo = "nophoto.png";
            }

            return View(model);
        }
        [HttpPost] //Attribute (chỉ nhận dữ liệu gửi lên dưới dạng là POST)
        public IActionResult Save(Employee model ,string birthDateInput ="",IFormFile? uploadPhoto = null)  // viết tường minh :  int customerID , string custormerName ,....
        {
            //Yêu cầu : Tên khách hàng , tên giao dịch ,Email và tỉnh thành không được để trống 
            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                ModelState.AddModelError(nameof(model.FullName), "Tên không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                ModelState.AddModelError(nameof(model.Phone), "Tên điện thoại không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), "Email không được để trống");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";
                return View("Edit", model);
            }




            //xử lý ngày sinh
            DateTime? date = birthDateInput.ToDateTime(); // mở rộng chức năng cho giá trị kiểu chuỗi  => this s
            if(date.HasValue)
            {
                model.BirthDate = date.Value;
            }
            //Xử lý ảnh upload : Nếu như có ảnh upload thì lưu ảnh lên server, gắn tên file ảnh đã lưu cho model.Photo
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; // Tên file sẽ lưu trên server $ tránh bị lưu file cùng tên 
                //đường dẫn đến file sẽ lưu trên server vd (: D:\MyWeb\wwwroot\images\employees\photo.png)
                string filePath = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\employees", fileName);

                //Lưu file lên server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                // Gán tên file ảnh cho model.Photo
                model.Photo = fileName;
            }
            if (model.EmployeeID == 0)
            {
                int id = CommonDataService.AddEmployee(model);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(model.Email), "Email bị trùng ");
                    ViewBag.Title = "Bổ sung nhân viên";
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateEmployee(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được nhân viên . Có thể email bị trùng");
                    ViewBag.Title = "Cập nhật nhân viên";
                    return View("Edit", model);
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
