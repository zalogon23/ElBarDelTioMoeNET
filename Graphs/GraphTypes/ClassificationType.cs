using backend.Models;
using GraphQL.Types;

namespace backend.Graphs.GraphTypes
{
  public class ClassificationType : ObjectGraphType<Classification>
  {
    public ClassificationType()
    {
      Field(x => x.Id).Description("The Id of the Classification.");
      Field(x => x.BeverageId).Description("The Id of the classified Beverage.");
      Field(x => x.KeywordId).Description("The Id of the classified Keyword.");
    }
  }
}