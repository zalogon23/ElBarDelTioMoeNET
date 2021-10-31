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
using System.Security.Claims;

namespace backend.Controllers
{
  [ApiController]
  [Route("api")]
  public class GraphController : ControllerBase
  {
    private readonly IJWTConfiguration _configuration;
    private readonly UsersServices _users;
    private readonly RefreshTokensServices _refreshTokens;
    private readonly Query _query;
    private readonly Mutation _mutation;
    public GraphController(
      UsersServices users,
      RefreshTokensServices refreshTokens,
      IJWTConfiguration configuration,
      Query query,
      Mutation mutation
      )
    {
      _users = users;
      _refreshTokens = refreshTokens;
      _configuration = configuration;
      _mutation = mutation;
      _query = query;
    }
    [HttpPost("graphql")]
    public async Task<IActionResult> GraphQL(GraphQLRequestDto graphQLRequestDto)
    {
      var schema = new Schema
      {
        Query = _query,
        Mutation = _mutation
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
        Password = "No hay nada que ver aca",
        Description = user.Description,
        Avatar = user.Avatar,
        Token = token,
      };
      return Ok(loggedUserDto);
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
      var previousRefreshToken = HttpContext.Request.Cookies["refresh-token"];
      if (previousRefreshToken is null || previousRefreshToken.Length == 0)
      {
        return Unauthorized();
      }

      var oldRefreshToken = await _refreshTokens.IsValid(previousRefreshToken);
      if (oldRefreshToken is null)
      {
        return Unauthorized();
      }

      string userId = oldRefreshToken.User;

      var refreshToken = new RefreshToken
      {
        Id = null,
        Hash = TokenHandler.CreateRandomToken(_configuration.SecretKey),
        User = userId,
        Valid = true,
        Expires = DateTime.UtcNow.AddDays(7)
      };

      string newToken = TokenHandler.CreateToken(secret: _configuration.SecretKey, userId: userId);

      await _refreshTokens.InsertRefreshToken(refreshToken);
      HttpContext.Response.Cookies.Append("refresh-token", refreshToken.Hash);

      return Ok(newToken);
    }
  }
}