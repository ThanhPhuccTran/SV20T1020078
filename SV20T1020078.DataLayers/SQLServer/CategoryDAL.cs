using SV20T1020078.DomainModels;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Update(Category data)
        {
            throw new NotImplementedException();
        }
    }
}
