using System;
using System.Collections.Generic;
using backend.Dtos;
using backend.Graphs.GraphTypes;
using backend.Models;
using backend.Services;
using GraphQL;
using GraphQL.Types;

namespace backend.Graphs.Queries
{
  public class Query : ObjectGraphType
  {
    public Query(
      BeveragesServices beverages,
      ClassificationsServices classifications,
      UsersServices users,
      KeywordsServices keywords,
      IJWTConfiguration jwtconfiguration
      )
    {
      FieldAsync<UserType>(
        "user",
        arguments: new QueryArguments(
          new QueryArgument<StringGraphType> { Name = "id" }
        ),
        resolve: async context =>
        {
          string userId = context.GetArgument<string>("id");
          var user = await users.GetUserById(userId);
          //Hide password
          user.Password = "No hay nada que ver aca";
          return user;

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
      FieldAsync<ListGraphType<KeywordType>>(
        "keywords",
        resolve: async context =>
        {
          var keywordsFound = await keywords.GetKeywords();
          return keywordsFound;
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