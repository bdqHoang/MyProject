using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interface.Data.Repositories;
using MyProject.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public virtual async Task AddAsync(T entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
        }

        public virtual void Delete(Guid id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity!);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(int page = 1, int pageSize = 50,CancellationToken ct = default)
        {
            return await _dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual void RemoveRange(IEnumerable<Guid> Id, CancellationToken ct = default)
        {
            var entities = _dbSet.Where(e => Id.Contains(EF.Property<Guid>(e, "Id"))).ToList();
            _dbSet.RemoveRange(entities);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
