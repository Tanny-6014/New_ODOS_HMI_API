using AutoMapper;
using Dapper;
using DetailingService.Dtos;
using DetailingService.Interfaces;
using DetailingService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System.Web;

namespace DetailingService.Controllers
{
    //[ApiController]
    [Route("api/[controller]")]
    public class DetailingController : ControllerBase
    {

        private readonly IMeshDetailing _MeshDetailing;
        private readonly ISlabService _Slabservice;
        private readonly IBOM _BomService;
        private readonly IAccessories _Accessories;
        private readonly IMapper _mapper;
        private readonly IColumnService _ColumnService;
        private readonly IBeamService _BeamService;
        private readonly IPRCDetailingService _PRCDetailingService;
        private readonly IGroupMarkDal _groupmarkdal;
        private readonly ICABService _cABService;
        private readonly IAccountService _accountService;
       




        public DetailingController(IMeshDetailing Detailing, IMapper mapper, ISlabService slabservice, IColumnService columnService, IBeamService beamService, IAccessories accessories, IPRCDetailingService PRCDetailingService, IBOM Bom, IGroupMarkDal groupMarkDal, ICABService cABService,IAccountService accountService)
        {
            _MeshDetailing = Detailing;
            _mapper = mapper;
            _Slabservice = slabservice;
            _ColumnService = columnService;
            _BeamService = beamService;
            _Accessories = accessories;
            _BomService = Bom;
            _PRCDetailingService = PRCDetailingService;
            _groupmarkdal = groupMarkDal;
            _cABService = cABService;
            _accountService = accountService;

        }

        #region Slab Detailing
        
        [HttpGet]
        [Route("/GetMeshDetailingList/{ProjectID}")]
        public async Task<IActionResult> GetMeshDetailingList(int ProjectID)
        {
            IEnumerable<GetGroupMarkListDto> DetailingList = await _MeshDetailing.GetMeshDetailingListAsync(ProjectID);
            var result = _mapper.Map<List<GetGroupMarkListDto>>(DetailingList);
            return Ok(result);
        }


        [HttpGet]
        [Route("/GetSlabParameterSetbyProjIdProdType/{ProjectID}/{productTypeId}")]
        public async Task<IActionResult> GetSlabParameterSetbyProjIdProdType(int ProjectID, int productTypeId)
        {
            IEnumerable<ShapeCodeParameterSetDto> shapeCodeParameterSetDtos = await _MeshDetailing.SlabParameterSetbyProjIdProdType(ProjectID, productTypeId);
            var result = shapeCodeParameterSetDtos;
            return Ok(result);
        }


        [HttpGet]
        [Route("/GetStructureMarkingDetails/{DetailingID}/{StructureElementId}")]
        public async Task<IActionResult> GetStructureMarkingDetails(int DetailingID, int StructureElementId)
        {
            string errorMessage = "No record Found";
            List<SlabStructure> SlabStructure = _Slabservice.GetStructureMarkingDetails(DetailingID, StructureElementId, out errorMessage);
            var result = SlabStructure;
            if (result == null)
            {
                //string message = "Employee doesn't exist";
                //throw new HttpResponseException(
                //    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            return Ok(result);
        }

   


        [HttpGet]
        [Route("/GetStructureMarkingDetailsCollap/{StructureMarkingID}")]
        public async Task<IActionResult> GetStructureMarkingDetailsCollap(int StructureMarkingID)
        {
            SlabProduct objSlabProductMark = new SlabProduct();

            List<SlabProduct> listSlabProductMark = new List<SlabProduct>();


            listSlabProductMark = objSlabProductMark.SlabProductByStructureMarkId_Get(StructureMarkingID, 4);
            var result = listSlabProductMark;
            return Ok(result);
        }
        [HttpGet]
        [Route("/SlabParameterSetbyProjIdProdType/{projectId}/{productTypeId}")]
        public async Task<IActionResult> SlabParameterSetbyProjIdProdType(int projectId, int productTypeId)
        {
            string errorMessage = "";
            List<ShapeCodeParameterSet> ShapeCodeParameterSet = _Slabservice.SlabParameterSetbyProjIdProdType(projectId, productTypeId);
            var result = ShapeCodeParameterSet;
            return Ok(result);
        }

        [HttpPost]
        [Route("/InsertSlabStructureMarking/{structureElementId}/{projectTypeId}/{projectId}/{productTypeId}/{userId}")]
        public async Task<IActionResult> InsertSlabStructureMarking([FromBody] InsertSlabStructureMarkingDto obj, int structureElementId, int projectTypeId, int projectId, int productTypeId, int userId)
        {
            string errorMessage = "";

            var check = obj.Shapecode.ShapeParam;


            try
            {
              
                
                SlabStructure slabStructure = new SlabStructure();
                slabStructure.SEDetailingID = obj.SEDetailingID;
                slabStructure.StructureMarkId = obj.StructureMarkId;
                slabStructure.ParentStructureMarkId = obj.ParentStructureMarkId;
                slabStructure.StructureMarkingName = obj.StructureMarkingName;
                slabStructure.ParamSetNumber = obj.ParameterSet.TNTPARAMSETNUMBER;
                slabStructure.MainWireLength = obj.MainWireLength;
                slabStructure.CrossWireLength = obj.CrossWireLength;
                slabStructure.MemberQty = obj.MemberQty;
                slabStructure.BendingCheck = obj.BendingCheck;
                slabStructure.MachineCheck = obj.MachineCheck;
                slabStructure.TransportCheck = obj.TransportCheck;
                slabStructure.MultiMesh = obj.MultiMesh;
                slabStructure.ProduceIndicator = obj.ProduceIndicator;
                slabStructure.PinSize = obj.PinSize;
                slabStructure.ProductGenerationStatus = obj.ProductGenerationStatus;
                slabStructure.SideForCode = obj.SideForCode;
                slabStructure.ProductSplitUp = obj.ProductSplitUp;

                slabStructure.ParameterSet = new ShapeCodeParameterSet
                {
                    MaxCWLength = obj.ParameterSet.MaxCWLength,
                    MaxMWLength = obj.ParameterSet.MaxMWLength,
                    MinMo1 = obj.ParameterSet.MINMO1,
                    MinMo2 = obj.ParameterSet.MINMO2,
                    MinCo1 = obj.ParameterSet.MINCO1,
                    MinCo2 = obj.ParameterSet.MINCO2,
                    MachineMaxCWLength = obj.ParameterSet.MACHINEMAXCWLENGTH,
                    MachineMaxMWLength = obj.ParameterSet.MACHINEMAXMWLENGTH,
                    TransportMaxHeight=obj.ParameterSet.TransportMaxHeight,
                    TransportMaxWeight=obj.ParameterSet.TransportMaxWeight,
                    TransportMaxLength=obj.ParameterSet.TransportMaxLength,
                    TransportMaxWidth=obj.ParameterSet.TransportMaxWidth

                };
                slabStructure.ProductCode = obj.ProductCode;

                var result = _Slabservice.InsertSlabStructureMarking(slabStructure, structureElementId, projectTypeId, projectId, productTypeId, out errorMessage, userId);

                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);

                }


                IEnumerable<MeshStructureMarkingDetailsDto> meshStructureMarkingDetailsDtos = await _MeshDetailing.MeshStructureMarkingDetails_Get();

                MeshStructureMarkingDetailsDto MeshDto = new MeshStructureMarkingDetailsDto();

                MeshDto = meshStructureMarkingDetailsDtos.FirstOrDefault();

                IEnumerable<AddSlabProductMarkingDto> addSlabProducts_IE = await _MeshDetailing.MeshProductMarkingDetails_Get(MeshDto.intStructureMarkId);

                List<AddSlabProductMarkingDto> addSlabProducts_List = addSlabProducts_IE.ToList();

                AddSlabProductMarkingDto addSlab = new AddSlabProductMarkingDto();

                for (int i = 0; i < addSlabProducts_List.Count; i++)
                {




                    addSlab = addSlabProducts_List[i];


                    slabStructure.ParameterSet = new ShapeCodeParameterSet
                    {
                        MaxCWLength = obj.ParameterSet.MaxCWLength,
                        MaxMWLength = obj.ParameterSet.MaxMWLength,
                        MinMo1 = obj.ParameterSet.MINMO1,
                        MinMo2 = obj.ParameterSet.MINMO2,
                        MinCo1 = obj.ParameterSet.MINCO1,
                        MinCo2 = obj.ParameterSet.MINCO2,
                        MachineMaxCWLength = obj.ParameterSet.MACHINEMAXCWLENGTH,
                        MachineMaxMWLength = obj.ParameterSet.MACHINEMAXMWLENGTH,
                        TransportMaxHeight = obj.ParameterSet.TransportMaxHeight,
                        TransportMaxWeight = obj.ParameterSet.TransportMaxWeight,
                        TransportMaxLength = obj.ParameterSet.TransportMaxLength,
                        TransportMaxWidth = obj.ParameterSet.TransportMaxWidth



                    };
                    slabStructure.ProductCode = obj.ProductCode;
                    SlabProduct Slab_prod = new SlabProduct();


                    Slab_prod.shapecode = obj.Shapecode;
                    Slab_prod.StructureMarkId = addSlab.INTSTRUCTUREMARKID;
                    Slab_prod.ProductCodeId = addSlab.INTPRODUCTCODEID;
                    Slab_prod.ProductMarkingName = addSlab.VCHPRODUCTMARKINGNAME;
                    Slab_prod.ShapeParam = addSlab.PARAMVALUES;
                    Slab_prod.MWLength = addSlab.NUMINVOICEMWLENGTH;
                    Slab_prod.CWLength = addSlab.NUMINVOICECWLENGTH;
                    Slab_prod.NoOfMainWire = 2;
                    Slab_prod.ProductionMWLength = addSlab.NUMPRODUCTIONMWLENGTH;
                    Slab_prod.ProductionCWLength = addSlab.NUMPRODUCTIONCWLENGTH;
                    Slab_prod.ProductionMWQty = addSlab.INTPRODUCTIONMAINQTY;
                    Slab_prod.ProductionCWQty = addSlab.INTPRODUCTIONCROSSQTY;
                    Slab_prod.MemberQty = addSlab.INTMEMBERQTY;
                    Slab_prod.MO1 = obj.MO1;
                    Slab_prod.MO2 = obj.MO2;
                    Slab_prod.CO1 = obj.CO1;
                    Slab_prod.CO2 = obj.CO2;
                    Slab_prod.ProductionMO1 = addSlab.INTPRODUCTIONMO1;
                    Slab_prod.ProductionMO2 = addSlab.INTPRODUCTIONMO2;
                    Slab_prod.ProductionCO1 = addSlab.INTPRODUCTIONCO1;
                    Slab_prod.ProductionCO2 = addSlab.INTPRODUCTIONCO2;
                    Slab_prod.MWSpacing = addSlab.INTMWSPACING;
                    Slab_prod.CWSpacing = addSlab.INTCWSPACING;
                    Slab_prod.ProductMarkId = addSlab.intMeshProductMarkId;
                    Slab_prod.TheoraticalWeight = addSlab.NUMTHEORATICALWEIGHT;
                    Slab_prod.InvoiceArea = addSlab.NUMINVOICEAREA;
                    Slab_prod.NetWeight = addSlab.NUMNETWEIGHT;
                    Slab_prod.ProductionWeight = addSlab.NUMPRODUCTIONWEIGHT;
                    Slab_prod.EnvelopeLength = addSlab.INTENVELOPLENGTH;
                    Slab_prod.EnvelopeWidth = addSlab.INTENVELOPWIDTH;
                    Slab_prod.EnvelopeHeight = addSlab.INTENVELOPHEIGHT;
                    Slab_prod.ProduceIndicator = obj.ProduceIndicator;
                    Slab_prod.BOMIndicator = "Yes";
                    Slab_prod.ParamValues = addSlab.PARAMVALUES;
                    Slab_prod.BOMDrawingPath = addSlab.BOMDrawingPath;
                    Slab_prod.MWBVBSString = addSlab.NVCHMWBVBSSTRING;
                    Slab_prod.CWBVBSString = addSlab.NVCHCWBVBSSTRING;
                    Slab_prod.InvoiceMWWeight = addSlab.NUMINVOICEMWWEIGHT;
                    Slab_prod.InvoiceCWWeight = addSlab.NUMINVOICECWWEIGHT;
                    Slab_prod.ProductionMWWeight = addSlab.NUMPRODUCTIONMWWEIGHT;
                    Slab_prod.ProductionCWWeight = addSlab.NUMPRODUCTIONCWWEIGHT;
                    Slab_prod.Deleteflag = 1;
                    Slab_prod.ProductValidator = 1;
                    Slab_prod.MWPitch = addSlab.MWPITCH;
                    Slab_prod.CWPitch = addSlab.CWPITCH;
                    Slab_prod.MWFlag = addSlab.MWFLAG;
                    Slab_prod.CWFlag = addSlab.CWFLAG;
                    Slab_prod.BendingCheckInd = addSlab.BENDINGCHECK;
                    Slab_prod.PinSize = addSlab.SITPINSIZE;
                    var res = _Slabservice.UpdateSlabProductMarking(slabStructure, Slab_prod, 1, structureElementId, true, "", true, 1, out errorMessage);




                }

                if (errorMessage == "")
                {

                    return Ok(errorMessage);
                }
                else
                {
                    _Slabservice.DeleteStructureMarking(slabStructure.StructureMarkId);

                    return BadRequest(errorMessage);
                }
            }


            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("/UpdateSlabStructureMarking/{structureElementId}/{projectTypeId}/{projectId}/{productTypeId}/{userId}/{updateWithProd}")]
        public async Task<IActionResult> UpdateSlabStructureMarking([FromBody] SlabStructure obj, int structureElementId, int projectTypeId, int projectId, int productTypeId, int userId,bool updateWithProd)
        {
            string errorMessage = "";

            try
            {
                ShapeCodeParameterSet objShapeCode = new ShapeCodeParameterSet();
                List<ShapeCodeParameterSet> listProjectParam = objShapeCode.ProjectParamLap_Get(obj.ProductCode.ProductCodeId, obj.ParamSetNumber);
                ShapeCodeParameterSet projectParamFiltered;
                if (structureElementId == 13)
                {
                    projectParamFiltered = (from item in listProjectParam
                                            where item.ParameterSetNumber == obj.ParamSetNumber && item.productCode.ProductCodeId == obj.ProductCode.ProductCodeId && item.StructureElementType == structureElementId
                                            select item).FirstOrDefault();
                }
                else
                {
                    projectParamFiltered = (from item in listProjectParam
                                            where item.ParameterSetNumber == obj.ParamSetNumber && item.productCode.ProductCodeId == obj.ProductCode.ProductCodeId && item.StructureElementType != 13  //Vanita Commented
                                            select item).FirstOrDefault();
                }

                if (projectParamFiltered == null) throw new Exception("MWLap and CWLap not found for the selected product and parameter set.");

                bool result_updateWithProd = false;
                List < SlabProduct > result  = new List < SlabProduct >();

                if (updateWithProd)
                {
                    result = _Slabservice.UpdateSlabStructureMarkingWithProductGeneration(obj, structureElementId, projectTypeId,projectId,productTypeId, out errorMessage, userId);
                }
                else
                {
                    result_updateWithProd = _Slabservice.UpdateSlabStructureMarkingWithoutProductGeneration(obj, structureElementId, projectTypeId, out errorMessage, userId);

                }
                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);
                }
                else
                {

                    if (updateWithProd)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return Ok(result_updateWithProd);
                        
                    }
                   
                }
       
            }


            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPost]
        [Route("/UpdateProductMark/{structureElementId}")]
        public async Task<IActionResult> UpdateProductMark([FromBody] UpdateSlabproduct slab, int structureElementId)
        {
            string errorMessage = "";

            string thisError = "";

            SlabStructure slabstr = slab.structureMark;

            try
            {
                slabstr.ParameterSet = new ShapeCodeParameterSet
                {
                    MaxCWLength = slab.ParameterSet.MaxCWLength,
                    MaxMWLength = slab.ParameterSet.MaxMWLength,
                    MinMo1 = slab.ParameterSet.MINMO1,
                    MinMo2 = slab.ParameterSet.MINMO2,
                    MinCo1 = slab.ParameterSet.MINCO1,
                    MinCo2 = slab.ParameterSet.MINCO2,
                    MachineMaxCWLength = slab.ParameterSet.MACHINEMAXCWLENGTH,
                    MachineMaxMWLength = slab.ParameterSet.MACHINEMAXMWLENGTH,
                    TransportMaxHeight = slab.ParameterSet.TransportMaxHeight,
                    TransportMaxWeight = slab.ParameterSet.TransportMaxWeight,
                    TransportMaxLength = slab.ParameterSet.TransportMaxLength,
                    TransportMaxWidth = slab.ParameterSet.TransportMaxWidth


                };
                var result = _Slabservice.UpdateSlabProductMarking(slabstr, slab.slabprod, 1,  structureElementId, true, "test", true, 1, out errorMessage);

                if (errorMessage == "")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPost]
        [Route("/CopySlabproductmark/{structureElementId}")]
        public async Task<IActionResult> CopyslabProduct([FromBody] SlabProduct objProductMarkingMarking, int structureElementId)
        {
            string errorMessage = "";
            var result = _Slabservice.InsertSlabProductMarking(objProductMarkingMarking, structureElementId, 1);

            return Ok(errorMessage);



        }

        [HttpPost]
        [Route("/RegenerateValidationSlab/{structureElementId}/{projectTypeId}/{projectId}/{productTypeId}/{seDetailingID}")]
        public async Task<IActionResult> RegenerateValidation([FromBody] ShapeCodeParameterSetDto ParameterSet, int structureElementId, int projectTypeId, int projectId, int productTypeId, int seDetailingID)
        {
            string errorMessage = "";

            SlabStructure structureMarkEntryCollection = new SlabStructure();

            ShapeCodeParameterSet parameterSet = new ShapeCodeParameterSet
            {
                MaxCWLength = ParameterSet.MaxCWLength,
                MaxMWLength = ParameterSet.MaxMWLength,
                MinMo1 = ParameterSet.MINMO1,
                MinMo2 = ParameterSet.MINMO2,
                MinCo1 = ParameterSet.MINCO1,
                MinCo2 = ParameterSet.MINCO2,
                MachineMaxCWLength = ParameterSet.MACHINEMAXCWLENGTH,
                MachineMaxMWLength = ParameterSet.MACHINEMAXMWLENGTH,
                ParameterSetNumber = ParameterSet.TNTPARAMSETNUMBER,
                ParameterSetValue = (int)ParameterSet.INTPARAMETESET,
                TransportMaxHeight =ParameterSet.TransportMaxHeight,
                TransportMaxWeight = ParameterSet.TransportMaxWeight,
                TransportMaxLength =ParameterSet.TransportMaxLength,
                TransportMaxWidth = ParameterSet.TransportMaxWidth


            };




            List<SlabStructure> structureMarkList = structureMarkEntryCollection.SlabStructureMark_Get(seDetailingID, structureElementId);


            var result = _Slabservice.RegenerateValidation(structureMarkList, parameterSet, structureElementId, projectTypeId, projectId, productTypeId, 1,out errorMessage);

            if(errorMessage!="")
            {
                return BadRequest(errorMessage);
            }
            else
            {
            return Ok(errorMessage);

            }



        }



        [HttpGet]
        [Route("/PopulateSlabShapeCode")]
        public async Task<IActionResult> PopulateSlabShapeCode()
        {
            string errorMessage = "";
            List<ShapeCode> shapeCodes = _Slabservice.PopulateSlabShapeCode();
            var result = shapeCodes;
            return Ok(result);
        }


        [HttpGet]
        [Route("/FilterProductCode/{enteredText}")]
        public async Task<IActionResult> FilterProductCode(string enteredText)
        {
            string errorMessage = "";
            List<ProductCode> productCodes = _Slabservice.PopulateProductCode(out errorMessage);
            var result = productCodes;
            return Ok(result);
        }

        [HttpGet]
        [Route("/ACSFilterProductCode/{enteredText}")]
        public async Task<IActionResult> AcsFilterProductCode(string enteredText)
        {
            string errorMessage = "";
            List<ProductCode> productCodes = _Slabservice.FilterProductCode(enteredText);
            var result = productCodes;
            return Ok(result);
        }

        [HttpGet]
        [Route("/PopulateHeaderByProjectId/{ProjectID}")]
        public async Task<IActionResult> PopulateHeaderByProjectId(int ProjectID)
        {

            List<BeamDetailinfo> beamDetailinfos = _MeshDetailing.PopulateHeaderByProjectId(ProjectID);
            var result = beamDetailinfos;
            return Ok(result);
        }

        [HttpDelete]
        [Route("/DeleteStructureMarking/{StructureMarkId}")]
        public bool DeleteStructureMarking(int StructureMarkId)
        {
            bool deleteStructMark = false;
            try
            {
                deleteStructMark = _Slabservice.DeleteStructureMarking(StructureMarkId);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return deleteStructMark;

        }


        [HttpDelete]
        [Route("/DeleteSlabProductMarking/{ProductMarkId}/{seDetailingId}")]
        public bool DeleteSlabProductMarking(int ProductMarkId, int seDetailingId)
        {
            bool deleteStructMark = false;
            string errorMessage = "";
            try
            {
                deleteStructMark = _Slabservice.DeleteProductMarking(ProductMarkId, seDetailingId, out errorMessage);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return deleteStructMark;

        }
        //DeleteProductMarking

        [HttpDelete]
        [Route("/DeleteGroupMark/{INTGROUPMARKID}")]
        public async Task<IActionResult> DeleteGroupMark(int INTGROUPMARKID)
        {

            var GroupMarkDelete = await _MeshDetailing.DeleteGroupMarkAsync(INTGROUPMARKID);
            return Ok(new { result = GroupMarkDelete, response = "success" });

        }
        [HttpGet]
        [Route("/MeshStructureMarkingDetails")]
        public async Task<IActionResult> MeshStructureMarkingDetails()
        {
            IEnumerable<MeshStructureMarkingDetailsDto> meshStructureMarkingDetailsDtos = await _MeshDetailing.MeshStructureMarkingDetails_Get();
            var result = _mapper.Map<List<MeshStructureMarkingDetailsDto>>(meshStructureMarkingDetailsDtos);
            return Ok(result);
        }



        [HttpGet]
        [Route("/MeshProductMarkingDetails/{StructureMarkID}")]
        public async Task<IActionResult> MeshProductMarkingDetails(int StructureMarkID)
        {
            IEnumerable<AddSlabProductMarkingDto> addSlabProducts = await _MeshDetailing.MeshProductMarkingDetails_Get(StructureMarkID);
            var result = _mapper.Map<List<AddSlabProductMarkingDto>>(addSlabProducts);
            return Ok(result);
        }

        

        #endregion

        #region Column Detailing   
        [HttpGet]
        [Route("/GetColumnStructureMarkingDetails/{projectID}/{seDetailingID}/{structureElementTypeID}/{productTypeID}")]
        public async Task<IActionResult> GetStructureMarkingDetails(int projectID, int seDetailingID, int structureElementTypeID, int productTypeID)
        {
            string errorMessage = "";
            List<ColumnStructure> ColumnStructure = _ColumnService.GetStructureMarkingDetails(projectID, seDetailingID, structureElementTypeID, productTypeID);
            var result = ColumnStructure;
            return Ok(result);
        }


        [HttpGet]
        [Route("/PopulateColumnShapeCode")]
        public async Task<IActionResult> PopulateShapeCode()
        {
            string errorMessage = "";
            List<ShapeCode> ColumnStructure = _ColumnService.PopulateShapeCode();
            var result = ColumnStructure;
            return Ok(result);
        }


        [HttpGet]
        [Route("/PopulateColumnProductCode/{structureElementTypeID}/{productTypeID}")]
        public async Task<IActionResult> PopulateProductCode(int structureElementTypeID, int productTypeID)
        {
            string errorMessage = "";
            List<ProductCode> ColumnProductCode = _ColumnService.PopulateProductCode(structureElementTypeID, productTypeID);
            var result = ColumnProductCode;
            return Ok(result);
        }

        [HttpGet]
        [Route("/PopulateFilterProductCode/{structureElementTypeID}/{productTypeID}/{enteredText}")]
        public async Task<IActionResult> FilterProductCode(int structureElementTypeID, int productTypeID, string enteredText)
        {
            string errorMessage = "";
            List<ProductCode> ColumnProductCode = _ColumnService.FilterProductCode(structureElementTypeID, productTypeID, enteredText, out errorMessage);
            var result = ColumnProductCode;
            return Ok(result);
        }


        [HttpGet]
        [Route("/PopulateFilterClinkProduct/{enteredText}")]
        public async Task<IActionResult> FilterClinkProduct(string enteredText)
        {
            string errorMessage = "";
            List<ProductCode> ColumnProductCode = _ColumnService.FilterClinkProduct(enteredText);
            var result = ColumnProductCode;
            return Ok(result);
        }

        [HttpPost]
        [Route("/InsertColumnStructureMarking/{topCover}/{bottomCover}/{leftCover}/{rightCover}/{leg}/{seDetailingID}/{userId}")]
        public async Task<IActionResult> InsertColumnStructureMarking([FromBody] AddColumnStructureMarkDto ColumnStructureObj, int topCover, int bottomCover, int leftCover, int rightCover, int leg, int seDetailingID, int userId)
        {
            string errorMessage = "";
            try
            {

                if (ColumnStructureObj != null)
                {
                    ColumnStructure columnstructureobj = new ColumnStructure();
                    columnstructureobj.StructureMarkingName = ColumnStructureObj.StructureMarkingName;
                    columnstructureobj.ProductCode = ColumnStructureObj.productCode;
                    columnstructureobj.Shape = ColumnStructureObj.ShapeCode;
                    columnstructureobj.ColumnHeight = ColumnStructureObj.ColumnHeight;
                    columnstructureobj.ColumnWidth = ColumnStructureObj.ColumnWidth;
                    columnstructureobj.ColumnLength = ColumnStructureObj.ColumnLength;
                    columnstructureobj.TotalNoOfLinks = ColumnStructureObj.TotalNoOfLinks;
                    columnstructureobj.MemberQty = ColumnStructureObj.MemberQty;
                    columnstructureobj.ClinkProductLength = ColumnStructureObj.ClinkProductLength;
                    columnstructureobj.ClinkProductWidth = ColumnStructureObj.ClinkProductWidth;
                    columnstructureobj.RowatLength = ColumnStructureObj.RowatLength;
                    columnstructureobj.RowatWidth = ColumnStructureObj.RowatWidth;
                    columnstructureobj.IsCLink = ColumnStructureObj.IsCLink;
                    columnstructureobj.CLOnly = ColumnStructureObj.CLOnly;
                    columnstructureobj.ProduceIndicator = ColumnStructureObj.ProduceIndicator;
                    columnstructureobj.BendingCheckInd = ColumnStructureObj.BendingCheckInd;
                    columnstructureobj.PinSize = ColumnStructureObj.PinSize;
                    columnstructureobj.SEDetailingID = ColumnStructureObj.SEDetailingID;
                    columnstructureobj.ParamSetNumber = ColumnStructureObj.ParamSetNumber;
                    columnstructureobj.ParentStructureMarkId = ColumnStructureObj.ParentStructureMarkId;

                    List<ColumnProduct> Columnproduct = _ColumnService.InsertColumnStructureMarking(columnstructureobj, topCover, bottomCover, leftCover, rightCover, leg, seDetailingID, userId, out errorMessage);

                    if (errorMessage == "")
                    {
                        var result = Columnproduct;
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(errorMessage);
                    }
                }
                else
                {
                    errorMessage = "Please contact System Administrator.";
                    return BadRequest(errorMessage);
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }


        [HttpGet]
        [Route("/ColumnParameterSetbyProjIdProdType/{projectId}/{productTypeId}")]
        public async Task<IActionResult> ColumnParameterSetbyProjIdProdType(int projectId, int productTypeId)
        {
            string errorMessage = "";
            List<ShapeCodeParameterSet> ShapeCodeParameterSet = _ColumnService.ColumnParameterSetbyProjIdProdType(projectId, productTypeId);
            var result = ShapeCodeParameterSet;
            return Ok(result);
        }



        [HttpDelete]
        [Route("/DeleteColumnStructureMarking/{StructureMarkId}")]
        public bool DeleteColumnStructureMarking(int StructureMarkId)
        {
            bool deleteStructMark = false;
            try
            {
                string errorMessage = "";
                deleteStructMark = _ColumnService.DeleteStructureMarking(StructureMarkId, out string errorMsg);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return deleteStructMark;

        }



        [HttpPost]
        [Route("/UpdateColumnStructureMarking/{topCover}/{bottomCover}/{leftCover}/{rightCover}/{leg}/{seDetailingID}/{userId}")]
        public async Task<IActionResult> UpdateColumnStructureMarking([FromBody] AddColumnStructureMarkDto ColumnStObj, int topCover, int bottomCover, int leftCover, int rightCover, int leg, int seDetailingID, int userId)
        {
            string errorMessage = "";
            try
            {
                ColumnStructure columnstructureobj = new ColumnStructure();
                columnstructureobj.StructureMarkId = ColumnStObj.StructureMarkId;
                columnstructureobj.StructureMarkingName = ColumnStObj.StructureMarkingName;
                columnstructureobj.ProductCode = ColumnStObj.productCode;
                columnstructureobj.Shape = ColumnStObj.ShapeCode;
                columnstructureobj.ColumnHeight = ColumnStObj.ColumnHeight;
                columnstructureobj.ColumnWidth = ColumnStObj.ColumnWidth;
                columnstructureobj.ColumnLength = ColumnStObj.ColumnLength;
                columnstructureobj.TotalNoOfLinks = ColumnStObj.TotalNoOfLinks;
                columnstructureobj.MemberQty = ColumnStObj.MemberQty;
                columnstructureobj.ClinkProductLength = ColumnStObj.ClinkProductLength;
                columnstructureobj.ClinkProductWidth = ColumnStObj.ClinkProductWidth;
                columnstructureobj.RowatLength = ColumnStObj.RowatLength;
                columnstructureobj.RowatWidth = ColumnStObj.RowatWidth;
                columnstructureobj.IsCLink = ColumnStObj.IsCLink;
                columnstructureobj.CLOnly = ColumnStObj.CLOnly;
                columnstructureobj.ProduceIndicator = ColumnStObj.ProduceIndicator;
                columnstructureobj.BendingCheckInd = ColumnStObj.BendingCheckInd;
                columnstructureobj.PinSize = ColumnStObj.PinSize;
                columnstructureobj.SEDetailingID = ColumnStObj.SEDetailingID;
                columnstructureobj.ParamSetNumber = ColumnStObj.ParamSetNumber;

                List<ColumnProduct> Columnproduct = _ColumnService.UpdateColumnStructureMarking(columnstructureobj, topCover, bottomCover, leftCover, rightCover, leg, seDetailingID, userId, out errorMessage);
                if (errorMessage == "")
                {
                    var result = Columnproduct;
                    return Ok(result);
                }
                else
                {
                    return BadRequest(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


            return Ok("");
        }




        [HttpGet]
        [Route("/UpdateGroupMarking/{SeDetailId}/{ParamSetNumber}")]
        public async Task<IActionResult> UpdateGroupMarking(int SeDetailId, int ParamSetNumber)
        {
            string errorMessage = "";
            var result = _ColumnService.UpdateGroupMarking(SeDetailId, ParamSetNumber);
            return Ok(result);
        }

        [HttpPost]
        [Route("/RegenerateValidation/{topCover}/{bottomCover}/{leftCover}/{rightCover}/{leg}/{seDetailingID}/{userId}/{structureElementId}")]
        public async Task<IActionResult> RegenerateValidation([FromBody] List<ColumnStructure> structureMarkList, int topCover, int bottomCover, int leftCover, int rightCover, int leg, int seDetailingID, int userId, int structureElementId)
        {
            string errorMessage = "";
            var result = _ColumnService.RegenerateValidation(structureMarkList, topCover, bottomCover, leftCover, rightCover, leg, seDetailingID, userId, structureElementId,out errorMessage);

            if(errorMessage!="")
            {
                return BadRequest(result);

            }
            else
            {
                return Ok(result);
            }
            

        }

        [HttpGet]
        [Route("/GetColumnLeg/{Parameternumber}/{productID}")]
        public async Task<IActionResult> GetColumnLeg(int Parameternumber, int productID)
        {
          string errorMessage = "";

            var result  = _ColumnService.GetColumnLeg(Parameternumber, productID);

            if (errorMessage == "")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }


        #endregion


        #region Accessories
        [HttpGet]
        [Route("/GetAccessoriesMarkingDetails/{DetailingID}")]
        public async Task<IActionResult> GetAccessoriesMarkingDetails(int DetailingID)
        {
            string errorMessage = "";
            List<Accessory> Accessories = _Accessories.GetACCProductMarkDetailsBySEDetailingID(DetailingID, out errorMessage);
            var result = Accessories;
            return Ok(result);
        }
        [HttpPost]
        [Route("/InsertAccessories")]
        public async Task<IActionResult> InsertAccessories([FromBody] Accessory accessoryItem)
        {
            string errorMessage = "";

            var result = _Accessories.InsUpdACCProdMarkDetails(accessoryItem, out errorMessage);

            if (errorMessage == "")
            {
                var returnresult = result;
                return Ok(returnresult);
            }
            else
            {
                return BadRequest(errorMessage);
            }


        }

        [HttpDelete]
        [Route("/DeleteAccessories/{accessory}")]
        public async Task<IActionResult> DeleteAccessories(int accessory)
        {
            string errorMessage = "";
            var result = _Accessories.DeleteACCProductMarkDetail(accessory);

            return Ok(result);

        }

        [HttpGet]
        [Route("/GetSapMaterial/{Sapmaterialcode}")]
        public async Task<IActionResult> GetSapMaterial(string Sapmaterialcode)
        {
            string errorMessage = "";
            List<SAPMaterial> SapcodeList = _Accessories.GetSAPMaterialDetails(Sapmaterialcode, out errorMessage);
            var result = SapcodeList;
            return Ok(result);

        }

        #endregion

        #region Beam Detailing

        [HttpGet]
        [Route("/GetBeamStructureMarkingDetails/{groupMarkName}/{projectID}/{seDetailingID}/{structureElementTypeID}/{productTypeID}/{groupMarkID}")]
        public async Task<IActionResult> GetStructureMarkingDetails(string groupMarkName, int projectID, int seDetailingID, int structureElementTypeID, int productTypeID, int groupMarkID)
        {
            string errorMessage = "";
            List<BeamStructure> BeamStructure = _BeamService.GetStructureMarkingDetails(groupMarkName, projectID, seDetailingID, structureElementTypeID, productTypeID, groupMarkID, out errorMessage);
            var result = BeamStructure;
            return Ok(result);
        }

        [HttpGet]
        [Route("/PopulateBeamProductCode/{structureElementTypeID}/{productTypeID}")]
        public async Task<IActionResult> PopulateBeamProductCode(int structureElementTypeID, int productTypeID)
        {
            string errorMessage = "";
            List<ProductCode> BeamProductCode = _BeamService.PopulateProductCode(structureElementTypeID, productTypeID);
            var result = BeamProductCode;
            return Ok(result);
        }

        [HttpGet]
        [Route("/PopulateBeamShapeCode")]
        public async Task<IActionResult> PopulateBeamShapeCode()
        {
            string errorMessage = "";
            List<ShapeCode> BeamStructure = _BeamService.PopulateShapeCode();
            var result = BeamStructure;
            return Ok(result);
        }

        //PopulateCapProductCode
        [HttpGet]
        [Route("/PopulateBeamCapProductCode")]
        public async Task<IActionResult> PopulateBeamCapProductCode()
        {
            string errorMessage = "";
            List<ProductCode> BeamCapProductCode = _BeamService.PopulateCapProductCode();
            var result = BeamCapProductCode;
            return Ok(result);
        }


        [HttpPost]
        [Route("/InsertBeamStructureMarking/{gap1}/{gap2}/{topCover}/{bottomCover}/{leftCover}/{rightCover}/{hook}/{leg}/{seDetailingID}")]
        public async Task<IActionResult> InsertBeamStructureMarking([FromBody] AddBeamStructureMarkDto BeamStructureObj, int gap1, int gap2, int topCover, int bottomCover, int leftCover, int rightCover, int hook, int leg, int seDetailingID)
        {
            try
            {
                string errorMessage = "";
                string bendingcheck = "";

                BeamStructure objStructureMarking = new BeamStructure();
                objStructureMarking.StructureMarkId = BeamStructureObj._structureMarkId;
                objStructureMarking.StructureMarkName = BeamStructureObj._structureMarkName;
                objStructureMarking.ProductCode = BeamStructureObj._productCode;
                objStructureMarking.Shape = BeamStructureObj._shape;
                objStructureMarking.Depth = BeamStructureObj._depth;
                objStructureMarking.Width = BeamStructureObj._width;
                objStructureMarking.Slope = BeamStructureObj._slope;
                objStructureMarking.Stirupps = BeamStructureObj._stirupps;
                objStructureMarking.Qty = BeamStructureObj._qty;
                objStructureMarking.Span = BeamStructureObj._span;
                objStructureMarking.IsCap = BeamStructureObj._iscap;
                objStructureMarking.CapProduct = BeamStructureObj._capProduct;
                objStructureMarking.ProduceInd = BeamStructureObj._produceInd;
                objStructureMarking.PinSize = BeamStructureObj._pinSize;
                objStructureMarking.ParentStructureMarkId = BeamStructureObj._parentStructureMarkId;

                List<BeamProduct> BeamProduct = _BeamService.VerifyStructureMarkingInputsNew(objStructureMarking, out errorMessage, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, seDetailingID, out bendingcheck);


                if (errorMessage == "")
                {
                    var result = BeamProduct;
                    return Ok(result);
                }
                else
                {
                    return BadRequest(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }



        [HttpPost]
        [Route("/UpdateBeamStructureMarking/{gap1}/{gap2}/{topCover}/{bottomCover}/{leftCover}/{rightCover}/{hook}/{leg}/{seDetailingID}")]
        public async Task<IActionResult> UpdateBeamStructureMarking([FromBody] AddBeamStructureMarkDto BeamStructureObj, int gap1, int gap2, int topCover, int bottomCover, int leftCover, int rightCover, int hook, int leg, int seDetailingID)
        {
            try
            {
                string errorMessage = "";
                string bendingcheck = "";

                BeamStructure objStructureMarking = new BeamStructure();
                objStructureMarking.StructureMarkId = BeamStructureObj._structureMarkId;
                objStructureMarking.StructureMarkName = BeamStructureObj._structureMarkName;
                objStructureMarking.ProductCode = BeamStructureObj._productCode;
                objStructureMarking.Shape = BeamStructureObj._shape;
                objStructureMarking.Depth = BeamStructureObj._depth;
                objStructureMarking.Width = BeamStructureObj._width;
                objStructureMarking.Slope = BeamStructureObj._slope;
                objStructureMarking.Stirupps = BeamStructureObj._stirupps;
                objStructureMarking.Qty = BeamStructureObj._qty;
                objStructureMarking.Span = BeamStructureObj._span;
                objStructureMarking.IsCap = BeamStructureObj._iscap;
                objStructureMarking.CapProduct = BeamStructureObj._capProduct;
                objStructureMarking.ProduceInd = BeamStructureObj._produceInd;
                objStructureMarking.PinSize = BeamStructureObj._pinSize;
                objStructureMarking.ParentStructureMarkId = BeamStructureObj._parentStructureMarkId;

                List<BeamProduct> BeamProduct = _BeamService.EditEnd(objStructureMarking, out errorMessage, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, seDetailingID, out bendingcheck);

                if (errorMessage == "")
                {
                    var result = BeamProduct;
                    return Ok(result);
                }
                else
                {
                    return BadRequest(errorMessage);
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


            return Ok("");
        }


        [HttpGet]
        [Route("/BeamParameterSetbyProjIdProdType/{projectId}/{productTypeId}")]
        public async Task<IActionResult> BeamParameterSetbyProjIdProdType(int projectId, int productTypeId)
        {
            string errorMessage = "";
            List<ShapeCodeParameterSet> ShapeCodeParameterSet = _BeamService.ParameterSetByProjectProductTypeId(projectId, productTypeId);
            var result = ShapeCodeParameterSet;
            return Ok(result);
        }



        [HttpDelete]
        [Route("/DeleteBeamStructureMarking/{StructureMarkId}")]
        public bool DeleteBeamStructureMarking(int StructureMarkId)
        {
            bool deleteStructMark = false;
            //string errorMessage = "";
            try
            {
                deleteStructMark = _BeamService.DeleteStructureMarking(StructureMarkId, out string errorMessage);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return deleteStructMark;

        }

        [HttpGet]
        [Route("/UpdateBeamGroupMarking/{SeDetailId}/{ParamSetNumber}")]
        public async Task<IActionResult> UpdateBeamGroupMarking(int SeDetailId, int ParamSetNumber)
        {
            string errorMessage = "";
            var result = _BeamService.UpdateGroupMarking(SeDetailId, ParamSetNumber, out errorMessage);
            return Ok(result);
        }

        [HttpPost]
        [Route("/RegenerateBeamValidation/{gap1}/{gap2}/{topCover}/{bottomCover}/{leftCover}/{rightCover}/{hook}/{leg}/{seDetailingID}/{structureElementId}")]
        public async Task<IActionResult> RegenerateBeamValidation([FromBody] List<BeamStructure> structureMarkList, int gap1, int gap2, int topCover, int bottomCover, int leftCover, int rightCover, int hook, int leg, int seDetailingID, int structureElementId)
        {
            string errorMessage = "";
            string bendingcheck = "";
            var result = _BeamService.RegenerateValidation(structureMarkList, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, seDetailingID, structureElementId, out errorMessage, out bendingcheck);

            if(errorMessage!="")
            {
                return BadRequest(result);
            }
            else
            {
            return Ok(result);

            }

        }


        #endregion

        #region PRC Detailing

        [HttpGet]
        [Route("/GetSAPMaterialByStructureElementId/{intStructureElementId}")]
        public async Task<IActionResult> GetSAPMaterialByStructureElementId(int intStructureElementId)
        {
            string errorMessage = "";
            List<SAPMaterial> SAPMaterialList = _PRCDetailingService.GetSAPMaterialByStructureElementId(intStructureElementId, out errorMessage);
            var result = SAPMaterialList;
            return Ok(result);

        }

        [HttpPost]
        [Route("/SavePRCGroupMarkDetails/{SeDetailId}")]
        public async Task<IActionResult> SavePRCGroupMarkDetails([FromBody] AddPRCGroupMarkDto PRCGroupMarkObj, int SeDetailId)
        {
            try
            {
                string errorMessage = "";
                string bendingcheck = "";

                GroupMark GroupMarkobj = new GroupMark();
                GroupMarkobj.SeDetailId = SeDetailId;
                GroupMarkobj.GroupMarkId = PRCGroupMarkObj.GroupMarkId;
                GroupMarkobj.GroupMarkingName = PRCGroupMarkObj.GroupMarkingName;
                GroupMarkobj.GroupRevisionNumber = PRCGroupMarkObj.GroupRevisionNumber;
                GroupMarkobj.ProjectId = PRCGroupMarkObj.ProjectId;
                GroupMarkobj.WBSTypeId = 0;
                GroupMarkobj.StructureElementTypeId = PRCGroupMarkObj.StructureElementTypeId;
                GroupMarkobj.SitProductTypeId = PRCGroupMarkObj.SitProductTypeId;
                GroupMarkobj.ParameterSetNumber = 0;
                GroupMarkobj.CreatedUserId = 0;
                GroupMarkobj.WBSElementId = 0;
                GroupMarkobj.IsCABOnly = 0;
                GroupMarkobj.Remarks = "";
                GroupMarkobj.InsertFlag = "";
                GroupMarkobj.CreatedUserName = "";
                GroupMarkobj.SAPMaterial = new SAPMaterial { MaterialCodeID = PRCGroupMarkObj.SAPMaterial };
                GroupMarkobj.DefaultDepth = PRCGroupMarkObj.DefaultDepth;
                GroupMarkobj.DefaultLength = PRCGroupMarkObj.DefaultLength;
                GroupMarkobj.DefaultWidth = PRCGroupMarkObj.DefaultWidth;
                GroupMarkobj.transport = null;
                GroupMarkobj.ParamValues = null;
                GroupMarkobj.ProductCode = null;
                GroupMarkobj.SiderForCode = PRCGroupMarkObj.SiderForCode;
                GroupMarkobj.isCABDE = PRCGroupMarkObj.IsCABDE;
                GroupMarkobj.AssemblyInd = PRCGroupMarkObj.AssemblyInd;

                List<GroupMark> GroupMarkList = _PRCDetailingService.SaveGroupMarkDetails(GroupMarkobj, PRCGroupMarkObj.CageSelector_Id, out errorMessage);
                if (GroupMarkList[0].ParentStructureMarkId==0)
                {
                  GroupMarkList = _PRCDetailingService.SaveGroupMarkDetails(GroupMarkobj, PRCGroupMarkObj.CageSelector_Id, out errorMessage);

                }
                var result = GroupMarkList;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }


        [HttpGet]
        [Route("/GetPRCSAPHeaderValuesByGroupMarkId/{GroupMarkId}")]
        public async Task<IActionResult> GetHeaderValuesByGroupMarkId(int GroupMarkId)
        {
            string errorMessage = "";
            // List<GroupMark> listHeaderDetails = new List<GroupMark>();

            List<GroupMark> listHeaderDetails = _PRCDetailingService.GetHeaderValuesByGroupMarkId(GroupMarkId, out errorMessage);
            var result = listHeaderDetails;
            return Ok(result);

        }


        #endregion

        #region BOM details
        [HttpGet]
        [Route("/GetBomDetails/{intProductMarkingId}/{BOMType}/{StructureElement}")]
        public async Task<IActionResult> GetBomDetails(int intProductMarkingId, string BOMType, string StructureElement)
        {
            IEnumerable<Get_bomDto> gradeTypes = await _BomService.GetBomDetails(intProductMarkingId, BOMType, StructureElement);
            var result = gradeTypes;
            return Ok(result);

        }

        [HttpPost]
        [Route("/InsertBOM")]
        public async Task<IActionResult> InsertBOM([FromBody] BOMInsert bOMInsert)
        {
            string errorMessage = "";



            var result = _BomService.InsertBOM(bOMInsert);



            return Ok(result);



        }

        [HttpDelete]
        [Route("/DeleteBOM/{BOMDetailId}")]
        public async Task<IActionResult> DeleteBOM(int BOMDetailId)
        {



            var BomDeleteObj = await _BomService.BOMDelete(BOMDetailId);
            return Ok(BomDeleteObj);



        }

        [HttpGet]
        [Route("/GetBOMHeader/{intProductMarkingId}/{nvchBOMType}/{strStructureElement}")]
        public async Task<IActionResult> GetBOMHeader(int intProductMarkingId, string nvchBOMType, string strStructureElement)
        {
            List<BomService> bOMHeaderDtos = _BomService.GetBOMHeader(intProductMarkingId, nvchBOMType, strStructureElement);
            var result = bOMHeaderDtos;
            return Ok(result);
        }

        [HttpGet]
        [Route("/Get_ShapeParamDetails/{ShapeId}")]
        public async Task<IActionResult> Get_ShapeParamDetails(int ShapeId)
        {
            string errorMessage = "";
            List<get_ShapeParamDetailsDTO> GetShapeParamDetails;

            GetShapeParamDetails = _BomService.GetShapeParamDetails(ShapeId);

            var result = GetShapeParamDetails;
            return Ok(result);

        }

        [HttpPost]
        [Route("/UpdateProductBom")]
        public async Task<IActionResult> UpdateProductBom([FromBody] UpdateBomProd_dto UpdateProdBOM)
        {
            string errorMessage = "";





            var result = _BomService.UpdateBOM_PROD(UpdateProdBOM);





            return Ok(result);





        }

        [HttpGet]
        [Route("/BendingGroup_Get/{ShapeId}")]
        public async Task<IActionResult> BendingGroup_Get(int ShapeId)
        {
         

            var result  = _BomService.BendingGroup_Get(ShapeId);

            
            return Ok(result);

        }

        #endregion

        #region ValidateGM
        [HttpGet]
        [Route("/ValidatedPostedGM/{intGroupMarkid}")]
        public async Task<IActionResult> ValidatedPostedGM(int intGroupMarkid)
        {
            string errorMessage = "";
            var result = _MeshDetailing.ValidatedPostedGM(intGroupMarkid, out errorMessage);

            return Ok(result);
        }


        //ValidatedPostedGM
        #endregion

        [HttpGet]
        [Route("/GetPostedGroupMark/{intGroupMarkid}")]
        public async Task<IActionResult> GetPostedGroupMark(int intGroupMarkid)
        {
            IEnumerable<GetPostedGMDto> getPostedGMDtos = _groupmarkdal.GetPostedGroupMark(intGroupMarkid);

            var result = _mapper.Map<List<GetPostedGMDto>>(getPostedGMDtos);
            result = _groupmarkdal.GetPostedGroupMark(intGroupMarkid);
            return Ok(result);
        }


        [HttpGet]
        [Route("/GetReleasedGroupMark/{intGroupMarkid}")]
        public async Task<IActionResult> GetReleasedGroupMark(int intGroupMarkid)
        {
            IEnumerable<GetReleasedGMDto> getReleasedGMDtos = _groupmarkdal.GetReleasedGroupMark(intGroupMarkid);
            var result = _mapper.Map<List<GetReleasedGMDto>>(getReleasedGMDtos);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetProductType/{intProductType}/{intGroupMarkId}")]
        public async Task<IActionResult> GetProductType(int intProductType, int intGroupMarkId)
        {

            var result = _groupmarkdal.GetProductType(intProductType, intGroupMarkId);
            return Ok(result);
        }

        [HttpPost]
        [Route("/UpdategroupMark")]
        public async Task<IActionResult> UpdategroupMark([FromBody] ReleaseGroupMarkDto groupMarkDto)
        {
            string errorMessage = "";
            try
            {
                var result = _MeshDetailing.GroupMarkList_Edit(groupMarkDto);
                    return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }

        [HttpPost]
        [Route("/SaveGroupMark")]
        public async Task<IActionResult> SaveGroupMark([FromBody] NewGroupMarkAddDto NewGroupMarkAddDtoobj)
        {
            string errorMessage = "";
            int GMId = 0;
            string InserFlag = "";
            bool SaveFlag = false;
            try
            {
                GroupMark groupmarkobj = new GroupMark();
                groupmarkobj.GroupRevisionNumber = 0;
                groupmarkobj.GroupMarkingName = NewGroupMarkAddDtoobj.GroupMarkName;
                groupmarkobj.ProjectId = NewGroupMarkAddDtoobj.ProjectId;
                groupmarkobj.WBSTypeId = NewGroupMarkAddDtoobj.WBSTypeId;
                groupmarkobj.StructureElementTypeId = NewGroupMarkAddDtoobj.StructureElementTypeId;
                groupmarkobj.SitProductTypeId = NewGroupMarkAddDtoobj.SitProductTypeId;
                groupmarkobj.ParameterSetNumber = NewGroupMarkAddDtoobj.ParameterSetNumber;
                groupmarkobj.transport = null;
                groupmarkobj.IsCABOnly = NewGroupMarkAddDtoobj.IsCABOnly;
                groupmarkobj.Remarks = NewGroupMarkAddDtoobj.Remarks;
                groupmarkobj.CreatedUserId = NewGroupMarkAddDtoobj.CreatedUserId;
                groupmarkobj.CreatedUserName = NewGroupMarkAddDtoobj.CreatedUserName;
                groupmarkobj.GroupMarkId = 0;
                groupmarkobj.SiderForCode = NewGroupMarkAddDtoobj.SiderForCode;
                var result = _MeshDetailing.SaveGroupMark(groupmarkobj, out GMId, out InserFlag, out errorMessage);
                SaveFlag = true;
                return Ok(GMId);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }


        [HttpGet]
        [Route("/FilterProductCode_Column/{structureElementTypeID}/{productTypeID}/{enteredText}")]
        public async Task<IActionResult> FilterProductCode_Column(int structureElementTypeID, int productTypeID, string enteredText)
        {
            string errorMessage = "";
            IEnumerable<ProductCode> GetProductCode = _ColumnService.FilterProductCode(structureElementTypeID, productTypeID, enteredText, out errorMessage);

            var result = _mapper.Map<List<ProductCode>>(GetProductCode);
            result = _ColumnService.FilterProductCode(structureElementTypeID, productTypeID, enteredText, out errorMessage);
            if (errorMessage == "")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }

        //[HttpGet]
        //[Route("/GetColumnLeg/{Parameternumber}/{productID}")]
        //public async Task<IActionResult> GetColumnLeg(int Parameternumber, int productID)
        //{
        //    string errorMessage = "";
        //    IEnumerable<ProductCode> GetProductCode = _ColumnService.GetColumnLeg(Parameternumber, productID);

        //    var result = _mapper.Map<List<ProductCode>>(GetProductCode);
           
        //    if (errorMessage == "")
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return BadRequest(errorMessage);
        //    }
        //}

        [HttpGet]
        [Route("/FilterShapeCode_column/{enteredText}")]
        public async Task<IActionResult> FilterShapeCode_column(string enteredText)
        {
            string errorMessage = "";
            IEnumerable<ShapeCode> GetFilterCapProductCode = _ColumnService.FilterShapeCode(enteredText, out errorMessage);

            var result = _mapper.Map<List<ShapeCode>>(GetFilterCapProductCode);
            result = _ColumnService.FilterShapeCode(enteredText, out errorMessage);
            if (errorMessage == "")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }

        [HttpGet]
        [Route("/FilterShapeCode_slab/{enteredText}")]
        public async Task<IActionResult> FilterShapeCode_slab(string enteredText)
        {
            string errorMessage = "";
            IEnumerable<ShapeCode> GetShapeCode = _Slabservice.FilterShapeCode(enteredText, out errorMessage);

            var result = _mapper.Map<List<ShapeCode>>(GetShapeCode);
            //result = _BeamService.FilterShapeCode(enteredText, out errorMessage); // commented by Tanmay 
            if (errorMessage == "")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }


        [HttpPost]
        [Route("/UpdateSlabProductMarking/{currentIndex}/{structureElementId}/{machineCheck}/{bendingCheck}/{transportCheck}/{userId}")]
        public async Task<IActionResult> UpdateProductMark([FromBody] SlabStructure structureMarking, [FromBody] SlabProduct productMark, int currentIndex, int structureElementId, bool machineCheck, string bendingCheck, bool transportCheck, int userId)
        {
            string errorMessage = "";

            string thisError = "";

            SlabStructure getslab = new SlabStructure();

            //SlabStructureMarkingDto obj = slab.structureMark;

            //List<SlabStructure> SlabStructure = getslab.SlabStructureMark_Get_byDto(obj, SeDetailingId);

            //SlabStructure slabstr = SlabStructure.FirstOrDefault();
            try
            {
                //slabstr.ParameterSet = new ShapeCodeParameterSet
                //{
                //    MaxCWLength = slab.ParameterSet.MaxCWLength,
                //    MaxMWLength = slab.ParameterSet.MaxMWLength,
                //    MinMo1 = slab.ParameterSet.MINMO1,
                //    MinMo2 = slab.ParameterSet.MINMO2,
                //    MinCo1 = slab.ParameterSet.MINCO1,
                //    MinCo2 = slab.ParameterSet.MINCO2,
                //    MachineMaxCWLength = slab.ParameterSet.MACHINEMAXCWLENGTH,
                //    MachineMaxMWLength = slab.ParameterSet.MACHINEMAXMWLENGTH,


                //};
                var result = _Slabservice.UpdateSlabProductMarking(structureMarking, productMark, currentIndex, structureElementId, machineCheck, bendingCheck, transportCheck, userId, out errorMessage);

                if (errorMessage == "")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpGet]
        [Route("/FilterShapeCode_beam/{enteredText}")]
        public async Task<IActionResult> FilterShapeCode(string enteredText)
        {
            string errorMessage = "";
            IEnumerable<ShapeCode> GetShapeCode = _BeamService.FilterShapeCode(enteredText, out errorMessage);

            var result = _mapper.Map<List<ShapeCode>>(GetShapeCode);
            result = _BeamService.FilterShapeCode(enteredText, out errorMessage);
            if (errorMessage == "")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }

        [HttpGet]
        [Route("/FilterProductCode_Beam/{structureElementTypeID}/{productTypeID}/{enteredText}")]
        public async Task<IActionResult> FilterProductCode_Beam(int structureElementTypeID, int productTypeID, string enteredText)
        {
            string errorMessage;
            IEnumerable<ProductCode> GetProductCode = _BeamService.FilterProductCode(structureElementTypeID, productTypeID, enteredText, out errorMessage);

            var result = _mapper.Map<List<ProductCode>>(GetProductCode);
            result = _BeamService.FilterProductCode(structureElementTypeID, productTypeID, enteredText, out errorMessage);
            if (errorMessage == "")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }

        [HttpGet]
        [Route("/FilterCapProductCode/{enteredText}")]
        public async Task<IActionResult> FilterCapProductCode(string enteredText)
        {
            string errorMessage;
            IEnumerable<ProductCode> GetFilterCapProductCode = _BeamService.FilterCapProductCode(enteredText, out errorMessage);

            var result = _mapper.Map<List<ProductCode>>(GetFilterCapProductCode);
            result = _BeamService.FilterCapProductCode(enteredText, out errorMessage);
            if (errorMessage == "")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }

        [HttpPost]
        [Route("/EditEnd/{gap1}/{gap2}/{topCover}/{bottomCover}/{leftCover}/{rightCover}/{hook}/{leg}/{seDetailingID}")]
        public async Task<IActionResult> EditEnd(BeamStructure objStructureMarking, int gap1, int gap2, int topCover, int bottomCover, int leftCover, int rightCover, int hook, int leg, int seDetailingID)
        {
            string errorMessage = "";
            string bendingcheck = "";
            IEnumerable<BeamProduct> _EditEnd = _BeamService.EditEnd(objStructureMarking, out errorMessage, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, seDetailingID, out bendingcheck);

            var result = _mapper.Map<List<BeamProduct>>(_EditEnd);
            result = _BeamService.EditEnd(objStructureMarking, out errorMessage, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, seDetailingID, out bendingcheck);

            if (errorMessage == "" || bendingcheck == "")
            {
                return Ok(result);
            }
            else
            {
                //_Slabservice.DeleteStructureMarking(slabStructure.StructureMarkId);

                return BadRequest(errorMessage);
            }

        }

        [HttpPost]
        [Route("/ValidateAndInsertTCTById/{isValidated}")]
        public async Task<IActionResult> InsertToDbTCT([FromBody] InsertToDbTCTDto[] insertToDbTCT, bool isValidated)
        {
            List<CABItem> cABs;
            string errorMessage = "";
            int GMId = 0;
            string InserFlag = "";
            bool SaveFlag = false;
            int totalcount = insertToDbTCT.Length;
            int counter = 0;




            try
            {
                foreach (InsertToDbTCTDto item in insertToDbTCT)
                {
                    // Perform actions with each item
                    // For example, you can access properties of the item like item.PropertyName
                    // InsertToDbTCTDto is the type of your objects


                    CABItem cABItem = new CABItem();
                    //cABItem.GroupRevisionNumber = 0;
                    cABItem.ShapeParametersList = item.ShapeParametersList;
                    cABItem.accList = item.accList;
                    cABItem.Shape = item.Shape;
                    cABItem.CABProductItem = item.CABProductItem;
                    cABItem.accItem = item.accItem;
                    cABItem.CABProductMarkName = item.CABProductMarkName;
                    cABItem.intSEDetailingId = item.intSEDetailingId;
                    cABItem.GroupMarkId = item.GroupMarkId;
                    cABItem.MemberQty = item.MemberQty; //by CS05
                    cABItem.Quantity = item.Quantity; //by CS05
                    cABItem.ShapeCode = item.ShapeCode;
                    cABItem.PinSizeID = item.PinSizeID;
                    cABItem.InvoiceLength = item.InvoiceLength;
                    cABItem.ProductionLength = item.ProductionLength;
                    cABItem.InvoiceWeight = item.InvoiceWeight;
                    cABItem.ProductionWeight = item.ProductionWeight;
                    cABItem.Grade = item.Grade;
                    cABItem.Diameter = item.Diameter;
                    cABItem.Coupler1Standard = item.Coupler1Standard;
                    cABItem.ShapeType = item.ShapeType;
                    cABItem.ShapeGroup = item.ShapeGroup;
                    cABItem.Coupler1Type = item.Coupler1Type;
                    cABItem.Coupler1 = item.Coupler1;
                    cABItem.Coupler1Standard = item.Coupler1Standard;
                    cABItem.BarMark = item.BarMark;
                    cABItem.SEDetailingID = item.SEDetailingID;
                    cABItem.Coupler2Type = item.Coupler2Type;
                    cABItem.Coupler2 = item.Coupler2;
                    cABItem.Coupler2Standard = item.Coupler2Standard;
                    cABItem.Status = item.Status;
                    cABItem.CustomerRemarks = item.CustomerRemarks;
                    cABItem.ShapeImage = item.ShapeImage;
                    cABItem.BVBS = item.BVBS;
                    cABItem.PageNumber = item.PageNumber;
                    cABItem.DescScript = item.DescScript;
                    cABItem.EnvLength = item.EnvLength;
                    cABItem.EnvWidth = item.EnvWidth;
                    cABItem.EnvHeight = item.EnvHeight;
                    cABItem.NoOfBends = item.NoOfBends;
                    cABItem.IsVariableBar = Convert.ToBoolean(item.IsVariableBar);
                    cABItem.CommercialDesc = item.CustomerRemarks;


                    var result = _cABService.InsertToDbTCT(cABItem, isValidated, out errorMessage);
                    SaveFlag = true;
                    counter++;
                    if (counter == totalcount)
                    {
                        if (errorMessage == "")
                        {
                            return Ok(result);
                        }
                        else
                        {
                            //_Slabservice.DeleteStructureMarking(slabStructure.StructureMarkId);

                            return BadRequest(errorMessage);
                        }
                    }
                }


                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }


        #region CoreCage Detailing       

        [HttpGet]
        [Route("/PopulateCoreCageProductCode")]
        public async Task<IActionResult> PopulateCoreCageProductCode()
        {
            string errorMessage = "";
            List<ProductCode> ColumnProductCode = _ColumnService.CoreCagePopulateProductCode();
            var result = ColumnProductCode;
            return Ok(result);
        }

        [HttpGet]
        [Route("/CoreCageSelectdProductCode/{GroupMarkId}")]
        public async Task<IActionResult> CoreCageSelectdProductCode(int GroupMarkId)
        {
            string errorMessage = "";
            List<ProductCode> ColumnProductCode = _ColumnService.CoreCageSelectdProductCode(GroupMarkId);
            var result = ColumnProductCode;
            return Ok(result);
        }
        #endregion

        //added by vidya 


        [HttpPost]
        [Route("/DeleteProductMarking")]
        public bool Delete_SlabProductMarking([FromBody] SlabProduct slabProduct, int seDetailingId)
        {
            bool deleteStructMark = false;
            string errorMessage = "";
            try
            {
                deleteStructMark = _Slabservice.DeleteProductMarking(slabProduct, seDetailingId, out errorMessage);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return deleteStructMark;

        }


        [HttpGet]
        [Route("/GetOverHang/{parameterSetNumber}/{projectId}/{structureElementId}/{productTypeId}/{mwLength}/{cwLength}/{mwSpace}/{cwSpace}")]
        public async Task<IActionResult> GetOverHang(int parameterSetNumber, int projectId, int structureElementId, int productTypeId, int mwLength, int cwLength, int mwSpace, int cwSpace)

        {

            string errormessage = "";

            IEnumerable<SlabProduct> addSlabProducts = _Slabservice.GetOverHang(parameterSetNumber, projectId, structureElementId, productTypeId, mwLength, cwLength, mwSpace, cwSpace, out errormessage);

            var result =addSlabProducts.ToList();

            return Ok(result);

        }

        [HttpGet]
        [Route("/GetDetailingForm/{UserName}")]
        public async Task<IActionResult> GetDetailingForm(string UserName)
        {
            string errorMessage = "No record Found";
            //var result = _Slabservice.GetStructureMarkingDetails(DetailingID, StructureElementId, out errorMessage);
            List<DetailingFormDto> result = _accountService.GetDetailingForm(UserName);

            if (result == null)
            {
                //string message = "Employee doesn't exist";
                //throw new HttpResponseException(
                //    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            return Ok(result);
        }


        [HttpGet]
        [Route("/GroupMarkID_insert/{intGroupMarkId}")]
        public async Task<IActionResult> GroupMarkID_insert(int intGroupMarkId)
        {
            ImageBuilderDWALL imageBuilderDWALL = new ImageBuilderDWALL();
            var result = imageBuilderDWALL.insertGroupMarkID(intGroupMarkId);
            return Ok(result);
        }

        //[HttpGet]
        //[Route("/updateStructureMarking_column/{structuremarking}/{StructureMarkId}/{qty}")]
        //public async Task<IActionResult> updateStructureMarking_column(string structuremarking, int StructureMarkId,int qty)
        //{
        //    string errorMessage = "";

        //    //List<ProductCode> ColumnProductCode = _ColumnService.CoreCageSelectdProductCode(GroupMarkId);

        //    var result = _ColumnService.UpdateStructureMarkingDetails(StructureMarkId, structuremarking,qty, out errorMessage);

        //    if (result == 1)
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return BadRequest(errorMessage);
        //    }


        //}


        //[HttpGet]
        //[Route("/updateStructureMarking_beam/{structuremarking}/{StructureMarkId}/{qty}")]
        //public async Task<IActionResult> updateStructureMarking_beam(string structuremarking, int StructureMarkId, int qty)
        //{
        //    string errorMessage = "";

        //    //List<ProductCode> ColumnProductCode = _ColumnService.CoreCageSelectdProductCode(GroupMarkId);

        //    var result = _BeamService.UpdateStructureMarkingDetails(StructureMarkId, structuremarking, qty, out errorMessage);

        //    if (result == 1)
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return BadRequest(errorMessage);
        //    }


        //}


        [HttpPost]
        [Route("/updateStructureMarking_New")]
        public async Task<IActionResult> updateStructureMarking_New([FromBody] Update_Structmarking_Name obj)
        {
            string errorMessage = "";
            var result = 0;
            //List<ProductCode> ColumnProductCode = _ColumnService.CoreCageSelectdProductCode(GroupMarkId);
            if (obj.StructureType=="beam")
            {
                result = _BeamService.UpdateStructureMarkingDetails(obj.StructureMarkId, obj.structuremarking, obj.qty, out errorMessage);

            }
            else if (obj.StructureType == "column")
            {
                result = _ColumnService.UpdateStructureMarkingDetails(obj.StructureMarkId, obj.structuremarking, obj.qty, out errorMessage);

            }

            if (result == 1)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(errorMessage);
            }


        }

        [HttpGet]
        [Route("/CallStructremarkingDetails/{intGroupMarkId}")]
        public async Task<IActionResult> CallStructremarkingDetails(int intGroupMarkId)
        {
            GetGmDetailsForCall result = _groupmarkdal.CallStructMarkListbyGMID(intGroupMarkId);
            if (result.StructureElementTypeID == 4 || result.StructureElementTypeID == 68 || result.StructureElementTypeID == 69)
            {
                GetStructureMarkingDetails(result.SeDetailingID, result.StructureElementTypeID);
            }
            if (result.StructureElementTypeID == 2)
            {
                GetStructureMarkingDetails(result.ProjectID, result.SeDetailingID, result.StructureElementTypeID, result.ProductTypeID);
            }
            else if (result.StructureElementTypeID == 1)
            {
                GetStructureMarkingDetails(result.GroupMarkName, result.ProjectID, result.SeDetailingID, result.StructureElementTypeID, result.ProductTypeID, result.GroupMarkID);
            }

            return Ok(result);
        }

    }




}


