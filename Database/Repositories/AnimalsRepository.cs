using Database.Entities;

namespace Database.Repositories;

public interface IAnimalsRepository : IRepository<Animal>
{
    
}

public class AnimalsRepository : RepositoryBase<Animal>, IAnimalsRepository
{
    public AnimalsRepository(AppDbContext context) : base(context)
    {
    }
}