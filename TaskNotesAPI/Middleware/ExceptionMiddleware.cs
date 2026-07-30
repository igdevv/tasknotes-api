using System.Net;
using System.Text.Json;

namespace TaskNotesAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidOperationException exception)
            {
                await EscribirRespuestaAsync(
                    context,
                    HttpStatusCode.BadRequest,
                    exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Ocurrió un error inesperado.");

                await EscribirRespuestaAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Ocurrió un error interno en el servidor.");
            }
        }

        private static async Task EscribirRespuestaAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string mensaje)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var respuesta = new
            {
                status = context.Response.StatusCode,
                mensaje
            };

            var json = JsonSerializer.Serialize(respuesta);

            await context.Response.WriteAsync(json);
        }
    }
}
