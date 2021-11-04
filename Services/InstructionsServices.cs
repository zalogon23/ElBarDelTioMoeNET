using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Services
{
  public class InstructionsServices
  {
    private readonly IMongoCollection<Instruction> _instructions;

    public InstructionsServices(IMongoDbConfiguration configuration)
    {
      var client = new MongoClient(configuration.ConnectionString);
      var database = client.GetDatabase(configuration.DatabaseName);
      _instructions = database.GetCollection<Instruction>(configuration.InstructionsCollectionName);
    }

    public async Task<List<Instruction>> GetInstructions()
    {
      return await _instructions.Find(x => true).ToListAsync();
    }
    public async Task<List<Instruction>> GetInstructionsByBeverageId(string beverageId)
    {
      return await _instructions.Find(x => x.BeverageId == beverageId).ToListAsync();
    }
    public async Task<List<Instruction>> CreateInstructions(List<Instruction> instructions)
    {
      await _instructions.InsertManyAsync(instructions);
      Console.WriteLine(instructions[0].Id);
      return instructions;
    }
    public async Task RemoveInstructionsByBeverageId(string beverageId)
    {
      await _instructions.DeleteManyAsync(x => x.BeverageId == beverageId);
    }
  }
}