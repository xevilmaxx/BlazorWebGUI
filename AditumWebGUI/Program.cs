using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AditumWebGUI
{
    public class Program
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            log.Info("-------------START-----------------------");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //IIS is useful for debugging / developments on local PC
                    webBuilder.UseStartup<Startup>();

                    //FOR KESTREL

                    //webBuilder.UseKestrel();
                    //webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    //webBuilder.UseIISIntegration();
                    //webBuilder.UseStartup<Startup>();
                    ////works good only with kestrel
                    //webBuilder.UseUrls("https://0.0.0.0:44367/");

                });
    }
}
