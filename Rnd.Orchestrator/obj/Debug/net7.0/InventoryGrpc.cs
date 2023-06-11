// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: inventory.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Grpc {
  public static partial class InventoryService
  {
    static readonly string __ServiceName = "Inventory.InventoryService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Grpc.ReserveInventoryRequest> __Marshaller_Inventory_ReserveInventoryRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Grpc.ReserveInventoryRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Grpc.ReserveInventoryResponse> __Marshaller_Inventory_ReserveInventoryResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Grpc.ReserveInventoryResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Grpc.CheckInventoryRequest> __Marshaller_Inventory_CheckInventoryRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Grpc.CheckInventoryRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Grpc.CheckInventoryResponse> __Marshaller_Inventory_CheckInventoryResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Grpc.CheckInventoryResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Grpc.ReturnInventoryRequest> __Marshaller_Inventory_ReturnInventoryRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Grpc.ReturnInventoryRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Grpc.ReturnInventoryResponse> __Marshaller_Inventory_ReturnInventoryResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Grpc.ReturnInventoryResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Grpc.ReserveInventoryRequest, global::Grpc.ReserveInventoryResponse> __Method_ReserveInventory = new grpc::Method<global::Grpc.ReserveInventoryRequest, global::Grpc.ReserveInventoryResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ReserveInventory",
        __Marshaller_Inventory_ReserveInventoryRequest,
        __Marshaller_Inventory_ReserveInventoryResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Grpc.CheckInventoryRequest, global::Grpc.CheckInventoryResponse> __Method_CheckInventory = new grpc::Method<global::Grpc.CheckInventoryRequest, global::Grpc.CheckInventoryResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CheckInventory",
        __Marshaller_Inventory_CheckInventoryRequest,
        __Marshaller_Inventory_CheckInventoryResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Grpc.ReturnInventoryRequest, global::Grpc.ReturnInventoryResponse> __Method_ReturnInventory = new grpc::Method<global::Grpc.ReturnInventoryRequest, global::Grpc.ReturnInventoryResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ReturnInventory",
        __Marshaller_Inventory_ReturnInventoryRequest,
        __Marshaller_Inventory_ReturnInventoryResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Grpc.InventoryReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of InventoryService</summary>
    [grpc::BindServiceMethod(typeof(InventoryService), "BindService")]
    public abstract partial class InventoryServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Grpc.ReserveInventoryResponse> ReserveInventory(global::Grpc.ReserveInventoryRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Grpc.CheckInventoryResponse> CheckInventory(global::Grpc.CheckInventoryRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Grpc.ReturnInventoryResponse> ReturnInventory(global::Grpc.ReturnInventoryRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(InventoryServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_ReserveInventory, serviceImpl.ReserveInventory)
          .AddMethod(__Method_CheckInventory, serviceImpl.CheckInventory)
          .AddMethod(__Method_ReturnInventory, serviceImpl.ReturnInventory).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, InventoryServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_ReserveInventory, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Grpc.ReserveInventoryRequest, global::Grpc.ReserveInventoryResponse>(serviceImpl.ReserveInventory));
      serviceBinder.AddMethod(__Method_CheckInventory, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Grpc.CheckInventoryRequest, global::Grpc.CheckInventoryResponse>(serviceImpl.CheckInventory));
      serviceBinder.AddMethod(__Method_ReturnInventory, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Grpc.ReturnInventoryRequest, global::Grpc.ReturnInventoryResponse>(serviceImpl.ReturnInventory));
    }

  }
}
#endregion
