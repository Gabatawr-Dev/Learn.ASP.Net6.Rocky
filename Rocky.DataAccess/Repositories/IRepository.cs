using System.Linq.Expressions;

namespace Rocky.DataAccess.Repositories;

public interface IRepository<T> where T : class
{
    T? Find(object? id);

    IEnumerable<T> GetAll(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? include = null,
        bool isTracking = true);

    T? FirstOrDefault(
        Expression<Func<T, bool>>? filter = null,
        string? include = null,
        bool isTracking = true);

    void Add(T entity);
    void Remove(T entity);

    /// <param name="properties">Only for untracked entities!</param>
    void Update(T entity, params string[] properties);
    void SaveChanges();
}

