using System;

namespace backend.Models
{
  public class RefreshToken
  {
    public string Hash { get; set; }
    public string User { get; set; }
    public DateTime Expires { get; set; }
    public bool Valid { get; set; }
  }
}