namespace SV20T1020078.Web.Models
{
    public class ProductSearchInput : PaginationSearchInput
    {
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
        public int ShipperID { get; set; } = 0;
        public int CustomerID { get; set; } = 0;
        public string Province { get; set; } = "";
        public decimal minPrice = 0;
        public decimal maxPrice = 0;
    }
}
