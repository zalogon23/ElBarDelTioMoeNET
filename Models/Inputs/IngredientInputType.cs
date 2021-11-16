using GraphQL.Types;

namespace backend.Models.Inputs
{
  public class IngredientInputType : InputObjectGraphType
  {
    public IngredientInputType()
    {
      Name = "IngredientInput";
      Field<NonNullGraphType<StringGraphType>>("product");
      Field<NonNullGraphType<FloatGraphType>>("quantity");
      Field<NonNullGraphType<StringGraphType>>("measure");
      Field<StringGraphType>("beverageId");
    }
  }
}