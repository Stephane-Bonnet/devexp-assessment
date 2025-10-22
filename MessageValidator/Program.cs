namespace MessageValidator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseKestrel(options => options.ListenAnyIP(3010));
            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseAuthorization();
            app.MapPost("/webhooks", async (HttpContext httpContext) =>
            {
                Console.WriteLine("Webhook received");

                var authorizationHeader = httpContext.Request.Headers.Authorization.ToString();

                if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Signature "))
                {
                    Console.WriteLine("Missing or invalid Authorization header");
                    return Results.BadRequest();
                }

                var splitHeader = authorizationHeader.Split(' ');
                if (splitHeader.Length != 2)
                {
                    Console.WriteLine("Invalid Authorization header format");
                    return Results.BadRequest();
                }

                var signature = splitHeader.Last();
                var message = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
                if (DevexpAssessment.Security.SignatureValidator.Validate(message, signature))
                {
                    Console.WriteLine("Signature validated!");
                    return Results.Ok();
                }

                return Results.BadRequest();
            });

            app.Run();
        }
    }
}
