using System;
using System.Data;
using System.Data.SqlClient;

namespace ProyectRepository.Conections
{
	public class SqlConnectionFactory : IDbConnectionFactory
	{
		private readonly string _connectionString;

		public SqlConnectionFactory(string connectionString)
		{
			_connectionString = connectionString;
		}

		public IDbConnection CreateConnection()
		{
			var conn = new SqlConnection(_connectionString);
			return conn;
		}

		public IDbConnection CreateConnection(string connectionString)
		{
			var conn = new SqlConnection(connectionString);
			return conn;
		}

	}
}
