using GeminiAdvancedAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Application.Interfaces.Repositories
{
	public interface IRepository<T> where T : BaseEntity
	{
        Task<T> GetByIdAsync(Guid id);
        Task<IQueryable<T>> GetAllAsync(); // Dönüş tipi IQueryable<T> oldu
        Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        IQueryable<T> GetAll();
    }
}
