using AutoMapper;
using ParameterSetService.Dtos;
using ParameterSetService.Models;

namespace ParameterSetService.Mappers
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<ProjectParameter, ProjectParameterDto>()
                .ForMember(dest => dest.ParamSetNumber, conf => conf.MapFrom(src => src.tntParamSetNumber))
                .ForMember(dest => dest.ProjectId, conf => conf.MapFrom(src => src.intProjectId))
                .ForMember(dest => dest.ProductTypeL2Id, conf => conf.MapFrom(src => src.sitProductTypeL2Id))
                .ForMember(dest => dest.ParameteSet, conf => conf.MapFrom(src => src.intParameteSet))
                .ForMember(dest => dest.RefParamSetNumber, conf => conf.MapFrom(src => src.tntRefParamSetNumber))
                .ForMember(dest => dest.ProjectAbbr, conf => conf.MapFrom(src => src.vchProjectAbbr))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.vchDescription))
                .ForMember(dest => dest.ParamCageID, conf => conf.MapFrom(src => src.intParamCageID))
                .ForMember(dest => dest.TransportModeId, conf => conf.MapFrom(src => src.tntTransportModeId))
                .ForMember(dest => dest.TopCover, conf => conf.MapFrom(src => src.sitTopCover))
                .ForMember(dest => dest.BottomCover, conf => conf.MapFrom(src => src.sitBottomCover))
                .ForMember(dest => dest.LeftCover, conf => conf.MapFrom(src => src.sitLeftCover))
                .ForMember(dest => dest.RightCover, conf => conf.MapFrom(src => src.sitRightCover))
                .ForMember(dest => dest.CPLeftCover, conf => conf.MapFrom(src => src.CPLeftCover))
                .ForMember(dest => dest.CPRightCover, conf => conf.MapFrom(src => src.CPRightCover))
                .ForMember(dest => dest.CPCWLength, conf => conf.MapFrom(src => src.CPCWLength))
                .ForMember(dest => dest.Gap1, conf => conf.MapFrom(src => src.sitGap1))
                .ForMember(dest => dest.Gap2, conf => conf.MapFrom(src => src.sitGap2))
                .ForMember(dest => dest.Hook, conf => conf.MapFrom(src => src.sitHook))
                .ForMember(dest => dest.Leg, conf => conf.MapFrom(src => src.sitLeg))
                .ForMember(dest => dest.PinSize, conf => conf.MapFrom(src => src.sitPinSize))
                .ForMember(dest => dest.CLMaterialType, conf => conf.MapFrom(src => src.chrCLMaterialType))
                .ForMember(dest => dest.StandardCP, conf => conf.MapFrom(src => src.chrStandardCP))
                .ForMember(dest => dest.MWLap, conf => conf.MapFrom(src => src.sitMWLap))
                .ForMember(dest => dest.CWLap, conf => conf.MapFrom(src => src.sitCWLap))
                .ForMember(dest => dest.MO1, conf => conf.MapFrom(src => src.intMO1))
                .ForMember(dest => dest.MO2, conf => conf.MapFrom(src => src.intMO2))
                .ForMember(dest => dest.CO1, conf => conf.MapFrom(src => src.intCO1))
                .ForMember(dest => dest.CO2, conf => conf.MapFrom(src => src.intCO2))
                .ForMember(dest => dest.LapLength, conf => conf.MapFrom(src => src.intLapLength))
                .ForMember(dest => dest.EndLength, conf => conf.MapFrom(src => src.intEndLength))
                .ForMember(dest => dest.AdjFactor, conf => conf.MapFrom(src => src.intAdjFactor))
                .ForMember(dest => dest.CoverToLink, conf => conf.MapFrom(src => src.intCoverToLink))
                .ForMember(dest => dest.StandarCL, conf => conf.MapFrom(src => src.chrStandarCL))
                .ForMember(dest => dest.StatusId, conf => conf.MapFrom(src => src.tntStatusId))
                .ForMember(dest => dest.ParameterType, conf => conf.MapFrom(src => src.vchParameterType))
                .ForMember(dest => dest.StructureMarkingLevel, conf => conf.MapFrom(src => src.bitStructureMarkingLevel))
                .ForMember(dest => dest.ProductTypeId, conf => conf.MapFrom(src => src.sitProductTypeId))
                .ForMember(dest => dest.CreatedUID, conf => conf.MapFrom(src => src.intCreatedUID))
                .ForMember(dest => dest.CreatedDate, conf => conf.MapFrom(src => src.datCreatedDate))
                .ForMember(dest => dest.UpdatedUID, conf => conf.MapFrom(src => src.intUpdatedUID))
                .ForMember(dest => dest.UpdatedDate, conf => conf.MapFrom(src => src.datUpdatedDate))
                .ForMember(dest => dest.InnerCover, conf => conf.MapFrom(src => src.sitInnerCover))
                .ForMember(dest => dest.OuterCover, conf => conf.MapFrom(src => src.sitOuterCover))
                .ForMember(dest => dest.NetWeight, conf => conf.MapFrom(src => src.bitNetWeight)).ReverseMap();
        }
    }
          
}
