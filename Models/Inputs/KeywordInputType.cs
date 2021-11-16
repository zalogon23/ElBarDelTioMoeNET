using GraphQL.Types;

namespace backend.Models.Inputs
{
  public class KeywordInputType : InputObjectGraphType
  {
    public KeywordInputType()
    {
      Name = "KeywordInput";
      Field<StringGraphType>("id");
      Field<StringGraphType>("content");
    }
  }
}