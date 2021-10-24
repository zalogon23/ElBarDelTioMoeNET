namespace backend.Models
{
  public class MongoDBConfiguration : IMongoDbConfiguration
  {
    public string BeveragesCollectionName { get; set; }
    public
string KeywordsCollectionName
    { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
  }

  public interface IMongoDbConfiguration
  {
    string BeveragesCollectionName { get; set; }
    string KeywordsCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
  }
}