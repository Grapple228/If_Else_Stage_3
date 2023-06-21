using System.Collections.Generic;
using Api.Dtos;
using Api.Requests;
using Api.Services;
using Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("Accounts/{accountId:long}/[Controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalsService _animalsService;

    public AnimalsController(IAnimalsService animalsService)
    {
        _animalsService = animalsService;
    }
    
    [HttpGet("")]
    public IEnumerable<AnimalDto> GetAnimals(long accountId)
    {
        return _animalsService.GetAnimals(accountId, HttpContext);
    }
    
    [HttpGet("{animalId:long}")]
    public AnimalDto GetAnimal(long accountId, long animalId)
    {
        return _animalsService.GetAnimal(accountId, animalId, HttpContext);
    }
    
    [HttpPost("")]
    [Authorize(Roles = "ADMIN, USER, MANAGER")]
    public AnimalDto CreateAnimal(long accountId, AnimalCreateRequest request)
    {
        return _animalsService.AddAnimal(accountId, request, HttpContext);
    }
    
    [HttpPut("{animalId:long}")]
    [Authorize(Roles = "ADMIN, USER, MANAGER")]
    public AnimalDto UpdateAnimal(long accountId, long animalId, AnimalUpdateRequest request)
    {
        return _animalsService.UpdateAnimal(accountId, animalId, request, HttpContext);
    }
    
    [HttpDelete("{animalId:long}")]
    [Authorize(Roles = "ADMIN, USER, MANAGER")]
    public void DeleteAnimal(long accountId, long animalId)
    {
        _animalsService.DeleteAnimal(accountId, animalId, HttpContext);
    }
}