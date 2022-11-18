using Newtonsoft.Json;
using System.Net;

namespace TwitterStatistics.Exceptions
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            if (exception != null)
            {
                var response = context.Response;
                //response.ContentType = "application/json";
                if (typeof(DataValidationException) == exception.GetType())
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(((DataValidationException)exception).ValidationDescriptions));
                }
                else
                {
                    _logger.LogError(exception, $"Unhandled exception encountered: {exception.GetBaseException().Message}");
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { Details = exception.InnerException?.Message ?? exception.Message }));

                }
            }

        }
    }
}
