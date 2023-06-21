using Database.Entities;

namespace Database.Repositories;

public interface IServicesRepository : IRepository<Service>
{
    
}

public class ServicesRepository : RepositoryBase<Service>, IServicesRepository
{
    public ServicesRepository(AppDbContext context) : base(context)
    {
    }
}