using Database.Entities;

namespace Database.Repositories;

public interface IVisitsRepository : IRepository<Visit>
{
    
}

public class VisitsRepository : RepositoryBase<Visit>, IVisitsRepository
{
    public VisitsRepository(AppDbContext context) : base(context)
    {
    }
}