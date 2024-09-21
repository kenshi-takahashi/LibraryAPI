using System.Net;
using System.Text.Json;

namespace LibraryAPI.WebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case ArgumentNullException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return WriteResponseAsync(context, "A required argument was null.", exception.Message);

                case ArgumentException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return WriteResponseAsync(context, "An argument was invalid.", exception.Message);

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return WriteResponseAsync(context, "Resource not found.", exception.Message);

                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return WriteResponseAsync(context, "Access denied.", exception.Message);

                case InvalidOperationException:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return WriteResponseAsync(context, "Invalid operation.", exception.Message);

                case TimeoutException:
                    context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    return WriteResponseAsync(context, "The request timed out.", exception.Message);

                case FormatException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return WriteResponseAsync(context, "The format of the input was invalid.", exception.Message);

                case InvalidCastException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return WriteResponseAsync(context, "An invalid cast occurred.", exception.Message);

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return WriteResponseAsync(context, "Internal server error. Please try again later.", exception.Message);
            }
        }

        private Task WriteResponseAsync(HttpContext context, string message, string detailedMessage)
        {
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                // Detailed = detailedMessage
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}