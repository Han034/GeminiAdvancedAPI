using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Exceptions
{
	public class NotFoundException : ApiException
	{
		public NotFoundException(string message) : base(message, 404)
		{
		}
	}
}
