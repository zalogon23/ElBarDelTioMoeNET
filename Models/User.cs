using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace backend.Models
{
  public class User
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Description { get; set; }
    public string Avatar { get; set; }
  }
  public class UserGraph : User
  {
    public List<BeverageGraph> FavoriteBeverages { get; set; }
    public List<BeverageGraph> CreatedBeverages { get; set; }
  }
}