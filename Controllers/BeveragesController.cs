using System;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Graphs.Queries;
using backend.Services;
using GraphQL;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
  [ApiController]
  [Route("api")]
  public class BeveragesController : ControllerBase
  {
    private readonly BeveragesServices _beverages;
    public BeveragesController(BeveragesServices beverages)
    {
      _beverages = beverages;
    }
    [HttpPost("graphql")]
    public async Task<IActionResult> GraphQL(GraphQLRequestDto graphQLRequestDto)
    {
      var schema = new Schema { Query = new BeveragesQuery(_beverages) };
      var json = await schema.ExecuteAsync(_ =>
      {
        _.Query = graphQLRequestDto.Query;
        _.Inputs = graphQLRequestDto.Variables;
      });

      if (json is null)
      {
        return BadRequest();
      }
      else
      {
        return Ok(json);
      }
    }
  }
}