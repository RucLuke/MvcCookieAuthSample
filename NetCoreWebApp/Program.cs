using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
namespace NetCoreWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            CreateWebHostBuilder(args).Build().Run();

            //DiscoveryClient.GetAsync("");
            //var builder = new ConfigurationBuilder()
            //    .AddJsonFile("constant.json");
            //var configuration = builder.Build();
            //Console.WriteLine($"ClassNo{configuration["ClassNo"]}");
            //Console.WriteLine($"ClassNo{configuration["ClassDesc"]}");

            //Console.WriteLine($"ClassNo{configuration["Student:0:name"]}");
            //Console.WriteLine($"ClassNo{configuration["Student:0:age"]}");

            //Console.WriteLine($"ClassNo{configuration["Student:1:name"]}");
            //Console.WriteLine($"ClassNo{configuration["Student:1:age"]}");

            //var settings = new Dictionary<string, string>();
            //var builder = new ConfigurationBuilder()
            //    .AddInMemoryCollection(settings)
            //    .AddCommandLine(args);
            //var configuration = builder.Build();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("settings.json");
            })
                .UseStartup<Startup>();
    }
}
