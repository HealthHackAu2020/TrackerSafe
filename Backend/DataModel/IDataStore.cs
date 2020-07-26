using System.Threading.Tasks;
using System.Collections.Generic;

namespace TrackerSafe.Backend.DataModel
{
  public interface IDataStore<T>
  {
    Task<List<T>> GetAllAsync();

    Task<T> GetByIdAsync(string id);

    Task<T> CreateAsync(T item);

    Task UpdateAsync(string id, T item);

    Task DeleteAsync(string id);
  }
}