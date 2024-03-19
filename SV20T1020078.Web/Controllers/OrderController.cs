using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020078.BusinessLayers;
using SV20T1020078.DomainModels;
using SV20T1020078.Web.Models;

namespace SV20T1020078.Web.Controllers
{
   


    [Authorize] //Có nghĩa khi truy cập Action Trong Controller , Kiểm tra xem đã đăng nhập , hay chưa đăng nhập
    public class OrderController : Controller
    {
        private const int ORDER_PAGE_SIZE = 20;
        private const string ORDER_SEARCH = "order_search";
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public IActionResult EditDetail(int id = 0 , int productID = 0)
        {   
            var model = OrderDataService.GetOrderDetail(id, productID);
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateDetail(OrderDetail data)
        {
            Order od = OrderDataService.GetOrder(data.OrderID);
            ViewBag.KQ = od.Status;
            if (Request.Method == "POST")
            {   
                OrderDataService.SaveOrderDetail(data.OrderID, data.ProductID, data.Quantity, data.SalePrice);
                return RedirectToAction("Details", new { id = data.OrderID });
            }
            return View("Details" , new {id= data.OrderID});
        }
        public  IActionResult Shipping(int id = 0)
        {
            return View();
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




    }
}
