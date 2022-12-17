using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Eshop_API.Repositories.Generics
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
        Task<List<T>> Find(Expression<Func<T, bool>> expression);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> expression);
        Task<T> Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task Remove(T entity);
        Task RemoveRange(IEnumerable<T> entities);
        Task SaveChangesAsync();
        Task<T> Update(T entity);
    }
}