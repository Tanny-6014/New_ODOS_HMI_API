using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DetailingService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using System.Collections.Generic;
using DetailingService.Repositories;
using DetailingService.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Dapper;
using DetailingService.Dtos;
using AutoMapper;

namespace DetailingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarpetController : ControllerBase
    {
        private readonly ICarpetService _carpetservice;
        private readonly IMeshDetailing _MeshDetailing;

        private readonly IMapper _mapper;


        public CarpetController(IMapper mapper, ICarpetService carpetService, IMeshDetailing Detailing)
        {
            _carpetservice = carpetService;
            _MeshDetailing= Detailing;
            _mapper = mapper;
         
        }


        #region GET

        [HttpGet]
        [Route("/CarpetParameterSetbyProjIdProdType/{ProjectID}/{productTypeId}")]
        public async Task<IActionResult> CarpetParameterSetbyProjIdProdType(int ProjectID, int productTypeId)
        {
            string errorMessage = "";
            List<ShapeCodeParameterSet> shapeCodeParameters = _carpetservice.CarpetParameterSetbyProjIdProdType(ProjectID, productTypeId,out errorMessage);
            var result = shapeCodeParameters;
            if (errorMessage != "")
            {
                return BadRequest(errorMessage);

            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("/PopulateProductCode_carpet")]
        public async Task<IActionResult> PopulateProductCode_carpet()
        {
            string errorMessage = "";
            List<ProductCode> productCodes = _carpetservice.PopulateProductCode(out errorMessage);
            var result = productCodes;
            if (errorMessage != "")
            {
                return BadRequest(errorMessage);

            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("/FilterProductCode_carpet/{enteredText}")]
        public async Task<IActionResult> FilterProductCode_carpet(string enteredText)
        {
            string errorMessage = "";
            List<ProductCode> productCodes = _carpetservice.FilterProductCode(enteredText, out errorMessage);
            var result = productCodes;
           
            if (errorMessage != "")
            {
                return BadRequest(errorMessage);

            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("/PopulateShapeCode_carpet")]
        public async Task<IActionResult>PopulateShapeCode_carpet()
        {
            string errorMessage = "";
            var result = _carpetservice.PopulateShapeCode(out errorMessage);
           
            if (errorMessage != "")
            {
                return BadRequest(errorMessage);

            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("/FilterShapeCode_carpet/{enteredText}")]
        public async Task<IActionResult> FilterShapeCode_carpet(string enteredText)
        {
            string errorMessage = "";
            //IEnumerable<ShapeCode> GetShapeCode = _carpetservice.FilterShapeCode(enteredText, out errorMessage);
            List<ShapeCode> shapeCodes = _carpetservice.FilterShapeCode(enteredText, out errorMessage);
            var result = shapeCodes;
            
            if (errorMessage != "")
            {
                return BadRequest(errorMessage);

            }
            else
            {
                return Ok(result);
            }
        }


        [HttpGet]
        [Route("/GetStructureMarkingDetails_carpet/{DetailingID}/{StructureElementId}")]
        public async Task<IActionResult> GetStructureMarkingDetails_carpet(int DetailingID, int StructureElementId)
        {
            string errorMessage = "No record Found";
            List<CarpetStructure> carpetStructures = _carpetservice.GetStructureMarkingDetails(DetailingID, StructureElementId, out errorMessage);
            var result = carpetStructures;
            if (errorMessage != "")
            {
                return BadRequest(errorMessage);

            }
            else
            {
                return Ok(result);
            }
        }



        [HttpGet]
        [Route("/GetOverHang_carpet/{parameterSetNumber}/{projectId}/{structureElementId}/{productTypeId}/{mwLength}/{cwLength}/{mwSpace}/{cwSpace}")]
        public async Task<IActionResult>GetOverHang_carpet(int parameterSetNumber, int projectId, int structureElementId, int productTypeId, int mwLength, int cwLength, int mwSpace, int cwSpace)

        {

            string errormessage = "";

            List<CarpetProduct> addSlabProducts = _carpetservice.GetOverHang(parameterSetNumber, projectId, structureElementId, productTypeId, mwLength, cwLength, mwSpace, cwSpace,  out errormessage);

            var result = addSlabProducts.ToList();
            if (errormessage != "")
            {
                return BadRequest(errormessage);

            }
            else
            {
                return Ok(result);
            }

        }



        #endregion

        #region POST

        [HttpPost]
        [Route("/InsertCarpetStructureMarking/{structureElementId}/{projectTypeId}/{projectId}/{productTypeId}/{userId}")]
        public async Task<IActionResult> InsertCarpetStructureMarking([FromBody] InsertCarpetStructureMarkingDto obj, int structureElementId, int projectTypeId, int projectId, int productTypeId, int userId)
        {
            string errorMessage = "";

            var check = obj.Shapecode.ShapeParam;


            try
            {

                CarpetStructure carpetStructure = new CarpetStructure();
                carpetStructure.SEDetailingID = obj.SEDetailingID;
                carpetStructure.StructureMarkId = obj.StructureMarkId;
                carpetStructure.ParentStructureMarkId = obj.ParentStructureMarkId;
                carpetStructure.StructureMarkingName = obj.StructureMarkingName;
                carpetStructure.ParamSetNumber = obj.ParameterSet.TNTPARAMSETNUMBER;
                carpetStructure.MainWireLength = obj.MainWireLength;
                carpetStructure.CrossWireLength = obj.CrossWireLength;
                carpetStructure.MemberQty = obj.MemberQty;
                carpetStructure.BendingCheck = obj.BendingCheck;
                carpetStructure.MachineCheck = obj.MachineCheck;
                carpetStructure.TransportCheck = obj.TransportCheck;
                carpetStructure.MultiMesh = obj.MultiMesh;
                carpetStructure.ProduceIndicator = obj.ProduceIndicator;
                carpetStructure.PinSize = obj.PinSize;
                carpetStructure.ProductGenerationStatus = obj.ProductGenerationStatus;
                carpetStructure.SideForCode = obj.SideForCode;
                carpetStructure.ProductSplitUp = obj.ProductSplitUp;

                carpetStructure.ParameterSet = new ShapeCodeParameterSet
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
                carpetStructure.ProductCode = obj.ProductCode;

                var result = _carpetservice.InsertCarpetStructureMarking(carpetStructure, structureElementId, projectTypeId, projectId, productTypeId, out errorMessage, userId);

                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);

                }


                IEnumerable<MeshStructureMarkingDetailsDto> meshStructureMarkingDetailsDtos = await _MeshDetailing.CarpetStructureMarkingDetails_Get();

                MeshStructureMarkingDetailsDto MeshDto = new MeshStructureMarkingDetailsDto();

                MeshDto = meshStructureMarkingDetailsDtos.FirstOrDefault();

                IEnumerable<AddSlabProductMarkingDto> addSlabProducts_IE = await _MeshDetailing.MeshProductMarkingDetails_Get(MeshDto.intStructureMarkId);

                List<AddSlabProductMarkingDto> addSlabProducts_List = addSlabProducts_IE.ToList();

                AddSlabProductMarkingDto addSlab = new AddSlabProductMarkingDto();

                for (int i = 0; i < addSlabProducts_List.Count; i++)
                {
                    addSlab = addSlabProducts_List[i];
                    carpetStructure.ParameterSet = new ShapeCodeParameterSet
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
                    carpetStructure.ProductCode = obj.ProductCode;
                    CarpetProduct carpet_prod = new CarpetProduct();


                    carpet_prod.shapecode = obj.Shapecode;
                    carpet_prod.StructureMarkId = addSlab.INTSTRUCTUREMARKID;
                    carpet_prod.ProductCodeId = addSlab.INTPRODUCTCODEID;
                    carpet_prod.ProductMarkingName = addSlab.VCHPRODUCTMARKINGNAME;
                    carpet_prod.ShapeParam = addSlab.PARAMVALUES;
                    carpet_prod.MWLength = addSlab.NUMINVOICEMWLENGTH;
                    carpet_prod.CWLength = addSlab.NUMINVOICECWLENGTH;
                    carpet_prod.NoOfMainWire = 2;
                    carpet_prod.ProductionMWLength = addSlab.NUMPRODUCTIONMWLENGTH;
                    carpet_prod.ProductionCWLength = addSlab.NUMPRODUCTIONCWLENGTH;
                    carpet_prod.ProductionMWQty = addSlab.INTPRODUCTIONMAINQTY;
                    carpet_prod.ProductionCWQty = addSlab.INTPRODUCTIONCROSSQTY;
                    carpet_prod.MemberQty = addSlab.INTMEMBERQTY;
                    carpet_prod.MO1 = obj.MO1;
                    carpet_prod.MO2 = obj.MO2;
                    carpet_prod.CO1 = obj.CO1;
                    carpet_prod.CO2 = obj.CO2;
                    carpet_prod.ProductionMO1 = addSlab.INTPRODUCTIONMO1;
                    carpet_prod.ProductionMO2 = addSlab.INTPRODUCTIONMO2;
                    carpet_prod.ProductionCO1 = addSlab.INTPRODUCTIONCO1;
                    carpet_prod.ProductionCO2 = addSlab.INTPRODUCTIONCO2;
                    carpet_prod.MWSpacing = addSlab.INTMWSPACING;
                    carpet_prod.CWSpacing = addSlab.INTCWSPACING;
                    carpet_prod.ProductMarkId = addSlab.intMeshProductMarkId;
                    carpet_prod.TheoraticalWeight = addSlab.NUMTHEORATICALWEIGHT;
                    carpet_prod.InvoiceArea = addSlab.NUMINVOICEAREA;
                    carpet_prod.NetWeight = addSlab.NUMNETWEIGHT;
                    carpet_prod.ProductionWeight = addSlab.NUMPRODUCTIONWEIGHT;
                    carpet_prod.EnvelopeLength = addSlab.INTENVELOPLENGTH;
                    carpet_prod.EnvelopeWidth = addSlab.INTENVELOPWIDTH;
                    carpet_prod.EnvelopeHeight = addSlab.INTENVELOPHEIGHT;
                    carpet_prod.ProduceIndicator = obj.ProduceIndicator;
                    carpet_prod.BOMIndicator = "Yes";
                    carpet_prod.ParamValues = addSlab.PARAMVALUES;
                    carpet_prod.BOMDrawingPath = addSlab.BOMDrawingPath;
                    carpet_prod.MWBVBSString = addSlab.NVCHMWBVBSSTRING;
                    carpet_prod.CWBVBSString = addSlab.NVCHCWBVBSSTRING;
                    carpet_prod.InvoiceMWWeight = addSlab.NUMINVOICEMWWEIGHT;
                    carpet_prod.InvoiceCWWeight = addSlab.NUMINVOICECWWEIGHT;
                    carpet_prod.ProductionMWWeight = addSlab.NUMPRODUCTIONMWWEIGHT;
                    carpet_prod.ProductionCWWeight = addSlab.NUMPRODUCTIONCWWEIGHT;
                    carpet_prod.Deleteflag = 0;
                    carpet_prod.ProductValidator = 1;
                    carpet_prod.MWPitch = addSlab.MWPITCH;
                    carpet_prod.CWPitch = addSlab.CWPITCH;
                    carpet_prod.MWFlag = addSlab.MWFLAG;
                    carpet_prod.CWFlag = addSlab.CWFLAG;
                    carpet_prod.BendingCheckInd = addSlab.BENDINGCHECK;
                    carpet_prod.PinSize = addSlab.SITPINSIZE;
                    var res = _carpetservice.UpdateCarpetProductMarking(carpetStructure, carpet_prod, 1, structureElementId, true, "", true, out errorMessage, 1);





                }
                if (errorMessage == "")
                {

                    return Ok(errorMessage);
                }
                else
                {
                    _carpetservice.DeleteStructureMarking(carpetStructure, out errorMessage);

                    return BadRequest(errorMessage);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("/UpdateCarpetProductMarking/{structureElementId}")]
        public async Task<IActionResult> UpdateCarpetProductMarking([FromBody] UpdateCarpetproduct updateCarpetproduct, int structureElementId)
        {
            string errorMessage = "";

            string thisError = "";

            CarpetStructure carpet = updateCarpetproduct.structureMark;

            try
            {
                carpet.ParameterSet = new ShapeCodeParameterSet
                {
                    MaxCWLength = updateCarpetproduct.ParameterSet.MaxCWLength,
                    MaxMWLength = updateCarpetproduct.ParameterSet.MaxMWLength,
                    MinMo1 = updateCarpetproduct.ParameterSet.MINMO1,
                    MinMo2 = updateCarpetproduct.ParameterSet.MINMO2,
                    MinCo1 = updateCarpetproduct.ParameterSet.MINCO1,
                    MinCo2 = updateCarpetproduct.ParameterSet.MINCO2,
                    MachineMaxCWLength = updateCarpetproduct.ParameterSet.MACHINEMAXCWLENGTH,
                    MachineMaxMWLength = updateCarpetproduct.ParameterSet.MACHINEMAXMWLENGTH,
                    TransportMaxHeight = updateCarpetproduct.ParameterSet.TransportMaxHeight,
                    TransportMaxWeight = updateCarpetproduct.ParameterSet.TransportMaxWeight,
                    TransportMaxLength = updateCarpetproduct.ParameterSet.TransportMaxLength,
                    TransportMaxWidth = updateCarpetproduct.ParameterSet.TransportMaxWidth


                };
                var result = _carpetservice.UpdateCarpetProductMarking(carpet, updateCarpetproduct.carpetprod, 1, structureElementId, true, "test", true,out errorMessage,1);

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
        [Route("/RegenerateValidation_carpet/{structureElementId}/{projectTypeId}/{projectId}/{productTypeId}/{seDetailingID}")]
        public async Task<IActionResult> RegenerateValidation_carpet([FromBody] ShapeCodeParameterSetDto ParameterSet, int structureElementId, int projectTypeId, int projectId, int productTypeId, int seDetailingID)
        {
            string errorMessage = "";

            CarpetStructure structureMarkEntryCollection = new CarpetStructure();

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
                TransportMaxHeight = ParameterSet.TransportMaxHeight,
                TransportMaxWeight = ParameterSet.TransportMaxWeight,
                TransportMaxLength = ParameterSet.TransportMaxLength,
                TransportMaxWidth = ParameterSet.TransportMaxWidth


            };




            List<CarpetStructure> structureMarkList = structureMarkEntryCollection.CarpetStructureMark_Get(seDetailingID, structureElementId);


            var result = _carpetservice.RegenerateValidation(structureMarkList, parameterSet, structureElementId, projectTypeId, projectId, productTypeId, out errorMessage, 1);

            if (errorMessage != "")
            {
                return BadRequest(errorMessage);
            }
            else
            {
                return Ok(result);

            }



        }


        [HttpPost]
        [Route("/InsertCarpetProductMarking/{structureElementId}")]
        public async Task<IActionResult> InsertCarpetProductMarking([FromBody] CarpetProduct objProductMarkingMarking, int structureElementId)
        {
            string errorMessage = "";
            var result = _carpetservice.InsertCarpetProductMarking(objProductMarkingMarking, structureElementId, out errorMessage, 1);
            if (errorMessage != "")
            {
                return BadRequest(errorMessage);
            }
            else
            {
                return Ok(result);

            }


        }



        #endregion

        #region DELETE

        [HttpDelete]
        [Route("/DeleteStructureMarking_carpet/{StructureMarkId}")]
        public bool DeleteStructureMarking_carpet(int StructureMarkId)
        {
            //bool deleteStructMark = false;
            string errorMessage;

            CarpetStructure carpetStructure = new CarpetStructure
            {
                StructureMarkId = StructureMarkId,
            };

            bool deleteStructMark = _carpetservice.DeleteStructureMarking(carpetStructure, out errorMessage);

            return deleteStructMark;

        }


        [HttpDelete]
        [Route("/DeleteProductMarking_carpet/{ProductMarkId}/{seDetailingId}")]
        public bool DeleteProductMarking_carpet(int ProductMarkId, int seDetailingId)
        {
            bool deleteStructMark = false;
            string errorMessage = "";
            try
            {
                CarpetProduct productMark = new CarpetProduct
                {
                    ProductMarkId = ProductMarkId,
                };

                deleteStructMark = _carpetservice.DeleteProductMarking(productMark, seDetailingId, out errorMessage);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return deleteStructMark;

        }



        #endregion



    }



}
