using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Threading.Tasks;
using Tacton.Configurator.ObjectModel;
using WBSService.Dtos;
using WBSService.Interfaces;
using WBSService.Model;
using WBSService.Repositories;


namespace WBSService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class WbsController : ControllerBase
    {
        private readonly IWbs wbsRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        public WbsController(IWbs wbs, IMapper mapper, HttpClient httpClient)
        {
            wbsRepository = wbs;
            _mapper = mapper;
            _httpClient = httpClient;


        }

        [HttpGet]
        [Route("/GetWBSElementsList/{wbsid}")]
        public async Task<IActionResult> GetWBSElementsList(int wbsid)
        {
            List<WBSElements> objwbs = await wbsRepository.GetWBSElementsListAsync(wbsid);
            var result = _mapper.Map<List<WBSElementsDto>>(objwbs);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetWBSCollapseLevel/{id}")]
        public async Task<IActionResult> GetWBSCollapseLevel(int id)
        {
            List<WBSAtCollapseLevel> wbsCollaps = await wbsRepository.GetWbsCollapsAsync(id);
            var result = _mapper.Map<List<WBSAtCollapseLevelDto>>(wbsCollaps);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetProductType")]
        public async Task<IActionResult> GetProductType()
        {
            List<ProductTypeMaster> productTypeMaster = await wbsRepository.GetProductType();
            var result = _mapper.Map<List<ProductTypeMasterDto>>(productTypeMaster).Distinct();
            return Ok(result);
        }


        [HttpGet]
        [Route("/GetStructElement")]
        public async Task<IActionResult> GetStructElement()
        {
            List<StructureElementMaster> structureElementMaster = await wbsRepository.GetStructElement();
            var result = _mapper.Map<List<StructureElementMasterDto>>(structureElementMaster).Distinct();
            return Ok(result);
        }


        //[HttpGet]
        //[Route("/GetWbsStoreyfrom")]
        //public async Task<IActionResult> GetWbsStoreyfrom()
        //{
        //    List<WBSAtCollapseLevel> WBSAtCollapseLevel = await wbsRepository.GetStoreyfrom();
        //    var result = _mapper.Map<List<WBSStoreyFromDto>>(WBSAtCollapseLevel).Distinct();
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("/GetWbsStorey")]
        public async Task<IActionResult> GetWbsStorey()
        {
            IEnumerable<StoryToFrom> StoryToFrom = await wbsRepository.GetStoreyTo();
            var result = _mapper.Map<List<WBSStoreyToDto>>(StoryToFrom).Distinct();
            return Ok(result);
        }

        [HttpPost]
        [Route("/AddWbs/{ProjectId}/{UserID}")]
        public async Task<IActionResult> AddListToWbs([FromBody] WBSMaintainenceDto wbsMaintainenceDto,int ProjectId,int UserID)
        {
            try
            {

                WBSMaintainence wbsMaintainence = new WBSMaintainence();

                var wbsMaintainenceobj = _mapper.Map<WBSMaintainence>(wbsMaintainenceDto);
                var result = await wbsRepository.AddWBSMaintainenceAsync(wbsMaintainenceobj, ProjectId,UserID);
                return Ok(result);
            }
            catch( Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        [HttpPost]
        [Route("/UpdateWbs/{UserID}")]
        public async Task<IActionResult> UpdateWbs([FromBody] WBSMaintainenceDto wbsMaintainenceDto,int UserID)
        {
            try
            {
                 
                WBSMaintainence wbsMaintainence = new WBSMaintainence();

                var wbsMaintainenceobj = _mapper.Map<WBSMaintainence>(wbsMaintainenceDto);
                var result = await wbsRepository.UpdateWBSMaintainenceAsync(wbsMaintainenceobj, UserID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        [HttpGet]
        [Route("/GetWbsMaintainanceList/{ProjectId}")]
        public async Task<IActionResult> GetWbsMaintainanceList(int ProjectId)
        {
            List<WBSMaintainence> objwbs = await wbsRepository.GetWbsMaintainanceList(ProjectId);
            var result = _mapper.Map<List<WBSMaintainenceDto>>(objwbs);
            return Ok(result);
        }

        [HttpDelete]
        [Route("/deleteWbs/{id}")]
        public async Task<IActionResult> deleteWbs(int id)
        {

            var WBSMaintainence = await wbsRepository.DeleteWbsAsync(id);
            return Ok(new { WBSMaintainence = WBSMaintainence, response = "success" });

        }
        
        [HttpDelete]
        [Route("/DeleteWbsCollapseLevel/{CollapseLevelId}")]
        public async Task<IActionResult> DeleteWbsCollapseLevel(int CollapseLevelId)
        {

            var WBSAtCollapseLevel = await wbsRepository.DeleteWbsCollapseLevelAsync(CollapseLevelId);
            return Ok(new { WBSAtCollapseLevel = WBSAtCollapseLevel, response = "success" });

        }
        [HttpDelete]
        [Route("/DeleteSelectedWbs/{wbsid}")]
        public async Task<IActionResult> DeleteSelectedWbs(string wbsid)
        {

            var WBSMaintainence = await wbsRepository.DeleteSelectedWbs(wbsid);
            return Ok(new { WBSMaintainence = WBSMaintainence, response = "success" });

        }

        [HttpPost]
        [Route("/DeleteSelectedStorey/{IsSet}")]
        public async Task<IActionResult> DeleteSelectedStorey([FromBody] WBSElementsDto WBSElementsDTO,string IsSet)
        {

          
            try
            {

                WBSElements WBSElements = new WBSElements();

                var wbsMaintainenceobj = _mapper.Map<WBSElements>(WBSElementsDTO);
                var result = await wbsRepository.DeleteStorey(wbsMaintainenceobj, IsSet);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPost]
        [Route("/AddWBSExtension/{ProjectCode}")]
        public async Task<IActionResult> AddWBSExtension([FromBody] WBSMaintainenceDto wbsMaintainenceDto, string ProjectCode)
        {
            try
            {

                WBSMaintainence wbsMaintainence = new WBSMaintainence();

                var wbsMaintainenceobj = _mapper.Map<WBSMaintainence>(wbsMaintainenceDto);
                var result = await wbsRepository.AddWBSExtension(wbsMaintainenceobj, ProjectCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }


        #region Posting
        [HttpGet]
        [Route("/GetWbsPostingList/{ProjectId}/{ProductTypeId}")]
        public async Task<IActionResult> GetWbsPostingList(int ProjectId,int ProductTypeId)
        {
            IEnumerable<WBSPosting> objwbs = await wbsRepository.GetWbsPostingListAsync(ProjectId, ProductTypeId);
            var result = _mapper.Map<List<WBSPosting>>(objwbs);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetGroupMarkingList")]
        public async Task<IActionResult> GetGroupMarkingList(int intProjectId, int intStructureElementId, int sitProductTypeId)
        {
            IEnumerable<WBSPostGroupMarkingDto> objwbs = await wbsRepository.GetGroupMarkingListAsync(intProjectId, intStructureElementId, sitProductTypeId);
            var result = _mapper.Map<List<WBSPostGroupMarkingDto>>(objwbs);
            return Ok(result);
        }

        [HttpGet]
        [Route("/UnPostBBS_Update/{PostHeaderId}")]
        public async Task<IActionResult> UnPostBBS_Update(int PostHeaderId)
        {
            try
            {

                Update_UnPostBBSDto wbsposting = new Update_UnPostBBSDto();

                //var wbspostingObj= _mapper.Map<Update_UnPostBBSDto>(wbsPostingDto);
                var result = await wbsRepository.UnPostBBS_Update(PostHeaderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        [HttpGet]
        [Route("/GetGroupMarkingDetails/{intProjectId}/{intWBSElementsId}/{intStructureElementId}/{sitProductTypeId}/{BBSNo}")]
        public async Task<IActionResult> GetGroupMarkingDetails(int intProjectId, int intWBSElementsId, int intStructureElementId, int sitProductTypeId, string BBSNo)
        {
            IEnumerable<WBSPostGroupMarkingDetailsDto> objwbs = await wbsRepository.GetGroupMarkingDetailsAsync(intProjectId, intWBSElementsId, intStructureElementId, sitProductTypeId, BBSNo);
            var result = _mapper.Map<List<WBSPostGroupMarkingDetailsDto>>(objwbs);
            return Ok(result);
        }

        [HttpPost]
        [Route("/PostBBS_Update")]
        public async Task<IActionResult> PostBBS_UpdateAsync([FromBody] PostingUpdateDto wbsPostingDto)
        {
            try
            {

                PostingUpdateDto wbsposting = new PostingUpdateDto();

                var wbspostingObj = _mapper.Map<PostingUpdateDto>(wbsPostingDto);
                var result = await wbsRepository.PostBBS_UpdateAsync(wbspostingObj);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        [HttpPost]
        [Route("/AddBBSRelease")]
        public async Task<IActionResult> AddBBSReleaseAsync([FromBody] AddBBSReleaseDto wbsMaintainenceDto)
        {
            try
            {

                AddBBSReleaseDto wbsMaintainence = new AddBBSReleaseDto();

                var wbsMaintainenceobj = _mapper.Map<AddBBSReleaseDto>(wbsMaintainenceDto);
                var result = await wbsRepository.AddBBSReleaseAsync(wbsMaintainenceobj);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        [HttpPost]
        [Route("/AddGroupMarkingAsync")]
        public async Task<IActionResult> AddGroupMarkingAsync([FromBody] AddGroupMarkingDtlDto addGroupMarkingDtlDto)
        {
            try
            {

                AddGroupMarkingDtlDto addGroupMarking = new AddGroupMarkingDtlDto();

                var addGroupMarkingDtlobj = _mapper.Map<AddGroupMarkingDtlDto>(addGroupMarkingDtlDto);
                var result = await wbsRepository.AddGroupMarkingAsync(addGroupMarkingDtlobj);
                if (result==0)
                {
                    return BadRequest("GM Already Present");
                }
                else if(result == 1)
                {
                    return Ok(result);
                }
                else if (result == 2)
                {
                    return BadRequest("GM Already Present");
                }
                if (result == 3)
                {
                    return BadRequest("HYBRID GM CAN NOT ADD");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        [HttpGet]
        [Route("/GetPostCappingInfoList/{INTPOSTHEADERID}")]
        public async Task<IActionResult> GetPostCappingInfoList(int INTPOSTHEADERID)
        {
            IEnumerable<WBSPostCappingInfoDto> objwbs = await wbsRepository.GetPostingCappingInfo(INTPOSTHEADERID);
            var result = _mapper.Map<List<WBSPostCappingInfoDto>>(objwbs);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetPostClinkInfoList/{INTPOSTHEADERID}")]
        public async Task<IActionResult> GetPostClinkInfoList(int INTPOSTHEADERID)
        {
            IEnumerable<WBSPostClinkInfoDto> objwbs = await wbsRepository.GetPostingClinkInfo(INTPOSTHEADERID);
            var result = _mapper.Map<List<WBSPostClinkInfoDto>>(objwbs);
            return Ok(result);
        }

        //added by vidya 
        [HttpPost]
        [Route("/AddPostingCCLMarkDetails")]
        public async Task<IActionResult> AddPostingCCLMarkDetailsAsync([FromBody] AddPostingCCLMarkDetailsDto postingCCLMarkDetailsDto)
        {
            try
            {

                AddPostingCCLMarkDetailsDto wbsMaintainence = new AddPostingCCLMarkDetailsDto();
                var wbsMaintainenceobj = _mapper.Map<AddPostingCCLMarkDetailsDto>(postingCCLMarkDetailsDto);
                var result = await wbsRepository.AddPostingCCLMarkDetailsAsync(wbsMaintainenceobj);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        [HttpPost]
        [Route("/AddPostingCLinkCCLMarkDetails")]
        public async Task<IActionResult> AddPostingCLinkCCLMarkDetailsAsync([FromBody] AddPostingCCLMarkDetailsDto postingClinkCCLMarkDetailsDto)
        {
            try
            {

                AddPostingCCLMarkDetailsDto wbsMaintainence = new AddPostingCCLMarkDetailsDto();
                var addPostingClink = _mapper.Map<AddPostingCCLMarkDetailsDto>(postingClinkCCLMarkDetailsDto);
                var result = await wbsRepository.AddPostingCLinkCCLMarkDetailsAsync(addPostingClink);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }


        [HttpGet]
        [Route("/GetPostingCappingHeaderInfo/{intWBSElementsId}/{intParentId}")]
        public async Task<IActionResult> GetPostingCappingHeaderInfoAsync( int intWBSElementsId, int intParentId)
        {
            IEnumerable<GetPostingCappingHeaderInfoDto> objwbs = await wbsRepository.GetPostingCappingHeaderInfoAsync(intWBSElementsId, intParentId);
            var result = _mapper.Map<List<GetPostingCappingHeaderInfoDto>>(objwbs);
            return Ok(result);
        }


        [HttpGet]
        [Route("/GetPostingCLinkHeaderInfo/{intWBSElementsId}/{intParentId}")]
        public async Task<IActionResult> GetPostingCLinkHeaderInfoAsync(int intWBSElementsId, int intParentId)
        {
            IEnumerable<GetPostingCLinkHeaderInfoDto> objwbs = await wbsRepository.GetPostingCLinkHeaderInfoAsync(intWBSElementsId, intParentId);
            var result = _mapper.Map<List<GetPostingCLinkHeaderInfoDto>>(objwbs);
            return Ok(result);
        }

        [HttpDelete]
        [Route("/DeletePostingCapStructure/{intPostHeaderId}/{vchProductCode}/{intWidth}/{chrShapeCode}/{StructMarkId}")]
        public async Task<IActionResult> DeletePostingCapStructure(int intPostHeaderId, string vchProductCode, int intWidth, string chrShapeCode, string StructMarkId)
        {

            int result = await wbsRepository.DeletePostingCapStructure(intPostHeaderId, vchProductCode, intWidth, chrShapeCode, StructMarkId);
            return Ok(result);

        }


        [HttpDelete]
        [Route("/DeletePostingCLinkStructure/{intPostHeaderId}/{vchProductCode}/{intWidth}/{chrShapeCode}/{StructMarkId}")]
        public async Task<IActionResult> DeletePostingCLinkStructure(int intPostHeaderId, string vchProductCode, int intWidth, string chrShapeCode, string StructMarkId)
        {

            int result = await wbsRepository.DeletePostingCLinkStructure(intPostHeaderId, vchProductCode, intWidth, chrShapeCode, StructMarkId);
            return Ok(result);

        }


        [HttpDelete]
        [Route("/DeletePostingGroupMarkingDetail/{intPostHeaderId}/{intGroupMarkId}")]
        public async Task<IActionResult> DeletePostingGroupMarkingDetail(int intPostHeaderId, int intGroupMarkId)
        {

            int result = await wbsRepository.DeletePostingGroupMarkDetails(intPostHeaderId, intGroupMarkId);
            return Ok(result);

        }


        [HttpGet]
        [Route("/GetCapProductList/{VCHENTEREDTEXT}")]
        public async Task<IActionResult> GetCapProductList(string VCHENTEREDTEXT)
        {
            IEnumerable<GetCapProductCodeDto> objwbs = await wbsRepository.GetProductCode(VCHENTEREDTEXT);
            var result = _mapper.Map<List<GetCapProductCodeDto>>(objwbs);
            return Ok(result);
        }   

        [HttpGet]
        [Route("/GetClinkProductList/{VCHENTEREDTEXT}")]
        public async Task<IActionResult> GetClinkProductList(string VCHENTEREDTEXT)
        {
            IEnumerable<GetCapProductCodeDto> objwbs = await wbsRepository.GetClinkProductCode(VCHENTEREDTEXT);
            var result = _mapper.Map<List<GetCapProductCodeDto>>(objwbs);
            return Ok(result);
        }


      
        [HttpGet]
        [Route("/PostingCapClinkExists/{INTPOSTHEADERID}/{INTSTRUCTUREELEMENTTYPEID}/{PRODUCTTYPEID}")]
        public async Task<IActionResult> PostingCapClinkExists(int INTPOSTHEADERID, int INTSTRUCTUREELEMENTTYPEID, int PRODUCTTYPEID)
        {
            try
            {
                var result = await wbsRepository.Check_PostingCapClinkExists(INTPOSTHEADERID, INTSTRUCTUREELEMENTTYPEID, PRODUCTTYPEID);

                if (result == 0)
                {
                    return Ok(result);
                }
                else if (result == 1)
                {
                    return BadRequest("Product Code Already Present");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }



        [HttpGet]
        [Route("/GetCapShapeCodeList/{VCHENTEREDTEXT}")]
        public async Task<IActionResult> GetCapShapeCodeList(string VCHENTEREDTEXT)
        {
            IEnumerable<GetCapShapeCodeDto> objwbs = await wbsRepository.GetCapShapeCode(VCHENTEREDTEXT);
            var result = _mapper.Map<List<GetCapShapeCodeDto>>(objwbs);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetClinkShapeCodeList/{VCHENTEREDTEXT}")]
        public async Task<IActionResult> GetClinkShapeCodeList(string VCHENTEREDTEXT)
        {
            IEnumerable<GetCapShapeCodeDto> objwbs = await wbsRepository.GetClinkShapeCode(VCHENTEREDTEXT);
            var result = _mapper.Map<List<GetCapShapeCodeDto>>(objwbs);
            return Ok(result);
        }


        [HttpGet]
        [Route("/GetPostCappingMO1MO2CO1Co2/{INTPOSTHEADERID}/{MWLength}/{CapProduct}/{CWlength}")]
        public async Task<IActionResult> GetPostCappingMO1MO2CO1Co2(int INTPOSTHEADERID, int MWLength, string CapProduct, int CWlength)
        {
            IEnumerable<GetCappingMO1MO2CO1CO2Dto> objwbs = await wbsRepository.GetCappingMO1MO2CO1Co2(INTPOSTHEADERID, MWLength, CapProduct, CWlength);
            var result = _mapper.Map<List<GetCappingMO1MO2CO1CO2Dto>>(objwbs);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetPostClinkMO1MO2CO1Co2/{INTPOSTHEADERID}/{MWLength}/{CapProduct}/{CWlength}")]
        public async Task<IActionResult> GetPostClinkMO1MO2CO1Co2(int INTPOSTHEADERID, int MWLength, string CapProduct, int CWlength)
        {
            IEnumerable<GetCappingMO1MO2CO1CO2Dto> objwbs = await wbsRepository.GetClinkMO1MO2CO1Co2(INTPOSTHEADERID, MWLength, CapProduct, CWlength);
            var result = _mapper.Map<List<GetCappingMO1MO2CO1CO2Dto>>(objwbs);
            return Ok(result);
        }

        [HttpPost]
        [Route("/UpdatePostingGroupMarkingDetailNew")]
        public async Task<IActionResult> UpdatePostingGroupMarkingDetail([FromBody] UpdateGroupMarkDetailsDto groupMarkDetails)
        {
            if (groupMarkDetails == null)
            {
                return BadRequest("Invalid input");
            }

            int result = await wbsRepository.Update_PostingGroupMarkDetails(
                groupMarkDetails.PostHeaderId,
                groupMarkDetails.intGroupMarkId,
                groupMarkDetails.tntGroupQty,
                groupMarkDetails.VCHRemarks,
                groupMarkDetails.intUpdatedId
            );

            return Ok(result);
        }
        [HttpPost]
        [Route("/SavePostGroupMarkingList/{wbsElementId}/{structureElementTypeId}/{productTypeId}/{wbsBBSNo}/{wbsBBSDesc}/{userId}/{postHeaderId}/{projectId}")]
        public async Task<IActionResult> SavePostGroupMarkingList([FromBody] PostGroupMark postGroupMarkList, int wbsElementId, int structureElementTypeId, int productTypeId, string wbsBBSNo, string wbsBBSDesc, int userId, int postHeaderId, int projectId)
        {
            try
            {
                List<PostGroupMark> listOfMarks = new List<PostGroupMark> { postGroupMarkList };

                var result = await wbsRepository.SavePostGroupMarking(listOfMarks, wbsElementId, structureElementTypeId, productTypeId, wbsBBSNo, wbsBBSDesc, userId, postHeaderId, projectId);
                //if (result == 0)
                //{
                //    return BadRequest("GM Already Present");
                //}
                //else if (result == 1)
                //{
                //    return Ok(result);
                //}
                //else if (result == 2)
                //{
                //    return BadRequest("GM Already Present");
                //}
                //if (result == 3)
                //{
                //    return BadRequest("HYBRID GM CAN NOT ADD");
                //}
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        [HttpGet]
        [Route("/PostBBSAsync/{postHeaderId}/{userId}/{Modular}/{UserName}")]

        public async Task<IActionResult> PostBBSAsync(int postHeaderId, int userId, string Modular, string UserName)
        {
            PostingWBS objPostWBS = new PostingWBS();
            bool isSuccuess = false;
            string errorMessage = "";
            try
            {
                List<PostGroupMark> listPGM = wbsRepository.getGroupMarkwithType(postHeaderId, out errorMessage); //added for CABDE By TCS on 17.03.2016
                List<PostGroupMark> listPGMempty = wbsRepository.getBlankGroupMarkList(postHeaderId, out errorMessage); //added By TCS for DIFOT
                List<PostGroupMark> listPGMerror = wbsRepository.getinvalidmaterialList(postHeaderId, out errorMessage); //added for difot

                if (listPGM.Count <= 0 && listPGMempty.Count <= 0 && listPGMerror.Count <= 0) //added
                {
                    //------Main logic>>
                    objPostWBS.PostHeaderId = postHeaderId;
                    if (Modular == "Y")
                    {
                        isSuccuess =  wbsRepository.PostBBS_Modular(userId, postHeaderId, out errorMessage, UserName);

                    }
                    else
                    {
                        isSuccuess =  wbsRepository.PostBBS(userId, postHeaderId, out errorMessage, UserName);
                    }
                    //--------Main Logic<<
                }
                //added for CABDE By TCS on 17.03.2016 -start
                else if (listPGM.Count > 0)
                {
                    string GM_names = "";
                    int slNo = 1;
                    foreach (PostGroupMark GM in listPGM)
                    {
                        GM_names += "$" + slNo++ + ".  " + GM.GroupMarkingName;
                        if (GM.isCABDE == 0)
                        {
                            GM_names += "  (ARMA Plus)";
                        }
                        else if (GM.isCABDE == 1)
                        {
                            GM_names += "  (CAB Data Entry)";
                        }
                        else if (GM.isCABDE == -1)
                        {
                            GM_names += "  (Hybrid/Invalid)";
                        }
                    }
                    throw new Exception("Mixed/Invalid Group Mark Present : " + GM_names);
                }
                //added for CABDE By TCS on 17.03.2016 -end
                //added for difot
                else if (listPGMerror.Count > 0)
                {
                    string GM_names = "";

                    int slNo = 1;
                    foreach (PostGroupMark GM in listPGMerror)
                    {
                        GM_names += "$" + slNo++ + ".  " + GM.GroupMarkingName + "- " + GM.productcode;

                    }
                    throw new Exception("Below GroupMark can not post.Please contact IT : " + GM_names);
                }
                //end added for difot
                else if (listPGMempty.Count > 0)
                {
                    string GM_names = "";
                    int slNo = 1;
                    foreach (PostGroupMark GM in listPGMempty)
                    {
                        GM_names += "$" + slNo++ + ".  " + GM.GroupMarkingName;
                    }
                    throw new Exception("Empty GroupMark can't post : " + GM_names);
                }
                else
                {
                    throw new Exception("Some unknown error with groupMarks, contact ODOS admin.");
                }
                ////added for difot
                //added for CABDE By TCS on 17.03.2016 -end
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return BadRequest(errorMessage);
            }
            return Ok(isSuccuess);
        }

        //end
        [HttpGet]
        [Route("/GetWBSAttachedGroupMark/{postHeaderId}")]
        public async Task<IActionResult> GetWBSAttachedGroupMark(int postHeaderId)
        {
            List<PostGroupMark> listGetWBSAttachedGroupMark = new List<PostGroupMark>();
            PostGroupMark objGetWBSAttachedGroupMark = new PostGroupMark();
            string errorMessage = "";
            try
            {
                objGetWBSAttachedGroupMark.PostHeaderId = postHeaderId;
                var res = wbsRepository.WBSAttachedGroupMark(objGetWBSAttachedGroupMark.PostHeaderId);
                



                return Ok(res);
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return BadRequest(errorMessage);
            }
            finally
            {
                listGetWBSAttachedGroupMark = null;
                objGetWBSAttachedGroupMark = null;
            }
        }

        [HttpPost]
        [Route("/SaveCappingDetails/{UserId}/{postHeaderId}")]
        public async Task<IActionResult> SaveCappingDetails([FromBody] SaveCappingDetailsDto saveCappingDetails, int UserId, int postHeaderId)
        {
            string errorMessage = "";
            int PMID = 0;
            int SMID = 0;
            bool SaveFlag = false;
            try
            {
                CapClink capClink = new CapClink();

                capClink.WBSElementId = saveCappingDetails.INTWBSELEMENTID;
                capClink.ProductCodeId = saveCappingDetails.INTPRODUCTCODEID;
                capClink.Width = saveCappingDetails.INTWIDTH;
                capClink.Depth = saveCappingDetails.INTDEPTH;
                capClink.MWLength = saveCappingDetails.INTMWLENGTH;
                capClink.CWLength = saveCappingDetails.INTCWLENGTH;
                capClink.Qty = saveCappingDetails.INTQTY;
                capClink.RevNo = saveCappingDetails.INTREVNO;
                capClink.AddFlag = saveCappingDetails.CHRADDFLAG;
                capClink.ShapeCodeName = saveCappingDetails.STRSHAPEID;
                capClink.MO1 = saveCappingDetails.MO1;
                capClink.MO2 = saveCappingDetails.MO2;
                capClink.CO1 = saveCappingDetails.CO1;
                capClink.CO2 = saveCappingDetails.CO2;
                capClink.StructureElementTypeId = saveCappingDetails.STRUCTUREELEMENTID;
                capClink.ProductTypeId = saveCappingDetails.PRODUCTTYPEL1ID;
                UserId = saveCappingDetails.USERID;
                postHeaderId = saveCappingDetails.INTPOSTHEADERID;


                var result = capClink.SaveCappingDetails(UserId, postHeaderId, out PMID, out SMID);
                SaveFlag = true;
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }
        [HttpGet]
        [Route("/GetCapping/{postHeaderId}")]
        public List<CapClink> GetCapping(int postHeaderId)
        {
            List<CapClink> listCapping = new List<CapClink>();
            CapClink objCapping = new CapClink();
            //errorMessage = "";
            try
            {
                //objCapping.WBSElementId = wbsElementId;
                listCapping = objCapping.Capping_Get(postHeaderId);
                return listCapping;
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                //errorMessage = ex.Message;
                return null;
            }
            finally
            {
                //listCapping = null;
                //objCapping = null;ajit
            }
        }

        [HttpPost]
        [Route("/SaveCappingDetails_tbe/{UserId}/{wbsElementId}/{StructureElementTypeId}/{ProductTypeId}/{postHeaderId}")]
        public async Task<IActionResult> SaveCappingDetails_tbe([FromBody] CapClink[] saveCappingDetail, int UserId, int wbsElementId, int StructureElementTypeId, int ProductTypeId, int postHeaderId)
        {
            string errorMessage = "";
            int PMID = 0;
            int SMID = 0;
            bool SaveFlag = false;
            int totalcount = saveCappingDetail.Length;
            int counter = 0;
            try
            {
                foreach (CapClink saveCappingDetails in saveCappingDetail)
                {
                    CapClink capClink = new CapClink();

                    capClink.WBSElementId = saveCappingDetails.WBSElementId;
                    capClink.ProductCodeId = saveCappingDetails.ProductCodeId;
                    capClink.Width = saveCappingDetails.Width;
                    capClink.Depth = saveCappingDetails.Depth;
                    capClink.MWLength = saveCappingDetails.MWLength;
                    capClink.CWLength = saveCappingDetails.CWLength;
                    capClink.Qty = saveCappingDetails.Qty;
                    capClink.RevNo = saveCappingDetails.RevNo;
                    capClink.AddFlag = saveCappingDetails.AddFlag;
                    capClink.ShapeCodeName = saveCappingDetails.ShapeCodeName;
                    capClink.MO1 = saveCappingDetails.MO1;
                    capClink.MO2 = saveCappingDetails.MO2;
                    capClink.CO1 = saveCappingDetails.CO1;
                    capClink.CO2 = saveCappingDetails.CO2;
                    capClink.StructureElementTypeId = saveCappingDetails.StructureElementTypeId;
                    capClink.ProductTypeId = saveCappingDetails.ProductTypeId;
                    UserId = UserId;
                    postHeaderId = postHeaderId;


                    var result = capClink.SaveCappingDetails(UserId, postHeaderId, out PMID, out SMID);
                    SaveFlag = true;
                    counter++;
                    if (counter == totalcount)
                    {
                        if (PMID ==  0 || SMID==0)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            //_Slabservice.DeleteStructureMarking(slabStructure.StructureMarkId);
                            if(PMID!=0)
                            {
                                return BadRequest(PMID);
                            }
                            else
                            {
                                return BadRequest(SMID);
                            }
                            
                        }
                    }
                   
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPost]
        [Route("/UpdatePostingGroupMarkingDetail")]
        public async Task<bool> Save_Wbs([FromBody] SaveWBSDto saveWBSDto)
        {
            bool isSuccuess = false;
            
            isSuccuess = wbsRepository.SaveWBS(saveWBSDto);
            return isSuccuess;

        }

        [HttpPost]
        [Route("/SaveClink_Details/{UserId}/{WBSElementID}/{StructElementID}/{ProductTypeL1Id}")]
        public async Task<bool> SaveClink_Details([FromBody] SaveClinkDetailsDto saveClinkDetailsDto, int UserId, int WBSElementID, int StructElementID, int ProductTypeL1Id)
        {
            try
            {
                bool isSuccuess = false;
                isSuccuess = wbsRepository.SaveClinkDetails(saveClinkDetailsDto, UserId, WBSElementID, StructElementID, ProductTypeL1Id);
                return isSuccuess;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        [HttpGet]
        [Route("/Generate_BBSNo/{ProjectId}/{StructureElementTypeId}/{ProductTypeId}")]
        public async Task<IActionResult> Generate_BBSNo(int ProjectId, int StructureElementTypeId, int ProductTypeId)
        {
            try
            {
                var result = await wbsRepository.GenerateBBSNo(ProjectId, StructureElementTypeId, ProductTypeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        //[HttpPost]
        //[Route("/PostBBS_Modular/{userId}/{postHeaderId}")]
        //public async Task<bool> PostBBS_Modular(int userId, int postHeaderId)
        //{
        //    string errormsg = "";
        //    try
        //    {
        //        bool isSuccuess = false;
        //        isSuccuess = wbsRepository.PostBBS_Modular(userId,postHeaderId, out errormsg);
        //        return isSuccuess;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }


        //}


        [HttpGet]
        [Route("/InvalidData_Get/{GroupmarkingId}")]
        public async Task<IActionResult> InvalidData_Get(int GroupmarkingId)
        {
            string DetailingServiceUrl = "https://devodos.natsteel.com.sg:444/Detailing/CallStructremarkingDetails/" + GroupmarkingId;
            //string DetailingServiceUrl = "https://localhost:5002/CallStructremarkingDetails/" + GroupmarkingId;



            var response = await _httpClient.GetAsync(DetailingServiceUrl);

            if (response.IsSuccessStatusCode)
            {
                //var result = await response.Content.ReadAsStringAsync();
                var result = wbsRepository.InvalidData(GroupmarkingId);
                return Ok(result);
            }
            else
            {
                return BadRequest(response.IsSuccessStatusCode);   
            }
        
        }


        [HttpGet]
        [Route("/Drawing_Status/{Status}/{wbsId}")]
        public async Task<IActionResult> Drawing_Status(int wbsId,bool Status)
        {
            var result = wbsRepository.Drawing_Approve(Status,wbsId);
            return Ok(result);
        }
        [HttpPost]
        [Route("/SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmail email)
        {
            var result = wbsRepository.Execute("", "", email.EmailTo, email.EmailCc, email.Subject, "", email.Content);

            return Ok(result);
        }

        [HttpPost]
        [Route("/uploadDrawingFiles_New")]
        //[ValidateAntiForgeryHeader]
        public async Task<IActionResult> uploadDrawingFiles_New([FromBody] List<DrawingSubmission> lst)
        {
            try
            {
                Insert_drawing_details obj = new Insert_drawing_details();
                obj.drawingSubmissions = lst;
                var result = wbsRepository.IsSubmitted_Drawing(obj);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("/uploadDrawingFiles_New_2")]
        //[ValidateAntiForgeryHeader]
        public async Task<IActionResult> uploadDrawingFiles_New_2([FromBody] List<DrawingSubmission> lst)
        {
            try
            {
                Insert_drawing_details obj = new Insert_drawing_details();
                obj.drawingSubmissions = lst;
                var result = wbsRepository.IsSubmitted_Drawing(obj);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/Get_User_drawingData/{projectcode}")]
        public async Task<IActionResult> Get_User_drawingData( string projectcode)
        {
            try
            {

            List<User_Drawing_get> result = wbsRepository.User_drawing_get(projectcode);

            return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("/Insert_User_review/{UserReview}/{DrawingId}")]
        public async Task<IActionResult> Insert_User_review(string UserReview,int DrawingId)
        {
            try
            {

                var result = wbsRepository.Edit_user_review(UserReview,DrawingId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]

        [Route("/checkOrderDocs_2/{OrderNumber}/{StructureElement}/{ProductType}/{ScheduledProd}")]

        public ActionResult checkOrderDocs_2(int OrderNumber, string StructureElement, string ProductType, string ScheduledProd)

        {
            try
            {

                var result = wbsRepository.Get_docs_table(OrderNumber, StructureElement, ProductType, ScheduledProd);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("/GetUserNamesByCustomerAndProject/{CustomerCode}/{ProjectCode}/{wbsIDs}")]
        public async Task<IActionResult> LastUpcomingMail(string CustomerCode, string ProjectCode,string wbsIDs)
        {
            IEnumerable<GetEmailDetails> orderServiceList = await wbsRepository.GetUserNamesByCustomerAndProject(CustomerCode, ProjectCode,wbsIDs);
            return Ok(orderServiceList);
        }


        [HttpGet]
        [Route("/GetDetailersByCustomerAndProject/{CustomerCode}/{ProjectCode}/{wbselementID}")]
        public async Task<IActionResult> GetDetailersByCustomerAndProject(string CustomerCode, string ProjectCode,int wbselementID)
        {
            List<GetEmailDetails> orderServiceList = await wbsRepository.GetDetailersByCustomerAndProject(CustomerCode, ProjectCode, wbselementID);
            return Ok(orderServiceList);
        }

        [HttpGet]
        [Route("/Get_Drawing_Data_new/{wbsid}")]
        public async Task<IActionResult> Get_Drawing_Data_new(int wbsid)
        {
            try
            {

                List<DrawingData> result = wbsRepository.Drawing_list_get(wbsid);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPost]

        [Route("/UpdateDrawingApprovalStatus")]

        public async Task<IActionResult> UpdateDrawingApprovalStatus([FromBody] DrawingApprovalDto approvalDto)

        {

            try

            {

                var result = await wbsRepository.UpdateDrawingApprovalStatus(approvalDto);

                return Ok(result);

            }

            catch (Exception ex)

            {

                return Ok(ex.Message);

            }


        }

        [HttpGet]

        [Route("/UpdateMailSubmitStatus/{ProjectCode}/{WBSElementID}")]

        public async Task<IActionResult> UpdateMailSubmitStatus(string ProjectCode, int WBSElementID)

        {

            try

            {

                var result = await wbsRepository.UpdateMailSubmitStatus(ProjectCode, WBSElementID);

                return Ok(result);

            }

            catch (Exception ex)

            {

                return Ok(ex.Message);

            }


        }


        [HttpPost]
        [Route("/uploadDrawingFiles_New_Table")]
        //[ValidateAntiForgeryHeader]
        public async Task<IActionResult> uploadDrawingFiles_New_Table([FromBody] DrawingSubmission_New obj)
        {
            try
            {
                
                var result = wbsRepository.IsSubmitted_Drawing_NEw(obj);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/Get_User_drawingData_new/{projectcode}")]
        public async Task<IActionResult> Get_User_drawingData_new(string projectcode)
        {
            try
            {

                List<User_Drawing_get_new> result = wbsRepository.User_drawing_get_new(projectcode);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("/Get_Submission_status/{wbsid}")]
        public async Task<IActionResult> Get_Submission_status(int wbsid)
        {
            try
            {

                List<User_Drawing_get_new> result = wbsRepository.Submission_status(wbsid);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("/modiftDrawing_posting")]
        public async Task<IActionResult> modiftDrawing_posting([FromBody] ModifyDrawing obj)
        {
            string errorMessage = "";
            try
            {

                var result = wbsRepository.ModifyDrawing(obj, out errorMessage);

                return Ok(result);
            }
            catch (Exception ex)
            {
                
                return BadRequest(errorMessage);
            }

        }

        [HttpGet]
        [Route("/modifyDrawing_Status/{WBSElementId}")]
        public async Task<IActionResult> modifyDrawing_Status(int WBSElementId)
        {
            string errorMessage = "";
            try
            {

                var result = wbsRepository.modifyDrawing_Status(WBSElementId, out errorMessage);

                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(errorMessage);
            }

        }

        [HttpGet]
        [Route("/GetDrawingReport/{fromDate}/{toDate}/{customerCode}/{projectCode}")]
        public async Task<IActionResult> GetDrawingReport(string customerCode, string projectCode, string fromDate, string toDate)
        {
            List<DrawingReportDto> result = wbsRepository.GetDrawingReport(customerCode, projectCode, fromDate, toDate);
            return Ok(result);
        }

        [HttpPost]

        [Route("/copydrawing/{drawingId}")]

        public async Task<IActionResult> CopyDrawingRecord(int drawingId, [FromBody] List<DrawingCopyDto> items)
        {

            try
            {
                string errorMessage = "";
                var result = wbsRepository.CopyDrawingDetails(items, drawingId, out errorMessage);
                if(errorMessage=="")
                {
                return Ok(result);

                }
                else
                {
                    return BadRequest(errorMessage);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
          


        }


        [HttpGet]
        [Route("/GetIsPrecast/{postheaderID}")]
        public async Task<IActionResult> GetIsPrecast(int postheaderID)
        {
            int result = wbsRepository.GetIsPreCast(postheaderID);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetFileSubmissionStatus/{customercode}/{projectcode}/{filename}/{wbselementid}")]
        public async Task<IActionResult> GetFileSubmissionStatus(string customercode, string projectcode, string filename, int wbselementid)
        {
            try
            {
                string errorMessage = "";
                var result = wbsRepository.GetFileSubmissionStatus(customercode, projectcode, filename, wbselementid, out errorMessage);

                if(errorMessage!="")
                {
                    return BadRequest(errorMessage);
                }
                else
                {
                    return Ok(new { result=result });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/MigrateWBSReleaseData")]
        public async Task<IActionResult> MigrateWBSReleaseData()
        {
            
            var result = await wbsRepository.MigrateWBSReleaseData();
            return Ok(result);
        }

        #endregion


    }
}
