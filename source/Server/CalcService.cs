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
            Log.Request(_logger, context, '+', request);
        }

        int sum = await _mediator.Send(new AddOperation(request.A, request.B));

        return new IntBinaryOperationResponse { C = sum };
    }
    //-------------------------------------------------------------------------
    public override async Task<IntBinaryOperationResponse> Mul(IntBinaryOperationRequest request, ServerCallContext context)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            Log.Request(_logger, context, '*', request);
        }

        int result = await _mediator.Send(new MulOperation(request.A, request.B));

        return new IntBinaryOperationResponse { C = result };
    }
    //-------------------------------------------------------------------------
    private static partial class Log
    {
        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Connection id: {ConnectionId}, handling request for {A} {Operation} {B}")]
        private static partial void Request(ILogger logger, string connectionId, int a, char operation, int b);

        public static void Request(ILogger logger, ServerCallContext context, char operation, IntBinaryOperationRequest request)
        {
            HttpContext httpContext = context.GetHttpContext();
            Request(logger, httpContext.Connection.Id, request.A, operation, request.B);
        }
    }
}
