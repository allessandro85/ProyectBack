using System;
using System.Collections.Generic;

namespace ProyectIO
{
	public class BusinessException : Exception
	{
		public BusinessException() { }
		public BusinessException(string message) : base(message) { }
		public BusinessException(string message, Exception innerException) : base(message, innerException) { }
	}
}
