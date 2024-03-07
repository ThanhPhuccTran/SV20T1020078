using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SV20T1020078.BusinessLayers;
using SV20T1020078.DomainModels;
using System.Buffers;

namespace SV20T1020078.Web.Controllers
{
    public class EmployeeController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            var model = new Models.EmployeeSearchResult()
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
            }
            else
            {
                bool result = CommonDataService.UpdateEmployee(model);
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
