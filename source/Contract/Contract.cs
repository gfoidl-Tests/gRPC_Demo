using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using ProtoBuf.Grpc;

namespace Contract;

[DataContract]
public class IntBinaryOperationRequest
{
    [DataMember(Order = 1)]
    public int A { get; set; }

    [DataMember(Order = 2)]
    public int B { get; set; }
}
//-----------------------------------------------------------------------------
[DataContract]
public class IntBinaryOperationResponse
{
    [DataMember(Order = 1)]
    public int C { get; set; }
}
//-----------------------------------------------------------------------------
[ServiceContract]
public interface ICalcService
{
    [OperationContract]
    Task<IntBinaryOperationResponse> AddAsync(IntBinaryOperationRequest request, CallContext context = default);
}
