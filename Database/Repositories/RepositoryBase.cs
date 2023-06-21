using System.Linq.Expressions;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
where TEntity : class, IEntity
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    protected RepositoryBase(AppDbContext context)
    {
        Context = context;
        DbSet = Context.Set<TEntity>();
    }

    public TEntity? Get(long id)
    {
        return GetIncludes(DbSet).FirstOrDefault(x => x.Id == id);
    }

    public TEntity? Get(Expression<Func<TEntity, bool>> filter)
    {
        return GetIncludes(DbSet).FirstOrDefault(filter);
    }

    protected IQueryable<TEntity> GetIncludes(IQueryable<TEntity> query)
    {
        var includes = Includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var include in includes)
        {
            query.Include(include);
        }

        return query;
    }

    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null)
           query = query.Where(filter);

        return GetIncludes(query);
    }

    public IQueryable<TEntity> GetAll(int from, int size, Expression<Func<TEntity, bool>>? filter = null)
    {
        return GetAll(filter).Skip(from).Take(size);
    }

    public void Update(TEntity entity)
    {
        DbSet.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(long id)
    {
        var entity = DbSet.Find(id);
        if(entity == null) return;
        Delete(entity);
    }

    public void Delete(TEntity entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            Context.Attach(entity);
        }

        DbSet.Remove(entity);
    }

    public void Create(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public virtual string Includes { get; protected set; } = "";
}