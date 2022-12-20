using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using eshop_api.Helpers;

namespace Eshop_API.Repositories.Generics
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _context;
        public GenericRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<T> Add(T entity)
        {
            var result = await _context.Set<T>().AddAsync(entity);
            return result.Entity;
        }
        public async Task AddRange(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }
        public async Task<List<T>> Find(Expression<Func<T, bool>> expression)
        {
            var result =  _context.Set<T>().Where(expression).ToList();
            return await Task.FromResult(result);
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            var result =  _context.Set<T>().FirstOrDefault(expression);
            return await Task.FromResult(result);
        }

        public async Task<List<T>> GetAll()
        {
            var result = _context.Set<T>().ToList();
            return await Task.FromResult(result);
        }
        public async Task<T> GetById(int id)
        {
            var result = _context.Set<T>().Find(id);
            return await Task.FromResult(result);
        }
        public async Task Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public async Task SaveChangesAsync(){
            await _context.SaveChangesAsync();
        }

        public async Task<T> Update(T entity)
        {
            return _context.Set<T>().Update(entity).Entity;
        }
        public async Task UpdateRange(List<T> entity)
        {
            _context.Set<T>().UpdateRange(entity);
        }
    }
}