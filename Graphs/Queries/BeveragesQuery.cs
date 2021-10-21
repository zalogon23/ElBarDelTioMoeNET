using System;
using backend.Graphs.GraphTypes;
using backend.Models;
using backend.Services;
using GraphQL.Types;

namespace backend.Graphs.Queries
{
  public class BeveragesQuery : ObjectGraphType
  {
    public BeveragesQuery(BeveragesServices beverages)
    {
      FieldAsync<ListGraphType<BeverageType>>(
          "beverages",
          resolve: async context =>
          {
            var allBeverages = await beverages.GetBeverages();
            return allBeverages;
          }
      );
      FieldAsync<BeverageType>(
          "beverage",
          resolve: async context =>
          {
            var beverage = await beverages.GetBeverage();
            Console.WriteLine(beverage.Name);
            return beverage;
          }
      );
    }
  }
}