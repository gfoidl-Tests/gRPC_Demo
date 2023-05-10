using Mediator;

namespace Server.Math;

public abstract class BinaryOperation : IRequest<int>
{
    public int A { get; }
    public int B { get; }
    //-------------------------------------------------------------------------
    protected BinaryOperation(int a, int b) => (this.A, this.B) = (a, b);
}
