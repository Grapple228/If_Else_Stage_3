using Api.Converters;
using Api.Dtos;
using Api.Exceptions;
using Api.Requests;
using Database;
using Database.Entities;

namespace Api.Services;

public interface IServicesService
{
    IEnumerable<ServiceDto> GetServices();
    ServiceDto GetService(long serviceId);
    ServiceDto CreateService(ServiceCreateRequest request);
    ServiceDto UpdateService(long serviceId, ServiceUpdateRequest request);
    void DeleteService(long serviceId);
}

public class ServicesService : IServicesService
{
    private readonly UnitOfWork _unitOfWork;

    public ServicesService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public IEnumerable<ServiceDto> GetServices()
    {
        return _unitOfWork.ServicesRepository.GetAll().AsEnumerable().AsDto();
    }

    public ServiceDto GetService(long serviceId)
    {
        var service = _unitOfWork.ServicesRepository.Get(serviceId);
        if (service == null) throw new NotFoundException("Service Not Found");

        return service.AsDto();
    }

    public ServiceDto CreateService(ServiceCreateRequest request)
    {
        var service = _unitOfWork.ServicesRepository.Get(x => x.Name.ToLower() == request.Name.ToLower());
        if (service != null) throw new ConflictException("Service with name already exists");
        
        service = new Service()
        {
            Name = request.Name,
            Description = request.Description
        };
        
        _unitOfWork.ServicesRepository.Create(service);
        _unitOfWork.Save();
        
        return service.AsDto();
    }

    public ServiceDto UpdateService(long serviceId, ServiceUpdateRequest request)
    {
        var existingService = _unitOfWork.ServicesRepository.Get(serviceId);
        if (existingService == null) throw new NotFoundException("Service Not Found");

        var service = _unitOfWork.ServicesRepository.Get(x => x.Name.ToLower() == request.Name.ToLower());
        if (service != null) throw new ConflictException("Service with name already exists");
        
        existingService.Name = request.Name;
        existingService.Description = request.Description;
        
        _unitOfWork.ServicesRepository.Update(existingService);
        _unitOfWork.Save();
        
        return existingService.AsDto();
    }

    public void DeleteService(long serviceId)
    {
        var service = _unitOfWork.ServicesRepository.Get(serviceId);
        if (service == null) throw new NotFoundException("Service Not Found");
        
        _unitOfWork.ServicesRepository.Delete(service);
        _unitOfWork.Save();
    }
}