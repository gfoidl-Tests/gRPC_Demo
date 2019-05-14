using System;
using MediatR;
using Server.Abstractions;
using Server.Operations;

namespace Server.Handlers
{
    // Only sequential, so use base class instead of interface IRequestHandler<TRequest, TResponse>
    // https://github.com/jbogard/MediatR/wiki#request-types
    public class AddHandler : RequestHandler<AddOperation, int>
    {
        private readonly IMathService _mathService;
        //---------------------------------------------------------------------
        public AddHandler(IMathService mathService)
            => _mathService = mathService ?? throw new ArgumentNullException(nameof(mathService));
        //---------------------------------------------------------------------
        protected override int Handle(AddOperation request) => _mathService.Add(request.A, request.B);
    }
}
