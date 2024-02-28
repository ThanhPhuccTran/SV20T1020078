using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020078.DomainModels
{
    public class Shipper
    {
        /// <summary>
        /// Người giao hàng
        /// </summary>
        public int SupplierID { get; set; }
        public string ShipperName { get; set; } = "";
        public string Phone { get; set; } = "";

    }
}
