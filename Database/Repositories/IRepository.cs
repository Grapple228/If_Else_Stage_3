using System.Linq.Expressions;
using Database.Entities;

namespace Database.Repositories;

public interface IRepository<TEntity>
where TEntity : IEntity
{
    TEntity? Get(long id);
    TEntity? Get(Expression<Func<TEntity, bool>> filter);
    IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null);
    IQueryable<TEntity> GetAll(int from, int size, Expression<Func<TEntity, bool>>? filter = null);
    void Update(TEntity entity);
    void Delete(long id);
    void Delete(TEntity entity);
    void Create(TEntity entity);
    string Includes { get; }
}