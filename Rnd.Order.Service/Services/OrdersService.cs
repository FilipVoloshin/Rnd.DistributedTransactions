using Grpc.Core;
using Rnd.Order.Service.Data;

namespace Rnd.Order.Service.Services;

public class OrdersService : Grpc.OrderService.OrderServiceBase
{
    private readonly OrdersContext _ordersContext;

    public OrdersService(OrdersContext ordersContext)
    {
        _ordersContext = ordersContext;
    }

    public override async Task<Grpc.CreateOrderResponse> CreateOrder(Grpc.CreateOrderRequest request,
        ServerCallContext context)
    {
        Data.Order order = new()
        {
            Id = Guid.NewGuid(),
            Quantity = request.Quantity,
            UserId = Guid.Parse(request.UserId),
            ItemId = Guid.Parse(request.ItemId),
            Status = OrderStatus.WaitingForPayment
        };

        await _ordersContext.AddAsync(order);
        await _ordersContext.SaveChangesAsync();

        return new Grpc.CreateOrderResponse { OrderId = order.Id.ToString() };
    }

    public override async Task<Grpc.CancelOrderResponse> CancelOrder(Grpc.CancelOrderRequest request,
        ServerCallContext context)
    {
        try
        {
            var order = await _ordersContext.Orders.FindAsync(Guid.Parse(request.OrderId));
            if (order is null)
            {
                return new Grpc.CancelOrderResponse { IsSuccess = false };
            }

            order.Status = OrderStatus.Canceled;
            _ordersContext.Orders.Update(order);
            await _ordersContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Grpc.CancelOrderResponse { IsSuccess = false };
        }

        return new Grpc.CancelOrderResponse { IsSuccess = true };
    }

    public override async Task<Grpc.SetOrderCompletedResponse> SetOrderCompleted(Grpc.SetOrderCompletedRequest request,
        ServerCallContext context)
    {
        try
        {
            var order = await _ordersContext.Orders.FindAsync(Guid.Parse(request.OrderId));
            if (order is null)
            {
                return new Grpc.SetOrderCompletedResponse { IsSuccess = false };
            }

            order.Status = OrderStatus.Completed;
            _ordersContext.Orders.Update(order);
            await _ordersContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Grpc.SetOrderCompletedResponse { IsSuccess = false };
        }

        return new Grpc.SetOrderCompletedResponse { IsSuccess = true };
    }
}