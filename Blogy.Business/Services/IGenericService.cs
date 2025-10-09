using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogy.Business.Services
{
    public interface IGenericService<TResult,TUpdate,TCreate> 
    {
        Task<List<TResult>> GetAllAsync();
        Task<TUpdate> GetByIdAsync(int id);
        Task CreateAsync(TCreate createDto);
        Task UpdateAsync(TUpdate updateDto);
        Task DeleteAsync(int id);
    }
}
