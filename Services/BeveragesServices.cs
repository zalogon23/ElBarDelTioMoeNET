using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Services
{
  public class BeveragesServices
  {
    private readonly IMongoCollection<Beverage> _beverages;
    public BeveragesServices(IMongoDbConfiguration configuration)
    {
      var client = new MongoClient(configuration.ConnectionString);
      var database = client.GetDatabase(configuration.DatabaseName);
      _beverages = database.GetCollection<Beverage>(configuration.BeveragesCollectionName);
    }

    public async Task<List<Beverage>> GetBeverages()
    {
      var beverages = await _beverages.Find(x => true).ToListAsync();
      return beverages;
    }
    public async Task<Beverage> GetBeverage(string id)
    {
      Console.WriteLine(id);
      var beverage = await _beverages.Find(bev => bev.Id == id).FirstOrDefaultAsync();
      return beverage;
    }
    public async Task<Beverage> CreateBeverage(Beverage beverage)
    {
      await _beverages.InsertOneAsync(beverage);
      Console.WriteLine(beverage.Id);
      return beverage;
    }
  }
}