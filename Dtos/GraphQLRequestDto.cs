using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using GraphQL;

namespace backend.Dtos
{
  public class GraphQLRequestDto
  {
    [Required]
    public string Query { get; set; }
    public JsonElement Variables { get; set; }
  }
}