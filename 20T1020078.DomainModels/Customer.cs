namespace SV20T1020078.DomainModels
{
    public class Customer
    {
        ///<summary>
        /// Khách hàng
        /// </summary>
        /// 
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = "";
        public string ContactName { get; set; } = "";
        public string Province { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsLocked { get; set; }


    }
}
