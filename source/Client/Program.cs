using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.Core;

namespace Client
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Waiting for server to start...any key to continue");
            Console.ReadKey();

            var channel = new Channel("localhost:50051", ChannelCredentials.Insecure);
            var client = new MathEndpoint.Calc.CalcClient(channel);

            try
            {
                var response = await client.AddAsync(new MathEndpoint.IntBinaryOperationRequest { A = 3, B = 4 });

                Console.WriteLine(response.C);
            }
            catch (RpcException ex) when (!Debugger.IsAttached)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(ex.Message);
                Console.ResetColor();
            }
            finally
            {
                await channel.ShutdownAsync();
            }

            Console.WriteLine("\nEnd.");
        }
    }
}
