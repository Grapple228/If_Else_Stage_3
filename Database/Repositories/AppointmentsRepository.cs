using Database.Entities;

namespace Database.Repositories;

public interface IAppointmentsRepository : IRepository<Appointment>
{
    
}

public class AppointmentsRepository : RepositoryBase<Appointment>, IAppointmentsRepository
{
    public AppointmentsRepository(AppDbContext context) : base(context)
    {
    }
}