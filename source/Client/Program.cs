using System.Diagnostics;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using MathEndpoint;
using StreamTest;

Console.WriteLine("Waiting for server to start...any key to continue");
Console.ReadKey();

//using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
using GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:5000");

try
{
    await RunCalc();

    Console.WriteLine("Run streaming...any key to continue");
    Console.ReadKey();

    await RunStreaming();
}
catch (RpcException ex) when (!Debugger.IsAttached)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(ex.Message);
    Console.ResetColor();
}

Console.WriteLine("\nEnd.");
//-----------------------------------------------------------------------------
async Task RunCalc()
{
    Calc.CalcClient client              = new(channel);
    IntBinaryOperationResponse response = await client.AddAsync(new IntBinaryOperationRequest { A = 3, B = 4 });

    Console.WriteLine(response.C);
}
//-----------------------------------------------------------------------------
async Task RunStreaming()
{
    CancellationTokenSource cts                           = new();
    Count.CountClient client                              = new(channel);
    AsyncServerStreamingCall<CountResponse> streamingCall = client.GetCountStream(new Empty(), cancellationToken: cts.Token);

    int count = 0;
    await foreach (CountResponse countResponse in streamingCall.ResponseStream.ReadAllAsync(cts.Token))
    {
        count++;

        Console.WriteLine($"{countResponse.TimeStamp}\t{countResponse.Count}");

        if (count > 10)
        {
            cts.Cancel();
        }
    }
}
