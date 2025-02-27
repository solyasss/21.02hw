using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace hw
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public IEnumerable<T> Query<T>(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.Query<T>(sql, parameters);
            }
        }

        public int Execute(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.Execute(sql, parameters);
            }
        }

        public T QueryFirstOrDefault<T>(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.QueryFirstOrDefault<T>(sql, parameters);
            }
        }
    }
}