using SV20T1020078.DomainModels;

namespace SV20T1020078.Web.Models
{
    public class OrderDetailModel
    {
        public Order Order { get; set; }
        public List<OrderDetail> Details { get; set; }
        public int OrderID { get; set; }
        public int shipperID { get; set; }
        public int employeeID { get; set; }
        public int customerID { get; set; }
    }
}
