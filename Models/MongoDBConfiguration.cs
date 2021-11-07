namespace backend.Models
{
  public class MongoDBConfiguration : IMongoDbConfiguration
  {
    public string BeveragesCollectionName { get; set; }
    public string UsersCollectionName { get; set; }
    public string RefreshTokensCollectionName { get; set; }
    public string IngredientsCollectionName { get; set; }
    public string InstructionsCollectionName { get; set; }
    public string FavoritesCollectionName { get; set; }
    public string KeywordsCollectionName { get; set; }
    public string ClassificationsCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
  }

  public interface IMongoDbConfiguration
  {
    string BeveragesCollectionName { get; set; }
    string UsersCollectionName { get; set; }
    string RefreshTokensCollectionName { get; set; }
    string IngredientsCollectionName { get; set; }
    string InstructionsCollectionName { get; set; }
    string FavoritesCollectionName { get; set; }
    string KeywordsCollectionName { get; set; }
    string ClassificationsCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
  }
}