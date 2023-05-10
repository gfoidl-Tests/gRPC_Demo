using Grpc.Core;
using MathEndpoint;
using Mediator;
using Server.Math;

namespace Server;

public partial class CalcService : Calc.CalcBase
{
    private readonly IMediator _mediator;
    private readonly ILogger   _logger;
    //-------------------------------------------------------------------------
    public CalcService(IMediator mediator, ILogger<CalcService> logger)
    {
        _mediator = mediator;
        _logger   = logger;
    }
    //-------------------------------------------------------------------------
    public override async Task<IntBinaryOperationResponse> Add(IntBinaryOperationRequest request, ServerCallContext context)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            HttpContext httpContext = context.GetHttpContext();

            Log.Request(_logger, httpContext.Connection.Id, request.A, request.B);
        }

        int sum = await _mediator.Send(new AddOperation(request.A, request.B));

        return new IntBinaryOperationResponse { C = sum };
    }
    //-------------------------------------------------------------------------
    private static partial class Log
    {
        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Connection id: {ConnectionId}, handling request for {A} + {B}")]
        public static partial void Request(ILogger logger, string connectionId, int a, int b);
    }
}
