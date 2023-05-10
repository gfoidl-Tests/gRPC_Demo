//#define USE_CUSTOM_KESTREL_PORT
//-----------------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server;
using Server.Math;

#if USE_CUSTOM_KESTREL_PORT
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
#endif

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

builder.Services.AddSingleton<IMathService, MathService>();
builder.Services.AddMediator();
builder.Services.AddGrpc();

#if USE_CUSTOM_KESTREL_PORT
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MinRequestBodyDataRate = null;
    options.ListenLocalhost(50051, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});
#endif

WebApplication app = builder.Build();

app.MapGrpcService<CalcService>();

await app.RunAsync();
