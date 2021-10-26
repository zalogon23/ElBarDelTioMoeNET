using System;
using System.Collections.Generic;
using backend.Graphs.GraphTypes;
using backend.Models;
using backend.Services;
using GraphQL;
using GraphQL.Types;

namespace backend.Graphs.Queries
{
  public class Query : ObjectGraphType
  {
    public Query(BeveragesServices beverages, ClassificationsServices classifications, UsersServices users)
    {
      FieldAsync<UserType>(
        "user",
        arguments: new QueryArguments(
          new QueryArgument<StringGraphType> { Name = "id" },
          new QueryArgument<StringGraphType> { Name = "username" },
          new QueryArgument<StringGraphType> { Name = "password" }
        ),
        resolve: async context =>
        {
          string userId = context.GetArgument<string>("id");
          if (userId != null)
          {
            var user = await users.GetUserById(userId);
            return user;
          }
          else
          {
            string username = context.GetArgument<string>("username");
            string password = context.GetArgument<string>("password");
            var user = await users.GetUserByLogin(username: username, password: password);
            return user;
          }
        }
      );
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
          arguments: new QueryArguments(
            new QueryArgument<NonNullGraphType<StringGraphType>>
            {
              Name = "id",
              Description = "The ID of the required beverage."
            }
          ),
          resolve: async context =>
          {
            var beverage = await beverages.GetBeverage(context.GetArgument<string>("id"));
            return beverage;
          }
      );
      FieldAsync<ListGraphType<BeverageType>>(
        "beveragesByKeyword",
        arguments: new QueryArguments(
          new QueryArgument<StringGraphType> { Name = "keywordId" }
        ),
        resolve: async context =>
        {
          var matchingClassifications = await classifications.GetClassificationsByKeyword(
            context.GetArgument<string>("keywordId")
          );
          List<string> beverageIds = new List<string> { };
          foreach (var classification in matchingClassifications)
          {
            beverageIds.Add(classification.BeverageId);
          }
          var classifiedBeverages = await beverages.GetBeveragesByIds(beverageIds);
          return classifiedBeverages;
        }
      );
    }
  }
}