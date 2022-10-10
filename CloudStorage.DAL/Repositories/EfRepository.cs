using CloudStorage.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.DAL.Repositories;

public class EfRepository<TEntity> where TEntity : class, IEntity, new()
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _table;

    public EfRepository(DbContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackEntities = false)
    {
        var query = trackEntities == false ? _table.AsNoTracking() : _table;

        return await query.ToListAsync();
    }

    public virtual async Task CreateAsync(TEntity entity) => await _table.AddAsync(entity);

    public virtual void Update(TEntity entity) => _table.Update(entity);

    public virtual void Delete(TEntity entity) => _table.Remove(entity);

    public virtual void Delete(int id)
    {
        var entity = _context.ChangeTracker
            .Entries<TEntity>()
            .FirstOrDefault(entry => entry.Entity.Id == id)
            ?.Entity ?? new TEntity { Id = id };

        _table.Remove(entity);
    }
}