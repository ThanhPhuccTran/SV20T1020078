using SV20T1020078.DomainModels;
using Dapper;
using SV20T1020078.DataLayers.SQLServer;
using SV20T1020078.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020078.DataLayers
{
    public class SupplierDAL : _BaseDAL, ICommonDAL<Supplier>
    {
        public SupplierDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Supplier data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "")
        {
           int count = 0;
            if(!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue +"%";
            }
            //ketnoi KSDL
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*) from Suppliers 
                            where (@searchValue = N'') or (SupplierName like @searchValue)";
                var parameters = new { searchValue = searchValue ?? "" };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
           return count;
        }

        public bool Delete(Supplier data)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Supplier? Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsUsed(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Supplier> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Supplier> list = new List<Supplier>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%"; // tìm kiếm tương đối sql

            }
            //ket csdl 
            using (var connection = OpenConnection())
            {
                var sql = @"with cte as
                            (
	                            select	*, row_number() over (order by SupplierName) as RowNumber
	                            from	Suppliers 
	                            where	(@searchValue = N'') or (SupplierName like @searchValue)
                            )
                            select * from cte
                            where  (@pageSize = 0) 
                                   or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            order by RowNumber";
                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue ?? ""
                };
                list = connection.Query<Supplier>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();

            }
            return list;
        }

        public bool Update(Supplier data)
        {
            throw new NotImplementedException();
        }
    }
}
