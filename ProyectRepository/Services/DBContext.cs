using ProyectRepository.Conections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectRepository.Services
{
    public class DBContext : IDisposable
    {
		private protected IDbConnection _connection;
		private protected IDbTransaction _transaction;
		private protected IDbConnectionFactory _dbConnectionFactory;
		public event EventHandler TransactionChangedEventHandler;
		protected void OnTransactionChanged() => TransactionChangedEventHandler?.Invoke(this, null);
		private protected bool _hasFactory => _dbConnectionFactory != null;

		private protected int _pendingTransactions = 0;

		/// <summary>
		/// Se utiliza automáticamente el IDbConnectionFactory especificado por la AppConfig para crear conexión
		/// </summary>
		public DBContext() : this(AppConfig.Instance.DbConnectionFactory) { }

		/// <summary>
		/// Se utiliza el IDbConnectionFactory especificado para crear conexión
		/// </summary>
		public DBContext(IDbConnectionFactory dbConnectionFactory)
		{
			_dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
			_connection = dbConnectionFactory.CreateConnection();
		}

		/// <summary>
		/// Se utilizan conexión y transacción existentes
		/// </summary>
		public DBContext(IDbConnection connection, IDbTransaction transaction)
		{
			_connection = connection ?? throw new ArgumentNullException(nameof(connection));
			_transaction = transaction;
		}

		public IDbConnection Connection
		{
			get
			{
				return _connection;
			}
		}

		public IDbTransaction Transaction
		{
			get
			{
				return _transaction;
			}
		}

		public void BeginTransaction()
		{
			if (_pendingTransactions == 0)
			{
				if (_connection.State == ConnectionState.Closed)
				{
					_connection.Open();
				}
				_transaction = _connection.BeginTransaction();
				OnTransactionChanged();
			}

			_pendingTransactions++;
		}

		public void CommitTransaction()
		{
			_pendingTransactions--;
			if (_pendingTransactions == 0)
			{
				_transaction.Commit();
			}
		}

		public void Dispose()
		{
			if (_hasFactory && _connection != null)
			{
				if (_transaction != null)
				{
					_transaction.Dispose();
					_transaction = null;
				}

				_connection.Dispose();
				_connection = null;
			}
		}
	
	}
}
