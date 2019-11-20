using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace ExpenseTracker.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /* Serilog Related Info For Future Use
             * All Sinks
             https://github.com/serilog/serilog/wiki/Provided-Sinks
             * Enricher Advice
             https://github.com/nblumhardt/serilog-enrichers-demystify
             * Retrace (probably we'll use this later
             https://stackify.com/retrace/
             */
            Environment.SetEnvironmentVariable("BASEDIR", AppDomain.CurrentDomain.BaseDirectory);

            // Create the logger
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                // Source: https://stackoverflow.com/questions/28292601/serilog-multiple-log-files
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(f => f.Level == Serilog.Events.LogEventLevel.Debug | f.Level == Serilog.Events.LogEventLevel.Information)
                    .WriteTo.File(Environment.GetEnvironmentVariable("BASEDIR") + "logs/log_DebugInfo_.txt",
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        fileSizeLimitBytes: 10485760)
                )
                .WriteTo.Logger(l => l.Filter.ByExcluding(f => f.Level == Serilog.Events.LogEventLevel.Debug | f.Level == Serilog.Events.LogEventLevel.Information)
                    .WriteTo.File(Environment.GetEnvironmentVariable("BASEDIR") + "logs/log_.txt",
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        fileSizeLimitBytes: 10485760)
                )
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
