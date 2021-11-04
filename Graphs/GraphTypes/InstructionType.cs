using backend.Models;
using GraphQL.Types;

namespace backend.Graphs.GraphTypes
{
  public class InstructionType : ObjectGraphType<Instruction>
  {
    public InstructionType()
    {
      Field(x => x.Id).Description("The Id of the Instruction");
      Field(x => x.Content).Description("The content of the Instruction");
      Field(x => x.Order).Description("The number of order of the Instruction");
      Field(x => x.BeverageId).Description("The Id of the beverage that owns the Instruction");
    }
  }
}