using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SV20T1020078.BusinessLayers;
using SV20T1020078.DomainModels;
using SV20T1020078.Web.Models;

namespace SV20T1020078.Web.Controllers
{



    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")] //Có nghĩa khi truy cập Action Trong Controller , Kiểm tra xem đã đăng nhập , hay chưa đăng nhập
    public class OrderController : Controller
    {
        private const int ORDER_PAGE_SIZE = 20;
        private const string ORDER_SEARCH = "order_search";
        private const string SHOPPING_CART = "Shopping_Cart";
        private const string PRODUCT_SEARCH = "Product_Search";
        private const int PRODUCT_PAGE_SIZE = 5;
        public IActionResult Index()
        {
            OrderSearchInput? input = ApplicationContext.GetSessionData<OrderSearchInput>(ORDER_SEARCH);
            if (input == null)
            {
                input = new OrderSearchInput()
                {
                    Page = 1,
                    PageSize = ORDER_PAGE_SIZE,
                    SearchValue = "",
                    Status = 0,
                    DateRange = string.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", DateTime.Today.AddMonths(-1), DateTime.Today)
                   
                };
               
            }
            else
            {
                ApplicationContext.SetSessionData(ORDER_SEARCH, input);
               
            }
            return View(input);
        }  
        public IActionResult Search(OrderSearchInput input)
        {
            int rowCount = 0;
            var data = OrderDataService.ListOrders(out rowCount,input.Page,input.PageSize,input.Status,input.FromTime,input.ToTime,input.SearchValue??"");
           
            var model = new OrderSearchResult
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                Status = input.Status,
                TimeRange = input.DateRange ?? "",
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(ORDER_SEARCH, input);
            return View(model);
        }

        public IActionResult Details(int id = 0)
        {
            var order = OrderDataService.GetOrder(id);
            if(order == null)
            {
                return RedirectToAction("Index");

            }
            var details = OrderDataService.ListOrderDetails(id);
            var model = new OrderDetailModel()
            {
                Order = order,
                Details = details,

            };
            return View(model);
        }
        /// <summary>
        /// Giao diện trang lập đơn hàng mới 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);
            if(input == null)
            {
                input = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PRODUCT_PAGE_SIZE,
                    SearchValue = ""
                };


            }    

            return View(input);
        }
        /// <summary>
        /// Tìm kiếm mặt hàng để đưa vào giỏ hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IActionResult SearchProduct(ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount,input.Page,input.PageSize,input.SearchValue??"");
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);
            return View(model);
        }
        /// <summary>
        /// Lấy giỏ hàng hiện đang lưu trong session
        /// </summary>
        /// <returns></returns>
        private List<OrderDetail> GetShoppingCart()
        {
            //Giỏ hàng là danh sách các mặt hàng (OrderDetail) được chọn để bán trong đơn hàng 
            // và được lưu trong session 
            var shoppingCart = ApplicationContext.GetSessionData<List<OrderDetail>>(SHOPPING_CART);
            if(shoppingCart == null)
            {
                shoppingCart = new List<OrderDetail>();
                ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);

            }
            return shoppingCart;
        }

        /// <summary>
        /// Trang hiển thị danh sách các mặt hàng có trong giỏ hàng 
        /// </summary>
        /// <returns></returns>
        public IActionResult ShowShoppingCart()
        {
            var model = GetShoppingCart();
            return View(model); 
        }
        /// <summary>
        /// Bổ sung thêm mặt hàng vào giỏ hàng
        /// Hàm trả về chuỗi khác rỗng thông báo lỗi nếu dữ liệu không hợp lệ
        /// hàm trả về chuỗi rỗng nếu thành công
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IActionResult AddToCart(OrderDetail data)
        {
            if(data.SalePrice <=0 || data.Quantity <=0)
            {
                TempData["Message"] = "Giá bán và số lượng mua không hợp lệ ";
            }
            var shoppingCart = GetShoppingCart();
            var existProduct = shoppingCart.FirstOrDefault(m=>m.ProductID == data.ProductID);
            if(existProduct == null) // Nếu mặt hàng chưa có trong giỏ hàng thì bổ sung thêm vào giỏ hàng
            {
                shoppingCart.Add(data);
            }    
            else
            {
                existProduct.Quantity += data.Quantity;
                existProduct.SalePrice = data.SalePrice;

            }
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");

        }
        /// <summary>
        /// Xóa mặt hàng ra khỏi giỏ hàng 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public IActionResult RemoveFromCart(int id = 0)
        {
            var shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m=>m.ProductID == id);
            if(index >= 0)
            {
                shoppingCart.RemoveAt(index);
            }
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }

        /// <summary>
        /// Xóa tất cả mặt hàng trong giỏ hàng
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearCart()
        {
            var shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }

        public IActionResult Init(int customerID = 0 , string Province = "",string deliveryAddress = "")
        {
            var shoppingCart = GetShoppingCart();
            if(shoppingCart.Count == 0)
            {
                TempData["Message"] = "Không thể tạo đơn hàng với giỏ hàng trống";
                return RedirectToAction("Create");
            }
            if(customerID<=0 || string.IsNullOrWhiteSpace(deliveryAddress) || string.IsNullOrWhiteSpace(Province))
            {
                TempData["Message"] = "Vui lòng nhập đầy đủ thông tin";
                return RedirectToAction("Create");
            }
            int employeeID = Convert.ToInt32(User.GetUserData()?.UserId);
            int orderID = OrderDataService.InitOrder(employeeID, customerID, Province, deliveryAddress, shoppingCart);
            ClearCart();
            return RedirectToAction("Details", new { id = orderID });
        }



        [HttpGet]
        public IActionResult EditDetail(int id = 0 , int productID = 0)
        {   
            var model = OrderDataService.GetOrderDetail(id, productID);
            return View(model);
        }
        [HttpGet]
        public IActionResult EditAddress(int id = 0)
        {
            var model = OrderDataService.GetOrder(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateAddress(Order data)
        {
            if (Request.Method == "POST")
            {
                if (data.OrderID != 0)
                {
                    bool result = OrderDataService.UpdateAddress(data);
                    if (!result)
                    {
                        return Json("Sửa địa chỉ không thành công");
                    }
                }

            }
            return Json("");
        }
        [HttpPost]
        public IActionResult UpdateDetail(OrderDetail data)
        {
            Order od = OrderDataService.GetOrder(data.OrderID);
            ViewBag.KQ = od.Status;
            if (data.Quantity <= 0)
            {
                return Json("Vui lòng nhập số lượng > 0 ");
            }
            if(data.SalePrice <= 0)
            {
                return Json("Vui lòng nhập số tiền hợp lý ");
            }
            OrderDataService.SaveOrderDetail(data.OrderID, data.ProductID, data.Quantity, data.SalePrice);
            return Json("");
            
           /* return View("Details" , new {id= data.OrderID});*/
        }
        public IActionResult DeleteDetail(int id = 0 , int productID = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            if (productID < 0)
            {
                return RedirectToAction("Details", new { id = id });
            }
            bool result = OrderDataService.DeleteOrderDetail(id, productID);
            if (!result)
            {
                TempData["Message"] = "Không thể xóa mặt hàng này ra khỏi đơn hàng";
                return RedirectToAction("Details", new { id = id });
            }
            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// Chuyển đơn hàng sang trạng thái đã được duyệt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Accept(int id = 0)
        {
            bool result = OrderDataService.AcceptOrder(id);
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            if (!result)
            {
                TempData["Message"] = "Không thể duyệt đơn hàng này ";
                return RedirectToAction("Details", new { id = data.OrderID });
            }
            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// Chuyển đơn hàng sang trạng thái đã kết thúc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Finish(int id = 0)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            bool result = OrderDataService.FinishOrder(id);
            if (!result)
            {
                TempData["Message"] = "Không thể ghi nhận trạng thái kết thúc cho đơn hàng này ";

            }
            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// Chuyển sang đơn hàng bị hủy
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Cancel(int id = 0) 
        { 
            bool result = OrderDataService.CancelOrder(id);
            if (!result)
            {
                TempData["Message"] = "Không thể thực hiện thao tác hủy đối với đơn hàng này ";

            }
            return RedirectToAction("Details", new { id = id });

        }

        public IActionResult Reject(int id = 0)
        {
            bool result = OrderDataService.RejectOrder(id);
            if(!result)
            {
                TempData["Message"] = "Không thể thực hiện thao tác từ chối đối với đơn hàng này ";
            }
            return RedirectToAction("Details", new { id = id });
        }


        public IActionResult Delete (int id)
        {
            bool result = OrderDataService.DeleteOrder(id);
            if (!result)
            {
                TempData["Message"] = "Không thể xóa đơn hàng này ";
                
            }
            return RedirectToAction("Details", new { id = id });
        }

        /// <summary>
        /// Giao diện để chọn ra người giao hàng cho đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Shipping(int id = 0)
        {
            ViewBag.OrderID = id;
            return View();
        }
        /// <summary>
        /// Ghi nhận người giao hàng cho đơn hàng và chuyển đơn hàng sang trạng thái đang giao hàng.
        /// Hàm trả về chuỗi khác rỗng thông báo nếu đầu vào không hợp lệ hoặc lỗi,
        /// hàm trả về chuỗi rỗng nếu thành công 
        /// </summary>
        /// <param name="id">Mã đơn hàng </param>
        /// <param name="shipperID">Mã người giao hàng</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Shipping(int id = 0 , int shipperID = 0)
        {
            if(shipperID < 0)
            {
                return Json("Vui lòng chọn người giao hàng");

            }
            bool result = OrderDataService.ShipOrder(id, shipperID);
            if(!result)
            {
                return Json("Đơn hàng không cho phép chuyển người giao hàng");
            }
            return Json("");
        }


    }
}
