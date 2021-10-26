using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Services
{
  public class UsersServices
  {
    private readonly IMongoCollection<User> _users;
    public UsersServices(IMongoDbConfiguration configuration)
    {
      var client = new MongoClient(configuration.ConnectionString);
      var database = client.GetDatabase(configuration.DatabaseName);
      _users = database.GetCollection<User>(configuration.UsersCollectionName);
    }
    public async Task<User> CreateUser(User user)
    {
      await _users.InsertOneAsync(user);
      return user;
    }
    public async Task<User> GetUserById(string id)
    {
      var user = await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
      return user;
    }
    public async Task<User> GetUserByLogin(string username, string password)
    {
      var user = await _users.Find(x => (x.Username == username && x.Password == password)).FirstOrDefaultAsync();
      return user;
    }
  }
}