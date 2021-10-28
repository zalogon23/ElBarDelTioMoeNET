using System;
using System.Web;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Graphs.Mutations;
using backend.Graphs.Queries;
using backend.Models;
using backend.Services;
using GraphQL;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace backend.Controllers
{
  [ApiController]
  [Route("api")]
  public class GraphController : ControllerBase
  {
    private readonly IJWTConfiguration _configuration;
    private readonly BeveragesServices _beverages;
    private readonly UsersServices _users;
    private readonly KeywordsServices _keywords;
    private readonly RefreshTokensServices _refreshTokens;
    private readonly ClassificationsServices _classifications;
    public GraphController(
      BeveragesServices beverages,
      KeywordsServices keywords,
      ClassificationsServices classifications,
      UsersServices users,
      RefreshTokensServices refreshTokens,
      IJWTConfiguration configuration
      )
    {
      _users = users;
      _beverages = beverages;
      _keywords = keywords;
      _classifications = classifications;
      _configuration = configuration;
      _refreshTokens = refreshTokens;

    }
    [HttpPost("graphql")]
    public async Task<IActionResult> GraphQL(GraphQLRequestDto graphQLRequestDto)
    {
      var schema = new Schema
      {
        Query = new Query(
          beverages: _beverages,
          classifications: _classifications,
          users: _users,
          jwtconfiguration: _configuration
          ),
        Mutation = new Mutation(
          beverages: _beverages,
          keywords: _keywords,
          classifications: _classifications,
          users: _users
          )
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
    [HttpPost("login")]
    public async Task<ActionResult<LoggedUserDto>> Login(LoginDto login)
    {
      string username = login.Username;
      string password = login.Password;
      var user = await _users.GetUserByLogin(username: username, password: password);
      if (user is null)
      {
        return BadRequest();
      }
      string token = TokenHandler.CreateToken(secret: _configuration.SecretKey, userId: user.Id);
      string refreshToken = TokenHandler.CreateRandomToken(secret: _configuration.SecretKey);

      RefreshToken refreshTokenInstance = new RefreshToken()
      {
        Hash = refreshToken,
        User = user.Id,
        Valid = true,
        Expires = DateTime.UtcNow.AddDays(7)
      };

      await _refreshTokens.InvalidAllRefreshTokens(user.Id);
      await _refreshTokens.InsertRefreshToken(refreshTokenInstance);

      CookieOptions options = new CookieOptions();
      options.Expires = DateTime.Now.AddDays(7);
      options.HttpOnly = true;
      HttpContext.Response.Cookies.Append("refresh-token", refreshToken, options);

      var loggedUserDto = new LoggedUserDto()
      {
        Id = user.Id,
        Username = user.Username,
        Password = user.Password,
        Description = user.Description,
        Avatar = user.Avatar,
        Token = token,
      };
      return Ok(loggedUserDto);
    }
  }
}