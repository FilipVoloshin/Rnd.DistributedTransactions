using Rnd.Orchestrator.Sagas.Stores;

namespace Rnd.Orchestrator.Sagas.Base;

public record SagaState<TStateEnum, TTriggerEnum>: ICorrelated
    where TStateEnum : Enum
    where TTriggerEnum : Enum
{
    public TStateEnum CurrentState { get; set; } = default!;
    public TTriggerEnum CurrentTrigger { get; set; } = default!;
    public Guid CorrelationId { get; init; }
}