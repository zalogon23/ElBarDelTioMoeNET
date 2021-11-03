using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Graphs.GraphTypes;
using backend.Models;
using backend.Models.Inputs;
using backend.Services;
using GraphQL;
using GraphQL.Types;

namespace backend.Graphs.Mutations
{
  public class Mutation : ObjectGraphType
  {
    public Mutation(
      BeveragesServices beverages,
       KeywordsServices keywords,
       ClassificationsServices classifications,
       UsersServices users,
       IngredientsServices ingredients
       )
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
      FieldAsync<BooleanGraphType>(
        "removeBeverage",
        arguments: new QueryArguments(
          new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "beverageId" }
        ),
        resolve: async context =>
        {
          string beverageId = context.GetArgument<string>("beverageId");
          bool isRemoved = await beverages.RemoveBeverage(beverageId);
          if (isRemoved)
          {
            bool areRemovedClassification = await classifications.RemoveClassificationsByBeverage(beverageId);
          }
          return isRemoved;
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
      FieldAsync<ClassificationType>(
        "createClassification",
        arguments: new QueryArguments(
          new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "beverageId" },
          new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "keywordId" }
        ),
        resolve: async context =>
        {
          string beverageId = context.GetArgument<string>("beverageId");
          string keywordId = context.GetArgument<string>("keywordId");
          var classification = await classifications.CreateClassification(beverageId: beverageId, keywordId: keywordId);
          return classification;
        }
      );
      FieldAsync<UserType>(
        "createUser",
        arguments: new QueryArguments(
          new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
        ),
        resolve: async context =>
        {
          var argumentUser = context.GetArgument<User>("user");
          string hashedPassword = SaltHandler.GetHash(argumentUser.Password);
          var user = new User
          {
            Id = null,
            Username = argumentUser.Username,
            Password = hashedPassword,
            Description = argumentUser.Description,
            Avatar = argumentUser.Avatar
          };
          var createdUser = await users.CreateUser(user);
          //Hide password
          createdUser.Password = "No hay nada que ver aca";
          return createdUser;
        }
      );
      FieldAsync<ListGraphType<IngredientType>>(
        "createIngredients",
        arguments: new QueryArguments(
          new QueryArgument<NonNullGraphType<ListGraphType<IngredientInputType>>> { Name = "ingredients" }
        ),
        resolve: async context =>
        {
          var passedIngredients = context.GetArgument<List<Ingredient>>("ingredients");
          if (passedIngredients[0] is null)
          {
            return null;
          }
          await ingredients.RemoveIngredientsByBeverageId(passedIngredients[0].BeverageId);
          var createdIngredients = await ingredients.CreateIngredients(passedIngredients);
          return createdIngredients;
        }
      );
    }
  }
}