﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020078.BusinessLayers;
using SV20T1020078.DomainModels;
using SV20T1020078.Web.Models;

namespace SV20T1020078.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")] //Có nghĩa khi truy cập Action Trong Controller , Kiểm tra xem đã đăng nhập , hay chưa đăng nhập
    public class ShipperController : Controller
    {
        const int PAGE_SIZE = 20;
        const string SHIPPER_SEARCH = "shipper_search";
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            /*int rowCount = 0;
            var data = CommonDataService.ListOfShippers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            var model = new Models.ShipperSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data

            };
            return View(model);*/
            Models.PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SHIPPER_SEARCH);
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
            var data = CommonDataService.ListOfShippers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new ShipperSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiênn tim kiếm
            ApplicationContext.SetSessionData(SHIPPER_SEARCH, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung người giao hàng";
            var model = new Shipper()
            {
                ShipperID = 0
            };
            // dùng chung giao diện với Edit
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Sửa thông tin người giao hàng";
            var model = CommonDataService.GetShipper(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
        [HttpPost] //Attribute (chỉ nhận dữ liệu gửi lên dưới dạng là POST)
        public IActionResult Save(Shipper model)  // viết tường minh :  int customerID , string custormerName ,....
        {
            if (string.IsNullOrWhiteSpace(model.ShipperName))
            {
                ModelState.AddModelError(nameof(model.ShipperName), "Tên không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                ModelState.AddModelError(nameof(model.Phone), "Số điện thoại không được để trống");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ShipperID == 0 ? "Bổ sung người giao hàng" : "Cập nhật thông tin người giao hàng";
                return View("Edit", model);
            }
            if (model.ShipperID == 0)
            {
                int id = CommonDataService.AddShipper(model);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(model.Phone), "SĐT bị trùng ");
                    ViewBag.Title = "Bổ sung khách hàng";
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateShipper(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được khách hàng . Có thể email bị trùng");
                    ViewBag.Title = "Cập nhật khách hàng";
                    return View("Edit", model);
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetShipper(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
