using AutoMapper;
using ShapeCodeService.Dtos;
using ShapeCodeService.Models;

namespace ShapeCodeService.Mappers
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {         

            CreateMap<Shapegroup, ShapegroupDto>()
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.SG_IDENT))
                .ForMember(dest => dest.ShapeGroupName, conf => conf.MapFrom(src => src.SG_CODE))
                .ForMember(dest => dest.ShapeGroupDesc, conf => conf.MapFrom(src => src.SG_DESC))
                .ForMember(dest => dest.DimentionType, conf => conf.MapFrom(src => src.SG_CON_DIM))
                .ForMember(dest => dest.BendingBarType, conf => conf.MapFrom(src => src.SG_CON_BEND))
                .ForMember(dest => dest.StructureType, conf => conf.MapFrom(src => src.SG_CON_STCUR))
                .ForMember(dest => dest.CouplerType, conf => conf.MapFrom(src => src.SG_CON_COU))
                .ForMember(dest => dest.IsArchived, conf => conf.MapFrom(src => src.SG_ARCHIVED)).ReverseMap();                  

            CreateMap<Shapesurchage, ShapeSurchageDto>()
                .ForMember(dest => dest.ID, conf => conf.MapFrom(src => src.IDENTITYNO))
                .ForMember(dest => dest.ShapeCode_Id, conf => conf.MapFrom(src => src.INTSHAPECODE))
                .ForMember(dest => dest.ShapeCode, conf => conf.MapFrom(src => src.CHRSHAPECODE))
                .ForMember(dest => dest.Bar_Dia, conf => conf.MapFrom(src => src.BARDIA))
                .ForMember(dest => dest.Invoice_Length, conf => conf.MapFrom(src => src.INVLEN))
                .ForMember(dest => dest.Surcharge, conf => conf.MapFrom(src => src.CHRSURCHARGE))
                .ForMember(dest => dest.Surchage_Code, conf => conf.MapFrom(src => src.INTSURCHARGECODE))
                .ForMember(dest => dest.Condition_Id, conf => conf.MapFrom(src => src.INTCONDITION))
                .ForMember(dest => dest.Dia_Condition, conf => conf.MapFrom(src => src.CHRCONDITION))
                .ForMember(dest => dest.User_Id, conf => conf.MapFrom(src => src.Updated_By))
                .ForMember(dest => dest.Updated_Date, conf => conf.MapFrom(src =>src.Updated_Date.ToString("dd/MM/yyyy"))).ReverseMap();

            
  //          Mapper.CreateMap<I_NEWS, NewsModel>().ForMember(x => x.DateCreated,
  //opt => opt.MapFrom(src => ((DateTime)src.DateCreated).ToShortDateString()));

            CreateMap<ShapeCodes, ShapeCodesDto>()
                .ForMember(dest => dest.Shape_Id, conf => conf.MapFrom(src => src.intShapeID))
                .ForMember(dest => dest.ShapeCode, conf => conf.MapFrom(src => src.chrShapeCode)).ReverseMap();

            CreateMap<SurchargeDropdown, SurchargeDropdownDto>()
                //.ForMember(dest => dest.ID, conf => conf.MapFrom(src => src.IDENTITYNO))
                .ForMember(dest => dest.Surcharge, conf => conf.MapFrom(src => src.CHRSURCHARGE));
                //.ForMember(dest => dest.Surchage_Code, conf => conf.MapFrom(src => src.INTSURCHARGECODE));

        }


    }
}
