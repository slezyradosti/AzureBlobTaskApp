using Microsoft.AspNetCore.Identity;
using WebApi;

namespace WebAapi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHost(args).Build();

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            await host.RunAsync();
        }

        public static IHostBuilder CreateHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:7177", "https://*:7172");
                    webBuilder.UseStartup<Startup>();
                });
    }
}