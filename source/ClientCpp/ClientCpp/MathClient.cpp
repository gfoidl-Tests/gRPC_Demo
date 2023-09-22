#include "MathClient.hpp"
#include <iostream>
#include <exception>
#include <sstream>
//-----------------------------------------------------------------------------
using namespace std;
using grpc::Channel, grpc::ClientContext, grpc::Status;
using MathEndpoint::Calc, MathEndpoint::IntBinaryOperationRequest, MathEndpoint::IntBinaryOperationResponse;
//-----------------------------------------------------------------------------
MathClient::MathClient(std::shared_ptr<Channel> channel)
    : _stub{ Calc::NewStub(channel) }
{}
//-----------------------------------------------------------------------------
int MathClient::Add(const int a, const int b)
{
    IntBinaryOperationRequest request;
    request.set_a(a);
    request.set_b(b);

    IntBinaryOperationResponse response;
    ClientContext context;

    Status status = _stub->Add(&context, request, &response);

    if (status.ok())
    {
        return response.c();
    }
    else
    {
        stringstream ss;

        ss << status.error_code() << ": " << status.error_message();
        string msg = ss.str();

        throw std::runtime_error{ msg };
    }
}
