#!/usr/bin/env bash

if [[ -z $GRPC_INSTALL_DIR ]]; then
    echo "GRPC_INSTALL_DIR must be set to the directoy where gRPC is installed"
    exit 1
fi

config=${1:-Debug}

mkdir -p out/$config
cd $_

cmake -DCMAKE_BUILD_TYPE=$config -DCMAKE_PREFIX_PATH=$GRPC_INSTALL_DIR ../..

cmake --build . --config $config
cmake --install .

cd - > /dev/null
