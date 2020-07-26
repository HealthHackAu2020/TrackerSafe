using System.Threading.Tasks;
using System.Collections.Generic;

namespace TrackerSafe.Backend.DataModel
{
  public interface IUserDataStore : IDataStore<User>
  {
    Task<User> GetByUserNameAsync(string userName);
    Task<User> GetByReferralCodeAsync(string referralCode);

  }
}