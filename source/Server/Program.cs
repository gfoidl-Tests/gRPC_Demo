//#define USE_CUSTOM_KESTREL_PORT
#define USE_UNIX_DOMAIN_SOCKETS
//-----------------------------------------------------------------------------
using Server;
using Server.Math;

#if USE_CUSTOM_KESTREL_PORT
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
#endif

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
#elif USE_UNIX_DOMAIN_SOCKETS
// Cf. https://learn.microsoft.com/en-us/aspnet/core/grpc/interprocess-uds

string socketsPath = Path.Combine(Path.GetTempPath(), "gRPC-Test.sock");
File.Delete(socketsPath);   // Not mandatory, but just as additional safety to prevent "address already in use" error

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenUnixSocket(socketsPath, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});
#endif

WebApplication app = builder.Build();

app.MapGrpcService<CalcService>();

await app.RunAsync();
