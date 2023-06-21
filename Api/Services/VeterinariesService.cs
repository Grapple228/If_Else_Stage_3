using Api.Converters;
using Api.Dtos;
using Api.Exceptions;
using Database;

namespace Api.Services;

public interface IVeterinariesService
{
    IEnumerable<ServiceDto> GetServices(long veterinaryId);
    void AddServiceToVeterinary(long veterinaryId, long serviceId);
    void RemoveServiceFromVeterinary(long veterinaryId, long serviceId);
}

public class VeterinariesService : IVeterinariesService
{
    private readonly UnitOfWork _unitOfWork;

    public VeterinariesService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public IEnumerable<ServiceDto> GetServices(long veterinaryId)
    {
        var veterinary = _unitOfWork.VeterinariesRepository.Get(x => x.AccountId == veterinaryId);
        if (veterinary == null) throw new NotFoundException("Veterinary not found");

        var services = _unitOfWork.ServicesRepository.GetAll(x => x.VeterinaryId == veterinaryId);
        return services.AsEnumerable().AsDto();
    }

    public void AddServiceToVeterinary(long veterinaryId, long serviceId)
    {
        var veterinary = _unitOfWork.VeterinariesRepository.Get(veterinaryId);
        if (veterinary == null) throw new NotFoundException("Veterinary not found");

        var service = _unitOfWork.ServicesRepository.Get(serviceId);
        if (service == null) throw new NotFoundException("Service not found");
        
        if (veterinary.Services.SingleOrDefault(x => x.Id == serviceId) != null)
            throw new BadRequestException("Veterinary already have this service");

        veterinary.Services.Add(service);
        _unitOfWork.VeterinariesRepository.Update(veterinary);
        _unitOfWork.Save();
    }

    public void RemoveServiceFromVeterinary(long veterinaryId, long serviceId)
    {
        var veterinary = _unitOfWork.VeterinariesRepository.Get(veterinaryId);
        if (veterinary == null) throw new NotFoundException("Veterinary not found");
        
        var service = _unitOfWork.ServicesRepository.Get(serviceId);
        if (service == null) throw new NotFoundException("Service not found");
        
        if (veterinary.Services.SingleOrDefault(x => x.Id == serviceId) == null)
            throw new BadRequestException("Veterinary have no this service");
        
        veterinary.Services.Remove(service);
        _unitOfWork.VeterinariesRepository.Update(veterinary);
        _unitOfWork.Save();
    }
}