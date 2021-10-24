using System;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Graphs.Mutations;
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
    private readonly KeywordsServices _keywords;
    private readonly ClassificationsServices _classifications;
    public BeveragesController(BeveragesServices beverages, KeywordsServices keywords, ClassificationsServices classifications)
    {
      _beverages = beverages;
      _keywords = keywords;
      _classifications = classifications;
    }
    [HttpPost("graphql")]
    public async Task<IActionResult> GraphQL(GraphQLRequestDto graphQLRequestDto)
    {
      var schema = new Schema
      {
        Query = new Query(beverages: _beverages, classifications: _classifications),
        Mutation = new Mutation(beverages: _beverages, keywords: _keywords, classifications: _classifications),
      };
      var inputs = graphQLRequestDto.Variables.ToInputs();
      var json = await schema.ExecuteAsync(_ =>
      {
        _.Query = graphQLRequestDto.Query;
        _.Inputs = inputs;
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