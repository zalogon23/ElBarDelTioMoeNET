using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
      IngredientsServices ingredients,
      FavoritesServices favorites,
      InstructionsServices instructions,
      IJWTConfiguration jwtconfiguration
      )
    {
      FieldAsync<UserType>(
        "self",
        resolve: async context =>
        {
          var userContext = context.UserContext;
          if (userContext is null)
          {
            return null;
          }
          object result;
          userContext.TryGetValue("current", out result);
          if (result is null)
          {
            return null;
          }
          User user = (User)result;

          var allFavorites = await favorites.GetFavoritesByUserId(user.Id);
          List<string> beverageIds = new List<string>();
          foreach (var favorite in allFavorites)
          {
            beverageIds.Add(favorite.BeverageId);
          }
          var createdBeverages = await beverages.GetCreatedBeveragesByUserId(user.Id);
          var favoriteBeverages = await beverages.GetBeveragesByIds(beverageIds);

          var allIngredients = await ingredients.GetIngredients();
          var allKeywords = await keywords.GetKeywords();
          var allClassifications = await classifications.GetClassifications();
          var allInstructions = await instructions.GetInstructions();

          var createdGraphBeverages = beverages.ConvertToGraphBeverages(
              createdBeverages,
              allKeywords,
              allIngredients,
              allClassifications,
              allInstructions
            );
          var favoriteGraphBeverages = beverages.ConvertToGraphBeverages(
              favoriteBeverages,
              allKeywords,
              allIngredients,
              allClassifications,
              allInstructions
            );

          return new UserGraph
          {
            Id = user.Id,
            Username = user.Username,
            Description = user.Description,
            Password = "No hay nada que ver aca",
            Avatar = user.Avatar,
            FavoriteBeverages = favoriteGraphBeverages,
            CreatedBeverages = createdGraphBeverages
          };
        }
      );
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

          var allFavorites = await favorites.GetFavoritesByUserId(user.Id);
          List<string> beverageIds = new List<string>();
          foreach (var favorite in allFavorites)
          {
            beverageIds.Add(favorite.BeverageId);
          }
          var createdBeverages = await beverages.GetCreatedBeveragesByUserId(user.Id);
          var favoriteBeverages = await beverages.GetBeveragesByIds(beverageIds);


          var allIngredients = await ingredients.GetIngredients();
          var allKeywords = await keywords.GetKeywords();
          var allClassifications = await classifications.GetClassifications();
          var allInstructions = await instructions.GetInstructions();

          var createdGraphBeverages = beverages.ConvertToGraphBeverages(
              createdBeverages,
              allKeywords,
              allIngredients,
              allClassifications,
              allInstructions
            );
          var favoriteGraphBeverages = beverages.ConvertToGraphBeverages(
              favoriteBeverages,
              allKeywords,
              allIngredients,
              allClassifications,
              allInstructions
            );

          return new UserGraph
          {
            Id = user.Id,
            Username = user.Username,
            Description = user.Description,
            Password = "No hay nada que ver aca",
            Avatar = user.Avatar,
            FavoriteBeverages = favoriteGraphBeverages,
            CreatedBeverages = createdGraphBeverages
          };
        }
      );
      FieldAsync<ListGraphType<BeverageType>>(
          "beverages",
          resolve: async context =>
          {
            var allBeverages = await beverages.GetBeverages();

            var allIngredients = await ingredients.GetIngredients();
            var allKeywords = await keywords.GetKeywords();
            var allClassifications = await classifications.GetClassifications();
            var allInstructions = await instructions.GetInstructions();

            var allBeveragesGraph = beverages.ConvertToGraphBeverages(
              allBeverages,
              allKeywords,
              allIngredients,
              allClassifications,
              allInstructions
            );
            return allBeveragesGraph;
          }
      );
      FieldAsync<ListGraphType<ClassificationType>>(
          "classifications",
          resolve: async context =>
          {
            var allClassifications = await classifications.GetClassifications();
            return allClassifications;
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
            var allIngredients = await ingredients.GetByBeverageId(beverage.Id);
            var allKeywords = await keywords.GetKeywords();
            var allClassifications = await classifications.GetClassifications();
            var allInstructions = await instructions.GetInstructionsByBeverageId(beverage.Id);
            return beverages.ConvertToGraphBeverage(
              beverage: beverage,
              ingredients: allIngredients,
              keywords: allKeywords,
              classifications: allClassifications,
              instructions: allInstructions
            );
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
          var allBeverages = await beverages.GetBeveragesByIds(beverageIds);
          var allIngredients = await ingredients.GetIngredients();
          var allKeywords = await keywords.GetKeywords();
          var allClassifications = await classifications.GetClassifications();
          var allInstructions = await instructions.GetInstructions();
          return beverages.ConvertToGraphBeverages(
            beverages: allBeverages,
            ingredients: allIngredients,
            instructions: allInstructions,
            classifications: allClassifications,
            keywords: allKeywords
          );
        }
      );
    }
  }
}