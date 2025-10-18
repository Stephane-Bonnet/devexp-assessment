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
            app.MapPost("/webhooks", (HttpContext httpContext) =>
            {
                Console.WriteLine("Webhook received!");
                return Results.Ok();
            });

            app.Run();
        }
    }
}
