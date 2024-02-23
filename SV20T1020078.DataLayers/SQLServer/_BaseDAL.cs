using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SV20T1020078.DataLayers.SQLServer
{
    //abstract là gì ??? 
    public class _BaseDAL
    {
        protected string _connectionString = "";
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public _BaseDAL(string connectionString) 
        {
            _connectionString = connectionString;
        }
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();

            return connection;
        }
    }
}
