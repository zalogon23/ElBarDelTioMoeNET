using backend.Dtos;
using GraphQL.Types;

namespace backend.Graphs.GraphTypes
{
  public class LoggedUserDtoType : ObjectGraphType<LoggedUserDto>
  {
    public LoggedUserDtoType()
    {
      Field(x => x.Id).Description("The id of the user.");
      Field(x => x.Username).Description("The username of the user.");
      Field(x => x.Password).Description("The password of the user.");
      Field(x => x.Description).Description("The description of the user.");
      Field(x => x.Avatar).Description("The avatar of the user.");
      Field(x => x.Token).Description("The token of the user.");
      Field(x => x.RefreshToken).Description("The refresh-token of the user.");
    }
  }
}