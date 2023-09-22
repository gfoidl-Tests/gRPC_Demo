include(../cmake/common.cmake)

add_executable(ClientCpp
    main.cpp
    MathClient.cpp
)

if (CMAKE_VERSION VERSION_GREATER 3.12)
    set_property(TARGET ClientCpp PROPERTY CXX_STANDARD 20)
endif()

install(TARGETS ClientCpp DESTINATION .)

# Proto file
get_filename_component(math_proto "../../Proto/Math.proto" ABSOLUTE)
get_filename_component(math_proto_path "${math_proto}" PATH)

# Generated sources (the names are produced by gRPC infrastructure)
set(math_proto_srcs "${CMAKE_CURRENT_BINARY_DIR}/Math.pb.cc")
set(math_proto_hdrs "${CMAKE_CURRENT_BINARY_DIR}/Math.pb.h")
set(math_grpc_srcs  "${CMAKE_CURRENT_BINARY_DIR}/Math.grpc.pb.cc")
set(math_grpc_hdrs  "${CMAKE_CURRENT_BINARY_DIR}/Math.grpc.pb.h")

add_custom_command(
    OUTPUT "${math_proto_srcs}" "${math_proto_hdrs}" "${math_grpc_srcs}" "${math_grpc_hdrs}"
    COMMAND ${_PROTOBUF_PROTOC}
    ARGS --grpc_out "${CMAKE_CURRENT_BINARY_DIR}"
         --cpp_out "${CMAKE_CURRENT_BINARY_DIR}"
         -I "${math_proto_path}"
         --plugin=protoc-gen-grpc="${_GRPC_CPP_PLUGIN_EXECUTABLE}"
         "${math_proto}"
    DEPENDS "${math_proto}"
)

# Include generated *.pb.h files
include_directories("${CMAKE_CURRENT_BINARY_DIR}")

# hw_grpc_proto
add_library(math_grpc_proto STATIC
    ${math_grpc_srcs}
    ${math_grpc_hdrs}
    ${math_proto_srcs}
    ${math_proto_hdrs}
)
target_link_libraries(math_grpc_proto
    ${_REFLECTION}
    ${_GRPC_GRPCPP}
    ${_PROTOBUF_LIBPROTOBUF}
)
target_link_libraries(ClientCpp
    math_grpc_proto
)
