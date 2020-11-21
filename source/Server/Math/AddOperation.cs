using MediatR;

namespace Server.Math
{
    public readonly struct AddOperation : IRequest<int>
    {
        public int A { get; }
        public int B { get; }
        //---------------------------------------------------------------------
        public AddOperation(int a, int b) => (this.A, this.B) = (a, b);
    }
}
