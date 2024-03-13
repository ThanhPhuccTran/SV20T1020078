using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020078.BusinessLayers;
using SV20T1020078.DomainModels;
using SV20T1020078.Web.Models;

namespace SV20T1020078.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        const int PAGE_SIZE = 20;
        const string PRODUCT_SEARCH = "product_search";
        public IActionResult Index()
        {
            /* int rowCount = 0;
             var data = ProductDataService.ListProducts(out rowCount, page, PAGE_SIZE, searchValue ?? "", 0, 0, 0, 0);
             var model = new Models.ProductSearchResult()
             {
                 Page = page,
                 PageSize = PAGE_SIZE,
                 SearchValue = searchValue ?? "",
                 RowCount = rowCount,
                 Data = data
             };
             return View(model);*/

            Models.ProductSearchInput? input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);
            if (input == null)
            {
                input = new ProductSearchInput
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                };
            }
            return View(input);
        }
        public IActionResult Search(ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "",input.CategoryID,input.SupplierID);
            var model = new Models.ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                categoryID = input.CategoryID,
                supplierID = input.SupplierID,
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);
            return View(model);

        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            ViewBag.IsEdit = false;
            var model = new Product()
            {
                ProductID = 0,
                Photo = "product_no_photo.png",
            };
            return View("Edit",model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Sửa mặt hàng";
            ViewBag.IsEdit = true;
            var model = ProductDataService.GetProduct(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrWhiteSpace(model.Photo))
            {
                model.Photo = "product_no_photo.png";
            }

            return View(model);
        }
        public IActionResult Save(Product model, IFormFile? uploadPhoto)
        {
            if (string.IsNullOrWhiteSpace(model.Photo))
            {
                model.Photo = "product_no_photo.png";
            }
            if (string.IsNullOrWhiteSpace(model.ProductName))
            {
                ModelState.AddModelError(nameof(model.ProductName), "Tên không được để trống");
            }
            if (model.CategoryID.ToString() == "0")
            {
                ModelState.AddModelError(nameof(model.CategoryID), "Loại sản phẩm không được để trống");
            }
            if (model.SupplierID.ToString() == "0")
            {
                ModelState.AddModelError(nameof(model.SupplierID), "Nhà cung cấp không được để trống");
            }
            List<Product> list
                = ProductDataService.ListProducts("");
            foreach (Product item in list)
            {
                if (model.ProductName == item.ProductName && model.ProductID != item.ProductID)
                {
                    ModelState.AddModelError(nameof(model.ProductName), $"Tên sản phẩm '{model.ProductName}' đã tồn tại.");
                    break;
                }
                
            }

            if (!ModelState.IsValid)
            {
                ViewBag.IsEdit = model.ProductID == 0 ? false : true;
                ViewBag.Title = model.ProductID == 0 ? "Bổ sung khách hàng" : "Cập nhật thông tin khách hàng";
                return View("Edit", model);
            }
            // xử lý ảnh upload:
            // Nếu có ảnh được upload thì lưu ảnh lên server, gán tên file ảnh đã lưu cho model.Photo
            if (uploadPhoto != null)
            {
                //string fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ff}_{uploadPhoto.FileName}";
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; // Tên file sẽ lưu trên server
                // đường dẫn đến file lưu trên server (ví dụ: D:\MyWeb\wwwroot\images\photo.png)
               // string filePath = Path.Combine(@"D:\laptrinhweb\SV20T1020390\SV20T1020390.web\wwwroot\images\employees", fileName);
                string filePath = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products", fileName);

                // lưu file lên Server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }

                // Gán tên file ảnh cho model.Photo
                model.Photo = fileName;
            }
            if (model.ProductID == 0)
            {
                int id = ProductDataService.AddProduct(model);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(model.ProductName), "Ten sản phẩm bị trùng ");
                    ViewBag.Title = "Bổ sung mặt hàng";
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = ProductDataService.UpdateProduct(model);
            }
            return RedirectToAction("Index");
        }
        public IActionResult SavePhoto(ProductPhoto model, IFormFile? uploadPhoto)
        {
            
            if (string.IsNullOrWhiteSpace(model.Description))
            {
                ModelState.AddModelError(nameof(model.Description), "Mô tả không được để trống");
            }
            if (model.DisplayOrder == 0)
            {
                ModelState.AddModelError(nameof(model.DisplayOrder), "Thứ tự hiển thị phải > 0 ");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.PhotoID == 0 ? "Bổ sung ảnh cho mặt hàng" : "Cập nhật ảnh của mặt hàng";
               /* if (string.IsNullOrWhiteSpace(model.Photo))
            {
                ModelState.AddModelError(nameof(model.Photo), model.Photo = "product_no_photo.png");
            }*/
                return View("Photo", model);
            }
            bool check = false;
            List<ProductPhoto> list = ProductDataService.ListPhotos(model.ProductID);
            foreach(var item in  list)
            {
                if(item.DisplayOrder == model.DisplayOrder && model.PhotoID != item.PhotoID)
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                ModelState.AddModelError(nameof(model.DisplayOrder), $"Thứ tự hiển thị {model.DisplayOrder} của hình ảnh đã có");
            }
            if (uploadPhoto != null)
            {
                //string fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ff}_{uploadPhoto.FileName}";
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; // Tên file sẽ lưu trên server
                                                                                  // đường dẫn đến file lưu trên server (ví dụ: D:\MyWeb\wwwroot\images\photo.png)
                                                                                  // string filePath = Path.Combine(@"D:\laptrinhweb\SV20T1020390\SV20T1020390.web\wwwroot\images\employees", fileName);
                string filePath = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products\productphotos", fileName);

                // lưu file lên Server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }

                // Gán tên file ảnh cho model.Photo
                model.Photo = fileName;
            }
           
            if (model.PhotoID == 0)
            {
                long id = ProductDataService.AddPhoto(model);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(model.Photo), "Ảnh bị trùng ");
                    ModelState.AddModelError(nameof(model.DisplayOrder), "Thứ tự hiển thị đã có ");
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return View("Photo", model);
                }

            }
            else
            {
                bool result = ProductDataService.UpdatePhoto(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được ảnh mặt hàng . Có thể ảnh bị trùng");
                    ViewBag.Title = "Cập nhật ảnh của mặt hàng";
                    return View("Photo", model);
                }

            }
            return RedirectToAction("Edit", new { id = model.ProductID });
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var model = ProductDataService.GetProduct(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Photo(int id , string method , int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    var  model = new ProductPhoto()
                    {
                        PhotoID = 0,
                        ProductID = id,
                    };
                    if (string.IsNullOrWhiteSpace(model.Photo))
                    {
                        model.Photo = "product_no_photo.png";
                    }
                    return View(model);
                case "edit":
                    ViewBag.Title = "Cập nhật ảnh của mặt hàng";
                    var model1 = ProductDataService.GetPhoto(photoId);
                    if (model1 == null)
                    {
                        return RedirectToAction("Index");
                    }
                    if (string.IsNullOrWhiteSpace(model1.Photo))
                    {
                        model1.Photo = "product_no_photo.png";
                    }
                    return View(model1);
                case "delete":
                    bool result = ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }

        }

        public IActionResult Attribute(int id, string method, int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    var model = new ProductAttribute()
                    {
                        AttributeID = 0,
                        ProductID = id,
                    };
                    return View(model);
                case "edit":
                    ViewBag.Title = "Cập nhật thuộc tính của mặt hàng";
                    var model1 = ProductDataService.GetAttribute(attributeId);
                    if (model1 == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(model1);
                case "delete":
                    //TODO : Xóa ảnh có mã photoId (xóa trực tiếp , không cần phải xác nhận ) 
                    bool result = ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult SaveAttribute(ProductAttribute model, IFormFile? uploadPhoto)
        {
            if (string.IsNullOrWhiteSpace(model.AttributeName))
            {
                ModelState.AddModelError(nameof(model.AttributeName), "Tên thuộc tính không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.AttributeValue))
            {
                ModelState.AddModelError(nameof(model.AttributeValue), "Giá trị thuộc tính không được để trống");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.AttributeID == 0 ? "Bổ sung ảnh cho mặt hàng" : "Cập nhật thuộc tính của mặt hàng";
                
                return View("Attribute", model);
            }
            if (model.AttributeID == 0)
            {
                long id = ProductDataService.AddAttribute(model);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(model.AttributeName), "Ảnh bị trùng ");
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return View("Attribute", model);
                }
            }
            else
            {
                bool result = ProductDataService.UpdateAttribute(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được ảnh mặt hàng . Có thể ảnh bị trùng");
                    ViewBag.Title = "Cập nhật thuộc tính của mặt hàng";
                    return View("Attribute", model);
                }
            }
            return RedirectToAction("Edit", new { id = model.ProductID });
        }
    }
}
