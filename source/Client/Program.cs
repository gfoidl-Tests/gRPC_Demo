#define USE_UNIX_DOMAIN_SOCKETS
//-----------------------------------------------------------------------------
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Grpc.Core;
using Grpc.Net.Client;
using MathEndpoint;
Console.WriteLine("Waiting for server to start...any key to continue");
Console.ReadKey();

#if USE_UNIX_DOMAIN_SOCKETS
string socketPath         = Path.Combine(Path.GetTempPath(), "gRPC-Test.sock");
using GrpcChannel channel = CreateChannel(socketPath);
#else
//using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
using GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:5000");
#endif

Calc.CalcClient client = new(channel);

try
{
    IntBinaryOperationResponse response = await client.AddAsync(new IntBinaryOperationRequest { A = 3, B = 4 });

    Console.WriteLine(response.C);
}
catch (RpcException ex) when (!Debugger.IsAttached)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(ex.Message);
    Console.ResetColor();
}

Console.WriteLine("\nEnd.");
//-----------------------------------------------------------------------------
#if USE_UNIX_DOMAIN_SOCKETS
// Cf. https://learn.microsoft.com/en-us/aspnet/core/grpc/interprocess-uds

static GrpcChannel CreateChannel(string socketPath)
{
    UnixDomainSocketEndPoint udsEndpoint                 = new(socketPath);
    UnixDomainSocketsConnectionFactory connectionFactory = new(udsEndpoint);
    SocketsHttpHandler socketsHttpHandler                = new()
    {
        ConnectCallback = connectionFactory.ConnectAsync
    };

    return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
    {
        HttpHandler = socketsHttpHandler
    });
}
//-----------------------------------------------------------------------------
public class UnixDomainSocketsConnectionFactory(EndPoint endPoint)
{
    private readonly EndPoint _endPoint = endPoint;
    //-------------------------------------------------------------------------
    public async ValueTask<Stream> ConnectAsync(SocketsHttpConnectionContext context, CancellationToken cancellationToken = default)
    {
        Socket socket = new(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

        try
        {
            await socket.ConnectAsync(_endPoint, cancellationToken);
            return new NetworkStream(socket, ownsSocket: true);
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }
}
#endif
