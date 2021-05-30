using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

namespace App.Server.Chat
{
    class Program
    {
        
        /// <summary>
        /// エントリーポイント
        /// </summary>
        static void Main(string[] args)
        {
            
            //ホストティングの設定
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    //appsettingsで設定を定義
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<Startup>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel(options =>
                        {
                            //Http2で動かす
                            options.ConfigureEndpointDefaults(endpointOptions =>
                            {
                                endpointOptions.Protocols = HttpProtocols.Http2;
                            });
                        })
                        .UseStartup<MagicOnionStartup>()
                        .UseUrls($"http://0.0.0.0:5000");
                })
                .UseConsoleLifetime();

            var chatHost = hostBuilder.Build();

            
            Task.WaitAll(chatHost.RunAsync());
        }
    }
}