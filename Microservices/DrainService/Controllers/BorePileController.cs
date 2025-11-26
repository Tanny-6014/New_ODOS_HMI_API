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
    [ApiController]
    public class BorePileController : ControllerBase
    {
        private readonly IDetailDal _detailDal;
        private readonly ICABDAL _cabDal;

        private readonly IBorePileParameterSetService _BoreParameterSet;

        private readonly IMapper _mapper;


        public BorePileController( IMapper mapper, IDetailDal detailDal, IBorePileParameterSetService BoreParameterSet, ICABDAL cABDAL)
        {
            _detailDal = detailDal;
            _mapper = mapper;
            _BoreParameterSet = BoreParameterSet;
            _cabDal = cABDAL;

        }


        #region BPC

        #region Get

        [HttpGet]
        [Route("/populateDDParamSetNumber/{intProjectId}")]
        public async Task<IActionResult> populateDDParamSetNumber(int intProjectId)
        {
            string errorMessage = "";

            DetailInfo detailInfo = new DetailInfo
            {
                intProjectId = intProjectId

            };

            IEnumerable<BorePileParamInfoDto> borePileParamInfoDtos = _detailDal.GetBorePileParameterInfoByPrjId(detailInfo);
            var result = borePileParamInfoDtos;
            return Ok(result);

        }

        [HttpGet]
        [Route("/PopulateParameterInfo/{tntParamSetNumber}")]
        public async Task<IActionResult> PopulateParameterInfo(int tntParamSetNumber)
        {
            string errorMessage = "";

            DetailInfo detailInfo = new DetailInfo
            {
                tntParamSetNumber = tntParamSetNumber

            };

            IEnumerable<BorePileParamInfoDto> borePileParamInfoDtos = _detailDal.GetBorePileParameterInfo(detailInfo);
            var result = borePileParamInfoDtos;
            return Ok(result);

        }


        [HttpGet]
        [Route("/GetProductType_Bore")]
        public async Task<IActionResult> GetProductType_Bore()
        {
            string errorMessage = "";
            List<BorePileParameterSet> borePileParamInfoDtos = _BoreParameterSet.GetProductType(out errorMessage);
            var result = borePileParamInfoDtos;
            return Ok(result);

        }

        [HttpGet]
        [Route("/GetBorePilePopulateMethods/{strType}/{intProductL2Id}/{strMainBarCode}")]
        public async Task<IActionResult> GetBorePilePopulateMethods(string strType, int intProductL2Id, string strMainBarCode)
        {
            string errorMessage = "";
            //strType = "FABMETHODS";

            List<GetBorePilePopulateMethodsDto> getBorePilePopulates = _detailDal.GetBorePilePopulateMethods(strType, intProductL2Id, strMainBarCode);
            var result = getBorePilePopulates;
            return Ok(result);

        }


        [HttpGet]
        [Route("/PopulateCageNotes")]
        public async Task<IActionResult> PopulateCageNotes()
        {
            string errorMessage = "";

            List<GetCageNotesDto> result = _detailDal.GetCageNotesPopulateMethods();

            return Ok(result);

        }


        [HttpGet]
        [Route("/PopulateMainBarBySelection/{vchMainBarPattern}")]
        public async Task<IActionResult> PopulateMainBarBySelection(string vchMainBarPattern)
        {
            string errorMessage = "";

            DetailInfo detailInfo = new DetailInfo
            {

                vchMainBarPattern = vchMainBarPattern,
            };

            IEnumerable<GetBPMainBarPatternDetailDto> getBPMainBarPatterns = _detailDal.GetBorePileMainBarPatternDetails(detailInfo);
            var result = getBPMainBarPatterns;
            return Ok(result);

        }

        [HttpGet]
        [Route("/PopulateElevationBySelection /{vchElevationPattern}")]
        public async Task<IActionResult> PopulateElevationBySelection(string vchElevationPattern)
        {
            string errorMessage = "";

            DetailInfo detailInfo = new DetailInfo
            {

                vchElevationPattern = vchElevationPattern,
            };

            IEnumerable<GetBPElevationPatternDetails> getBPElevationPatterns = _detailDal.GetBorePileElevationPatternDetails(detailInfo);
            var result = getBPElevationPatterns;
            return Ok(result);

        }

        [HttpGet]
        [Route("/PopulateAddSpiral/{intCageLength}/{intPileDia}")]
        public async Task<IActionResult> PopulateAddSpiral(int intCageLength, int intPileDia)
        {
            string errorMessage = "";

            DetailInfo detailInfo = new DetailInfo
            {

                intCageLength = intCageLength,
                intPileDia = intPileDia

            };

            IEnumerable<getAdditionalSpiralDetails> getAdditionalSpirals = _detailDal.getAdditionalSpiralDetails(detailInfo);
            var result = getAdditionalSpirals;
            return Ok(result);

        }

        [HttpGet]
        [Route("/PopulateCentralizerDropDown")]
        public async Task<IActionResult> PopulateCentralizerDropDown()
        {
            string errorMessage = "";

            IEnumerable<GetCentralizerDto> getCentralizers = _detailDal.GetPopulateCentralizer();
            var result = getCentralizers;
            return Ok(result);

        }

        [HttpGet]
        [Route("/PopulateStiffner /{intCageLength}/{intPileDia}")]
        public async Task<IActionResult> PopulateStiffner(int intCageLength, int intPileDia)
        {
            string errorMessage = "";

            DetailInfo detailInfo = new DetailInfo
            {

                intCageLength = intCageLength,
                intPileDia = intPileDia

            };

            IEnumerable<StiffenerDetailsDto> stiffenerDetails = _detailDal.getStiffenerDetails(detailInfo);
            var result = stiffenerDetails;
            return Ok(result);

        }

        [HttpGet]
        [Route("/PopulateStructGrid/{GroupMarkingName}/{intProjectId}/{SELevelDetailsID}")]
        public async Task<IActionResult> PopulateStructGrid(string GroupMarkingName, int intProjectId, int SELevelDetailsID)
        {
            string errorMessage = "";

            DetailInfo detailInfo = new DetailInfo
            {
                vchGroupMarkingName = GroupMarkingName,
                intProjectId = intProjectId,
                intSELevelDetailsID = SELevelDetailsID
            };

            IEnumerable<GetBorePilStructureMarkingDto> getBorePilStructureMarkings = _detailDal.GetBorePilStructureMarkingDetails(detailInfo);
            var result = getBorePilStructureMarkings;
            return Ok(result);

        }

        //testing
        [HttpGet]
        [Route("/GetSchnellMachineConfig/{intNoOfMainBar}/{intHoletoHoleDia}/{vchTemplateCode}")]
        public async Task<IActionResult> GetSchnellMachineConfig(int intNoOfMainBar, int intHoletoHoleDia, string vchTemplateCode)
        {
            string errorMessage = "";

            DetailInfo detailInfo = new DetailInfo
            {

                intNoOfMainBar = intNoOfMainBar,
                intHoletoHoleDia = intHoletoHoleDia,
                vchTemplateCode = vchTemplateCode

            };

            IEnumerable<GetSchnellMachineConfig> stiffenerDetails = _detailDal.GetSchnellMachineConfig(detailInfo);
            var result = stiffenerDetails;
            return Ok(result);

        }

        [HttpGet]
        [Route("/PopulateLinksValues/{intPileDia}")]
        public async Task<IActionResult> PopulateLinksValues(int intPileDia)
        {
            string errorMessage = "";

            DetailInfo detailInfo = new DetailInfo
            {
                intPileDia = intPileDia
            };

            IEnumerable<getAdditionalSpiralDetails> getbpcpilediavalue = _detailDal.GetBPCPileDiaDependentValue(detailInfo);
            var result = getbpcpilediavalue;
            return Ok(result);

        }


        [HttpGet]
        [Route("/GetTemplateForSchnellMachine/{intHoletoHoleDia}/{intNoOfMainBarLayer1}")]
        public async Task<IActionResult> GetTemplateForSchnellMachine(int intHoletoHoleDia, int intNoOfMainBarLayer1)
        {
            string errorMessage = "";

            DetailInfo detailInfo = new DetailInfo
            {
                intHoletoHoleDia = intHoletoHoleDia,
                intNoOfMainBarLayer1 = intNoOfMainBarLayer1
            };

            IEnumerable<GetSchnellMachineConfig> getbpcpilediavalue = _detailDal.GetTemplateForSchnellMachine(detailInfo);
            var result = getbpcpilediavalue;
            return Ok(result);

        }

        [HttpGet]
        [Route("/GetSchnellTemplates")]
        public async Task<IActionResult> GetSchnellTemplates()
        {
            string errorMessage = "";

            IEnumerable<GetSchnellMachineConfig> getTemplateforschnell = _detailDal.GetSchnellTemplates();
            var result = getTemplateforschnell;
            return Ok(result);

        }


        [HttpGet]
        [Route("/GetSchnellConfiguration/{intNoOfMainBar}/{vchTemplateCode}")]
        public async Task<IActionResult> GetSchnellConfiguration(int intNoOfMainBar, string vchTemplateCode)
        {                                                            
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                intNoOfMainBar= intNoOfMainBar,
                vchTemplateCode = vchTemplateCode,
            };

            var result = _detailDal.GetSchnellConfiguration(detailInfo);
            return Ok(result);

        }

        [HttpGet]
        [Route("/GetMEPDetailsForBPCDrawing/{MEPConfigId}")]
        public async Task<IActionResult> GetMEPDetailsForBPCDrawing(int MEPConfigId)
        {
            DetailInfo detailInfo = new DetailInfo
            {
                intMEPConfigId = MEPConfigId
            };
            IEnumerable<BPCMEPDetailsDto> bPCMEPDetails = _detailDal.GetMEPDetailsForBPCDrawing(detailInfo);
            var result = bPCMEPDetails;
            return Ok(result);

        }

        [HttpGet]
        [Route("/PopulateWBSElements/{GroupMarkId}")]
        public async Task<IActionResult> PopulateWBSElements(int GroupMarkId)
        {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                intGroupMarkId = GroupMarkId,
            };

            List<GetWBSElementByIdDto> result = _detailDal.GetWBSElementByGroupMarkId(detailInfo);

            return Ok(result);

        }


        [HttpGet]
        [Route("/getCoverCodeForStrucMarking/{intCoverLink}")]
        public async Task<IActionResult> getCoverCodeForStrucMarking(int intCoverLink)
        {
            string errorMessage = "";
            DetailInfo detailInfo = new DetailInfo
            {
                intCoverLink = intCoverLink,
            };

            var result = _detailDal.getCoverCodeForStrucMarking(detailInfo);

            List<string> res = new List<string>();

            try
            {
                res.Add(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



            return Ok(res);

        }

        [HttpGet]
        [Route("/getBPCMaterialCodeIDForStrucMarking/{vchMainBarPattern}/{vchElevationPattern}/{intSmallerMainBarLength}/{numPileDia}/{bitCoat}/{sitProductTypeId}")]
        public async Task<IActionResult> getBPCMaterialCodeIDForStrucMarking(string vchMainBarPattern,string vchElevationPattern,int intSmallerMainBarLength,int numPileDia,bool bitCoat,int sitProductTypeId)
        {
            string errorMessage = "";

            DetailInfo detailInfo = new DetailInfo
            {
                vchMainBarPattern = vchMainBarPattern,
                vchElevationPattern = vchElevationPattern,
                intSmallerMainBarLength= intSmallerMainBarLength,
                numPileDia= numPileDia,
                bitCoat=bitCoat, 
                sitProductTypeId= sitProductTypeId
            };
            var result   = _detailDal.getBPCMaterialCodeIDForStrucMarking(detailInfo);
            
            return Ok(result);

        }

        [HttpGet]
        [Route("/GetCABDetails/{SEDetailingId}/{intStructureMarkId}")]
        public async Task<IActionResult> GetCABDetails(int SEDetailingId,  int intStructureMarkId)
        {
            string errorMessage = "";

            DetailInfo detailInfo = new DetailInfo
            {
                SEDetailingId= SEDetailingId,
                intStructureMarkId= intStructureMarkId,
            };
            var result = _detailDal.GetCABDetails(detailInfo);

            return Ok(result);

        }



        #endregion

        #region POST

        [HttpPost]
        [Route("/InsertParameterSet_BorePile/{ProjectId}/{UserId}")]
        public async Task<IActionResult> InsertParameterSet_BorePile([FromBody] int ProductTypeId, int ProjectId, int UserId)
        {
            string errorMessage = "";
            try
            {

                var result = _BoreParameterSet.InsertParameterSet(ProjectId, ProductTypeId, UserId, out errorMessage);

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

        [HttpPost]
        [Route("/UpdateParameterSet_BorePile")]
        public async Task<IActionResult> UpdateParameterSet_BorePile([FromBody] UpdateParameterSet Parameterset)
        {
            string errorMessage = "";
            try
            {

                BorePileParameterSet borePilePS = new BorePileParameterSet
                {
                    ParameterSetId = Parameterset.ParameterSetId,
                    TransportModeId = Parameterset.TransportModeId,
                    LapLength = Parameterset.LapLength,
                    EndLength = Parameterset.EndLength,
                    AdjFactor = Parameterset.AdjFactor,
                    CoverToLink = Parameterset.CoverToLink,
                    ProductTypeL2Id = Parameterset.ProductTypeL2Id,
                    UserId = Parameterset.UserId,
                    Description = Parameterset.Description,
                };



                var result = _BoreParameterSet.UpdateParameterSet(Parameterset.ProjectId, borePilePS, out errorMessage);

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


        [HttpPost]
        [Route("/InsertBPCStructureMarkingDetails")]
        public async Task<IActionResult> InsertBPCStructureMarkingDetails([FromBody] InsertBPCStructureMarkingDto insertBPCStructureMarking)
        {
            string errorMessage = "";
            try
            {
               
                var result = _detailDal.InsertBPCStructureMarkingDetails(insertBPCStructureMarking,out errorMessage);

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

        [HttpPost]
        [Route("/UpdateBPCStructureMarkingDetails")]
        public async Task<IActionResult> UpdateBPCStructureMarkingDetails([FromBody] UpdateBPCStructureMarkingDto insertBPCStructureMarking)
        {
            string errorMessage = "";
            try
            {
               
                var result = _detailDal.UpdateBPCStructureMarkingDetails(insertBPCStructureMarking,out errorMessage);

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


        [HttpPost]
        [Route("/InsertAccessoriesCentralizer")]
        public async Task<IActionResult> InsertAccessoriesCentralizer([FromBody] InsertAccessoriesCentralizerDto insertAccessoriesCentralizer)
        {
            string errorMessage = "";
            try
            {

                var result = _detailDal.InsertAccessoriesCentralizer(insertAccessoriesCentralizer,out errorMessage);

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

        [HttpPost]
        [Route("/InsertBPCProjectParameterSet")]
        public async Task<IActionResult> InsertBPCProjectParameterSet([FromBody] BPCProjectParamInsertDto obj)
        {
            string errorMessage = "";
            try
            {

                var result = _detailDal.InsertBPCProjectParameterSet(obj, out errorMessage);

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

        [HttpPost]
        [Route("/UpdateBPCMainBarPattern")]
        public async Task<IActionResult> UpdateBPCMainBarPattern([FromBody] UpdateBPCMainBarDto updateBPCMainBar)
        {
            string errorMessage = "";
            try
            {

                var result = _detailDal.UpdateBPCMainBarPattern(updateBPCMainBar,out errorMessage);

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

        [HttpPost]
        [Route("/UpdateBarPostionForSchnelllMcn")]
        public async Task<IActionResult> UpdateBarPostionForSchnelllMcn([FromBody] SnlMcnBarPositonsUpdateDto snlMcnBarPositons)
        {
            string errorMessage = "";
            try
            {

                var result = _detailDal.UpdateBarPostionForSchnelllMcn(snlMcnBarPositons,out errorMessage);
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

        [HttpPost]
        [Route("/UpdateBorePileProducts")]
        public async Task<IActionResult> UpdateBorePileProducts([FromBody] BPCProductMarkingUpdateDto bPCProductMarking)
        {
            string errorMessage = "";
            try
            {

                var result = _detailDal.UpdateBorePileProducts(bPCProductMarking,out errorMessage);
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


        [HttpPost]
        [Route("/UpdateBPCElevationPattern")]
        public async Task<IActionResult> UpdateBPCElevationPattern([FromBody] BPCElevationUpdateDto bPCElevation)
        {
            string errorMessage = "";
            try
            {

                var result = _detailDal.UpdateBPCElevationPattern(bPCElevation,out errorMessage);

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
        [Route("/DeleteBPCStructureMarking/{StructureMarkId}/{CageSeqNo}/{GroupMarkId}")]
        public async Task<IActionResult> DeleteBPCStructureMarkingDetails(int StructureMarkId, int CageSeqNo, int GroupMarkId)
        {
            DetailInfo detailInfo = new DetailInfo
            {
                intStructureMarkId = StructureMarkId,
                intCageSeqNo = CageSeqNo,
                intGroupMarkId = GroupMarkId,
            };
            var DrainWMDelete = _detailDal.DeleteBPCStructureMarkingDetails(detailInfo);
            return Ok(new { DeleteDrainDepth = DrainWMDelete, response = "success" });

        }


        [HttpGet]
        [Route("/updateStructureMarking/{structuremarking}/{StructureMarkId}")]
        public async Task<IActionResult> updateStructureMarking(string structuremarking, int StructureMarkId)
        {
            string errorMessage = "";


            var result = _detailDal.UpdateStructureMarkingDetails(StructureMarkId, structuremarking, out errorMessage);

            if(result==1)
            {
                return Ok(result);
            }
            else
            {
             return BadRequest(errorMessage);
            }
           

        }


        #endregion


        #endregion



    }
}
