using AutoMapper;
using PROG3340_MidtermProject_V3.Models.Domain;
using PROG3340_MidtermProject_V3.Models.DTOs;

namespace PROG3340_MidtermProject_V3.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EquipmentCreateDto, Equipment>();
            CreateMap<Equipment, EquipmentResponseDto>();

            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<Customer, CustomerResponseDto>();

            CreateMap<RentalCreateDto, Rental>();
            CreateMap<Rental, RentalResponseDto>();
        }
    }
}
