using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Driver;

namespace TrackerSafe.Backend.DataModel
{
  public class UserDataStore : IUserDataStore
  {
    private readonly IMongoCollection<User> _items;

    public UserDataStore(string connectionString, string databaseName)
    {
      var client = new MongoClient(connectionString);
      var database = client.GetDatabase(databaseName);
      _items = database.GetCollection<User>("user");
    }

    public async Task<List<User>> GetAllAsync()
    {
      return await _items.Find(s => true).ToListAsync();
    }

    public async Task<User> GetByIdAsync(string id)
    {
      return await _items.Find<User>(s => s.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User> CreateAsync(User item)
    {
      await _items.InsertOneAsync(item);
      return item;
    }

    public async Task UpdateAsync(string id, User item)
    {
      await _items.ReplaceOneAsync(s => s.Id == id, item);
    }

    public async Task DeleteAsync(string id)
    {
      await _items.DeleteOneAsync(s => s.Id == id);
    }

    public async Task<User> GetByUserNameAsync(string userName)
    {
      return await _items.Find<User>(s => s.UserNameLower == userName.ToLower()).FirstOrDefaultAsync();
    }

    public async Task<User> GetByReferralCodeAsync(string referralCode)
    {
      return await _items.Find<User>(s => s.MyReferralCode == referralCode).FirstOrDefaultAsync();
    }
  }
}