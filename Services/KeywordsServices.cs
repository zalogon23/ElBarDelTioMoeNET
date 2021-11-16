using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Services
{
  public class KeywordsServices
  {
    private readonly IMongoCollection<Keyword> _keywords;

    public KeywordsServices(IMongoDbConfiguration configuration, MongoClient client)
    {

      var database = client.GetDatabase(configuration.DatabaseName);
      _keywords = database.GetCollection<Keyword>(configuration.KeywordsCollectionName);
    }

    public async Task<Keyword> CreateKeyword(string content)
    {
      var keyword = new Keyword
      {
        Id = null,
        Content = content
      };
      await _keywords.InsertOneAsync(keyword);
      return keyword;
    }
    public async Task<List<Keyword>> GetKeywords()
    {
      var keywords = await _keywords.Find(x => true).ToListAsync();
      return keywords;
    }

    public async Task<List<Keyword>> GetKeywords(List<string> ids)
    {
      var keywords = await _keywords.Find(x => ids.Contains(x.Id)).ToListAsync();
      return keywords;
    }
  }
}