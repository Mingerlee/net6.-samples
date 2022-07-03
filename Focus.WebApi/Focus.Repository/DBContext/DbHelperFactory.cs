using Infrastructure.Config;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Focus.Repository.DBContext
{
    public class DbHelperFactory
    {
        private static IDbConnection _dbConnection = new SqlConnection(); 
        public static IDbConnection DbConnection { get {
                if (string.IsNullOrEmpty(_dbConnection.ConnectionString))
                    _dbConnection.ConnectionString = ConnectionString;
                return new ProfiledDbConnection((SqlConnection)_dbConnection, MiniProfiler.Current);
            } }

        private static string _connectionString;
        public static string ConnectionString { get=> _connectionString; set=> _connectionString=value; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DbHelper Create()
        {
            return new DbHelper();
        }
    }
}
