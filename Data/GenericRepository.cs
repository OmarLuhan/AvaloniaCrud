using System.Linq;
using System.Threading.Tasks;
using Crud.Models;
using Microsoft.EntityFrameworkCore;

namespace Crud.Data;

public class GenericRepository<T>(CrudDbContext context) : IGenericRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet= context.Set<T>();
    public IQueryable<T> Get(bool track=false)
    {
        var query = track ? _dbSet : _dbSet.AsNoTracking();
        return query;
    }

    public async Task<T> Add(T entity)
    {
       var entry= _dbSet.Add(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> Update(T entity)
    {
       _dbSet.Update(entity);
       await context.SaveChangesAsync();
       return true;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) return false;
        _dbSet.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}