using Grpc;
using Rnd.Orchestrator.Sagas.Stores;
using Stateless;

namespace Rnd.Orchestrator.Sagas.OrderSaga;

#nullable disable
public class OrderSaga
{
    private static Guid UserId => Guid.NewGuid();

    private readonly ILogger<OrderSaga> _logger;
    private readonly InventoryService.InventoryServiceClient _inventoryServiceClient;
    private readonly OrderService.OrderServiceClient _orderServiceClient;
    private readonly PaymentService.PaymentServiceClient _paymentServiceClient;
    private readonly IStateStore<OrderSagaState> _stateStore;
    private readonly StateMachine<State, Triggers> _stateMachine;

    private OrderSagaState _sagaState;

    public OrderSaga(InventoryService.InventoryServiceClient inventoryServiceClient,
        OrderService.OrderServiceClient orderServiceClient,
        PaymentService.PaymentServiceClient paymentServiceClient,
        ILogger<OrderSaga> logger,
        IStateStore<OrderSagaState> stateStore)
    {
        _inventoryServiceClient = inventoryServiceClient;
        _orderServiceClient = orderServiceClient;
        _paymentServiceClient = paymentServiceClient;
        _logger = logger;
        _stateStore = stateStore;

        #region Configure state machine

        _stateMachine = new StateMachine<State, Triggers>(GetSagaState, SetSagaState);

        _stateMachine.OnTransitionedAsync(TransitAsync);

        _stateMachine.Configure(State.OrderCreated)
            .Permit(Triggers.CheckInventory, State.CheckingInventory);

        _stateMachine.Configure(State.CheckingInventory)
            .OnEntryAsync(CheckingInventoryAvailabilityAsync)
            .Permit(Triggers.InventoryAvailable, State.ReservingInventory)
            .Permit(Triggers.InventoryUnavailable, State.OrderFailed);

        _stateMachine.Configure(State.ReservingInventory)
            .OnEntryAsync(ReserveInventoryAsync)
            .Permit(Triggers.ReservationSuccessful, State.PaymentProcessing)
            .Permit(Triggers.ReservationFailed, State.OrderFailed);

        _stateMachine.Configure(State.PaymentProcessing)
            .OnEntryAsync(ProcessPaymentAsync)
            .Permit(Triggers.PaymentSuccessful, State.OrderCompleted)
            .Permit(Triggers.PaymentFailed, State.PaymentFailed);

        _stateMachine.Configure(State.OrderCompleted)
            .OnEntryAsync(SetOrderCompletedAsync)
            .Permit(Triggers.OrderCompleteFailed, State.OrderCancelled);

        _stateMachine.Configure(State.PaymentFailed)
            .OnEntryAsync(HandlePaymentFailureAsync);

        _stateMachine.Configure(State.OrderCancelled)
            .OnEntryAsync(HandleOrderCancellationAsync);

        _stateMachine.Configure(State.OrderFailed)
            .OnEntry(HandleFailureAsync);

        #endregion
    }

    private void SetSagaState(State state)
    {
        _sagaState.CurrentState = state;
    }

    private State GetSagaState()
    {
        return _sagaState?.CurrentState ?? State.OrderCreated;
    }

    private async Task TransitAsync(StateMachine<State, Triggers>.Transition context)
    {
        _sagaState.CurrentTrigger = context.Trigger;
        
        await _stateStore.SaveStateAsync(_sagaState);
    }

    public async Task BeginOrderAsync(BeginOrderingRequest beginOrderingRequest)
    {
        var orderResponse = await _orderServiceClient.CreateOrderAsync(new CreateOrderRequest
        {
            Quantity = beginOrderingRequest.Quantity,
            ItemId = beginOrderingRequest.ItemId,
            UserId = UserId.ToString()
        });

        _sagaState = new OrderSagaState
        {
            CorrelationId = Guid.NewGuid(),
            ItemId = beginOrderingRequest.ItemId,
            Quantity = beginOrderingRequest.Quantity,
            OrderId = orderResponse.OrderId,
            CurrentState = State.OrderCreated
        };

        await _stateMachine.FireAsync(Triggers.CheckInventory);
    }

    #region Successful triggers

    private async Task CheckingInventoryAvailabilityAsync()
    {
        var response = await _inventoryServiceClient.CheckInventoryAsync(new CheckInventoryRequest
        {
            ItemId = _sagaState.ItemId,
            Quantity = _sagaState.Quantity
        });

        if (response.IsAvailable)
        {
            await _stateMachine.FireAsync(Triggers.InventoryAvailable);
        }
        else
        {
            _logger.LogDebug(
                "Order with id {Id} will be cancelled, because {Quantity} items with id {ItemId} is not available ",
                _sagaState.OrderId, _sagaState.Quantity, _sagaState.ItemId);
            await _stateMachine.FireAsync(Triggers.InventoryUnavailable);
        }
    }

    private async Task ReserveInventoryAsync()
    {
        try
        {
            var response = await _inventoryServiceClient.ReserveInventoryAsync(new ReserveInventoryRequest
            {
                ItemId = _sagaState.ItemId,
                Quantity = _sagaState.Quantity
            });

            _sagaState.Price = response.Price;
            await _stateMachine.FireAsync(Triggers.ReservationSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogError("Order with id {Id} will be cancelled, because of the error {Message}",
                _sagaState.OrderId, ex.Message);
            await _stateMachine.FireAsync(Triggers.ReservationFailed);
        }
    }

    private async Task ProcessPaymentAsync()
    {
        try
        {
            var response = await _paymentServiceClient.ProcessPaymentAsync(new ProcessPaymentRequest
            {
                Amount = _sagaState.TotalPrice ?? default,
                OrderId = _sagaState.OrderId,
                UserId = UserId.ToString()
            });

            _sagaState.PaymentId = response.PaymentId;

            await _stateMachine.FireAsync(Triggers.PaymentSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Order with id {Id} will be cancelled with refunding of the payment and returning the inventory because of the error {Message}",
                _sagaState.OrderId, ex.Message);
            await _stateMachine.FireAsync(Triggers.PaymentFailed);
        }
    }

    private async Task SetOrderCompletedAsync()
    {
        try
        {
            await _orderServiceClient.SetOrderCompletedAsync(new SetOrderCompletedRequest
                { OrderId = _sagaState.OrderId });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Order with id {Id} will be cancelled with refunding of the payment and returning the inventory because of the error {Message}",
                _sagaState.OrderId, ex.Message);
            await _stateMachine.FireAsync(Triggers.OrderCompleteFailed);
        }
    }

    #endregion

    #region Failures and compensations

    private void HandleFailureAsync(StateMachine<State, Triggers>.Transition transition)
    {
        _logger.LogError("Failed for order with id {Id}. While {PreviousState} we triggered {Trigger}",
            _sagaState.OrderId, transition.Source, transition.Trigger);
    }

    private async Task HandleOrderCancellationAsync()
    {
        await Task.WhenAll(RefundPaymentAsync(),
            ReturnInventoryAsync(),
            CancelOrderAsync());
    }

    private async Task HandlePaymentFailureAsync()
    {
        await Task.WhenAll(RefundPaymentAsync(),
            ReturnInventoryAsync());
    }

    private async Task CancelOrderAsync() => await _orderServiceClient.CancelOrderAsync(
        new CancelOrderRequest { OrderId = _sagaState.OrderId });

    private async Task RefundPaymentAsync()
    {
        await _paymentServiceClient.RefundPaymentAsync(new RefundPaymentRequest
        {
            OrderId = _sagaState.OrderId,
            UserId = UserId.ToString()
        });
    }

    private async Task ReturnInventoryAsync() => await _inventoryServiceClient.ReturnInventoryAsync(
        new ReturnInventoryRequest
        {
            Quantity = _sagaState.Quantity,
            ItemId = _sagaState.ItemId
        });

    #endregion
}

#nullable enable