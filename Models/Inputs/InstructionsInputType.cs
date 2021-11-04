using GraphQL.Types;

namespace backend.Models.Inputs
{
  public class InstructionsInputType : InputObjectGraphType
  {
    public InstructionsInputType()
    {
      Name = "InstructionInput";
      Field<NonNullGraphType<StringGraphType>>("content");
      Field<NonNullGraphType<IntGraphType>>("order");
      Field<NonNullGraphType<StringGraphType>>("beverageId");
    }
  }
}