using System.Threading;
using System.Threading.Tasks;
using Mediator;

namespace Server.Math;

public abstract class MathHandler<TRequest> : IRequestHandler<TRequest, int> where TRequest : IRequest<int>
{
    protected readonly IMathService _mathService;
    //-------------------------------------------------------------------------
    protected MathHandler(IMathService mathService) => _mathService = mathService;
    //-------------------------------------------------------------------------
    public abstract ValueTask<int> Handle(TRequest request, CancellationToken cancellationToken);
}
