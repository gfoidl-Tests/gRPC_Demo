using MediatR;

namespace Server.Math
{
    // Only sequential, so use base class instead of interface IRequestHandler<TRequest, TResponse>
    // https://github.com/jbogard/MediatR/wiki#request-types
    public class AddHandler : RequestHandler<AddOperation, int>
    {
        private readonly IMathService _mathService;
        //---------------------------------------------------------------------
        public AddHandler(IMathService mathService) => _mathService = mathService;
        //---------------------------------------------------------------------
        protected override int Handle(AddOperation request) => _mathService.Add(request.A, request.B);
    }
}
