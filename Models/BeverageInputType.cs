using GraphQL.Types;

namespace backend.Models
{
  public class BeverageInputType : InputObjectGraphType
  {
    public BeverageInputType()
    {
      Name = "BeverageInput";
      Field<NonNullGraphType<StringGraphType>>("name");
      Field<NonNullGraphType<StringGraphType>>("description");
      Field<NonNullGraphType<StringGraphType>>("image");
      Field<NonNullGraphType<BooleanGraphType>>("native");
    }
  }
}