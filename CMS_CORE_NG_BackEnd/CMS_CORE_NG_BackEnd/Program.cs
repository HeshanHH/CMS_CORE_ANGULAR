using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_CORE_NG_BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // logging to catche errors even app is  building.
            // by creating scope we can consiume the UseSerilog define on CreateHostBuilder.
            using (var scope = host.Services.CreateScope())
            {

            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog((hostingContext, loggingConfiguration) => loggingConfiguration
                    .Enrich.FromLogContext()
                    // printing usefull informations. (ex: if we want to kill a sertaing task  we can use ProcessId)
                      .Enrich.WithProperty("Application", "CMS_CORE_NG")
                      .Enrich.WithProperty("MachineName", Environment.MachineName)
                      .Enrich.WithProperty("CurrentManagedThreadId", Environment.CurrentManagedThreadId)
                      .Enrich.WithProperty("OSVersion", Environment.OSVersion)
                      .Enrich.WithProperty("Version", Environment.Version)
                      .Enrich.WithProperty("UserName", Environment.UserName)
                      .Enrich.WithProperty("ProcessId", Process.GetCurrentProcess().Id)
                      .Enrich.WithProperty("ProcessName", Process.GetCurrentProcess().ProcessName)
                      //.WriteTo.Console(theme: CustomConsoleTheme.VisualStudioMacLight)
                      .WriteTo.File(//formatter: new CustomTextFormatter(), 
                      path: Path.Combine(hostingContext.HostingEnvironment.ContentRootPath + $"{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}", $"cms_core_ng_{DateTime.Now:yyyyMMdd}.txt"))
                     .ReadFrom.Configuration(hostingContext.Configuration));
                    webBuilder.UseStartup<Startup>();
                });
    }
}
