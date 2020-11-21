//#define USE_CUSTOM_KESTREL_PORT
//-----------------------------------------------------------------------------
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Server;

await CreateHostBuilder(args).RunAsync();
//-----------------------------------------------------------------------------
static IHost CreateHostBuilder(string[] args)
    => Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder
#if USE_CUSTOM_KESTREL_PORT
                .ConfigureKestrel(options =>
                {
                    options.Limits.MinRequestBodyDataRate = null;
                    options.ListenLocalhost(50051, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http2;
                    });
                })
#endif
                .UseStartup<Startup>();
        })
        .Build();
