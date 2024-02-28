using SV20T1020078.DomainModels;
using SV20T1020078.DataLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020078.BusinessLayers
{
    /// <summary>
    /// Cung cấp các chức năng nghiệp vụ liên quan đến các dữ liệu "chung"
    /// (tỉnh / thành , khách hàng , nhà cung cấp , loại hàng , người giao hàng , nhân viên ) 
    /// </summary>
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Province> provinceDB;
        private static readonly ICommonDAL<Customer> customerDB;

        private static readonly ICommonDAL<Supplier> supplierDB;
        /// <summary>
        /// Ctor ( Câu hỏi static constructor hoạt động như thế nào ? cách viết ? )
        /// </summary>
        static CommonDataService()
        {
            string connectionString = Configuration.ConnectionString;
            provinceDB = new ProvinceDAL(connectionString);
            customerDB = new CustomerDAL(connectionString);
            supplierDB = new SupplierDAL(connectionString);
        }
        public static List<Province> ListOfCustomers() 
        {
            return provinceDB.List().ToList();
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách
        /// </summary>
        /// <param name="rowCount">Tham số đầu ra cho biết số dòng dữ liệu tìm được </param>
        /// <param name="page">Trang cần hiển thị </param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 nếu không phân trang ) </param>
        /// <param name="searchValue">Gía trị tìm kiếm (rỗng nếu toàn bộ khách hàng ) </param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1,int pageSize = 0 , string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page,pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin của một khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Customer? GetCustomer(int id)
        {
            return customerDB.Get(id);
        }
        /// <summary>
        /// Bổ sung 1 khách hàng mới . Hàm trả về mã của khách hàng mới được bổ sung 
        /// (Hàm trả về -1 nếu email bị trùng , trả về giá trị 0 nếu lỗi)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer data)
        {
            return customerDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin khách hàng 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return customerDB.Update(data);
        }
        /// <summary>
        /// Xóa 1 khách hàng (nếu khách hàng đó dữ liệu liên quan) (bảng Order)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int id)
        {
            if(customerDB.IsUsed(id))
            {
                return false;
            }
            return customerDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem 1 khách hàng hiện có dữ liệu liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCustomer(int id) 
        { 
            return customerDB.IsUsed(id);
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách Nhà Cung Cấp
        /// </summary>
        /// <param name="rowCount">Tham số đầu ra cho biết số dòng dữ liệu tìm được </param>
        /// <param name="page">Trang cần hiển thị </param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 nếu không phân trang ) </param>
        /// <param name="searchValue">Gía trị tìm kiếm (rỗng nếu toàn bộ nhà cung cấp ) </param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(out int rowCount,int page = 1 , int pageSize = 0 , string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page,pageSize,searchValue).ToList();
        }
        
    }
}
