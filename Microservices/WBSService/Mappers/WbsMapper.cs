using AutoMapper;
using WBSService.Dtos;
using WBSService.Model;

namespace WBSService.Mappers
{
    public class WbsMapper : Profile
    {
        public WbsMapper()
        {
            CreateMap<WBSMaintainence, WBSMaintainenceDto>().ReverseMap();

            CreateMap<WBS, WBSDto>()
                .ForMember(dest => dest.WBSId, conf => conf.MapFrom(src => src.intWBSId))
                .ForMember(dest => dest.WBSTypeId, conf => conf.MapFrom(src => src.intWBSTypeId))
                .ForMember(dest => dest.ProjectId, conf => conf.MapFrom(src => src.intProjectId))
                .ForMember(dest => dest.CreatedUID, conf => conf.MapFrom(src => src.intCreatedUID))
                .ForMember(dest => dest.CreatedDate, conf => conf.MapFrom(src => src.datCreatedDate))
                .ForMember(dest => dest.UpdatedUID, conf => conf.MapFrom(src => src.intUpdatedUID))
                .ForMember(dest => dest.UpdatedDate, conf => conf.MapFrom(src => src.datUpdatedDate)).ReverseMap();




            CreateMap<WBSElements, WBSElementsDto>()
                .ForMember(dest => dest.ElementId, conf => conf.MapFrom(src => src.intWBSElementId))
                .ForMember(dest => dest.StoreyLevelWBSId, conf => conf.MapFrom(src => src.intStoreyLevelWBSId))
                .ForMember(dest => dest.WBSId, conf => conf.MapFrom(src => src.intWBSId))
                .ForMember(dest => dest.WBS1, conf => conf.MapFrom(src => src.vchWBS1))
                .ForMember(dest => dest.WBS2, conf => conf.MapFrom(src => src.vchWBS2))
                .ForMember(dest => dest.WBS3, conf => conf.MapFrom(src => src.vchWBS3))
                .ForMember(dest => dest.WBS4, conf => conf.MapFrom(src => src.vchWBS4))
                .ForMember(dest => dest.WBS5, conf => conf.MapFrom(src => src.vchWBS5))
                .ForMember(dest => dest.StatusId, conf => conf.MapFrom(src => src.tntStatusId))
                .ForMember(dest => dest.WBSMTNID, conf => conf.MapFrom(src => src.WBSMTNID)).ReverseMap();

            CreateMap<WBSAtCollapseLevel, WBSAtCollapseLevelDto>()
                .ForMember(dest => dest.StoreyLevelWBSId, conf => conf.MapFrom(src => src.intStoreyLevelWBSId))
                .ForMember(dest => dest.WBSId, conf => conf.MapFrom(src => src.intStoreyLevelWBSId))
                .ForMember(dest => dest.WBS1From, conf => conf.MapFrom(src => src.vchWBS1From))
                .ForMember(dest => dest.WBS1To, conf => conf.MapFrom(src => src.vchWBS1To))
                .ForMember(dest => dest.WBS2From, conf => conf.MapFrom(src => src.vchWBS2From))
                .ForMember(dest => dest.WBS2To, conf => conf.MapFrom(src => src.vchWBS2To))
                .ForMember(dest => dest.WBS3From, conf => conf.MapFrom(src => src.vchWBS3From))
                .ForMember(dest => dest.WBS3To, conf => conf.MapFrom(src => src.vchWBS3To))
                .ForMember(dest => dest.WBS4From, conf => conf.MapFrom(src => src.vchWBS4From))
                .ForMember(dest => dest.WBS4To, conf => conf.MapFrom(src => src.vchWBS4To))
                .ForMember(dest => dest.WBS5From, conf => conf.MapFrom(src => src.vchWBS5From))
                .ForMember(dest => dest.WBS5To, conf => conf.MapFrom(src => src.vchWBS5To))
                .ForMember(dest => dest.StatusId, conf => conf.MapFrom(src => src.tntStatusId))
                .ForMember(dest => dest.StructureElementTypeId, conf => conf.MapFrom(src => src.intStructureElementTypeId))
                .ForMember(dest => dest.sitProductTypeId, conf => conf.MapFrom(src => src.sitProductTypeId))
                .ForMember(dest=>dest.CreatedBy,conf=> conf.MapFrom(src=>src.CreatedBy))
                .ForMember(dest => dest.ModifiedBy, conf => conf.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.Created_On, conf => conf.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.Updated_On, conf => conf.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.WBSMTNID, conf => conf.MapFrom(src => src.WBSMTNID)).ReverseMap();

            CreateMap<ProductTypeMaster, ProductTypeMasterDto>()
                .ForMember(dest => dest.ProductTypeID, conf => conf.MapFrom(src => src.sitProductTypeID))
                .ForMember(dest => dest.ProductType, conf => conf.MapFrom(src => src.vchProductType))
                .ForMember(dest => dest.Status, conf => conf.MapFrom(src => src.tntStatusId)).ReverseMap();
                

            CreateMap<StructureElementMaster, StructureElementMasterDto>()
                .ForMember(dest => dest.StructureElementTypeId, conf => conf.MapFrom(src => src.intStructureElementTypeId))
                .ForMember(dest => dest.StructureElementType, conf => conf.MapFrom(src => src.vchStructureElementType))
                .ForMember(dest => dest.Status, conf => conf.MapFrom(src => src.tntStatusId)).ReverseMap();


            CreateMap<WBSAtCollapseLevel, WBSStoreyFromDto>()
                .ForMember(dest => dest.StoreyFrom, conf => conf.MapFrom(src => src.vchWBS2From)).ReverseMap();

            CreateMap<StoryToFrom, WBSStoreyToDto>()
                .ForMember(dest => dest.StoreyID, conf => conf.MapFrom(src => src.intWBSAbbreviationId))
                .ForMember(dest => dest.StoreyName, conf => conf.MapFrom(src => src.vchAbbreviation)).ReverseMap();

            //CreateMap<WBSMaintainence, WBSMaintainenceDto>()
            //    .ForMember(dest => dest.intWBSMTNCId, conf => conf.MapFrom(src => src.intWBSMTNCId))
            //    .ForMember(dest => dest.intWBSId, conf => conf.MapFrom(src => src.intWBSId))
            //    .ForMember(dest => dest.Block, conf => conf.MapFrom(src => src.Block))
            //    .ForMember(dest => dest.StoryFrom, conf => conf.MapFrom(src => src.StoryFrom))
            //    .ForMember(dest => dest.StoryTo, conf => conf.MapFrom(src => src.StoryTo))
            //    .ForMember(dest => dest.Part, conf => conf.MapFrom(src => src.Part))
            //    .ForMember(dest => dest.ProductType, conf => conf.MapFrom(src => src.ProductType))
            //    .ForMember(dest => dest.Structure, conf => conf.MapFrom(src => src.Structure)).ReverseMap();





        }
    }
}
