#include <iostream>
#include "MathClient.hpp"
#include <string>
//-----------------------------------------------------------------------------
using namespace std;
//-----------------------------------------------------------------------------
int main()
{
    cout << "Waiting for server to start...any key to continue" << endl;
    cin.get();

    string serverAddress              = "localhost:5000";
    shared_ptr<grpc::Channel> channel = grpc::CreateChannel(serverAddress, grpc::InsecureChannelCredentials());

    try
    {
        MathClient mathClient{ channel };

        for (int i = 0; i < 2; ++i)
        {
            int result = mathClient.Add(3, 4);
            cout << "Result: " << result << endl;
        }
    }
    catch (runtime_error& e)
    {
        cerr << e.what() << endl;
    }
}
