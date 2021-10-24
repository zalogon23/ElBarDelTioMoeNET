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
  }
}