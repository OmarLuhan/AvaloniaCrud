using System.Linq;
using System.Threading.Tasks;

namespace Crud.Data;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T>Get(bool track=false);
    Task<T> Add(T entity);
    Task<bool> Update(T entity);
    Task<bool>Delete(int id);
}