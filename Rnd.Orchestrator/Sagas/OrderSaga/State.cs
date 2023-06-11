namespace Rnd.Orchestrator.Sagas.OrderSaga;

public enum State
{
    OrderCreated,
    CheckingInventory,
    ReservingInventory,
    PaymentProcessing,
    OrderCompleted,
    OrderCancelled,
    OrderFailed,
    PaymentFailed
}