using backend.Models;
using GraphQL.Types;

namespace backend.Graphs.GraphTypes
{
  public class UserType : ObjectGraphType<User>
  {
    public UserType()
    {
      Field(x => x.Id).Description("The ID of the User");
      Field(x => x.Username).Description("The Username of the User");
      Field(x => x.Password).Description("The Password of the User");
      Field(x => x.Description).Description("The Description of the User");
      Field(x => x.Avatar).Description("The Avatar url of the User");
    }
  }
}