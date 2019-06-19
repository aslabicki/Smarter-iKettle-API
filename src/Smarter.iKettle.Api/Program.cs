using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace Smarter.iKettle.Api
{
    public class Program
    {
        private static readonly IConfiguration defaultConfiguration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>()
                .Build();

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                         .ReadFrom.Configuration(defaultConfiguration)
                         .Enrich.FromLogContext()
                         .WriteTo.Console()
                         .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration((hostingContext, config) =>
                   {
                       config.AddConfiguration(defaultConfiguration);
                   })
                   .UseStartup<Startup>()
                   .UseSerilog();
    }
}