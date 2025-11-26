using AutoMapper;
using ProductCodeMasterService.Dtos;
using ProductCodeMasterService.Model;

namespace ProductCodeMasterService.Mapper
{
    public class DtoMapper :Profile
    {
        public DtoMapper()
        {

            CreateMap<ProductCodeMaster, ProductCodeMasterDto>()
                .ForMember(dest => dest.ProductCodeId, conf => conf.MapFrom(src => src.intProductCodeId))
                .ForMember(dest => dest.ProductCode, conf => conf.MapFrom(src => src.vchProductCode))
                .ForMember(dest => dest.ProductDescription, conf => conf.MapFrom(src => src.vchProductDescription))
                .ForMember(dest => dest.RawMaterial, conf => conf.MapFrom(src => src.bitRawMaterial)).ReverseMap();
            //.ForMember(dest => dest.SAPMaterialCodeId, conf => conf.MapFrom(src => src.intSAPMaterialCodeId))
            //.ForMember(dest => dest.ShapeCodeId, conf => conf.MapFrom(src => src.intShapeCodeId))
            //.ForMember(dest => dest.MWProductCodeId, conf => conf.MapFrom(src => src.intMWProductCodeId))
            //.ForMember(dest => dest.MWDiameter, conf => conf.MapFrom(src => src.decMWDiameter))
            //.ForMember(dest => dest.MWSpace, conf => conf.MapFrom(src => src.intMWSpace))
            //.ForMember(dest => dest.MaterialTypeId, conf => conf.MapFrom(src => src.intMaterialTypeId))
            //.ForMember(dest => dest.MWMaterialAbbr, conf => conf.MapFrom(src => src.vchMWMaterialAbbr))
            //.ForMember(dest => dest.MWGrade, conf => conf.MapFrom(src => src.vchMWGrade))
            //.ForMember(dest => dest.CWProductCodeId, conf => conf.MapFrom(src => src.intCWProductCodeId))
            //.ForMember(dest => dest.CWDiameter, conf => conf.MapFrom(src => src.decCWDiameter))
            //.ForMember(dest => dest.CWSpace, conf => conf.MapFrom(src => src.intCWSpace))
            //.ForMember(dest => dest.CWLength, conf => conf.MapFrom(src => src.decCWLength))
            //.ForMember(dest => dest.CWMaterialType, conf => conf.MapFrom(src => src.vchCWMaterialType))
            //.ForMember(dest => dest.CWMaterialAbbr, conf => conf.MapFrom(src => src.vchCWMaterialAbbr))
            //.ForMember(dest => dest.CWGrade, conf => conf.MapFrom(src => src.vchCWGrade))
            //.ForMember(dest => dest.WeightArea, conf => conf.MapFrom(src => src.decWeightArea))
            //.ForMember(dest => dest.WeigthPerMeterRun, conf => conf.MapFrom(src => src.decWeigthPerMeterRun))
            //.ForMember(dest => dest.CWWeigthPerMeterRun, conf => conf.MapFrom(src => src.decCWWeigthPerMeterRun))
            //.ForMember(dest => dest.MinLinkFactor, conf => conf.MapFrom(src => src.intMinLinkFactor))
            //.ForMember(dest => dest.MaxLinkFactor, conf => conf.MapFrom(src => src.intMaxLinkFactor))
            //.ForMember(dest => dest.TwinIndicator, conf => conf.MapFrom(src => src.chrTwinIndicator))
            //.ForMember(dest => dest.BOMIndicator, conf => conf.MapFrom(src => src.chrBOMIndicator))
            //.ForMember(dest => dest.StaggeredIndicator, conf => conf.MapFrom(src => src.bitStaggeredIndicator))
            //.ForMember(dest => dest.BendIndicator, conf => conf.MapFrom(src => src.bitBendIndicator))
            //.ForMember(dest => dest.MWMaxBendLength, conf => conf.MapFrom(src => src.decMWMaxBendLength))
            //.ForMember(dest => dest.CWMaxBendLength, conf => conf.MapFrom(src => src.decCWMaxBendLength))
            //.ForMember(dest => dest.MaterialNumber, conf => conf.MapFrom(src => src.vchMaterialNumber))
            //.ForMember(dest => dest.BaseUOM, conf => conf.MapFrom(src => src.vchBaseUOM))
            //.ForMember(dest => dest.SalesUOM, conf => conf.MapFrom(src => src.vchSalesUOM))
            //.ForMember(dest => dest.StatusId, conf => conf.MapFrom(src => src.tntStatusId))
            //.ForMember(dest => dest.CreatedUID, conf => conf.MapFrom(src => src.intCreatedUID))
            //.ForMember(dest => dest.CreatedDate, conf => conf.MapFrom(src => src.datCreatedDate))
            //.ForMember(dest => dest.UpdatedUID, conf => conf.MapFrom(src => src.intUpdatedUID))
            //.ForMember(dest => dest.UpdatedDate, conf => conf.MapFrom(src => src.datUpdatedDate))
            //.ForMember(dest => dest.ConvertionFactor, conf => conf.MapFrom(src => src.decConvertionFactor))
            //.ForMember(dest => dest.ApprovalId, conf => conf.MapFrom(src => src.intApprovalId))
            //.ForMember(dest => dest.ApprovedDate, conf => conf.MapFrom(src => src.datApprovedDate)).ReverseMap();


            CreateMap<CABProductCodeMaster, CABProductCodeMasterDto>()
                .ForMember(dest => dest.CABProductCodeID, conf => conf.MapFrom(src => src.intCABProductCodeID))
                .ForMember(dest => dest.GradeType, conf => conf.MapFrom(src => src.chrGradeType))
                .ForMember(dest => dest.GradeId, conf => conf.MapFrom(src => src.intGrade))
                .ForMember(dest => dest.Diameter, conf => conf.MapFrom(src => src.intDiameter))
                .ForMember(dest => dest.StdId, conf => conf.MapFrom(src => src.intStdID))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.vchDescription))
                .ForMember(dest => dest.CouplerIndicator, conf => conf.MapFrom(src => src.bitCouplerIndicator))
                .ForMember(dest => dest.CouplerType, conf => conf.MapFrom(src => src.vchCouplerType))
                .ForMember(dest => dest.CoupGradeID, conf => conf.MapFrom(src => src.intCoupGradeID))
                .ForMember(dest => dest.CouplStdID, conf => conf.MapFrom(src => src.intCouplStdID))
                .ForMember(dest => dest.CoupDiameter, conf => conf.MapFrom(src => src.intCoupDiameter))
                .ForMember(dest => dest.FGSAPMaterialID, conf => conf.MapFrom(src => src.intFGSAPMaterialID))
                .ForMember(dest => dest.RMSAPMaterialID, conf => conf.MapFrom(src => src.intRMSAPMaterialID))
                //.ForMember(dest => dest.FG_SAPMaterialCode, conf => conf.MapFrom(src => src.FG_SAPMaterialCode))
                //.ForMember(dest => dest.RM_SAPMaterialCode, conf => conf.MapFrom(src => src.RM_SAPMaterialCode))
                .ForMember(dest => dest.CoupSAPMaterialID, conf => conf.MapFrom(src => src.intCoupSAPMaterialID))
                .ForMember(dest => dest.WeightPerMRun, conf => conf.MapFrom(src => src.intWeightPerMRun))
                .ForMember(dest => dest.BaseUOM, conf => conf.MapFrom(src => src.intBaseUOM))
                .ForMember(dest => dest.SalesUOM, conf => conf.MapFrom(src => src.intSalesUOM))
                .ForMember(dest => dest.BPCItem, conf => conf.MapFrom(src => src.bitBPCItem))
                .ForMember(dest => dest.StatusId, conf => conf.MapFrom(src => src.tntStatusId))
                .ForMember(dest => dest.CreatedUser, conf => conf.MapFrom(src => src.CreatedUser))
                .ForMember(dest => dest.CreatedUser, conf => conf.MapFrom(src => src.CreatedUser))
                .ForMember(dest => dest.CreatedUId, conf => conf.MapFrom(src => src.intCreatedUId))
                .ForMember(dest => dest.CreatedDate, conf => conf.MapFrom(src => src.datCreatedDate))
                .ForMember(dest => dest.UpdatedUId, conf => conf.MapFrom(src => src.intUpdatedUId))
                .ForMember(dest => dest.ProductCode, conf => conf.MapFrom(src => src.ProductCode))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description))
                .ForMember(dest => dest.UpdatedDate, conf => conf.MapFrom(src => src.datUpdatedDate)).ReverseMap();

           

            
      
        
        }
    }
}
