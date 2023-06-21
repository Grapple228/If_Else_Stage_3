using System;
using System.Collections.Generic;
using System.Linq;
using Api.Converters;
using Api.Dtos;
using Api.Exceptions;
using Api.Misc;
using Api.Requests;
using Database;
using Database.Entities;
using Database.Enums;
using Microsoft.AspNetCore.Http;

namespace Api.Services;

public interface IAccountsService
{
    AccountDto Register(RegistrationRequest request);
    AccountDto GetMe(HttpContext context);
    IEnumerable<AccountDto> GetAccounts(HttpContext context);
    AccountDto GetAccount(long accountId, HttpContext context);
    AccountDto CreateAccount(AccountCreateRequest request, HttpContext context);
    AccountDto UpdateAccount(long accountId, AccountUpdateRequest request, HttpContext context);
    void DeleteAccount(long accountId, HttpContext context);
}

public class AccountsService : IAccountsService
{
    private readonly UnitOfWork _unitOfWork;

    public AccountsService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public AccountDto Register(RegistrationRequest request)
    {
        var account = _unitOfWork.AccountsRepository.GetByUsername(request.Username);
        if (account != null) throw new ConflictException("User with this username already exists");
        
        account = new Account
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password,
            Username = request.Username,
            Role = Roles.USER
        };

        _unitOfWork.AccountsRepository.Create(account);
        _unitOfWork.Save();

        return account.AsDto();
    }

    
    
    public AccountDto GetMe(HttpContext context)
    {
        var username = context.GetUsername();
        if (username == null) throw new UnauthorizedException("Username not found");
        var account = _unitOfWork.AccountsRepository.GetByUsername(username);
        if (account == null) throw new NotFoundException("Account not found");
        return account.AsDto();
    }

    public IEnumerable<AccountDto> GetAccounts(HttpContext context)
    {
        var accounts = context.CheckRole(Roles.MANAGER) 
            ? _unitOfWork.AccountsRepository.GetAll(x => x.Role != Roles.ADMIN) 
            : _unitOfWork.AccountsRepository.GetAll();

        return accounts.AsEnumerable().AsDto();
    }

    public AccountDto GetAccount(long accountId, HttpContext context)
    {
        var accountToReturn = _unitOfWork.AccountsRepository.Get(accountId);
        if (accountToReturn == null) throw new NotFoundException("Account not found");
        
        var account = GetMe(context);
        
        switch (account.Role)
        {
            case Roles.ADMIN:
                return accountToReturn.AsDto();
            case Roles.MANAGER:
                if (accountToReturn.Role == Roles.ADMIN)
                    throw new ForbiddenException("Have no rights to get information");
                return accountToReturn.AsDto();
            case Roles.VETERINARY:
            case Roles.USER:
                if(accountToReturn.Id != account.Id)
                    throw new ForbiddenException("Have no rights to get information");
                return accountToReturn.AsDto();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public AccountDto CreateAccount(AccountCreateRequest request, HttpContext context)
    {
        var creator = GetMe(context);

        var account = _unitOfWork.AccountsRepository.GetByUsername(request.Username);
        if (account != null) throw new ConflictException("User with this username already exists");

        if (creator.Role == Roles.MANAGER)
        {
            if (request.Role == Roles.ADMIN)
                throw new BadRequestException("Have no rights to create admin");
        }

        account = new Account()
        {
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password,
            Role = request.Role
        };

        _unitOfWork.AccountsRepository.Create(account);
        _unitOfWork.Save();

        if (account.Role != Roles.VETERINARY) return account.AsDto();
        
        var veterinary = new Veterinary()
        {
            FirstName = account.FirstName,
            LastName = account.LastName,
            AccountId = account.Id
        };
        _unitOfWork.VeterinariesRepository.Create(veterinary);
        _unitOfWork.Save();
        
        return account.AsDto();
    }

    public AccountDto UpdateAccount(long accountId, AccountUpdateRequest request, HttpContext context)
    {
        var creator = GetMe(context);

        var account = _unitOfWork.AccountsRepository.Get(accountId);
        if (account == null) throw new NotFoundException("Account not found");
        
        switch (creator.Role)
        {
            case Roles.ADMIN:

                if (account.Role == Roles.VETERINARY && request.Role != Roles.VETERINARY)
                {
                    var veterinary = _unitOfWork.VeterinariesRepository.Get(x => x.AccountId == accountId);
                    if(veterinary == null) break;
                    
                    _unitOfWork.VeterinariesRepository.Delete(veterinary);
                    _unitOfWork.Save();
                }
                else if (account.Role != Roles.VETERINARY && request.Role == Roles.VETERINARY)
                {
                    var veterinary = new Veterinary()
                    {
                        FirstName = account.FirstName,
                        LastName = account.LastName,
                        AccountId = account.Id
                    };
                    _unitOfWork.VeterinariesRepository.Create(veterinary);
                    _unitOfWork.Save();
                }
                else if (account.Role == Roles.VETERINARY && request.Role == Roles.VETERINARY)
                {
                    var veterinary = _unitOfWork.VeterinariesRepository.Get(x => x.AccountId == accountId);
                    if(veterinary == null) break;

                    veterinary.FirstName = request.FirstName;
                    veterinary.LastName = request.LastName;
                    
                    _unitOfWork.VeterinariesRepository.Update(veterinary);
                    _unitOfWork.Save();
                }
                break;
            case Roles.MANAGER:
            case Roles.VETERINARY:
            case Roles.USER:
                if (creator.Id != account.Id)
                    throw new ForbiddenException("Have no rights to change account");
                if (request.Role != creator.Role)
                    throw new ForbiddenException("Have no rights to change role");
                break;
        }

        account.FirstName = request.FirstName;
        account.LastName = request.LastName;
        account.Role = request.Role;
        account.Password = request.Password;
        account.Username = request.Username;

        _unitOfWork.AccountsRepository.Update(account);
        _unitOfWork.Save();

        return account.AsDto();
    }

    public void DeleteAccount(long accountId, HttpContext context)
    {
        var changer = GetMe(context);
        
        var account = _unitOfWork.AccountsRepository.Get(accountId);
        if (account == null) throw new NotFoundException("Account not found");

        if (changer.Role == Roles.USER)
        {
            if (changer.Id != accountId)
                throw new ForbiddenException("Have no rights to delete account");
        }

        if (account.Role == Roles.VETERINARY)
        {
            var veterinary = _unitOfWork.VeterinariesRepository.Get(x => x.AccountId == accountId);
            if (veterinary != null)
            {
                _unitOfWork.VeterinariesRepository.Delete(veterinary);
            }
        }

        _unitOfWork.AccountsRepository.Delete(accountId);
        _unitOfWork.Save();
    }
}