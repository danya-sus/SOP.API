
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace SOP.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting SOP.API...");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => {
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.ConfigureKestrel(options => {
                        options.ListenAnyIP(5000, listenOptions => listenOptions.Protocols = HttpProtocols.Http1AndHttp2);
                        options.AllowSynchronousIO = true;
                    });
                    webBuilder.UseStartup<Startup>();
                }
            );

    }
}

/*
1) добавить сущность "владелец автомобиля"
2) расширить репозиторий
3) добавить контроллер владельца автомобиля
4) сделать отдачу данных hypermedia (по примеру vehicle)
*/