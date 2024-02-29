﻿using Dapper;
using SV20T1020078.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020078.DataLayers.SQLServer
{
    public class CategoryDAL : _BaseDAL, ICommonDAL<Category>
    {
        public CategoryDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Category data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "")
        {
            int count = 0;


            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%"; // tìm kiếm dữ liệu đếm được 
            // kết nối đến CSDL
            using (var connection = OpenConnection())
            {
                //Ctr K + D
                var sql = @"select count(*) from Categories 
                            where (@searchValue = N'') or (CategoryName like @searchValue)";
                var parameters = new { searchValue = searchValue ?? "" };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();

            }
            return count;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Category? Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsUsed(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Category> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Category> list = new List<Category>();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%"; // tìm kiếm tương đối sql

            }

            //ket csdl 
            using (var connection = OpenConnection())
            {
                var sql = @"with cte as
                            (
	                            select	*, row_number() over (order by CategoryName) as RowNumber
	                            from	Categories 
	                            where	(@searchValue = N'') or (CategoryName like @searchValue)
                            )
                            select * from cte
                            where  (@pageSize = 0) 
                                   or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            order by RowNumber";
                //gán giá trị cho tham siis
                var parameters = new
                {
                    page,
                    pageSize,
                    searchValue = searchValue ?? ""
                };
                list = connection.Query<Category>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();

            }
            return list;
        }

        public bool Update(Category data)
        {
            throw new NotImplementedException();
        }
    }
}
