using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
  public class Beverage
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public string Creator { get; set; }
    public bool Native { get; set; }
  }

  public class BeverageGraph : Beverage
  {
    public List<Ingredient> Ingredients { get; set; }
    public List<Keyword> Keywords { get; set; }
    public List<Instruction> Instructions { get; set; }
  }
}