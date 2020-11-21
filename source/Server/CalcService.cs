using System.Threading.Tasks;
using Grpc.Core;
using MathEndpoint;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Server.Math;

namespace Server
{
    public class CalcService : MathEndpoint.Calc.CalcBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger   _logger;
        //---------------------------------------------------------------------
        public CalcService(IMediator mediator, ILogger<CalcService> logger)
        {
            _mediator = mediator;
            _logger   = logger;
        }
        //---------------------------------------------------------------------
        public override async Task<IntBinaryOperationResponse> Add(IntBinaryOperationRequest request, ServerCallContext context)
        {
            HttpContext httpContext = context.GetHttpContext();

            _logger.LogInformation($"Connection id: {httpContext.Connection.Id}");
            _logger.LogInformation($"Handling request for {request.A} + {request.B}");

            int sum = await _mediator.Send(new AddOperation(request.A, request.B));

            return new IntBinaryOperationResponse { C = sum };
        }
    }
}
