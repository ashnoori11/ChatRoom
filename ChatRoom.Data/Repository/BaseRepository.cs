using ChatRoom.Data.Context;
using ChatRoom.Domain.InterfaceRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Data.Repository
{
   public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity:class
    {
        protected ChatRoomContext _context { get; set; }
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(ChatRoomContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAllAsList()
        {
            return _dbSet.AsQueryable();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsListAsync() => await _dbSet.ToListAsync();
        public TEntity FindById(dynamic id) => _dbSet.Find(id);
        public async Task<TEntity> FindByIdAsync(dynamic id) => await _dbSet.FindAsync(id);
        public async Task InsertAsync(TEntity entity) => await _dbSet.AddAsync(entity);
        public async Task InsertRangeAsync(IEnumerable<TEntity> entities) => await _dbSet.AddRangeAsync(entities);
        public void Update(TEntity entity) => _dbSet.Update(entity);
        public void Delete(TEntity entity) => _dbSet.Remove(entity);
        public async Task DeleteByIdAsync(dynamic id)
        {
            var entity = await _dbSet.FindAsync(id);
            Delete(entity);
        }

        public async Task<int> GetCount() => await _dbSet.CountAsync();
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
