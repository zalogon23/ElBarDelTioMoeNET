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
       IngredientsServices ingredients,
       InstructionsServices instructions
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
          var completeBeverage = beverages.ConvertToGraphBeverage(
            beverage: beverage,
            keywords: new List<Keyword>(),
            ingredients: new List<Ingredient>(),
            classifications: new List<Classification>(),
            instructions: new List<Instruction>()
          );
          return completeBeverage;
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
          var passedUser = new User
          {
            Id = null,
            Username = argumentUser.Username,
            Password = hashedPassword,
            Description = argumentUser.Description,
            Avatar = argumentUser.Avatar
          };
          var user = await users.CreateUser(passedUser);
          return new UserGraph
          {
            Id = user.Id,
            Username = user.Username,
            Description = user.Description,
            Password = "No hay nada que ver aca",
            Avatar = user.Avatar,
            FavoriteBeverages=new List<Beverage>(),
            CreatedBeverages=new List<Beverage>()
          };
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
      FieldAsync<ListGraphType<InstructionType>>(
        "createInstructions",
        arguments: new QueryArguments(
          new QueryArgument<NonNullGraphType<ListGraphType<InstructionsInputType>>> { Name = "instructions" }
        ),
        resolve: async context =>
        {
          var passedInstructions = context.GetArgument<List<Instruction>>("instructions");
          if (passedInstructions[0] is null)
          {
            return null;
          }
          await instructions.RemoveInstructionsByBeverageId(passedInstructions[0].BeverageId);
          var createdInstructiosn = await instructions.CreateInstructions(passedInstructions);
          return createdInstructiosn;
        }
      );
    }
  }
}