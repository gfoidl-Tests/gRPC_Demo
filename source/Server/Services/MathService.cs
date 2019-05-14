using Server.Abstractions;

namespace Server.Services
{
    public class MathService : IMathService
    {
        public int Add(int a, int b) => a + b;
    }
}
