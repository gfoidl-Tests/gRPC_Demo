using System.Diagnostics;
using Grpc.Core;
using Grpc.Net.Client;
using MathEndpoint;

Console.WriteLine("Waiting for server to start...any key to continue");
Console.ReadKey();

//using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
using GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:5000");

Calc.CalcClient client = new(channel);

try
{
    IntBinaryOperationResponse response = await client.AddAsync(new IntBinaryOperationRequest { A = 3, B = 4 });
    Console.WriteLine(response.C);

    response = await client.MulAsync(new IntBinaryOperationRequest { A = 5, B = 4 });
    Console.WriteLine(response.C);
}
catch (RpcException ex) when (!Debugger.IsAttached)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(ex.Message);
    Console.ResetColor();
}

Console.WriteLine("\nEnd.");
