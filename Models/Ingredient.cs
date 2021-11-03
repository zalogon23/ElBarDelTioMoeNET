using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
  public class Ingredient
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Product { get; set; }
    public float Quantity { get; set; }
    public string Measure { get; set; }
    public string BeverageId { get; set; }
  }
}