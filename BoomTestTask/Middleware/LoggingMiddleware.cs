namespace CustodialWalletAPI.Middleware
{
    public class LoggingMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public LoggingMiddleware(RequestDelegate next,IServiceProvider serviceProvider)
        {
            _next=next;
            _serviceProvider=serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<LoggingMiddleware>>();

            logger.LogInformation(
                "Incoming request: {Method} {Path}", 
                context.Request.Method, 
                context.Request.Path);

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex, 
                    "An error occurred while processing the request: {Message}",
                    ex.Message);

                throw;
            }
            finally
            {
                // Логируем информацию о статусе ответа
                logger.LogInformation("Outgoing response: {StatusCode}", context.Response.StatusCode);
            }
        }
    }
}
