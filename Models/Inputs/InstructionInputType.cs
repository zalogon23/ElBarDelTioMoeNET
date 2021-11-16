using GraphQL.Types;

namespace backend.Models.Inputs
{
  public class InstructionInputType : InputObjectGraphType
  {
    public InstructionInputType()
    {
      Name = "InstructionInput";
      Field<NonNullGraphType<StringGraphType>>("content");
      Field<NonNullGraphType<IntGraphType>>("order");
      Field<StringGraphType>("beverageId");
    }
  }
}