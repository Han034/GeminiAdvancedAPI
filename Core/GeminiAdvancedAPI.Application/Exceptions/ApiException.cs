using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Exceptions
{
	public class ApiException : Exception
	{
		public int StatusCode { get; set; }

		public ApiException(string message, int statusCode = 500) : base(message)
		{
			StatusCode = statusCode;
		}
	}
}
