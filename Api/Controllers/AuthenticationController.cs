using Api.Dtos;
using Api.Exceptions;
using Api.Misc;
using Api.Requests;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("")]
[Authorize]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAccountsService _accountsService;

    public AuthenticationController(IAccountsService accountsService)
    {
        _accountsService = accountsService;
    }
    
    [AllowAnonymous]
    [HttpPost("registration")]
    public AccountDto Registration(RegistrationRequest request)
    {
        if (HttpContext.IsAuthenticated()) 
            throw new ForbiddenException("Unauthorized users only");
        
        return _accountsService.Register(request);
    }
}