using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using StreamTest;

namespace Server;

public partial class CountService : Count.CountBase
{
    private readonly ILogger<CountService> _logger;
    //-------------------------------------------------------------------------
    public CountService(ILogger<CountService> logger) => _logger = logger;
    //-------------------------------------------------------------------------
    public override async Task GetCountStream(Empty _, IServerStreamWriter<CountResponse> responseStream, ServerCallContext context)
    {
        using PeriodicTimer ticker = new(TimeSpan.FromSeconds(1));
        int count                  = 0;

        try
        {
            while (await ticker.WaitForNextTickAsync(context.CancellationToken))
            {
                count++;
                CountResponse response = new() { Count = count, TimeStamp = Timestamp.FromDateTimeOffset(DateTimeOffset.Now) };

                Log.SendCount(_logger, context);
                await responseStream.WriteAsync(response);

                context.CancellationToken.ThrowIfCancellationRequested();
            }
        }
        catch (OperationCanceledException)
        { }
    }
    //-------------------------------------------------------------------------
    private static partial class Log
    {
        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Sending count response to {ConnectionId}")]
        private static partial void SendCount(ILogger logger, string connectionId);

        public static void SendCount(ILogger logger, ServerCallContext context)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            HttpContext httpContext = context.GetHttpContext();
            SendCount(logger, httpContext.Connection.Id);
        }
    }
}
