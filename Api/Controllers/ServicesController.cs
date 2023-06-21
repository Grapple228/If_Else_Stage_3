using Api.Dtos;
using Api.Requests;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("[Controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServicesService _servicesService;

    public ServicesController(IServicesService servicesService)
    {
        _servicesService = servicesService;
    }
    
    [HttpGet("")]
    public IEnumerable<ServiceDto> GetServices()
    {
        return _servicesService.GetServices();
    }
    
    [HttpGet("{serviceId:long}")]
    public ServiceDto GetService(long serviceId)
    {
        return _servicesService.GetService(serviceId);
    }
    
    [HttpPost("")]
    [Authorize(Roles = "ADMIN")]
    public ServiceDto CreateService(ServiceCreateRequest request)
    {
        return _servicesService.CreateService(request);
    }
    
    [HttpPut("{serviceId:long}")]
    [Authorize(Roles = "ADMIN")]
    public ServiceDto UpdateService(long serviceId, ServiceUpdateRequest request)
    {
        return _servicesService.UpdateService(serviceId, request);
    }
    
    [HttpDelete("{serviceId:long}")]
    [Authorize(Roles = "ADMIN")]
    public void DeleteService(long serviceId)
    {
        _servicesService.DeleteService(serviceId);
    }
}