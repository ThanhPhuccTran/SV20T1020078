using Microsoft.AspNetCore.Mvc.Rendering;
using SV20T1020078.BusinessLayers;

namespace SV20T1020078.Web
{
    public static class SelectListHelper
    {
        /// <summary>
        /// Danh sách tỉnh / thành
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Pronvinces()
        {
            List<SelectListItem> list   = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn Tỉnh/Thành --",

            });
            foreach (var item in CommonDataService.ListOfProvinces())
            {
                
                list.Add(new SelectListItem()
                {
                    Value = item.ProvinceName,
                    Text = item.ProvinceName,

                });
            }
            return list;

        }
        public static List<SelectListItem> Categoryss()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "-- Tất cả loại hàng --",

            });
            foreach (var item in CommonDataService.ListOfCategoryName())
            {

                list.Add(new SelectListItem()
                {
                    Value = item.CategoryID.ToString(),
                    Text = item.CategoryName,

                });
            }
            return list;
        }

        public static List<SelectListItem> Supplierss()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "-- Nhà cung cấp --",

            });
            foreach (var item in CommonDataService.ListOfSupplierName())
            {

                list.Add(new SelectListItem()
                {
                    Value = item.SupplierID.ToString(),
                    Text = item.SupplierName,

                });
            }
            return list;
        }

        public static List<SelectListItem> Shippers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "-- Chọn người giao hàng --",

            });
            foreach (var item in CommonDataService.ListOfShipperName())
            {

                list.Add(new SelectListItem()
                {
                    Value = item.ShipperID.ToString(),
                    Text = item.ShipperName,

                });
            }
            return list;
        }
        public static List<SelectListItem> Customers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "-- Chọn người giao hàng --",

            });
            foreach (var item in CommonDataService.ListOfCustomerName())
            {

                list.Add(new SelectListItem()
                {
                    Value = item.CustomerID.ToString(),
                    Text = item.CustomerName,

                });
            }
            return list;
        }

    }
}
