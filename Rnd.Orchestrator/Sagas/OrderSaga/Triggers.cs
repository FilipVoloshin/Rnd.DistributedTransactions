namespace Rnd.Orchestrator.Sagas.OrderSaga;

public enum Triggers
{
    CheckInventory,
    InventoryAvailable,
    InventoryUnavailable,
    ReservationSuccessful,
    ReservationFailed,
    PaymentSuccessful,
    PaymentFailed,
    OrderCompleteFailed
}