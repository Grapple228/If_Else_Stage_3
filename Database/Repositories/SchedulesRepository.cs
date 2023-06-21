using Database.Entities;

namespace Database.Repositories;

public interface ISchedulesRepository : IRepository<Schedule>
{
    
}

public class SchedulesRepository : RepositoryBase<Schedule>, ISchedulesRepository
{
    public SchedulesRepository(AppDbContext context) : base(context)
    {
    }
}