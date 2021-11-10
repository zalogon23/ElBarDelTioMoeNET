using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Services
{
  public class IngredientsServices
  {
    private readonly IMongoCollection<Ingredient> _ingredients;

    public IngredientsServices(IMongoDbConfiguration configuration, MongoClient client)
    {
      var database = client.GetDatabase(configuration.DatabaseName);
      _ingredients = database.GetCollection<Ingredient>(configuration.IngredientsCollectionName);
    }

    public async Task<List<Ingredient>> GetIngredients()
    {
      return await _ingredients.Find(x => true).ToListAsync();
    }

    public async Task<List<Ingredient>> GetByBeverageId(string beverageId)
    {
      return await _ingredients.Find(x => x.BeverageId == beverageId).ToListAsync();
    }

    public async Task<List<Ingredient>> CreateIngredients(List<Ingredient> ingredients)
    {
      await _ingredients.InsertManyAsync(ingredients);
      return ingredients;
    }

    public async Task RemoveIngredientsByBeverageId(string beverageId)
    {
      await _ingredients.DeleteManyAsync(x => x.BeverageId == beverageId);
    }
  }
}