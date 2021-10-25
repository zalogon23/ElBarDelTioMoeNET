using GraphQL.Types;

namespace backend.Models.Inputs
{
  public class UserInputType : InputObjectGraphType
  {
    public UserInputType()
    {
      Name = "UserInput";
      Field<NonNullGraphType<StringGraphType>>("username");
      Field<NonNullGraphType<StringGraphType>>("password");
      Field<NonNullGraphType<StringGraphType>>("description");
      Field<NonNullGraphType<StringGraphType>>("avatar");
    }
  }
}