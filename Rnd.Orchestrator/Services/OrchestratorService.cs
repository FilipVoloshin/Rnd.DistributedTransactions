using Grpc;
using Grpc.Core;
using Rnd.Orchestrator.Sagas.OrderSaga;

namespace Rnd.Orchestrator.Services;

public class OrchestratorService : Grpc.OrchestratorService.OrchestratorServiceBase
{
    // private readonly InventoryService.InventoryServiceClient _inventoryService;
    // private readonly OrderService.OrderServiceClient _orderService;
    // private readonly PaymentService.PaymentServiceClient _paymentService;
    private OrderSaga _orderSaga;
    public OrchestratorService(OrderSaga orderSaga 
        /*PaymentService.PaymentServiceClient paymentService, 
        OrderService.OrderServiceClient orderService, 
        InventoryService.InventoryServiceClient inventoryService*/)
    {
        _orderSaga = orderSaga;
        /*_paymentService = paymentService;
        _orderService = orderService;
        _inventoryService = inventoryService;*/
    }

    // #region Implementation without state machine
    //
    // public override async Task<BeginOrderingResponse> BeginOrdering(BeginOrderingRequest request,
    //     ServerCallContext context)
    // {
    //     try
    //     {
    //         var userId = Guid.NewGuid();
    //         
    //         var orderResponse = await _orderService.CreateOrderAsync(new CreateOrderRequest
    //         {
    //             ItemId = request.ItemId,
    //             Quantity = request.Quantity,
    //             UserId = userId.ToString() 
    //         });
    //
    //         var inventoryAvailable = (await _inventoryService.CheckInventoryAsync(new CheckInventoryRequest
    //         {
    //             Quantity = request.Quantity,
    //             ItemId = request.ItemId
    //         }))?.IsAvailable ?? false;
    //
    //         if (!inventoryAvailable)
    //         {
    //             return new BeginOrderingResponse { Status = $"Inventory is not available" };
    //         }
    //
    //         var reserveInventoryResponse = await _inventoryService.ReserveInventoryAsync(new ReserveInventoryRequest
    //         {
    //             Quantity = request.Quantity,
    //             ItemId = request.ItemId
    //         });
    //
    //         ProcessPaymentResponse processPaymentResponse;
    //         try
    //         {
    //             processPaymentResponse = await _paymentService.ProcessPaymentAsync(new ProcessPaymentRequest
    //             {
    //                 OrderId = orderResponse.OrderId,
    //                 Amount = reserveInventoryResponse.Price.Value * request.Quantity,
    //                 UserId = userId.ToString()
    //             });
    //         }
    //         catch (Exception ex)
    //         {
    //             await CompensateFailedPaymentAsync(orderResponse.OrderId, request.ItemId, request.Quantity);                
    //             return new BeginOrderingResponse { Status = $"Failed payment. {ex.Message}" };
    //         }
    //
    //         if (string.IsNullOrEmpty(processPaymentResponse.PaymentId))
    //         {
    //             await CompensateFailedPaymentAsync(orderResponse.OrderId, request.ItemId, request.Quantity);
    //             return new BeginOrderingResponse { Status = $"Failed payment. PaymentId is missing" };
    //         }
    //         
    //         var orderRequest = await _orderService.SetOrderCompletedAsync(new SetOrderCompletedRequest
    //         {
    //             OrderId = orderResponse.OrderId
    //         });
    //
    //         if (!orderRequest.IsSuccess)
    //         {
    //             await CompensateFailedPaymentAsync(orderResponse.OrderId, request.ItemId, request.Quantity);
    //             // await _paymentService.RefundPaymentAsync(new RefundPaymentRequest
    //             // {
    //             //     PaymentId = processPaymentResponse.PaymentId
    //             // });
    //         }
    //         
    //         return new BeginOrderingResponse { Status = "Ordered" };
    //     }
    //     catch (Exception ex)
    //     {
    //         return new BeginOrderingResponse { Status = $"Error occured. {ex.Message}" };
    //     }
    // }
    //
    // private async Task CompensateFailedPaymentAsync(string orderId, string itemId, int quantity)
    // {
    //     await _orderService.CancelOrderAsync(new CancelOrderRequest
    //     {
    //         OrderId = orderId
    //     });
    //     await _inventoryService.ReturnInventoryAsync(new ReturnInventoryRequest
    //     {
    //         ItemId = itemId,
    //         Quantity = quantity
    //     });
    // }
    //
    // #endregion

    public override async Task<BeginOrderingResponse> BeginOrdering(BeginOrderingRequest request, ServerCallContext context)
    {
        await _orderSaga.BeginOrderAsync(new BeginOrderingRequest
        {
            ItemId = request.ItemId,
            Quantity = request.Quantity
        });

        return new BeginOrderingResponse { Status = "Success" };
    }
}