using AutoMapper;
using UtilityService.Dtos;
using UtilityService.Interface;
//using UtilitiesService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using UtilityService.Repositories;


namespace UtilityService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UtilityServiceController : ControllerBase
    {
        private readonly IUtilities _utilities;
        private readonly ICopyWBSService _copyWBSService;
        private readonly ICopyGroupMarkingService _copyGroupMarkingService;
        private readonly IMapper _mapper;

        public UtilityServiceController(IUtilities utilities, IMapper mapper, ICopyWBSService copyWBSService, ICopyGroupMarkingService copyGroupMarkingService)
        {
            _utilities = utilities;
            _mapper = mapper;
            _copyWBSService = copyWBSService;
            _copyGroupMarkingService = copyGroupMarkingService;
        }


        #region Copy WBS
        [HttpPost]
        [Route("/GetBBSNoAndBBSDesc/{WBSTypeId}")]
        public  async Task<IActionResult> GetBBSNoAndBBSDesc([FromBody] GetCopyWBSDTO GetCopyWBSDTO, int WBSTypeId)
        {
          
         

            WBSInfoClass wBSInfoClass = new WBSInfoClass
            {
                ProjectId = GetCopyWBSDTO.ProjectId,
                StructureElementId = GetCopyWBSDTO.structureElementId,
                ProductTypeId = GetCopyWBSDTO.productTypeId,
                Block = GetCopyWBSDTO.WBS1,
                WBSTypeId = WBSTypeId,
                Part = GetCopyWBSDTO.WBS3,
                Storey = GetCopyWBSDTO.WBSFrom,
            };


            List<GetBBSNoDto> result =  _copyWBSService.GetBBSNoAndBBSDesc(wBSInfoClass, GetCopyWBSDTO.structureElementId, GetCopyWBSDTO.productTypeId);

            return Ok(result);

        }


        [HttpPost]
        [Route("/GetDestWBSDetails/{WBSTypeId}")]
        public async Task<IActionResult> GetDestWBSDetails([FromBody] GetCopyWBSDTO GetCopyWBSDTO)
        {

            string errorMessage = "";
            WBSInfoClass wBSInfoClass = new WBSInfoClass
            {
                ProjectId = GetCopyWBSDTO.ProjectId,
                StructureElementId = GetCopyWBSDTO.structureElementId,
                ProductTypeId = GetCopyWBSDTO.productTypeId,        
                Block = GetCopyWBSDTO.WBS1,
                Part = GetCopyWBSDTO.WBS3,
            };

           var res = _copyWBSService.GetDestinationWBSDetails(wBSInfoClass, GetCopyWBSDTO.structureElementId, GetCopyWBSDTO.productTypeId, GetCopyWBSDTO.WBSFrom, GetCopyWBSDTO.WBSTo, out errorMessage);

            return Ok(res);

        }
        [HttpPost]
        [Route("/GetPostedQuantityandTonnageByWBSDetails/{WBSTypeId}/{postHeaderId}")]
        public async Task<IActionResult> GetPostedQuantityandTonnageByWBSDetails([FromBody] GetCopyWBSDTO GetCopyWBSDTO, int WBSTypeId, int postHeaderId)
        {

            //List<WBSInfoClass> wBSInfoClasses = _copyWBSService.GetPostedQuantityandTonnageByWBSDetails(ProjectId,structureElementId, productTypeId, WBSTypeId, WBS1,WBS2,WBS3,PostHeaderId);
            //var result = wBSInfoClasses;
            //return Ok(result);
            List<WBSInfoClass> destinationWBSInfo = null;
            string errorMessage = "";
           
            WBSInfoClass wBSInfo = new WBSInfoClass
            {
                ProjectId = GetCopyWBSDTO.ProjectId,
                //StructureElementId = structureElementId,
                //ProductTypeId = productTypeId,
                WBSTypeId = WBSTypeId,
                Block = GetCopyWBSDTO.WBS1,
                Part = GetCopyWBSDTO.WBS3,
                PostHeaderId = postHeaderId
            };
            List<WBSInfoClass> getDestinationList  = _copyWBSService.GetPostedQuantityandTonnageByWBSDetails(wBSInfo,GetCopyWBSDTO.structureElementId, GetCopyWBSDTO.productTypeId, GetCopyWBSDTO.WBSFrom, out destinationWBSInfo, out errorMessage);
            var result=getDestinationList;
            return Ok(result);
            
        }

        [HttpGet]
        [Route("/GetWBS1/{ProjectCode}/{structureElement}/{productType}")]
        public async Task<IActionResult> GetWBS1(string ProjectCode,string structureElement, string productType)
        {
           
            List<GetWBS1> result = _copyWBSService.GetWBS1(ProjectCode,structureElement,productType);

            return Ok(result);
        }

        [HttpPost]
        [Route("/GetWBS2/{ProjectCode}/{structureElement}/{productType}")]
        public async Task<IActionResult> GetWBS2([FromBody] GetCopyWBSDTO GetCopyWBSDTO, string ProjectCode,string structureElement, string productType)
        {
           
            List<GetWBS2> result = _copyWBSService.GetWBS2(ProjectCode,structureElement,  productType, GetCopyWBSDTO.WBS1);

            return Ok(result);
        }

        [HttpPost]
        [Route("/GetWBS3/{ProjectCode}/{StructureElement}/{ProductType}")]
        public async Task<IActionResult> GetWBS3([FromBody] GetCopyWBSDTO GetCopyWBSDTO,string ProjectCode, string StructureElement, string ProductType)
        {
            
            List<GetWBS3> result = _copyWBSService.GetWBS3(ProjectCode,StructureElement, ProductType, GetCopyWBSDTO.WBS1, GetCopyWBSDTO.WBSFrom);

            return Ok(result);
        }


        [HttpGet]
        [Route("/GetCopyWBS1/{ProjectId}/{StructureElementId}/{ProductTypeId}/{WBSTypeId}")]
        public async Task<IActionResult> GetWBS1(int StructureElementId, int ProductTypeId, int ProjectId, int WBSTypeId)
        {

            List<GetWBS1> result = _copyWBSService.GetCopyWBS1(StructureElementId, ProductTypeId, ProjectId, WBSTypeId);

            return Ok(result);
        }

        [HttpPost]
        [Route("/GetCopyWBS2")]
        public async Task<IActionResult> GetWBS2([FromBody] GetCopyWBSDTO getCopyWBSDTO)
        {

            List<GetWBS2> result = _copyWBSService.GetCopyWBS2(getCopyWBSDTO.structureElementId, getCopyWBSDTO.productTypeId, getCopyWBSDTO.ProjectId, 0, getCopyWBSDTO.WBS1);

            return Ok(result);
        }

        [HttpPost]
        [Route("/GetCopyWBS3")]
        public async Task<IActionResult> GetWBS3([FromBody] GetCopyWBSDTO getCopyWBSDTO)
        {

            List<GetWBS3> result = _copyWBSService.GetCopyWBS3(getCopyWBSDTO.structureElementId, getCopyWBSDTO.productTypeId, getCopyWBSDTO.ProjectId, 0, getCopyWBSDTO.WBS1, getCopyWBSDTO.WBSFrom);

            return Ok(result);
        }


        [HttpPost]
        [Route("/CopyWBSDetailing")]
        public async Task<IActionResult> CopyWBSDetailing([FromBody] CopyWBSDto copyWBSDto)
        {
            string errorMessage = "";
            try
            {

              
                WBSInfoClass wBSInfoClass = new WBSInfoClass();
                wBSInfoClass.ProjectId = copyWBSDto.DESTPROJECTID;
                wBSInfoClass.StructureElementId = copyWBSDto.STRUCTUREELEMENTTYPEID;
                wBSInfoClass.ProductTypeId = copyWBSDto.PRODUCTTYPEID;
                wBSInfoClass.WBSTypeId = copyWBSDto.WBSTYPEID;
                wBSInfoClass.PostHeaderId = copyWBSDto.SOURCEPOSTHEADERID;
                //WBSElements = copyWBSDto.DESTWBSELEMENTIDS;
                wBSInfoClass.BBSNo = copyWBSDto.BBSNOS;
                wBSInfoClass.BBSDesc = copyWBSDto.BBSDESCS;
                wBSInfoClass.UserId = wBSInfoClass.GetUserIDByName(copyWBSDto.UserName);
               


                var result = _copyWBSService.CopyWBSDetailing(wBSInfoClass, copyWBSDto.DESTWBSELEMENTIDS, copyWBSDto.BBSNOS, copyWBSDto.BBSDESCS, out errorMessage);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


            }


        [HttpGet]
        [Route("/ValidateBBSDetails/{WBSElements}/{ProjectId}/{StructureElementId}/{ProductTypeId}/{BBSNos}/{BBSDescriptions}")]
        public async Task<IActionResult> ValidateBBSDetails(string WBSElements, int ProjectId,int StructureElementId,int ProductTypeId, string BBSNos, string BBSDescriptions)
        {
            string errorMessage = "";
            try
            {

                WBSInfoClass wBSInfoClass = new WBSInfoClass();

                wBSInfoClass.ProjectId = ProjectId;
                wBSInfoClass.StructureElementId= StructureElementId;
                wBSInfoClass.ProductTypeId= ProductTypeId;


                List<ValidateBBSDetailsDto> result = _copyWBSService.ValidateBBSDetails(wBSInfoClass, WBSElements, BBSNos, BBSDescriptions, out errorMessage);
              
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }



        #endregion


        #region Copy Parameter
        [HttpPost]
        [Route("/InsertCopyProjectParameter")]
        public async Task<IActionResult> InsertCopyProjectParameter([FromBody] InsertCopyProjectParamDto utilitiesInfo)
        {

            try
            {

                var result = await _utilities.InsertCopyProjectParameter(utilitiesInfo);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }

        [HttpGet]
        [Route("/CopyProjectParameterSetGet/{ProjectId}/{ProductTypeId}/{MeshFlag}/{SElement}")]
        public async Task<IActionResult> CopyProjectParameterSetGet(int ProjectId, int ProductTypeId, int MeshFlag, string SElement)
        {
            string errorMessage = "";

            IEnumerable<getCopyParameterSetDto> Meshdata = await _utilities.CopyProjectParameterSetGet(ProjectId, ProductTypeId, MeshFlag, SElement);
            var result = _mapper.Map<List<getCopyParameterSetDto>>(Meshdata);
            return Ok(result);

        }




        #endregion


        #region Copy GroupMarking

        [HttpGet]
        [Route("/CopyGroupmarkGetGroupmarkingName/{ProjectId}/{ProductTypeId}/{SElement}")]
        public async Task<IActionResult> CopyGroupmark_GetGroupmarkingName(int ProjectId, int ProductTypeId, int SElement)
        {
            string errorMessage = "";
            GroupMark groupMarkName = new GroupMark
            {
                ProjectId = ProjectId,
                StructureElementTypeId = SElement,
                SitProductTypeId = ProductTypeId

            };

            List<Groupmarking_Name> result = _copyGroupMarkingService.GetGroupMarkName(groupMarkName, out errorMessage);

            return Ok(result);

        }



        [HttpPost]
        [Route("/GetRevisionAndParameterValuesByGroupMarkName")]
        public async Task<IActionResult> GetRevisionAndParameterValuesByGroupMarkName([FromBody] getRevAndParameterset getRevision)
        {
            string errorMessage = "";
            GroupMark groupMarkName = new GroupMark
            {
                ProjectId = getRevision.ProjectId,
                StructureElementTypeId = getRevision.SElement,
                SitProductTypeId = getRevision.ProductTypeId,
                GroupMarkingName = getRevision.GroupMarkingName

            };

            List<GetRevisionAndParamValuesDto> result = _copyGroupMarkingService.GetSourceParamValues(groupMarkName, out errorMessage);

            return Ok(result);

        }

        [HttpPost]
        [Route("/CopyGroupMarking/{productType}")]
        public async Task<IActionResult> CopyGroupMarking([FromBody] CopyGroupMarkDto copyGroupMarkDto,string productType)
        {
            string errorMessage = "";
            int destRevisionNo = 0;
            bool IsParameterSetCreationRequired = false;
            try
            {

                GroupMark groupMark = new GroupMark();

                groupMark.StructureElementTypeId = copyGroupMarkDto.STRUCTUREELEMENTTYPEID;
                groupMark.SitProductTypeId = copyGroupMarkDto.PRODUCTTYPEID;
                groupMark.WBSTypeId = copyGroupMarkDto.WBSTYPEID; 
                groupMark.CreatedUserId = copyGroupMarkDto.USERID;
                var result = _copyGroupMarkingService.CopyGroupMarking(groupMark,productType, copyGroupMarkDto.SOURCEPROJECTID,copyGroupMarkDto.DESTPROJECTID,copyGroupMarkDto.SOURCEPARAMETERSETID,copyGroupMarkDto.DESTPARAMETERSETID,copyGroupMarkDto.SOURCEGROUPMARKID,copyGroupMarkDto.DESTGROUPMARKNAME,copyGroupMarkDto.COPYFROM, copyGroupMarkDto.WBSELEMENTIDS, copyGroupMarkDto.IsParameterSetCreationRequired, copyGroupMarkDto.ISGROUPMARKREVISION, out errorMessage, out destRevisionNo);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPost]
        [Route("/CheckGroupmarkExist")]
        public async Task<IActionResult> CheckGroupmarkExist([FromBody] CheckGroupmarkExistDto checkGroupmarkExist) 
        {
            GroupMark groupMark = new GroupMark();
            var result = groupMark.CheckGroupmarkExists(checkGroupmarkExist);
            return Ok(result);
        }


        #endregion
        //test

    }


}
