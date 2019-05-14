using System;
using System.Threading.Tasks;
using Grpc.Core;
using MathEndpoint;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Server.Operations;

namespace Server
{
    public class CalcService : MathEndpoint.Calc.CalcBase
    {
        private readonly ILogger   _logger;
        private readonly IMediator _mediator;
        //---------------------------------------------------------------------
        public CalcService(ILogger<CalcService> logger, IMediator mediator)
        {
            _logger   = logger   ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
