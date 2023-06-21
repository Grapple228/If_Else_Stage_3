using System.ComponentModel.DataAnnotations;
using Api.Dtos;
using Api.Requests;
using Api.Services;
using Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("[Controller]/{accountId:long}")]
public class VisitsController : ControllerBase
{
    private readonly IVisitsService _visitsService;

    public VisitsController(IVisitsService visitsService)
    {
        _visitsService = visitsService;
    }
    
    [HttpGet("")]
    public ActionResult<VisitDto> GetVisits(long accountId, [FromQuery] DateTime? date)
    {
        if (date == null)
            return Ok(_visitsService.GetVisits(accountId, HttpContext));
        return _visitsService.GetVisit(accountId, date.Value, HttpContext);
    }
    
    [HttpPost("")]
    [Authorize(Roles = "ADMIN, MANAGER")]
    public VisitDto AddVisit(long accountId, VisitCreateRequest request)
    {
        return _visitsService.CreateVisit(accountId, request);
    }
    
    [HttpPut("")]
    [Authorize(Roles = "ADMIN, MANAGER, VETERINARY")]
    public VisitDto ChangeVisit(long accountId, [Required][FromQuery] DateTime date, VisitUpdateRequest request)
    {
        return _visitsService.UpdateVisit(accountId, date, request);
    }
    
    [HttpDelete("")]
    [Authorize(Roles = "ADMIN")]
    public void DeleteVisit(long accountId, [Required][FromQuery] DateTime date)
    {
        _visitsService.DeleteVisit(accountId, date);
    }
}