using MongoDB.Driver;

namespace Rnd.Orchestrator.Sagas.Stores;

public class MongoStateStore<TState>: IStateStore<TState> 
where TState: class, ICorrelated
{
    private readonly IMongoCollection<TState> _collection;

    public MongoStateStore(IMongoDatabase database)
    {
        _collection = database.GetCollection<TState>(typeof(TState).Name);
    }

    public async Task SaveStateAsync(TState newState)
    {
        var filter = Builders<TState>.Filter.Eq(state => state.CorrelationId, newState.CorrelationId);
        await _collection.ReplaceOneAsync(filter, newState, new ReplaceOptions { IsUpsert = true });
    }

    public async Task<TState> RetrieveStateAsync(TState currentState)
    {
        var filter = Builders<TState>.Filter.Eq(state => state.CorrelationId, currentState.CorrelationId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}