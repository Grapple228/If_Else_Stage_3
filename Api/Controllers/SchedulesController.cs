using System.ComponentModel.DataAnnotations;
using Api.Dtos;
using Api.Requests;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("Veterinaries")]
public class SchedulesController : ControllerBase
{
    private readonly ISchedulesService _schedulesService;

    public SchedulesController(ISchedulesService schedulesService)
    {
        _schedulesService = schedulesService;
    }
    
    [HttpGet("[Controller]")]
    public IEnumerable<ScheduleDto> GetSchedules([FromQuery]DateTime? date)
    {
        return _schedulesService.GetSchedules(date);
    }
    
    [HttpGet("{veterinaryId:long}/[Controller]")]
    public ScheduleDto GetSchedule(long veterinaryId, [Required][FromQuery]DateTime date)
    {
        return _schedulesService.GetSchedule(veterinaryId, date);
    }

    [HttpPost("{veterinaryId:long}/[Controller]")]
    [Authorize(Roles = "ADMIN, MANAGER, VETERINARY")]
    public ScheduleDto CreateSchedule(long veterinaryId, ScheduleCreateRequest request)
    {
        return _schedulesService.CreateSchedule(veterinaryId, request, HttpContext);
    }
    
    [HttpPut("{veterinaryId:long}/[Controller]")]
    [Authorize(Roles = "ADMIN, MANAGER, VETERINARY")]
    public ScheduleDto UpdateSchedule(long veterinaryId, [Required][FromQuery]DateTime date, ScheduleUpdateRequest request)
    {
        return _schedulesService.UpdateSchedule(veterinaryId, date, request, HttpContext);
    }
    
    [HttpDelete("{veterinaryId:long}/[Controller]")]
    [Authorize(Roles = "ADMIN, MANAGER, VETERINARY")]
    public void DeleteSchedule(long veterinaryId, [Required][FromQuery]DateTime date)
    {
        _schedulesService.DeleteSchedule(veterinaryId, date, HttpContext);
    }
}