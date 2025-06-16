using Bookify.Application.Abstractions.Data;
using Npgsql;
using System.Data;

namespace Bookify.Infrastructure.Data
{
    internal sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        public SqlConnectionFactory(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string ConnectionString { get; }
        public IDbConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
