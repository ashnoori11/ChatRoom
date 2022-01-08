using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Domain.InterfaceRepositories
{
   public interface IBaseRepository<TEntity>
    {
        IQueryable<TEntity> GetAllAsList();
        Task<IEnumerable<TEntity>> GetAllAsListAsync();
        TEntity FindById(dynamic id);
        Task<TEntity> FindByIdAsync(dynamic id);
        Task InsertAsync(TEntity entity);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task DeleteByIdAsync(dynamic id);
        Task<int> GetCount();
        Task SaveAsync();
    }
}
