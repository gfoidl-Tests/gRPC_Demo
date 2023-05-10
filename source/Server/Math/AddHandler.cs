using System.Threading;
using System.Threading.Tasks;

namespace Server.Math;

public class AddHandler : MathHandler<AddOperation>
{
    public AddHandler(IMathService mathService) : base(mathService) { }
    //-------------------------------------------------------------------------
    public override ValueTask<int> Handle(AddOperation request, CancellationToken cancellationToken)
    {
        int result = _mathService.Add(request.A, request.B);
        return ValueTask.FromResult(result);
    }
}
