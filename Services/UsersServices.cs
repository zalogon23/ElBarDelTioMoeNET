using System;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Services
{
  public class UsersServices
  {
    private readonly IMongoCollection<User> _users;
    public UsersServices(IMongoDbConfiguration configuration, MongoClient client)
    {
      var database = client.GetDatabase(configuration.DatabaseName);
      _users = database.GetCollection<User>(configuration.UsersCollectionName);
    }
    public async Task<User> CreateUser(User user)
    {
      try
      {
        await _users.InsertOneAsync(user);
        return user;
      }
      catch (MongoException e)
      {
        Console.WriteLine(e.Message);
        return null;
      }
    }
    public async Task<User> GetUserById(string id)
    {
      try
      {
        var user = await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
        return user;
      }
      catch (MongoException e)
      {
        Console.WriteLine(e.Message);
        return null;
      }
    }
    public async Task<User> GetUserByLogin(string username, string password)
    {
      try
      {
        User user = await _users.Find(x => x.Username == username).FirstAsync();
        if (user is null)
        {
          return null;
        }
        bool isPasswordValid = SaltHandler.Match(password: password, hash: user.Password);

        if (!isPasswordValid)
        {
          return null;
        }
        return user;
      }
      catch (MongoException e)
      {
        Console.WriteLine(e.Message);
        return null;
      }
    }
  }
}