using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("[Controller]/{veterinaryId:long}/services")]
public class VeterinariesController : ControllerBase
{
    private readonly IVeterinariesService _veterinariesService;

    public VeterinariesController(IVeterinariesService veterinariesService)
    {
        _veterinariesService = veterinariesService;
    }
    
    [HttpGet("")]
    public IEnumerable<ServiceDto> GetServices(long veterinaryId)
    {
        return _veterinariesService.GetServices(veterinaryId);
    }
    
    [HttpPost("{serviceId:long}")]
    [Authorize(Roles = "ADMIN")]
    public void AddService(long veterinaryId, long serviceId)
    {
        _veterinariesService.AddServiceToVeterinary(veterinaryId, serviceId);
    }
    
    [HttpDelete("{serviceId:long}")]
    [Authorize(Roles = "ADMIN")]
    public void DeleteService(long veterinaryId, long serviceId)
    {
        _veterinariesService.RemoveServiceFromVeterinary(veterinaryId, serviceId);
    }
}