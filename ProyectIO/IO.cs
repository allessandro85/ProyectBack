using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectIO
{
    public abstract class IO
    {
		protected private readonly IDbConnection _connection;
		protected private IDbTransaction _transaction;
		public void SetTransaction(IDbTransaction dbTransaction) => _transaction = dbTransaction;

		protected IO(IDbConnection connection, IDbTransaction transaction)
		{
			_connection = connection;
			_transaction = transaction;
		}

	}
}
