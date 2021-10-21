using System.ComponentModel.DataAnnotations;
using GraphQL;

namespace backend.Dtos
{
  public class GraphQLRequestDto
  {
      [Required]
    public string Query { get; set; }
    public Inputs Variables { get; set; }
  }
}