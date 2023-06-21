using Api.Converters;
using Api.Dtos;
using Api.Exceptions;
using Api.Requests;
using Database;
using Database.Entities;
using Database.Enums;

namespace Api.Services;

public interface IAppointmentsService
{
    IEnumerable<AppointmentDto> GetAppointments(long accountId, HttpContext context);
    AppointmentDto GetAppointment(long accountId, DateTime date, HttpContext context);
    AppointmentDto CreateAppointment(long accountId, AppointmentCreateRequest request, HttpContext context);
    AppointmentDto UpdateAppointment(long accountId, DateTime date, AppointmentUpdateRequest request, HttpContext context);
    void DeleteAppointment(long accountId, DateTime date, HttpContext context);
}

public class AppointmentsService : IAppointmentsService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IAccountsService _accountsService;

    public AppointmentsService(UnitOfWork unitOfWork, IAccountsService accountsService)
    {
        _unitOfWork = unitOfWork;
        _accountsService = accountsService;
    }
    
    public IEnumerable<AppointmentDto> GetAppointments(long accountId, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);

        var account = _unitOfWork.AccountsRepository.Get(accountId);
        if (account == null) throw new NotFoundException("Account not found");
        
        if (executor.Role == Roles.USER && accountId != executor.Id)
            throw new ForbiddenException("Have no rights to get appointments of another user");

        var appointments = _unitOfWork.AppointmentsRepository.GetAll(x => x.AccountId == accountId);
        return appointments.AsEnumerable().AsDto();
    }

    public AppointmentDto GetAppointment(long accountId, DateTime date, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);

        var account = _unitOfWork.AccountsRepository.Get(accountId);
        if (account == null) throw new NotFoundException("Account not found");
        
        var appointment = _unitOfWork.AppointmentsRepository.Get(x => x.AccountId == accountId && x.Date == date);
        if (appointment == null) throw new NotFoundException("Appointment not found");
        
        if (executor.Role == Roles.USER && accountId != executor.Id)
            throw new ForbiddenException("Have no rights to get appointments of another user");

        return appointment.AsDto();
    }

    public AppointmentDto CreateAppointment(long accountId, AppointmentCreateRequest request, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);

        var account = _unitOfWork.AccountsRepository.Get(accountId);
        if (account == null) throw new NotFoundException("Account not found");
        
        if (executor.Role == Roles.USER && accountId != executor.Id)
            throw new ForbiddenException("Have no rights to change appointments of another user");

        var veterinary = _unitOfWork.VeterinariesRepository.Get(request.VeterinaryId);
        if (veterinary == null) throw new NotFoundException("Veterinary not found");

        var schedule = _unitOfWork.SchedulesRepository.Get(x => x.VeterinaryId == veterinary.Id &&
                                                                 x.Date == request.Date);
        if (schedule == null) throw new BadRequestException("Veterinary have no schedule to this date");
        
        var appointment = new Appointment()
        {
            Date = request.Date,
            AccountId = accountId,
            VeterinaryId = request.VeterinaryId
        };
        _unitOfWork.AppointmentsRepository.Create(appointment);
        _unitOfWork.Save();

        return appointment.AsDto();
    }

    public AppointmentDto UpdateAppointment(long accountId, DateTime date, AppointmentUpdateRequest request, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);

        var account = _unitOfWork.AccountsRepository.Get(accountId);
        if (account == null) throw new NotFoundException("Account not found");
        
        var appointment = _unitOfWork.AppointmentsRepository.Get(x => x.AccountId == accountId && x.Date == date);
        if (appointment == null) throw new NotFoundException("Appointment not found");
        
        if (executor.Role == Roles.USER && accountId != executor.Id)
            throw new ForbiddenException("Have no rights to change appointments of another user");

        var veterinary = _unitOfWork.VeterinariesRepository.Get(request.VeterinaryId);
        if (veterinary == null) throw new NotFoundException("Veterinary not found");
        
        var schedule = _unitOfWork.SchedulesRepository.Get(x => x.VeterinaryId == veterinary.Id &&
                                                                x.Date == request.Date);
        if (schedule == null) throw new BadRequestException("Veterinary have no schedule to this date");
        
        appointment.Date = request.Date;
        appointment.VeterinaryId = request.VeterinaryId;

        _unitOfWork.AppointmentsRepository.Update(appointment);
        _unitOfWork.Save();

        return appointment.AsDto();
    }

    public void DeleteAppointment(long accountId, DateTime date, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);
        
        var account = _unitOfWork.AccountsRepository.Get(accountId);
        if (account == null) throw new NotFoundException("Account not found");
        
        var appointment = _unitOfWork.AppointmentsRepository.Get(x => x.AccountId == accountId && x.Date == date);
        if (appointment == null) throw new NotFoundException("Appointment not found");
        
        if (executor.Role == Roles.USER && accountId != executor.Id)
            throw new ForbiddenException("Have no rights to change appointments of another user");

        _unitOfWork.AppointmentsRepository.Delete(appointment);
        _unitOfWork.Save();
    }
}