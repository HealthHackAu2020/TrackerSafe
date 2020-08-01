using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Driver;

namespace TrackerSafe.Backend.DataModel
{
  public class CheckInDataStore : ICheckInDataStore
  {
    private readonly IMongoCollection<CheckIn> _items;

    public CheckInDataStore(string connectionString, string databaseName)
    {
      var client = new MongoClient(connectionString);
      var database = client.GetDatabase(databaseName);
      _items = database.GetCollection<CheckIn>("checkin");
    }

    public async Task<List<CheckIn>> GetAllAsync()
    {
      return await _items.Find(s => true).ToListAsync();
    }

    public async Task<CheckIn> GetByIdAsync(string id)
    {
      return await _items.Find<CheckIn>(s => s.Id == id).FirstOrDefaultAsync();
    }

    public async Task<CheckIn> CreateAsync(CheckIn item)
    {
      await _items.InsertOneAsync(item);
      return item;
    }

    public async Task UpdateAsync(string id, CheckIn item)
    {
      await _items.ReplaceOneAsync(s => s.Id == id, item);
    }

    public async Task DeleteAsync(string id)
    {
      await _items.DeleteOneAsync(s => s.Id == id);
    }

  }
}