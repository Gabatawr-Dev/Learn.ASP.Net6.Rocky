using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rocky.DataAccess.Contexts;

namespace Rocky.DataAccess.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext Context;
    internal DbSet<T> Set;

    public Repository(ApplicationDbContext context)
    {
        Context = context;
        Set = Context.Set<T>();
    }

    public T? Find(object? id) => Set.Find(id);

    public IEnumerable<T> GetAll(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? include = null,
        bool isTracking = true) => GetQuery(filter, orderBy, include, isTracking)
            .ToList();

    public T? FirstOrDefault(
        Expression<Func<T, bool>>? filter = null,
        string? include = null,
        bool isTracking = true) => GetQuery(filter, null, include, isTracking)
            .FirstOrDefault();

    private IQueryable<T> GetQuery(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? include = null,
        bool isTracking = true)
    {
        IQueryable<T> query = Set;

        if (filter is not null)
            query = query.Where(filter);
        if (string.IsNullOrWhiteSpace(include) is false)
            query = include.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, property) => current.Include(property.Trim(' ')));
        if (isTracking is false)
            query = query.AsNoTracking();
        if (orderBy is not null)
            query = orderBy(query);

        return query;
    }

    public void Add(T entity) => Set.Add(entity);

    public void Remove(T entity) => Set.Remove(entity);
    public void RemoveRange(IEnumerable<T> entities) => Set.RemoveRange(entities);

    public void Update(T entity, params string[] properties)
    {
        if (properties.Any() is false)
            Set.Update(entity);

        var trackEntity = Find(GetEntityId(entity));
        if (trackEntity is null) return;

        foreach (var property in properties)
            UpdateProperty(trackEntity, entity, property);

        Set.Update(trackEntity);
    }

    private static void UpdateProperty(T setEntity, T getEntity, string propertyName)
    {
        var type = typeof(T);
        var propertyInfo = type.GetProperty(propertyName);
        propertyInfo?.SetValue(setEntity, propertyInfo.GetValue(getEntity));
    }

    private static object? GetEntityId(T entity)
    {
        var type = typeof(T);
        var propertyInfo = type.GetProperty("Id");
        return propertyInfo?.GetValue(entity);
    }

    public void SaveChanges() => Context.SaveChanges();
}
