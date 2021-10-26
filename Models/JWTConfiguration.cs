namespace backend.Models
{
  public class JWTConfiguration : IJWTConfiguration
  {
    public string SecretKey { get; set; }
  }
  public interface IJWTConfiguration
  {
    string SecretKey { get; set; }

  }

}