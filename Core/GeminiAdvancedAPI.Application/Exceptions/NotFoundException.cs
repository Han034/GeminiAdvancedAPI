using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound) // 404 yerine HttpStatusCode.NotFound
        {
        }
        public NotFoundException(string entityName, object key)
           : base($"{entityName} with id '{key}' was not found", HttpStatusCode.NotFound)
        {
        }
    }
}
