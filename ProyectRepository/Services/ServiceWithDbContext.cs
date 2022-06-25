using System;
using System.Collections.Generic;

namespace ProyectRepository.Services
{
	public abstract class ServiceWithDbContext
	{

		protected private readonly DBContext _dbContext;

		public ServiceWithDbContext(DBContext dBContext)
		{
			_dbContext = dBContext ?? throw new ArgumentNullException(nameof(dBContext));
		}

	}
}
