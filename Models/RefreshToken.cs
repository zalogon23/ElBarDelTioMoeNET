using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
  public class RefreshToken
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Hash { get; set; }
    public string User { get; set; }
    public DateTime Expires { get; set; }
    public bool Valid { get; set; }
  }
}