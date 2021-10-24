using backend.Models;
using GraphQL.Types;

namespace backend.Graphs.GraphTypes
{
  public class KeywordType : ObjectGraphType<Keyword>
  {
    public KeywordType()
    {
      Field(x => x.Id).Description("The Id of the Keyword.");
      Field(x => x.Content).Description("The Keyword Content.");
    }
  }
}