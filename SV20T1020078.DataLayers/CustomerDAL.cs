﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using SV20T1020078.DomainModels;
using SV20T1020078.DataLayers.SQLServer;

namespace SV20T1020078.DataLayers
{
    public class CustomerDAL : _BaseDAL, ICommonDAL<Customer>
    {
        public CustomerDAL(string connetionString) : base(connetionString)
        {
        }

        public int Add(Customer data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "")
        {
            int count = 0;


            if(!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%"; // tìm kiếm dữ liệu đếm được 
            using (var connection = OpenConnection())
            {
                //Ctr K + D
                var sql = @"select count(*) from Customers 
                            where (@searchValue = N'') or (CustomerName like @searchValue)";
                var parameters = new { searchValue = searchValue ?? "" };
                count = connection.ExecuteScalar<int>(sql: sql,param:parameters,commandType: CommandType.Text);
                connection.Close();

            }
            return count;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Customer data)
        {
            throw new NotImplementedException();
        }

        public Customer? Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsUsed(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Customer> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Customer> list = new List<Customer>();

            if(!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%"; // tìm kiếm tương đối sql

            }

            //ket csdl 
            using (var connection = OpenConnection())
            {
                var sql = @"with cte as
                            (
	                            select	*, row_number() over (order by CustomerName) as RowNumber
	                            from	Customers 
	                            where	(@searchValue = N'') or (CustomerName like @searchValue)
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
                list = connection.Query<Customer>(sql : sql,param : parameters ,commandType : CommandType.Text).ToList();
                connection.Close();
                    
            }
            return list;
        }

        public bool Update(Customer data)
        {
            throw new NotImplementedException();
        }

        public bool Updater(Customer data)
        {
            throw new NotImplementedException();
        }
    }
}