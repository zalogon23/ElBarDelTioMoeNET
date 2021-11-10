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
    public BeveragesServices(IMongoDbConfiguration configuration, MongoClient client)
    {
      var database = client.GetDatabase(configuration.DatabaseName);
      _beverages = database.GetCollection<Beverage>(configuration.BeveragesCollectionName);
    }

    public async Task<List<Beverage>> GetBeverages()
    {
      var beverages = await _beverages.Find(x => true).ToListAsync();
      return beverages;
    }
    public async Task<List<Beverage>> GetBeveragesByIds(List<string> ids)
    {
      var beverages = await _beverages.Find(x => ids.Contains(x.Id)).ToListAsync();
      return beverages;
    }
    public async Task<Beverage> GetBeverage(string id)
    {
      var beverage = await _beverages.Find(bev => bev.Id == id).FirstOrDefaultAsync();
      return beverage;
    }
    public async Task<Beverage> CreateBeverage(Beverage beverage)
    {
      await _beverages.InsertOneAsync(beverage);
      return beverage;
    }
    public async Task<bool> RemoveBeverage(string beverageId)
    {
      var response = await _beverages.DeleteOneAsync(x => x.Id == beverageId);
      bool isDone = response.DeletedCount > 0;
      return isDone;
    }
    public async Task<List<Beverage>> GetCreatedBeveragesByUserId(string userId)
    {
      var beverages = await _beverages.Find(x => x.Creator == userId).ToListAsync();
      return beverages;
    }

    public BeverageGraph ConvertToGraphBeverage(
      Beverage beverage,
      List<Keyword> keywords,
      List<Ingredient> ingredients,
      List<Classification> classifications,
      List<Instruction> instructions
    )
    {
      var completeBeverage = new BeverageGraph
      {
        Id = beverage.Id,
        Name = beverage.Name,
        Description = beverage.Description,
        Creator = beverage.Creator,
        Image = beverage.Image,
        Native = beverage.Native,
        Keywords = new List<Keyword>(),
        Ingredients = new List<Ingredient>(),
        Instructions = new List<Instruction>()
      };
      foreach (var classification in classifications)
      {
        var keyword = keywords.Find(x => x.Id == classification.KeywordId);
        if (keyword is null) continue;
        completeBeverage.Keywords.Add(keyword);
      }
      foreach (var ingredient in ingredients)
      {
        completeBeverage.Ingredients.Add(ingredient);
      }
      foreach (var instruction in instructions)
      {
        completeBeverage.Instructions.Add(instruction);
      }
      return completeBeverage;
    }

    public List<BeverageGraph> ConvertToGraphBeverages(
      List<Beverage> beverages,
      List<Keyword> keywords,
      List<Ingredient> ingredients,
      List<Classification> classifications,
      List<Instruction> instructions
    )
    {
      var completeBeverages = new List<BeverageGraph>();
      foreach (var beverage in beverages)
      {
        completeBeverages.Add(
          new BeverageGraph
          {
            Id = beverage.Id,
            Name = beverage.Name,
            Description = beverage.Description,
            Creator = beverage.Creator,
            Image = beverage.Image,
            Native = beverage.Native,
            Keywords = new List<Keyword>(),
            Ingredients = new List<Ingredient>(),
            Instructions = new List<Instruction>()
          }
        );
      }
      foreach (var classification in classifications)
      {
        var beverage = completeBeverages.Find(x => x.Id == classification.BeverageId);
        var keyword = keywords.Find(x => x.Id == classification.KeywordId);
        if (beverage is null || keyword is null) continue;
        beverage.Keywords.Add(keyword);
      }
      foreach (var ingredient in ingredients)
      {
        var beverage = completeBeverages.Find(x => x.Id == ingredient.BeverageId);
        if (beverage is null) continue;
        beverage.Ingredients.Add(ingredient);
      }
      foreach (var instruction in instructions)
      {
        var beverage = completeBeverages.Find(x => x.Id == instruction.BeverageId);
        if (beverage is null) continue;
        beverage.Instructions.Add(instruction);
      }
      return completeBeverages;
    }
  }
}