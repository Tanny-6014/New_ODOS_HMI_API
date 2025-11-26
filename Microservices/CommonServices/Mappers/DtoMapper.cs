using AutoMapper;
using CommonServices.Dtos;
using CommonServices.Models;

namespace CommonServices.Mappers
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<CustomerMaster, CustomerDto>()
              .ForMember(dest => dest.Customername, conf => conf.MapFrom(src => src.vchCustomername))
              .ForMember(dest => dest.CustomerNo, conf => conf.MapFrom(src => src.vchCustomerNo))
              .ForMember(dest => dest.CustomerCode, conf => conf.MapFrom(src => src.intCustomerCode));

            CreateMap<ProjectMaster, ProjectMasterDtos>()
             .ForMember(dest => dest.ProjectId, conf => conf.MapFrom(src => src.intProjectId))
             .ForMember(dest => dest.ProjectCode, conf => conf.MapFrom(src => src.vchprojectcode))
             .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.proj_desc1));

            CreateMap<ProductType, ProductTypeDtos>()
           .ForMember(dest => dest.ProductTypeID, conf => conf.MapFrom(src => src.sitProductTypeID))
           .ForMember(dest => dest.ProductType, conf => conf.MapFrom(src => src.vchProductType))
           .ForMember(dest => dest.ProductTypeDescription, conf => conf.MapFrom(src => src.vchProductTypeDescription));

        }



    
    }
}
