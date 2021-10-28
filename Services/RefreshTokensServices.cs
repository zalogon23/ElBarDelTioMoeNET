using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Services
{
  public class RefreshTokensServices
  {
    private readonly IMongoCollection<RefreshToken> _refreshTokens;
    public RefreshTokensServices(IMongoDbConfiguration configuration)
    {
      var client = new MongoClient(configuration.ConnectionString);
      var database = client.GetDatabase(configuration.DatabaseName);
      _refreshTokens = database.GetCollection<RefreshToken>(configuration.RefreshTokensCollectionName);
    }

    public async Task<RefreshToken> InsertRefreshToken(RefreshToken refreshToken)
    {
      await _refreshTokens.InsertOneAsync(refreshToken);
      return refreshToken;
    }

    public async Task InvalidAllRefreshTokens(string userId)
    {

      UpdateDefinition<RefreshToken> update = Builders<RefreshToken>.Update
  .Set(r => r.Valid, false);

      await _refreshTokens.UpdateManyAsync(
          x => (x.User == userId),
          update);
    }
  }
}