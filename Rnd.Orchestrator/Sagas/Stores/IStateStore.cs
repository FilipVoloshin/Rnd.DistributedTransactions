namespace Rnd.Orchestrator.Sagas.Stores;

public interface IStateStore<TState> where TState: class, ICorrelated
{
    Task SaveStateAsync(TState newState);
    Task<TState> RetrieveStateAsync(TState currentState);
}