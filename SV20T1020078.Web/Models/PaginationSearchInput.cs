namespace SV20T1020078.Web.Models
{
    /// <summary>
    /// Đầu vào tìm kiếm dữ liệu để nhận dữ liệu dưới dạng phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        public int Page { get; set; } 
        public int PageSize { get; set; } 
        public string SearchValue { get; set; } = "";

    }
    /// <summary>
    /// Lưu trữ thông tin mặt để tìm kiếm mặt hàng 
    /// Lọc mặt hàng theo loại , nhà cung cấp 
    /// Lọc theo giá 
    /// </summary>

    /*public class ProductSearchInput : PaginationSearchInput
    {
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
        
    }*/

    
}
