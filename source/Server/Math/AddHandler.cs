using System.Threading;
using System.Threading.Tasks;
using Mediator;

namespace Server.Math;

public class AddHandler : IRequestHandler<AddOperation, int>
{
    private readonly IMathService _mathService;
    //-------------------------------------------------------------------------
    public AddHandler(IMathService mathService) => _mathService = mathService;
    //-------------------------------------------------------------------------
    public ValueTask<int> Handle(AddOperation request, CancellationToken cancellationToken)
    {
        int result = _mathService.Add(request.A, request.B);
        return ValueTask.FromResult(result);
    }
}
