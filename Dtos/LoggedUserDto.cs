using backend.Models;

namespace backend.Dtos
{
  public class LoggedUserDto : User
  {
    public string Token { get; set; }
    public string RefreshToken { get; set; }
  }
}