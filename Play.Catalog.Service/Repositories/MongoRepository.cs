using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories;

public class MongoRepository<T> : IRepository<T> where T : IEntity
{
    private readonly IMongoCollection<T> dbCollection;

    private readonly FilterDefinitionBuilder<T> filterBuilder;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        dbCollection = database.GetCollection<T>(collectionName);
        filterBuilder = Builders<T>.Filter;
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    /// <summary>
    /// Retrieves an item from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the item to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation, containing the item if found, or null otherwise.</returns>
    public async Task<T?> GetAsync(Guid id)
    {
        var filter = filterBuilder.Eq(item => item.Id, id);
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Creates a new item in the database.
    /// </summary>
    /// <param name="entity">The item to be created.</param>
    /// <exception cref="ArgumentNullException">Thrown if the entity is null.</exception>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CreateAsync(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        await dbCollection.InsertOneAsync(entity);
    }

    /// <summary>
    /// Updates an existing item in the database.
    /// </summary>
    /// <param name="entity">The item to be updated.</param>
    /// <exception cref="ArgumentNullException">Thrown if the entity is null.</exception>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateAsync(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        var filter = filterBuilder.Eq(item => item.Id, entity.Id);
        await dbCollection.ReplaceOneAsync(filter, entity);
    }
    /// <summary>
    /// Removes an item from the database.
    /// </summary>
    /// <param name="id">The ID of the item to be removed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RemoveAsync(Guid id)
    {
        var filter = filterBuilder.Eq(item => item.Id, id);
        await dbCollection.DeleteOneAsync(filter);
    }
}

