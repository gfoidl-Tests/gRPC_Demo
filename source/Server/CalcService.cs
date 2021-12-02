using System.Threading.Tasks;
using Contract;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using Server.Math;

namespace Server
{
    public class CalcService : ICalcService
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
        public async Task<IntBinaryOperationResponse> AddAsync(IntBinaryOperationRequest request, CallContext context = default)
        {
            HttpContext? httpContext = context.ServerCallContext?.GetHttpContext();

            _logger.LogInformation($"Connection id: {httpContext?.Connection.Id}");
            _logger.LogInformation($"Handling request for {request.A} + {request.B}");

            int sum = await _mediator.Send(new AddOperation(request.A, request.B));

            return new IntBinaryOperationResponse { C = sum };
        }
    }
}
