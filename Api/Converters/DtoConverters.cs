using System.Collections.Generic;
using System.Linq;
using Api.Dtos;
using Database.Entities;

namespace Api.Converters;

public static class DtoConverters
{
    public static AccountDto AsDto(this Account account) =>
        new(account.Id, account.Username, account.FirstName, account.LastName, account.Role);
    public static IEnumerable<AccountDto> AsDto(this IEnumerable<Account> accounts) =>
        accounts.Select(x => x.AsDto());

    public static AnimalDto AsDto(this Animal animal) =>
        new(animal.Id, animal.Name, animal.Weight, animal.Height, animal.Length, animal.OwnerId, animal.Gender);
    public static IEnumerable<AnimalDto> AsDto(this IEnumerable<Animal> animals) =>
        animals.Select(x => x.AsDto());
    
    public static ServiceDto AsDto(this Service service) =>
        new(service.Id, service.Name, service.Description);
    
    public static IEnumerable<ServiceDto> AsDto(this IEnumerable<Service> services) =>
        services.Select(x => x.AsDto());
    
    public static ScheduleDto AsDto(this Schedule schedule) =>
        new(schedule.Id, schedule.VeterinaryId, schedule.Date);
    public static IEnumerable<ScheduleDto> AsDto(this IEnumerable<Schedule> schedules) =>
        schedules.Select(x => x.AsDto());
    
    public static AppointmentDto AsDto(this Appointment appointment) =>
        new(appointment.Id, appointment.AccountId, appointment.Date, appointment.VeterinaryId);
    public static IEnumerable<AppointmentDto> AsDto(this IEnumerable<Appointment> appointments) =>
        appointments.Select(x => x.AsDto());
    
    public static VisitDto AsDto(this Visit visit) =>
        new(visit.Id, visit.VisiterId, visit.VeterinaryId, visit.Date);
    public static IEnumerable<VisitDto> AsDto(this IEnumerable<Visit> visits) =>
        visits.Select(x => x.AsDto());
}