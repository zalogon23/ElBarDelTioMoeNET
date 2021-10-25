using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Services
{
  public class ClassificationsServices
  {
    private readonly IMongoCollection<Classification> _classifications;
    public ClassificationsServices(IMongoDbConfiguration configuration)
    {
      var client = new MongoClient(configuration.ConnectionString);
      var database = client.GetDatabase(configuration.DatabaseName);
      _classifications = database.GetCollection<Classification>(configuration.ClassificationsCollectionName);
    }
    public async Task<Classification> CreateClassification(string beverageId, string keywordId)
    {
      var classification = new Classification
      {
        Id = null,
        BeverageId = beverageId,
        KeywordId = keywordId
      };
      await _classifications.InsertOneAsync(classification);
      return classification;
    }
    public async Task<List<Classification>> GetClassificationsByKeyword(string keywordId)
    {
      var classifications = await _classifications.Find(x => x.KeywordId == keywordId).ToListAsync();
      return classifications;
    }
    public async Task<bool> RemoveClassification(string keywordId, string beverageId)
    {
      var response = await _classifications.DeleteOneAsync(x => (x.KeywordId == keywordId && x.BeverageId == beverageId));
      bool isDone = response.DeletedCount > 0;
      return isDone;
    }
    public async Task<bool> RemoveClassificationsByKeyword(string keywordId)
    {
      var response = await _classifications.DeleteOneAsync(x => x.KeywordId == keywordId);
      bool isDone = response.DeletedCount > 0;
      return isDone;
    }
    public async Task<bool> RemoveClassificationsByBeverage(string beverageId)
    {
      var response = await _classifications.DeleteOneAsync(x => x.BeverageId == beverageId);
      bool isDone = response.DeletedCount > 0;
      return isDone;
    }
  }
}