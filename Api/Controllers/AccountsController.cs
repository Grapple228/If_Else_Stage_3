using System.Collections.Generic;
using Api.Dtos;
using Api.Misc;
using Api.Requests;
using Api.Services;
using Database.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("[Controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountsService _accountsService;

    public AccountsController(IAccountsService accountsService)
    {
        _accountsService = accountsService;
    }
    
    [HttpGet("me")]
    public ActionResult<AccountDto> Me()
    {
        return _accountsService.GetMe(HttpContext);
    }
    
    [HttpGet("")]
    [Authorize(Roles = "ADMIN, MANAGER")]
    public IEnumerable<AccountDto> GetAccounts()
    {
        return _accountsService.GetAccounts(HttpContext);
    }
    
    [HttpGet("{accountId:long}")]
    public AccountDto GetAccount(long accountId)
    {
        return _accountsService.GetAccount(accountId, HttpContext);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN, MANAGER")]
    public AccountDto CreateAccount(AccountCreateRequest request)
    {
        return _accountsService.CreateAccount(request, HttpContext);
    }
    
    [HttpPut("{accountId:long}")]
    public AccountDto UpdateAccount(long accountId, AccountUpdateRequest request)
    {
        return _accountsService.UpdateAccount(accountId, request, HttpContext);
    }
    
    [HttpDelete("{accountId:long}")]
    [Authorize(Roles = "ADMIN, USER")]
    public void DeleteAccount(long accountId)
    {
        _accountsService.DeleteAccount(accountId, HttpContext);
    }
}
