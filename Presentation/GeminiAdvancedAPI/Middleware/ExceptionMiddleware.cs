using GeminiAdvancedAPI.Application.Exceptions;
using System.Net;

namespace GeminiAdvancedAPI.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                var statusCode = (int)HttpStatusCode.InternalServerError;
                var message = "Internal Server Error";

                switch (ex)
                {
                    case NotFoundException notFoundException:
                        statusCode = notFoundException.StatusCode;
                        message = notFoundException.Message;
                        break;
                    case ProductNotFoundException productNotFoundException:
                        statusCode = (int)HttpStatusCode.NotFound; // 404
                        message = productNotFoundException.Message;
                        break;
                    case CartNotFoundException cartNotFoundException:
                        statusCode = (int)HttpStatusCode.NotFound; // 404
                        message = cartNotFoundException.Message;
                        break;
                    case CartItemNotFoundException cartItemNotFoundException:
                        statusCode = (int)HttpStatusCode.NotFound; // 404
                        message = cartItemNotFoundException.Message;
                        break;
                    case BadRequestException badRequestException:
                        statusCode = (int)HttpStatusCode.BadRequest; //400
                        message = badRequestException.Message;
                        break;
                    // Diğer özel exception'lar buraya eklenebilir
                    default:
                        statusCode = (int)HttpStatusCode.InternalServerError; // 500
                        if (_env.IsDevelopment())
                        {
                            message = ex.Message;
                        }
                        break;
                }

                var response = _env.IsDevelopment()
                    ? new ErrorDetails { StatusCode = statusCode, Message = message, StackTrace = ex.StackTrace }
                    : new ErrorDetails { StatusCode = statusCode, Message = message };

                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsync(response.ToString());
            }
        }
    }
}
