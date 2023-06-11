namespace Rnd.Orchestrator.Sagas.Stores;

public interface ICorrelated
{
    Guid CorrelationId { get; init; }
}