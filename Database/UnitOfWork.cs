using Database.Repositories;

namespace Database;

public class UnitOfWork : IDisposable
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    
    private IAccountsRepository? _accountsRepository;
    private IVeterinariesRepository? _veterinariesRepository;
    private IAnimalsRepository? _animalsRepository;
    private IAppointmentsRepository? _appointmentsRepository;
    private ISchedulesRepository? _schedulesRepository;
    private IServicesRepository? _servicesRepository;
    private IVisitsRepository? _visitsRepository;

    public IAccountsRepository AccountsRepository
    {
        get { return _accountsRepository ??= new AccountsRepository(_context); }
    }
    
    public IVeterinariesRepository VeterinariesRepository
    {
        get { return _veterinariesRepository ??= new VeterinariesRepository(_context); }
    }
    
    public IAnimalsRepository AnimalsRepository
    {
        get { return _animalsRepository ??= new AnimalsRepository(_context); }
    }
    
    public IAppointmentsRepository AppointmentsRepository
    {
        get { return _appointmentsRepository ??= new AppointmentsRepository(_context); }
    }
    
    public ISchedulesRepository SchedulesRepository
    {
        get { return _schedulesRepository ??= new SchedulesRepository(_context); }
    }
    
    public IServicesRepository ServicesRepository
    {
        get { return _servicesRepository ??= new ServicesRepository(_context); }
    }
    
    public IVisitsRepository VisitsRepository
    {
        get { return _visitsRepository ??= new VisitsRepository(_context); }
    }

    public void Save()
    {
        _context.SaveChanges();
    }
    
    ~UnitOfWork()
    {  
        Dispose();
    }

    private bool _disposed;
    
    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}