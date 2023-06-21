using Api.Converters;
using Api.Dtos;
using Api.Exceptions;
using Api.Requests;
using Database;
using Database.Entities;
using Database.Enums;

namespace Api.Services;

public interface ISchedulesService
{
    IEnumerable<ScheduleDto> GetSchedules(DateTime? date);
    ScheduleDto GetSchedule(long veterinaryId, DateTime date);
    ScheduleDto CreateSchedule(long veterinaryId, ScheduleCreateRequest request, HttpContext context);
    ScheduleDto UpdateSchedule(long veterinaryId, DateTime date, ScheduleUpdateRequest request, HttpContext context);
    void DeleteSchedule(long veterinaryId, DateTime date, HttpContext context);
}

public class SchedulesService : ISchedulesService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IAccountsService _accountsService;

    public SchedulesService(UnitOfWork unitOfWork, IAccountsService accountsService)
    {
        _unitOfWork = unitOfWork;
        _accountsService = accountsService;
    }
    
    public IEnumerable<ScheduleDto> GetSchedules(DateTime? date)
    {
        var query = date == null
            ? _unitOfWork.SchedulesRepository.GetAll()
            : _unitOfWork.SchedulesRepository.GetAll(x => x.Date == date);

        return query.AsEnumerable().AsDto();
    }

    public ScheduleDto GetSchedule(long veterinaryId, DateTime date)
    {
        var veterinary = _unitOfWork.VeterinariesRepository.Get(veterinaryId);
        if (veterinary == null) throw new NotFoundException("Veterinary not found");

        var schedule = _unitOfWork.SchedulesRepository.Get(x => x.VeterinaryId == veterinaryId && x.Date == date);
        if (schedule == null) throw new NotFoundException("Schedule not found");

        return schedule.AsDto();
    }

    public ScheduleDto CreateSchedule(long veterinaryId, ScheduleCreateRequest request, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);
        
        var veterinary = _unitOfWork.VeterinariesRepository.Get(veterinaryId);
        if (veterinary == null) throw new NotFoundException("Veterinary not found");

        if (executor.Role == Roles.VETERINARY && veterinaryId != executor.Id)
            throw new ForbiddenException("Cant change schedule of another veterinary");
        
        var schedule = _unitOfWork.SchedulesRepository.Get(x => x.VeterinaryId == veterinaryId && x.Date == request.Date);
        if (schedule != null) throw new ConflictException("Schedule with date already exists");

        schedule = new Schedule()
        {
            Date = request.Date,
            VeterinaryId = veterinaryId
        };
        _unitOfWork.SchedulesRepository.Create(schedule);
        _unitOfWork.Save();

        return schedule.AsDto();
    }

    public ScheduleDto UpdateSchedule(long veterinaryId, DateTime date, ScheduleUpdateRequest request, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);
        
        var veterinary = _unitOfWork.VeterinariesRepository.Get(veterinaryId);
        if (veterinary == null) throw new NotFoundException("Veterinary not found");

        var existingSchedule = _unitOfWork.SchedulesRepository.Get(x => x.VeterinaryId == veterinaryId && x.Date == date);
        if (existingSchedule == null) throw new NotFoundException("Schedule not found");
        
        var schedule = _unitOfWork.SchedulesRepository.Get(x => x.VeterinaryId == veterinaryId && x.Date == request.Date);
        if (schedule != null) throw new ConflictException("Schedule with date already exists");

        if (executor.Role == Roles.VETERINARY && veterinaryId != executor.Id)
            throw new ForbiddenException("Cant change schedule of another veterinary");

        existingSchedule.Date = request.Date;

        _unitOfWork.SchedulesRepository.Update(existingSchedule);
        _unitOfWork.Save();

        return existingSchedule.AsDto();
    }

    public void DeleteSchedule(long veterinaryId, DateTime date, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);
        
        var veterinary = _unitOfWork.VeterinariesRepository.Get(veterinaryId);
        if (veterinary == null) throw new NotFoundException("Veterinary not found");
        
        var schedule = _unitOfWork.SchedulesRepository.Get(x => x.VeterinaryId == veterinaryId && x.Date == date);
        if (schedule == null) throw new NotFoundException("Schedule not found");
        
        if(executor.Role == Roles.VETERINARY && veterinaryId != executor.Id)
            throw new ForbiddenException("Cant change schedule of another veterinary");

        _unitOfWork.SchedulesRepository.Delete(schedule);
        _unitOfWork.Save();
    }
}