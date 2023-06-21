using Database.Entities;

namespace Database.Repositories;

public interface IVeterinariesRepository : IRepository<Veterinary>
{
    
}

public class VeterinariesRepository : RepositoryBase<Veterinary>, IVeterinariesRepository
{
    public VeterinariesRepository(AppDbContext context) : base(context)
    {
    }

    public override string Includes { get; protected set; } = "Services";
}