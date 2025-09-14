using System.Linq.Expressions;
using eCommerce.Shared.Library.Responses;

namespace eCommerce.Shared.Library.Interface;

public interface IGenericInterface<T> where T : class
{
    Task<Response> CreateAsync(T entity);
    Task<Response> UpdateAsync(T entity);
    Task<Response> DeleteAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> FindByIdAsync(int id);
    Task<T> GetByAsync(Expression<Func<T, bool>> predicate); //
}