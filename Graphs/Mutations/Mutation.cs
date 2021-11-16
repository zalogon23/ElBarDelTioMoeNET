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
       FavoritesServices favorites,
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
          try
          {
            var argumentBeverage = context.GetArgument<BeverageGraph>("beverage");
            var newBeverage = new Beverage
            {
              Id = null,
              Name = argumentBeverage.Name,
              Description = argumentBeverage.Description,
              Image = argumentBeverage.Image,
              Creator = argumentBeverage.Creator,
              Native = argumentBeverage.Native
            };
            var beverage = await beverages.CreateBeverage(newBeverage);
            foreach (var ingredient in argumentBeverage.Ingredients)
            {
              ingredient.BeverageId = beverage.Id;
            }
            foreach (var instruction in argumentBeverage.Instructions)
            {
              instruction.BeverageId = beverage.Id;
            }
            var createdIngredients = await ingredients.CreateIngredients(argumentBeverage.Ingredients);
            var createdInstructions = await instructions.CreateInstructions(argumentBeverage.Instructions);

            var newClassifications = new List<Classification>();
            foreach (var keyword in argumentBeverage.Keywords)
            {
              var newClassification = new Classification
              {
                Id = null,
                BeverageId = beverage.Id,
                KeywordId = keyword.Id
              };
              newClassifications.Add(newClassification);
            };

            var createdClassifications = await classifications.CreateClassifications(newClassifications);
            var argumentKeywords = new List<string>();
            foreach (var keyword in argumentBeverage.Keywords)
            {
              argumentKeywords.Add(keyword.Id);
            }
            var selectedKeywords = await keywords.GetKeywords(argumentKeywords);

            var completeBeverage = new BeverageGraph
            {
              Id = beverage.Id,
              Name = beverage.Name,
              Creator = beverage.Creator,
              Description = beverage.Description,
              Native = beverage.Native,
              Ingredients = createdIngredients,
              Instructions = createdInstructions,
              Keywords = selectedKeywords
            };
            return completeBeverage;
          }
          catch (Exception e)
          {
            Console.WriteLine(e.StackTrace);
            return null;
          }
          finally
          {
          }
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
      FieldAsync<BooleanGraphType>(
        "createFavorite",
        arguments: new QueryArguments(
          new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "userId" },
          new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "beverageId" }
        ),
        resolve: async context =>
        {
          var userId = context.GetArgument<string>("userId");
          var beverageId = context.GetArgument<string>("beverageId");
          if (userId.Length == 0 || beverageId.Length == 0) return false;
          await favorites.CreateFavorite(userId: userId, beverageId: beverageId);
          return true;
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
            FavoriteBeverages = new List<BeverageGraph>(),
            CreatedBeverages = new List<BeverageGraph>()
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
          new QueryArgument<NonNullGraphType<ListGraphType<InstructionInputType>>> { Name = "instructions" }
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