using System;
using System.Threading.Tasks;
using backend.Graphs.GraphTypes;
using backend.Models;
using backend.Services;
using GraphQL;
using GraphQL.Types;

namespace backend.Graphs.Mutations
{
  public class BeveragesMutation : ObjectGraphType
  {
    public BeveragesMutation(BeveragesServices beverages)
    {
      FieldAsync<BeverageType>(
            "createBeverage",
            arguments: new QueryArguments(
            new QueryArgument<NonNullGraphType<BeverageInputType>> { Name = "beverage" }
            ),
            resolve: async context =>
            {
              var argumentBeverage = context.GetArgument<Beverage>("beverage");
              var newBeverage = new Beverage
              {
                Id = null,
                Name = argumentBeverage.Name,
                Description = argumentBeverage.Description,
                Image = argumentBeverage.Image,
                Native = argumentBeverage.Native
              };
              var beverage = await beverages.CreateBeverage(newBeverage);
              return beverage;
            }
      );
    }
  }
}