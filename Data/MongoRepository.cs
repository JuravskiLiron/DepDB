using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq.Expressions;
using DepDB.Models;

namespace DepDB.Data;

public class MongoRepository<T> : IRepository<T> where T : BaseDocument
{
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(MongoContext context)
    {
        var name = typeof(T).Name.ToLower() + "s";
        _collection = context.GetCollection<T>(name);
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetAllAsync(int skip = 0, int take = 50)
    {
        return await _collection.Find(Builders<T>.Filter.Empty)
            .Skip(skip)
            .Limit(take)
            .ToListAsync();
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> filter, int skip = 0, int take = 50)
    {
        return await _collection.Find(filter)
            .Skip(skip)
            .Limit(take)
            .ToListAsync();
    }

    public async Task InsertAsync(T entity)
    {
        if (string.IsNullOrEmpty(entity.Id))
            entity.Id = ObjectId.GenerateNewId().ToString();

        entity.UpdatedAtUtc = DateTime.UtcNow;

        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id);
    }
}