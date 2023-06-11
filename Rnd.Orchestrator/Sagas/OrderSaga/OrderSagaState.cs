using Rnd.Orchestrator.Sagas.Base;

namespace Rnd.Orchestrator.Sagas.OrderSaga;

public record OrderSagaState : SagaState<State, Triggers>
{
    public string OrderId { get; init; } = null!;
    public string ItemId { get; init; } = null!;
    public int Quantity { get; init; }
    public double? Price { get; set; }
    public double? TotalPrice => Price * Quantity;
    public string? PaymentId { get; set; }
}