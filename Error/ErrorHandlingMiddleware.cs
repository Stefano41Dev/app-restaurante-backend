namespace app_restaurante_backend.Error
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
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
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var errorDto = new ErrorResponseDTO
                {
                    Message = ex.Message,
                    Timestamp = DateTime.UtcNow,
                };

                await context.Response.WriteAsJsonAsync(errorDto);
            }
        }
    }
}
