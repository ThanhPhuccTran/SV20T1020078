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
    }
}
