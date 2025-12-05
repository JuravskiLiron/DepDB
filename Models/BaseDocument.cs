using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DepDB.Models;

public abstract class BaseDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public DateTime UpdatedAtUtc { get; set; } =  DateTime.UtcNow;
}