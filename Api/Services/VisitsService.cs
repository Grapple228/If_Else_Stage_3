using Api.Converters;
using Api.Dtos;
using Api.Exceptions;
using Api.Requests;
using Database;
using Database.Entities;
using Database.Enums;

namespace Api.Services;

public interface IVisitsService
{
    IEnumerable<VisitDto> GetVisits(long accountId, HttpContext context);
    VisitDto GetVisit(long accountId, DateTime date, HttpContext context);
    VisitDto CreateVisit(long accountId, VisitCreateRequest request);
    VisitDto UpdateVisit(long accountId, DateTime date, VisitUpdateRequest request);
    void DeleteVisit(long accountId, DateTime date);
}
public class VisitsService : IVisitsService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IAccountsService _accountsService;

    public VisitsService(UnitOfWork unitOfWork, IAccountsService accountsService)
    {
        _unitOfWork = unitOfWork;
        _accountsService = accountsService;
    }

    public IEnumerable<VisitDto> GetVisits(long accountId, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);
        
        var account = _unitOfWork.AccountsRepository.Get(accountId);
        if (account == null) throw new NotFoundException("Account not found");
        
        if (executor.Role == Roles.USER && accountId != executor.Id)
            throw new ForbiddenException("Have no rights to get visits of another user");

        var visits = _unitOfWork.VisitsRepository.GetAll(x => x.VisiterId == accountId);
        return visits.AsEnumerable().AsDto();
    }

    public VisitDto GetVisit(long accountId, DateTime date, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);
        
        var account = _unitOfWork.AccountsRepository.Get(accountId);
        if (account == null) throw new NotFoundException("Account not found");
        
        if (executor.Role == Roles.USER && accountId != executor.Id)
            throw new ForbiddenException("Have no rights to get visits of another user");

        var visit = _unitOfWork.VisitsRepository.Get(x => x.VisiterId == accountId && x.Date == date);
        if (visit == null) throw new NotFoundException("Visit not gound");

        return visit.AsDto();
    }

    public VisitDto CreateVisit(long accountId, VisitCreateRequest request)
    {
        var account = _unitOfWork.AccountsRepository.Get(accountId);
        if (account == null) throw new NotFoundException("Account not found");
        
        var veterinary = _unitOfWork.VeterinariesRepository.Get(request.VeterinaryId);
        if (veterinary == null) throw new NotFoundException("Veterinary not found");

        var schedule = _unitOfWork.SchedulesRepository.Get(x => x.VeterinaryId == veterinary.Id &&
                                                                x.Date == request.Date);
        if (schedule == null) throw new BadRequestException("Veterinary have no schedule to this date");

        var existingVisit = _unitOfWork.VisitsRepository.Get(x =>
            x.VisiterId == accountId && x.VeterinaryId == request.VeterinaryId && x.Date == request.Date);
        if (existingVisit != null) throw new ConflictException("Such visit already exists");
        
        var visit = new Visit()
        {
            Date = request.Date,
            VisiterId = accountId,
            VeterinaryId = veterinary.Id
        };
        
        _unitOfWork.VisitsRepository.Create(visit);
        _unitOfWork.Save();

        return visit.AsDto();
    }

    public VisitDto UpdateVisit(long accountId, DateTime date, VisitUpdateRequest request)
    {
        var account = _unitOfWork.AccountsRepository.Get(accountId);
        if (account == null) throw new NotFoundException("Account not found");

        var veterinary = _unitOfWork.VeterinariesRepository.Get(request.VeterinaryId);
        if (veterinary == null) throw new NotFoundException("Veterinary not found");

        var schedule = _unitOfWork.SchedulesRepository.Get(x => x.VeterinaryId == veterinary.Id &&
                                                                x.Date == request.Date);
        if (schedule == null) throw new BadRequestException("Veterinary have no schedule to this date");

        var existingVisit = _unitOfWork.VisitsRepository.Get(x => x.VisiterId == accountId && x.Date == date);
        if (existingVisit == null) throw new NotFoundException("Visit not found");

        var visit = _unitOfWork.VisitsRepository.Get(x =>
            x.VisiterId == accountId && x.VeterinaryId == request.VeterinaryId && x.Date == request.Date);
        if (visit != null) throw new ConflictException("Such visit already exists");
        
        existingVisit.Date = request.Date;
        existingVisit.VeterinaryId = request.VeterinaryId;

        _unitOfWork.VisitsRepository.Update(existingVisit);
        _unitOfWork.Save();

        return existingVisit.AsDto();
    }

    public void DeleteVisit(long accountId, DateTime date)
    {
        var account = _unitOfWork.AccountsRepository.Get(accountId);
        if (account == null) throw new NotFoundException("Account not found");
        
        var visit = _unitOfWork.VisitsRepository.Get(x => x.VisiterId == accountId && x.Date == date);
        if (visit == null) throw new NotFoundException("Visit not found");

        _unitOfWork.VisitsRepository.Delete(visit);
    }
}