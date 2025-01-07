using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Exceptions
{
	public class ErrorDetails
	{
		public int StatusCode { get; set; }
		public string Message { get; set; }
		public string? StackTrace { get; set; } // Sadece development ortamında gösterilecek

		public override string ToString()
		{
			return System.Text.Json.JsonSerializer.Serialize(this);
		}
	}
}
