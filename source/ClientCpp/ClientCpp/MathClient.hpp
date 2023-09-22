#pragma once
//-----------------------------------------------------------------------------
#include <memory>
#include <grpcpp/grpcpp.h>
#include "Math.grpc.pb.h"
//-----------------------------------------------------------------------------
class MathClient
{
public:
    MathClient(std::shared_ptr<grpc::Channel> channel);

    int Add(const int a, const int b);

private:
    std::unique_ptr<MathEndpoint::Calc::Stub> _stub;
};
