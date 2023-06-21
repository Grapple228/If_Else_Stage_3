using System.Collections.Generic;
using System.Linq;
using Api.Converters;
using Api.Dtos;
using Api.Exceptions;
using Api.Requests;
using Database;
using Database.Entities;
using Database.Enums;
using Microsoft.AspNetCore.Http;

namespace Api.Services;

public interface IAnimalsService
{
    IEnumerable<AnimalDto> GetAnimals(long accountId, HttpContext context);
    AnimalDto GetAnimal(long accountId, long animalId, HttpContext context);
    AnimalDto AddAnimal(long accountId, AnimalCreateRequest request, HttpContext context);
    AnimalDto UpdateAnimal(long accountId, long animalId, AnimalUpdateRequest request, HttpContext context);
    void DeleteAnimal(long accountId, long animalId, HttpContext context);
}

public class AnimalsService : IAnimalsService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IAccountsService _accountsService;

    public AnimalsService(UnitOfWork unitOfWork, IAccountsService accountsService)
    {
        _unitOfWork = unitOfWork;
        _accountsService = accountsService;
    }

    public IEnumerable<AnimalDto> GetAnimals(long accountId, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);
        _accountsService.GetAccount(accountId, context);

        if (executor.Role == Roles.USER && executor.Id != accountId)
            throw new ForbiddenException("Can't get animals of another user");

        var animals = _unitOfWork.AnimalsRepository.GetAll(x => x.OwnerId == accountId);

        return animals.AsEnumerable().AsDto();
    }

    public AnimalDto GetAnimal(long accountId, long animalId, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);
        _accountsService.GetAccount(accountId, context);

        var animal = _unitOfWork.AnimalsRepository.Get(animalId);
        if (animal == null) throw new NotFoundException("Animal not found");
        
        if (animal.OwnerId != accountId)
            throw new BadRequestException("User is not owner of an animal");
        
        if (executor.Role == Roles.USER && executor.Id != accountId)
            throw new ForbiddenException("Can't get animal of another user");

        return animal.AsDto();
    }

    public AnimalDto AddAnimal(long accountId, AnimalCreateRequest request, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);
        var account = _accountsService.GetAccount(accountId, context);

        if (account.Role != Roles.USER)
            throw new BadRequestException("Only user can have animals");

        if (executor.Role == Roles.USER && accountId != executor.Id)
            throw new ForbiddenException("Have no rights to add animal to another user");
        
        var animal = new Animal()
        {
            Gender = request.Gender,
            OwnerId = accountId,
            Height = request.Height,
            Length = request.Lenght,
            Name = request.Name,
            Weight = request.Weight 
        };
        
        _unitOfWork.AnimalsRepository.Create(animal);
        _unitOfWork.Save();
        
        return animal.AsDto();
    }

    public AnimalDto UpdateAnimal(long accountId, long animalId, AnimalUpdateRequest request, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);
        _accountsService.GetAccount(accountId, context);
        
        var animal = _unitOfWork.AnimalsRepository.Get(animalId);
        if (animal == null) throw new NotFoundException("Animal not found");

        if (animal.OwnerId != accountId)
            throw new BadRequestException("User is not owner of an animal");
        
        if (executor.Role == Roles.USER && executor.Id != animal.OwnerId)
            throw new ForbiddenException("Have no rights to change animal of another user");

        animal.Gender = request.Gender;
        animal.Height = request.Height;
        animal.Weight = request.Weight;
        animal.Length = request.Length;
        animal.Name = request.Name;
        animal.OwnerId = request.OwnerId;

        _unitOfWork.AnimalsRepository.Update(animal);
        _unitOfWork.Save();

        return animal.AsDto();
    }

    public void DeleteAnimal(long accountId, long animalId, HttpContext context)
    {
        var executor = _accountsService.GetMe(context);
        _accountsService.GetAccount(accountId, context);
        
        var animal = _unitOfWork.AnimalsRepository.Get(animalId);
        if (animal == null) throw new NotFoundException("Animal not found");

        if (animal.OwnerId != accountId)
            throw new BadRequestException("User is not owner of an animal");
        
        if (executor.Role == Roles.USER && executor.Id != animal.OwnerId)
            throw new ForbiddenException("Have no rights to delete animal of another user");

        _unitOfWork.AnimalsRepository.Delete(animal);
        _unitOfWork.Save();
    }
}