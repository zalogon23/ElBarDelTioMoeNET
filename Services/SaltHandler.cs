using BCryptNet = BCrypt.Net.BCrypt;
namespace backend.Services
{
  static public class SaltHandler
  {
    static public string GetHash(string password)
    {
      string hash = BCryptNet.HashPassword(password);
      return hash;
    }
    static public bool Match(string password, string hash)
    {
      bool match = BCryptNet.Verify(password, hash);
      return match;
    }
  }
}