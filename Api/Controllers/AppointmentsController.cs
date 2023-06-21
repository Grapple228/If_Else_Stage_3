using System.ComponentModel.DataAnnotations;
using Api.Dtos;
using Api.Requests;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("[Controller]/{accountId:long}")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentsService _appointmentsService;

    public AppointmentsController(IAppointmentsService appointmentsService)
    {
        _appointmentsService = appointmentsService;
    }
    
    [HttpGet("")]
    public ActionResult<AppointmentDto> GetAppointment(long accountId, [FromQuery] DateTime? date)
    {
        if (date == null)
            return Ok(_appointmentsService.GetAppointments(accountId, HttpContext));
        return _appointmentsService.GetAppointment(accountId, date.Value, HttpContext);
    }
    
    [HttpPost("")]
    [Authorize(Roles = "ADMIN, MANAGER, USER")]
    public void CreateAppointment(long accountId, AppointmentCreateRequest request)
    {
        _appointmentsService.CreateAppointment(accountId, request, HttpContext);
    }
    
    [HttpPut("")]
    [Authorize(Roles = "ADMIN, MANAGER, USER")]
    public void UpdateAppointment(long accountId, [Required][FromQuery] DateTime date, AppointmentUpdateRequest request)
    {
        _appointmentsService.UpdateAppointment(accountId, date, request, HttpContext);
    }
    
    [HttpDelete("")]
    [Authorize(Roles = "ADMIN, MANAGER, USER")]
    public void DeleteAppointment(long accountId, [Required][FromQuery] DateTime date)
    {
        _appointmentsService.DeleteAppointment(accountId, date, HttpContext);
    }
}