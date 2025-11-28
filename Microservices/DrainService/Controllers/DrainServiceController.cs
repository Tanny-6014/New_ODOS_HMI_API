using Microsoft.AspNetCore.Http;
using AutoMapper;
using DrainService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using DrainService.Repositories;
using DrainService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;


namespace DrainService.Controllers

{
    [Route("api/[controller]")]
    //[ApiController]

    public class DrainServiceController : ControllerBase
    {
        private readonly IAdminDal _adminDal;
        private readonly IDetailDal _detailDal;

        private readonly IBorePileParameterSetService _BoreParameterSet;
        private readonly IMapper _mapper;


        public DrainServiceController(IAdminDal adminDal, IMapper mapper, IDetailDal detailDal, IBorePileParameterSetService BoreParameterSet)
        {
            _adminDal = adminDal;
            _detailDal = detailDal;
            _mapper = mapper;
            _BoreParameterSet = BoreParameterSet;

        }


        [HttpGet]
        [Route("/GetCommonParameterSet_Dropdown_Drain/{projectId}")]
        public async Task<IActionResult> GetParameterSet_Dropdown(int projectId)
        {
            try
            {
                var result = await _adminDal.GetParameterSetAsync(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpPost]
        [Route("/InsertProjectParameter")]
        public async Task<IActionResult> InsertProjectParameter([FromBody] InsertProjectParameterDto obj)
        {
            string errorMessage = "";
            try
            {

                var result = _adminDal.InsertProjectParameter(obj,out errorMessage);

                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);
                }
                else
                {
                    return Ok(result);

                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        #region ProjectParamDrainLap

        [HttpGet]
        [Route("/GetProjectParamDrainLap/{tntParamSetNumber}")]
        public async Task<IActionResult> GetProjectParamDrainLap(int tntParamSetNumber)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                ParameterSetNo = tntParamSetNumber
            };

            List<GetProjectParamDrainLapDto> result = _adminDal.GetProjectParamDrainLap(adminInfo);

            return Ok(result);
        }



        [HttpPost]
        [Route("/InsertProjectParamDrainLap")]
        public async Task<IActionResult> InsertProjectParamDrainLap([FromBody] InsertProjectParamDrainLapDto obj)
        {
            string errorMessage = "";
            try
            {

                var result = _adminDal.InsertProjectParamDrainLap(obj,out errorMessage);

                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);
                }
                else
                {
                    return Ok(result);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }

        [HttpDelete]
        [Route("/DeleteProductParamDrainLap/{intDrainLapId}/{bitConfirm}")]
        public async Task<IActionResult> DeleteProductParamDrainLap(int intDrainLapId, byte bitConfirm)
        {
            AdminInfo adminInfo = new AdminInfo
            {
                DrainLapId = intDrainLapId,
                Confirm = bitConfirm
            };
            var DrainLapDelete = _adminDal.DeleteProductParamDrainLap(adminInfo);
            return Ok(new { DrainLap = DrainLapDelete, response = "success" });

        }


        #endregion


        #region ProjectParamDrainDepth

        [HttpGet]
        [Route("/GetProjectParamDrainDepth/{tntParamSetNumber}")]
        public async Task<IActionResult> GetProjectParamDrainDepth(int tntParamSetNumber)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                ParameterSetNo = tntParamSetNumber,

            };

            List<GetProjectParamDrainDepthDto> result = _adminDal.GetProjectParamDrainDepth(adminInfo);

            return Ok(result);

        }




        [HttpPost]
        [Route("/InsertProjectParamDrainDepth")]
        public async Task<IActionResult> InsertProjectParamDrainDepth([FromBody] GetProjectParamDrainDepthDto obj)
        {
            string errorMessage = "";

            try
            {

                var result = _adminDal.InsertProjectParamDrainDepth(obj,out errorMessage);

                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);
                }
                else
                {
                    return Ok(result);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        [HttpDelete]
        [Route("/DeleteProductParamDrainDepth/{intDrainDepthParamId}/{bitConfirm}")]
        public async Task<IActionResult> DeleteProductParamDrainDepth(int intDrainDepthParamId, byte bitConfirm)
        {
            AdminInfo adminInfo = new AdminInfo
            {
                DepthId = intDrainDepthParamId,
                Confirm = bitConfirm
            };
            var DrainLapDelete = _adminDal.DeleteProductParamDrainDepth(adminInfo);
            return Ok(new { DeleteDrainDepth = DrainLapDelete, response = "success" });

        }



        #endregion


        #region ProjectParamDrainWM

        [HttpGet]
        [Route("/GetProjectParamDrainWM/{tntParamSetNumber}")]
        public async Task<IActionResult> GetProjectParamDrainWM(int tntParamSetNumber)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                ParameterSetNo = tntParamSetNumber,

            };

            List<GetProjectParamDrainWMDto> result = _adminDal.GetProjectParamDrainWM(adminInfo);

            return Ok(result);

        }

        [HttpPost]
        [Route("/InsertProjectParamDrainWM")]
        public async Task<IActionResult> InsertProjectParamDrainWM([FromBody] InsertProjectParamDrainWMDto obj)
        {
            string errorMessage = "";
            try
            {

                var result= _adminDal.InsertProjectParamDrainWM(obj,out errorMessage);
                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);
                }
                else
                {
                    return Ok(result);

                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        [HttpDelete]
        [Route("/DeleteProductParamDrainWM/{intDrainWMId}")]
        public async Task<IActionResult> DeleteProductParamDrainWM(int intDrainWMId)
        {
            AdminInfo adminInfo = new AdminInfo
            {
                WMId = intDrainWMId,
            };
            var DrainWMDelete = _adminDal.DeleteProductParamDrainWM(adminInfo);
            return Ok(new { DeleteDrainDepth = DrainWMDelete, response = "success" });

        }

        #endregion


        #region GET Methods


        [HttpGet]
        [Route("/GetDrainProductCode")]
        public async Task<IActionResult> GetDrainProductCode()
        {
            string errorMessage = "";
            IEnumerable<getDrainProductCodeDto> getDrainProductCodes = await _adminDal.GetDrainProductCode();
            var result = getDrainProductCodes;
            return Ok(result);

        }


        [HttpGet]
        [Route("/GetLapProductCodeWM/{tntParamSetNumber}")]
        public async Task<IActionResult> GetLapProductCodeWM(int tntParamSetNumber)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                ParameterSetNo = tntParamSetNumber,

            };
            List<getDrainProductCodeDto> getDrainProductCodes = _adminDal.GetLapProductCodeWM(adminInfo);
            var result = getDrainProductCodes;
            return Ok(result);

        }

        [HttpGet]
        [Route("/GetDrainProductTypeById/{tntParamSetNumber}")]
        public async Task<IActionResult> GetDrainProductTypeById(int tntParamSetNumber)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                ParameterSetNo = tntParamSetNumber,

            };

            List<GetDrainProductTypeDto> result = _adminDal.GetDrainProductType(adminInfo);

            return Ok(result);

        }

        [HttpGet]
        [Route("/GetDrainProductType")]
        public async Task<IActionResult> GetDrainProductType()
        {
            string errorMessage = "";

            List<GetDrainProductTypeDto> getDrainProductTypes = _adminDal.GetDrainProductType();
            var result = getDrainProductTypes;
            return Ok(result);

        }


        [HttpGet]
        [Route("/GetDrainWidthWM/{tntParamSetNumber}")]
        public async Task<IActionResult> GetDrainWidthWM(int tntParamSetNumber)
        {
            string errorMessage = "";
            IEnumerable<GetDrainWidthWMDto> getDrainProductTypes = await _adminDal.GetDrainWidthWM(tntParamSetNumber);
            var result = getDrainProductTypes;
            return Ok(result);

        }

        [HttpGet]
        [Route("/GetProjectParamDrainLayer")]
        public async Task<IActionResult> GetProjectParamDrainLayer()
        {
            string errorMessage = "";

            List<GetProjectParamDrainLayerDto> getDrainProductTypes = _adminDal.GetProjectParamDrainLayer();
            var result = getDrainProductTypes;
            return Ok(result);

        }

        [HttpGet]
        [Route("/GetProjectParamDrainMaxDepth/{intDrainDepthParamId}")]
        public async Task<IActionResult> GetProjectParamDrainMaxDepth(int intDrainDepthParamId)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                DepthId = intDrainDepthParamId,

            };
            List<GetProjectParamDrainLayerDto> getDrainProductTypes = _adminDal.GetProjectParamDrainMaxDepth(adminInfo);
            var result = getDrainProductTypes;
            return Ok(result);

        }


        [HttpGet]
        [Route("/GetProjectParamDrainParamDetails/{intDrainWMId}")]
        public async Task<IActionResult> GetProjectParamDrainParamDetails(int intDrainWMId)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                WMId = intDrainWMId,

            };

            List<GetProjectParamDrainParamDto> result = _adminDal.GetProjectParamDrainParamDetails(adminInfo);

            return Ok(result);

        }

        [HttpGet]
        [Route("/GetProjectParamDrainShapeforLayer/{tntDrainLayerId}")]
        public async Task<IActionResult> GetProjectParamDrainShapeforLayer(int tntDrainLayerId)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                LayerId = tntDrainLayerId,
            };

            List<ProjectParamDrainShapeDto> result = _adminDal.GetProjectParamDrainShapeforLayer(adminInfo);

            return Ok(result);

        }

        [HttpGet]
        [Route("/GetDrainShapeCode/{intDrainWMId}/{intShapeId}")]
        public async Task<IActionResult> GetDrainShapeCode(int intDrainWMId, int intShapeId)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                WMId = intDrainWMId,
                ShapeId = intShapeId

            };

            List<DrainShapeCodeDto> result = _adminDal.GetDrainShapeCode(adminInfo);

            return Ok(result);

        }

        [HttpGet]
        [Route("/GetDrainParameterProductCodeDetails/{intProductCodeId}")]
        public async Task<IActionResult> GetDrainParameterProductCodeDetails(int intProductCodeId)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                ProductCodeId = intProductCodeId.ToString(),
            };

            List<DrainProductCodeDetailsDto> result = _adminDal.GetDrainParameterDetails(adminInfo);

            return Ok(result);

        }


        [HttpGet]
        [Route("/GetDrainDepthWidth/{tntParamSetNumber}/{sitDrainTypeId}")]
        public async Task<IActionResult> GetDrainDepthWidth(int tntParamSetNumber, int sitDrainTypeId)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                ParameterSetNo = tntParamSetNumber,
                Type = sitDrainTypeId

            };

            List<GetDrainWidthWMDto> result = _adminDal.GetDrainDepthWidth(adminInfo);

            return Ok(result);

        }

        [HttpGet]
        [Route("/GetDrainWidthDepth/{tntParamSetNumber}/{sitDrainWidth}")]
        public async Task<IActionResult> GetDrainWidthDepth(int tntParamSetNumber, string sitDrainWidth)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                ParameterSetNo = tntParamSetNumber,
                DrainDepthWidth = sitDrainWidth

            };

            List<DrainWidthDepthDto> result = _adminDal.GetDrainWidthDepth(adminInfo);

            return Ok(result);

        }


        [HttpGet]
        [Route("/GetShapeParamDetails/{intShapeId}")]
        public async Task<IActionResult> GetShapeParamDetails(int intShapeId)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                ShapeId = intShapeId
            };

            List<GetShapeParamDetailsDto> result = _adminDal.GetShapeParamDetails(adminInfo);

            return Ok(result);

        }


        [HttpGet]
        [Route("/GetParameterSet/{intProjectId}/{vchStructureElement}/{sitProductTypeL2Id}")]
        public async Task<IActionResult> GetParameterSet(int intProjectId,string vchStructureElement, int sitProductTypeL2Id)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                ProjectId = intProjectId,
                StructureElement = vchStructureElement,
                ProductType= sitProductTypeL2Id
            };

            List<GetParameterSetDto> result = _adminDal.GetParameterSet(adminInfo);

            return Ok(result);

        }


        [HttpGet]
        [Route("/GetProjectParameter/{intProjectId}/{tntParamSetNumber}/{vchParameterType}")]
        public async Task<IActionResult> GetProjectParameter(int intProjectId, int tntParamSetNumber, string vchParameterType)
        {
            string errorMessage = "";
            AdminInfo adminInfo = new AdminInfo
            {
                ProjectId = intProjectId,
                ParameterSetNo=tntParamSetNumber,
                ParameterType = vchParameterType
            };

            List<GetProjectParameterDto> result = _adminDal.GetProjectParameter(adminInfo);

            return Ok(result);

        }

        #endregion


        #region Main Drain

        [HttpPost]
        [Route("/InsertProjectDrainParamDetails")]
        public async Task<IActionResult> InsertProjectDrainParamDetails([FromBody] InsertProjectDrainParamDetailsDto insertProjectDrainParamDetails)
        {
            string errorMessage = "";
            try
            {

                var result = _adminDal.InsertProjectDrainParamDetails(insertProjectDrainParamDetails,out errorMessage);

                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);
                }
                else
                {
                    return Ok(result);

                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

 
        [HttpGet]
        [Route("/GetDrainParamDepthValues/{intGroupMarkID}/{intParamSet}/{intDrainWidth}/{vchDrainType}")]
        public async Task<IActionResult> GetDrainParamDepthValues(int intGroupMarkID, int intParamSet, int intDrainWidth, string vchDrainType)
        {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                intGroupMarkId = intGroupMarkID,
                tntParamSetNumber = intParamSet,
                intDrainWidth = intDrainWidth,
                vchDrainType = vchDrainType

            };

            List<DrainParamDepthValues_GetNewDto> result = _detailDal.DrainParamDepthValues_GetNew(detailInfo);

            return Ok(result);

        }


        [HttpGet]
        [Route("/CheckDrainGroupMarking/{ParamSetNumber}/{DrainWidth}/{DrainType}")]
        public async Task<IActionResult> CheckDrainGroupMarking(int ParamSetNumber, int DrainWidth, string DrainType)
        {
            DetailInfo detailInfo = new DetailInfo
            {
                tntParamSetNumber = ParamSetNumber,
                intDrainWidth = DrainWidth,
                vchDrainType = DrainType
            };
            var CheckDrainGM = _detailDal.CheckDrainGroupMarking(detailInfo);
            return Ok(new { checkGM = CheckDrainGM, response = "success" });

        }

        [HttpGet]
        [Route("/GetSWShapeCode")]
        public async Task<IActionResult> GetSWShapeCode()
        {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo { };

            List<GetSWShapeCodeDto> result = _detailDal.SWShapeCode_Get_Drain(detailInfo);

            return Ok(result);

        }

        [HttpDelete]
        [Route("/DeleteAccessories/{SELevelDetailsID}")]
        public async Task<IActionResult> DeleteAccessories(int SELevelDetailsID)
        {
            DetailInfo detailInfo = new DetailInfo
            {
                intSELevelDetailsID = SELevelDetailsID
            };
            var deleteACS = _detailDal.DeleteAccessories(detailInfo);
            return Ok(new { ACS = deleteACS, response = "success" });

        }

        [HttpGet]
        [Route("/GetSWProductCode/{vchProductCode}")]
        public async Task<IActionResult> GetSWProductCode(string vchProductCode)
        {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                vchProductCode = vchProductCode
            };

            List<GetSWProductCodeDto> result = _detailDal.SWProductCode_Drain_Get(detailInfo);

            return Ok(result);

        }


       
        [HttpGet]
        [Route("/GetPRCEnvelopeDetails/{StructureElementTypeId}")]
        public async Task<IActionResult> GetPRCEnvelopeDetails(int StructureElementTypeId)
        {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                intStructureElementTypeId = StructureElementTypeId
            };

            List<PRCEnvelopeDetailsDto> result = _detailDal.PRCEnvelopeDetails_Get(detailInfo);

            return Ok(result);

        }

        [HttpGet]
        [Route("/GetDrainStructureMarking/{GroupMarkingName}/{ProjectId}/{SEDetailingId}/{GroupMarkID}")]
        public async Task<IActionResult> DrainStructureMarking_Get(string GroupMarkingName,int ProjectId,int SEDetailingId,int GroupMarkID)
        {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                vchGroupMarkingName = GroupMarkingName,
                intProjectId=ProjectId,
                SEDetailingId=SEDetailingId,
                intGroupMarkId=GroupMarkID
            };

            List<DrainStructureMarkingDto> result = _detailDal.DrainStructureMarking_Get(detailInfo);

            return Ok(result);
           
        }


        [HttpPost]
        [Route("/DrainStructureMarking_InsUpd")]
        public async Task<IActionResult> DrainStructureMarking_InsUpd([FromBody] AddDrainStructMarkingDto addDrainStructMarking)
        {
            string errorMessage = "";
            try
            {

                var result = _detailDal.DrainStructureMarking_InsUpd(addDrainStructMarking,out errorMessage);
                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);
                }
                else
                {
                    return Ok(result);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpDelete]
        [Route("/DrainStructureMarking_Del/{intStructureMarkId}")]
        public async Task<IActionResult> DrainStructureMarking_Del(int intStructureMarkId)
        {
            DetailInfo detailInfo = new DetailInfo
            {
                intStructureMarkId = intStructureMarkId
            };
            var deleteACS = _detailDal.DrainStructureMarking_Del(detailInfo);
            return Ok(new { deletstructMark = deleteACS, response = "success" });

        }


        [HttpGet]
        [Route("/GetDrainParamDepthValues/{intGroupMarkID}/{intParamSet}")]
        public async Task<IActionResult> GetDrainParamDepthValues(int intGroupMarkID, int intParamSet)
        {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                intGroupMarkId = intGroupMarkID,
                tntParamSetNumber = intParamSet
            };

            List<DrainParamDepthValues_GetNewDto> result = _detailDal.DrainParamDepthValues_Get(detailInfo);

            return Ok(result);

        }

        [HttpGet]
        [Route("/DrainProductMarkingDetails_Get/{intStructureMarkId}")]
        public async Task<IActionResult> DrainProductMarkingDetails_Get(int intStructureMarkId)
        {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                intStructureMarkId=intStructureMarkId,
            };
            
            List<GetDrainProdMarkDto> result = _detailDal.DrainProductMarkingDetails_Get(detailInfo);

            return Ok(result);

        }

        
        [HttpPost]
        [Route("/DrainProductMarkingDetails_InsUpd")]
        public async Task<IActionResult> DrainProductMarkingDetails_InsUpd([FromBody] SaveProductMarkingDetailsDto drainProductMarkingDetails)
        {
            string errorMessage = "";
            var result = false;

            try
            {


                DrainBLL drainBLL = new DrainBLL();

                drainBLL.drainOuterCrossWire = (float)drainProductMarkingDetails.drainOuterCrossWire/1000;
                drainBLL.drainInnerCrossWire = (float)drainProductMarkingDetails.drainInnerCrossWire/1000;
                drainBLL.drainSlabCrossWire = (float)drainProductMarkingDetails.drainSlabCrossWire/1000;
                drainBLL.drainBaseCrossWire = (float)drainProductMarkingDetails.drainBaseCrossWire/1000;
                drainBLL.IsCascade = drainProductMarkingDetails.IsCascade;
                drainBLL.intCascadeNo = drainProductMarkingDetails.intCascadeNo;
                drainBLL.flCsCrossLength = (float)drainProductMarkingDetails.flCsCrossLength;
                drainBLL.flCsDropHeight = (float)drainProductMarkingDetails.flCsDropHeight;
                drainBLL.flCsWidth = (float)drainProductMarkingDetails.flCsWidth;
                drainBLL.startChainage = (float)drainProductMarkingDetails.startChainage;
                drainBLL.endChainage = (float)drainProductMarkingDetails.endChainage;
                drainBLL.startTopLevel = (float)drainProductMarkingDetails.startTopLevel;
                drainBLL.endTopLevel = (float)drainProductMarkingDetails.endTopLevel;
                drainBLL.startInvertLevel = (float)drainProductMarkingDetails.startInvertLevel;
                drainBLL.endInvertLevel = (float)drainProductMarkingDetails.endInvertLevel;
                drainBLL.structMarkingName = drainProductMarkingDetails.structMarkingName;

                drainBLL.txtMemQty = drainProductMarkingDetails.txtMemQty;
                drainBLL.startDepth = (float)drainProductMarkingDetails.startDepth;
                drainBLL.endDepth = (float)drainProductMarkingDetails.endDepth;
                drainBLL.tntStructureRevNo = drainProductMarkingDetails.tntStructureRevNo;
                drainBLL.vchGroupMarkName = drainProductMarkingDetails.vchGroupMarkName;
                drainBLL.intParameterSet = drainProductMarkingDetails.intParameterSet;
                drainBLL.bitTransportChk = drainProductMarkingDetails.bitTransportChk;
                drainBLL.bitBendingChk = drainProductMarkingDetails.bitBendingChk;
                drainBLL.bitProduceIndicator = drainProductMarkingDetails.bitProduceIndicator;
                drainBLL.bitMachineChk = drainProductMarkingDetails.bitMachineChk;
                drainBLL.intUserID = drainProductMarkingDetails.intUserID;
                drainBLL.drainTopCover = drainProductMarkingDetails.  drainTopCover;
                drainBLL.drainBottomCover= drainProductMarkingDetails.drainBottomCover;
                drainBLL.drainOuterCover = drainProductMarkingDetails.drainOuterCover;
                drainBLL.drainInnerCover = drainProductMarkingDetails.drainInnerCover;
                drainBLL.intDrainStructureMarkId = drainProductMarkingDetails.intDrainStructureMarkId;


                result = drainBLL.GenerateDrainProducts(out errorMessage);

                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);
                }
                else
                {
                    return Ok(result);

                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpDelete]
        [Route("/DeleteDrainProductMarking/{ProductMarkId}")]
        public async Task<IActionResult> DeleteDrainProductMarking(int ProductMarkId)
        {
            DetailInfo detailInfo = new DetailInfo
            {
                intProductMarkId = ProductMarkId
            };
            var DrainProductMarking = _detailDal.DrainProductMarking_Del(detailInfo);
            return Ok(new { deleteProductMarking = DrainProductMarking, response = "success" });

        }


        [HttpGet]
        [Route("/GetDrainParameterSetByPrjID/{intProjectId}")]
        public async Task<IActionResult> DrainParameterSetByPrjID_Get(int intProjectId)
        {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                intProjectId = intProjectId,
            };

            List<DrainParameterSetByPrjIDDto> result = _detailDal.DrainParameterSetByPrjID_Get(detailInfo);

            return Ok(result);

        }

        [HttpGet]
        [Route("/GetStructElementIsExist/{GroupMarkingId}/{StructureElementName}")]
        public async Task<IActionResult> GetStructElementIsExist(int GroupMarkingId, string StructureElementName)
        {

            try
            {

                var result = _detailDal.GetStructElementIsExist(GroupMarkingId, StructureElementName);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("/GetHeaderInfo/{intProjectID}")]
        public async Task<IActionResult> GetHeaderInfo(int intProjectID)
        {

            try
            {
                DetailInfo detailInfo = new DetailInfo 
                {
                 intProjectId=intProjectID,
                };

                List<GetHeaderInfoDto> result = _detailDal.GetHeaderInfo(detailInfo);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("/GetRoundOffValue/{ProductTypeId}/{StructureElementTypeId}/{ConsignmentType}/{MWDiameter}/{MWProductionLength}")]
        public async Task<IActionResult> GetRoundOffValue(int ProductTypeId, int StructureElementTypeId,int ConsignmentType,decimal MWDiameter, int MWProductionLength)
        {

            try
            {
                DetailInfo detailInfo = new DetailInfo
                {
                    sitProductTypeId = ProductTypeId,
                    intStructureElementTypeId=StructureElementTypeId,
                    intConsignmentType=ConsignmentType,
                    decMWDiameter=MWDiameter,
                    numProductionMWLength = MWProductionLength,
                };
                var result = _detailDal.GetRoundOffValue(detailInfo);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [HttpGet]
        [Route("/DrainParamInfo_Get/{tntparameterSet}")]
        public async Task<IActionResult> DrainParamInfo_Get(int tntparameterSet)
        {

            try
            {
                DetailInfo detailInfo = new DetailInfo
                {
                    tntParamSetNumber = tntparameterSet,
                   
                };

                var result = _detailDal.DrainParamInfo_Get(detailInfo);

           

             

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpPost]
        [Route("/DrainProductMarkingDetails_Update")]
        public async Task<IActionResult> DrainProductMarkingDetails_Update([FromBody] GetDrainProdMarkDto drainProductMarkingDetails)
        {
            string errorMessage = "";
            var result = false;
            String txtMO1 = drainProductMarkingDetails.MhMo1.ToString();
            String txtMO2 = drainProductMarkingDetails.MhMo2.ToString();
            String txtMO3 = drainProductMarkingDetails.MhCo1.ToString();
            String txtMO4 = drainProductMarkingDetails.MhCo2.ToString();
            bool chkProInd = true;

            string hdnStructureMarkId = drainProductMarkingDetails.intDrainStructureMarkId.ToString();
            string hdnProductCodeId = drainProductMarkingDetails.intProductCodeId.ToString();
            string hdnProductMarkId = drainProductMarkingDetails.intProductMarkId.ToString();
            string hdnShapeCode = drainProductMarkingDetails.Shape;
            String lblSM_TC = "True";//drainProductMarkingDetails.TC;
            String lblSM_MC = "True";//drainProductMarkingDetails.MC;
            String lblSM_BC = "True";//drainProductMarkingDetails.BC;
            string hdnMWSpacing = drainProductMarkingDetails.intMWSpacing.ToString();
            string hdnCWSpacing = drainProductMarkingDetails.intCWSpacing.ToString();
            String lblProductCode = drainProductMarkingDetails.vchProductCode.ToString();
            int lblShapeId = drainProductMarkingDetails.ShapeId;
            String txtShapeTC = drainProductMarkingDetails.TC;
            String lblMWlen = drainProductMarkingDetails.MhMwLength.ToString();
            String lblCWlen = drainProductMarkingDetails.MhCwLength.ToString();
            string strSafeState = drainProductMarkingDetails.xmlResult;

            String lblProductMarking = drainProductMarkingDetails.vchProductMarkingName;
            int TotalQty = drainProductMarkingDetails.TotalQty;

            try
            {
                DrainBLL drainBLL = new DrainBLL();                
                int result1 = drainBLL.ReLoadOverHangs(strSafeState, "", txtMO1, txtMO2, txtMO3, txtMO4, hdnProductMarkId, hdnStructureMarkId, hdnProductCodeId, lblSM_TC, lblSM_BC, lblSM_MC, hdnMWSpacing, hdnCWSpacing, hdnShapeCode, lblProductCode, lblShapeId, txtShapeTC, "", "", "0@0", lblMWlen, lblCWlen, lblProductMarking, TotalQty);
                return Ok(result1);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion

        #region Drain Other

        [HttpPost]
        [Route("/GetOthProductCode")]
        public async Task<IActionResult> OthProductCode_Get([FromBody] DrainProductCodeDetailsDto DrainProductCodeDetailsDto)
            {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                vchProductCode = DrainProductCodeDetailsDto.vchProductCode
            };

            List<DrainProductCodeDetailsDto> result = _detailDal.OthProductCode_Get(detailInfo);

            return Ok(result);

        }

        [HttpGet]
        [Route("/GetOthDrainOverHangs/{intMWSpace}/{intCWSpace}/{chrShapeCode}")]
        public async Task<IActionResult> GetOthDrainOverHangs(int intMWSpace, int intCWSpace, string chrShapeCode)
        {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                intMWSpacing = intMWSpace,
                intCWSpace = intCWSpace,
                chrShapeCode = chrShapeCode
            };

            List<GetOthDrainOverHangsDto> result = _detailDal.OthDrainOverHangs_Get(detailInfo);

            return Ok(result);

        }

        [HttpGet]
        [Route("/PopulateOtherProductMarking/{StructureMarkId}")]
        public async Task<IActionResult> PopulateOtherProductMarking(int StructureMarkId)
        {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                intStructureMarkId = StructureMarkId
            };

            List<OtherProductMarkingDetailsDto> result = _detailDal.DrainOtherProductMarkingDetails_Get(detailInfo);

            return Ok(result);

        }



        [HttpPost]
        [Route("/GenerateOtherDrainProduct")]
        public async Task<IActionResult> GenerateOtherDrainProduct([FromBody] GenerateOtherDrainDto otherDrainDto)
        {
            string strErrorMesg = "";

            try
            {
                string strProductMarkName = otherDrainDto.strProductMarkName;
                string vchMeshShape = otherDrainDto.vchMeshShape;
                int intShapeId = otherDrainDto.intShapeId;
                string strMWLen = otherDrainDto.strMWLen;
                string strCWLen = otherDrainDto.strCWLen;
                string strMWSpace = otherDrainDto.strMWSpace;
                string strCWSpace = otherDrainDto.strCWSpace;
                string strMWDia = otherDrainDto.strMWDia;
                string strCWDia = otherDrainDto.strCWDia;
                string strMO1 = otherDrainDto.strMO1;
                string strMO2 = otherDrainDto.strMO2;
                string strCO1 = otherDrainDto.strCO1;
                string strCO2 = otherDrainDto.strCO2;

                string strProductMarkId = otherDrainDto.strProductMarkId;

                int intStructureMarkId = otherDrainDto.intStructureMarkId;

                string strProductCodeId = otherDrainDto.strProductCodeId;

                bool blnTC = otherDrainDto.blnTC;
                bool blnBC = otherDrainDto.blnBC;
                bool blnMC = otherDrainDto.blnMC;
                string strShapeCode = otherDrainDto.strShapeCode;
                string strProductCode = otherDrainDto.strProductCode;
                int intTransHeaderId = otherDrainDto.intTransHeaderId;
                int TotMeshQty = otherDrainDto.TotMeshQty;

                string strSequence = otherDrainDto.strSequence;

                string strParamValues = otherDrainDto.strParamValues;

                string strCriticalIndicator = otherDrainDto.strCriticalIndicator;

                int intParameterSet = otherDrainDto.intParameterSet;
                int tntStructureRevNo = otherDrainDto.tntStructureRevNo;
                string strUserId = otherDrainDto.strUserId;
                string strPrdInd = otherDrainDto.strPrdInd;
                string bitOHVal=otherDrainDto.bitOHVal;


                DrainOther drainOther = new DrainOther();
                var result = drainOther.GenerateOtherDrainProducts(strProductMarkName, vchMeshShape, intShapeId, strMWLen, strCWLen, strMWSpace, strCWSpace, strMWDia, strCWDia, strMO1, strMO2, strCO1, strCO2, strProductMarkId, intStructureMarkId, strProductCodeId, blnTC, blnBC, blnMC, strShapeCode, strProductCode, intTransHeaderId, TotMeshQty, strSequence, strParamValues, strCriticalIndicator, intParameterSet, tntStructureRevNo, strUserId, strPrdInd, out strErrorMesg, bitOHVal);

                if (strErrorMesg == "")
                {
                    return Ok(result);    
                }
                else
                {
                    return BadRequest(strErrorMesg);

                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        #endregion
        //test

    }
}
