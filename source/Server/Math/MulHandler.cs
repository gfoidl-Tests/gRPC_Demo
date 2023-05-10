using System.Threading;
using System.Threading.Tasks;

namespace Server.Math;

public class MulHandler : MathHandler<MulOperation>
{
    public MulHandler(IMathService mathService) : base(mathService) { }
    //-------------------------------------------------------------------------
    public override ValueTask<int> Handle(MulOperation request, CancellationToken cancellationToken)
    {
        int result = _mathService.Mul(request.A, request.B);
        return ValueTask.FromResult(result);
    }
}
