using SV20T1020078.DomainModels;
using SV20T1020078.DataLayers.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace SV20T1020078.DataLayers
{
    public class ProvinceDAL : _BaseDAL, ICommonDAL<Province>
    {
        public ProvinceDAL(string connetionString) : base(connetionString)
        {
        }

        public int Add(Province data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Province data)
        {
            throw new NotImplementedException();
        }

        public Province? Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsUsed(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Province> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Province> list = new List<Province>();
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Provinces";
                list = connection.Query<Province>(sql:sql, commandType : CommandType.Text).ToList();
                connection.Close();
            }
            return list;
        }

        public bool Update(Province data)
        {
            throw new NotImplementedException();
        }

       
    }
}
