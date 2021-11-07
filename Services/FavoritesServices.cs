using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Services
{
  public class FavoritesServices
  {
    private readonly IMongoCollection<Favorite> favorites;
    public FavoritesServices(IMongoDbConfiguration configuration)
    {
      var client = new MongoClient(configuration.ConnectionString);
      var database = client.GetDatabase(configuration.DatabaseName);
      favorites = database.GetCollection<Favorite>(configuration.FavoritesCollectionName);
    }

    public async Task<List<Favorite>> GetFavoritesByUserId(string userId)
    {
      var allFavorites = await favorites.Find(x => x.UserId == userId).ToListAsync();
      return allFavorites;
    }
    public async Task<Favorite> CreateFavorite(string userId, string beverageId)
    {
      var favorite = new Favorite
      {
        Id = null,
        BeverageId = beverageId,
        UserId = userId
      };
      await favorites.InsertOneAsync(favorite);
      return favorite;
    }
  }
}