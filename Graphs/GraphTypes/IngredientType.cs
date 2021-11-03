using backend.Models;
using GraphQL.Types;

namespace backend.Graphs.GraphTypes
{
  public class IngredientType : ObjectGraphType<Ingredient>
  {
    public IngredientType()
    {
      Field(x => x.Id).Description("The Id of the Ingredient");
      Field(x => x.Product).Description("The product of the Ingredient");
      Field(x => x.Quantity).Description("The quantity of the Ingredient");
      Field(x => x.Measure).Description("The measure of the Ingredient");
      Field(x => x.BeverageId).Description("The Id of the Ingredient's beverage");
    }
  }
}