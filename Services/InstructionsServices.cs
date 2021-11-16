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

    public InstructionsServices(IMongoDbConfiguration configuration, MongoClient client)
    {
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
      if (instructions.Count == 0) return null;
      await _instructions.InsertManyAsync(instructions);
      return instructions;
    }
    public async Task RemoveInstructionsByBeverageId(string beverageId)
    {
      await _instructions.DeleteManyAsync(x => x.BeverageId == beverageId);
    }
  }
}