using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories;

public class ItemsRepository
{
    private const string collectionName = "items";

    private readonly IMongoCollection<Item> dbCollection;

    private readonly FilterDefinitionBuilder<Item> filterBuilder;

    public ItemsRepository()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var database = mongoClient.GetDatabase("Catalog");
        dbCollection = database.GetCollection<Item>(collectionName);
    }

    public async Task<IReadOnlyCollection<Item>> GetAllAsync()
    {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    /// <summary>
    /// Retrieves an item from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the item to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation, containing the item if found, or null otherwise.</returns>
    public async Task<Item?> GetAsync(Guid id)
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
    public async Task CreateAsync(Item entity)
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
    public async Task UpdateAsync(Item entity)
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

