using System;
using System.Threading.Tasks;
using backend.Graphs.GraphTypes;
using backend.Models;
using backend.Services;
using GraphQL;
using GraphQL.Types;

namespace backend.Graphs.Mutations
{
  public class Mutation : ObjectGraphType
  {
    public Mutation(BeveragesServices beverages, KeywordsServices keywords)
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
      FieldAsync<KeywordType>(
        "createKeyword",
        arguments: new QueryArguments(
          new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "content" }
        ),
        resolve: async context =>
        {
          var keyword = await keywords.CreateKeyword(context.GetArgument<string>("content"));
          return keyword;
        }
      );
    }
  }
}