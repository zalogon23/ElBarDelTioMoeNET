using backend.Graphs.GraphTypes;
using backend.Models.Inputs;
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
      Field<StringGraphType>("creator");
      Field<NonNullGraphType<BooleanGraphType>>("native");
      Field<NonNullGraphType<ListGraphType<IngredientInputType>>>("ingredients");
      Field<NonNullGraphType<ListGraphType<InstructionInputType>>>("instructions");
      Field<NonNullGraphType<ListGraphType<KeywordInputType>>>("keywords");
    }
  }
}