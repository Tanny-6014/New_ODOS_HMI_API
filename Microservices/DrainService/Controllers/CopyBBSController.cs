using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using DrainService.Repositories;
using DrainService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using AutoMapper;
using DrainService.Interfaces;

namespace DrainService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CopyBBSController : ControllerBase
    {
        private readonly IDetailDal _detailDal;
        private readonly ICABDAL _cabDal;

        private readonly IBorePileParameterSetService _BoreParameterSet;

        private readonly IMapper _mapper;


        public CopyBBSController(IMapper mapper, IDetailDal detailDal, IBorePileParameterSetService BoreParameterSet, ICABDAL cABDAL)
        {
            _detailDal = detailDal;
            _mapper = mapper;
            _BoreParameterSet = BoreParameterSet;
            _cabDal = cABDAL;

        }

        //load sor

        [HttpGet]
        [Route("/LoadSOR_CopyBBS/{ORD_REQ_NO}")]
        public async Task<IActionResult>BBSPostingCABSOR_Get(string ORD_REQ_NO)
        {
            string errorMessage = "";
            CABInfo cABInfo = new CABInfo
            {
                ORD_REQ_NO = ORD_REQ_NO,
            };

            var result = _cabDal.BBSPostingCABSOR_Get(cABInfo);

            return Ok(result);

        }

        //load BBS
        [HttpPost]
        [Route("/LoadBBS_Copybbs")]
        public async Task<IActionResult> GetBBS([FromBody] getBBSDto getBBSDto)
        {
            string errorMessage = "";
            OESCABItem oESCAB = new OESCABItem();



           var result = oESCAB.GetBBS(getBBSDto.projectId, getBBSDto.enteredText);

            return Ok(result);

        }


        //Show Grid
        [HttpGet]
        [Route("/GetBBSPostingCABRange/{fromSor}/{toSor}")]
        public async Task<IActionResult> GetBBSPostingCABRange(string fromSor, string toSor)
        {
            string errorMessage = "";
            OESCABItem oESCABItem=new OESCABItem();
           
            var result = oESCABItem.GetBBSPostingCABRange(fromSor, toSor);

            return Ok(result);

        }


        [HttpPost]
        [Route("/GetGroupMarkID_CopyBBS")]
        public async Task<IActionResult> GroupMarkIdCAB_Get([FromBody] GetGMId_CopyBBSdto Obj)
        {
            string errorMessage = "";

            OESCABItem oESCABItem = new OESCABItem();

            var result = oESCABItem.GetGroupMarkID_CopyBBS(Obj);

            return Ok(result);

        }


        [HttpPost]
        [Route("/CheckBbsSource")]
        public async Task<IActionResult> CheckBbsSource([FromBody] getBBSDto getBBS)
        {
            string errorMessage = "";
            OESCABItem oESCABItem = new OESCABItem();

            var result = oESCABItem.CheckBbsSource(getBBS.enteredText);

            return Ok(result);

        }

        [HttpPost]
        [Route("/GetCopyBBSUID")]
        public async Task<IActionResult> GetCopyBBSUID([FromBody] getcopybbs getcopybbs)
        {
            string errorMessage = "";

            OESCABItem oESCABItem = new OESCABItem();

            var result = oESCABItem.GetCopyBBSUID(getcopybbs.bbsSource, getcopybbs.bbsTarget, getcopybbs.wbsId, getcopybbs.vchStructureElementType, getcopybbs.pojId);

            return Ok(result);

        }

        [HttpGet]
        [Route("/InsertProductMarkCopyBBS/{seIdSource}/{seIdTarget}/{groupMarkId}")]
        public async Task<IActionResult> InsertProductMarkCopyBBS(int seIdSource, int seIdTarget, int groupMarkId)
        {
            string errorMessage = "";

            OESCABItem oESCABItem = new OESCABItem();

            var result = oESCABItem.InsertProductMarkCopyBBS(seIdSource, seIdTarget, groupMarkId);

            return Ok(result);

        }

        [HttpPost]
        [Route("/InsertTransdetailsCopyBBS")]
        public async Task<IActionResult> InsertTransdetailsCopyBBS([FromBody] InsertTransdetailsDto dto)
        {
            string errorMessage = "";

            OESCABItem oESCABItem = new OESCABItem();

            var result = oESCABItem.InsertTransdetailsCopyBBS(dto.sourceTransId, dto.seIdTarget);

            return Ok(result);

        }


        [HttpGet]
        [Route("/GetAccessoryCopyBBS/{seIdSource}")]
        public async Task<IActionResult> GetAccessoryCopyBBS(int seIdSource)
        {
            string errorMessage = "";

            OESCABItem oESCABItem = new OESCABItem();

            var result = oESCABItem.GetAccessoryCopyBBS(seIdSource);

            return Ok(result);

        }

        [HttpPost]
        [Route("/InsertAccessoryCopyBBS")]
        public async Task<IActionResult> InsertAccessoryCopyBBS([FromBody] InsertAcSCopyBBSDto acSCopyBBSDto)
        {
            string errorMessage = "";

            OESCABItem oESCABItem = new OESCABItem();

            var result = oESCABItem.InsertAccessoryCopyBBS(acSCopyBBSDto);

            return Ok(result);

        }


    }   
}
