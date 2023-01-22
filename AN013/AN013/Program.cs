namespace AN013
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                // Do work that can write to the Response.
                await context.Response.WriteAsync(" 執行此中介軟體前 ");
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
                await context.Response.WriteAsync(" 執行此中介軟體後 ");
            });

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}