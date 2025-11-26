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
using System.Drawing;

namespace DrainService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ESMCabBBSPostingController : ControllerBase
    {
        private readonly IDetailDal _detailDal;
        private readonly ICABDAL _cabDal;

        private readonly IBorePileParameterSetService _BoreParameterSet;

        private readonly IMapper _mapper;


        public ESMCabBBSPostingController(IMapper mapper, IDetailDal detailDal, IBorePileParameterSetService BoreParameterSet, ICABDAL cABDAL)
        {
            _detailDal = detailDal;
            _mapper = mapper;
            _BoreParameterSet = BoreParameterSet;
            _cabDal = cABDAL;

        }

        #region ESMBBSCABPOST


        [HttpGet]
        [Route("/LoadSOR/{ORD_REQ_NO}")]
        public async Task<IActionResult> ESM_BBSPostingCABSOR_Get(string ORD_REQ_NO)
        {
            string errorMessage = "";
            CABInfo cABInfo = new CABInfo
            {
                ORD_REQ_NO = ORD_REQ_NO,
            };

            var result = _cabDal.ESM_BBSPostingCABSOR_Get(cABInfo);

            return Ok(result);

        }

        //ShowWBSGrid
        [HttpGet]
        [Route("/LoadWBSBySorRange/{FROM_SOR}/{TO_SOR}")]
        public async Task<IActionResult> ESM_BBSPostingCAB_Get_Range(string FROM_SOR, string TO_SOR)
        {
            string errorMessage = "";
            CABInfo cABInfo = new CABInfo
            {
                FROM_SOR = FROM_SOR,
                TO_SOR = TO_SOR,
            };

            var result = _cabDal.ESM_BBSPostingCAB_Get_Range(cABInfo);

            return Ok(result);

        }

        //LoadWBS
        [HttpGet]
        [Route("/LoadWBS/{ORD_REQ_NO}")]
        public async Task<IActionResult> BBSPostingCAB_Get(string ORD_REQ_NO)
        {
            string errorMessage = "";
            CABInfo cABInfo = new CABInfo
            {
                ORD_REQ_NO = ORD_REQ_NO,
            };

            var result = _cabDal.BBSPostingCAB_Get(cABInfo);

            return Ok(result);

        }


        [HttpGet]
        [Route("/CheckDataDiscrepancy/{GroupMarkId}")]
        public async Task<IActionResult> BBSPostingGetDataDiscrepancy(int GroupMarkId)
        {
            string errorMessage = "";


            var result = _cabDal.BBSPostingGetDataDiscrepancy(GroupMarkId);

            return Ok(result);

        }

        [HttpPost]
        [Route("/BBSPostingCABGM_Get")]
        public async Task<IActionResult> BBSPostingCABGM_Get([FromBody] getBBSDto getBBS)
        {
            string errorMessage = "";
            CABInfo cABInfo = new CABInfo
            {
                intProjectId = getBBS.projectId,
                BBS_NO = getBBS.enteredText,
            };

            var result = _cabDal.BBSPostingCABGM_Get(cABInfo);

            return Ok(result);

        }

        [HttpPost]
        [Route("/BBSPostingCAB_Post")]
        public async Task<IActionResult> BBSPostingCAB_Post([FromBody] BBSCABPostDto obj )
        {
            string errorMessage = "";
            
            
            var result = _cabDal.BBSPostingCAB_Post(obj);

            return Ok(result);

        }

        [HttpPost]
        [Route("/BBSPostingCAB_Unpost")]
        public async Task<IActionResult> BBSPostingCAB_Unpost([FromBody] BBSCABUnPostDto bBSCABPost)
        {
            string errorMessage = "";
            CABInfo cABInfo = new CABInfo
            {
                intProjectId = bBSCABPost.IntProjectID,
                intWBSElementId = bBSCABPost.IntWBSElementID,
                intGroupMarkId = bBSCABPost.IntGroupMarkID,
                BBS_NO = bBSCABPost.BBS_NO,


            };

            var result = _cabDal.BBSPostingCAB_Unpost(cABInfo);

            return Ok(result);

        }


        [HttpPost]
        [Route("/ESM_BBSReleaseCAB_Insert")]
        public async Task<IActionResult> ESM_BBSReleaseCAB_Insert([FromBody] BBSCABReleaseDto obj)
        {
            string errorMessage = "";
           
            var result = _cabDal.ESM_BBSReleaseCAB_Insert(obj);

            return Ok(result);

        }


        [HttpPost]
        [Route("/GroupMarkIdCAB_Get")]
        public async Task<IActionResult> GroupMarkIdCAB_Get([FromBody]GroupMarkIdCABDto Obj)
        {
            string errorMessage = "";

            var result = _cabDal.GroupMarkIdCAB_Get(Obj);

            return Ok(result);

        }



        #endregion

        #region  New ESM Module

        [HttpGet]
        [Route("/GetColumnName/{tableName}")]
        public async Task<IActionResult> getColumnName(string tableName)
        {
            try
            {
                Response_New<List<string>> result = await _cabDal.getColumnName(tableName);
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("/GetEsmTrackingDetails/{TrackingNo}")]
        public async Task<IActionResult> GetEsmTrackingDetails(string TrackingNo)
        {
            try
            {
                Response_New<List<GetESMTrackingData_New>> result = await _cabDal.Get_Esm_TrackinDetails(TrackingNo);
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("/GetColumnName_ESM")]
        public async Task<IActionResult> GetColumnName_ESM()
        {
            try
            {
                Response_New<List<GetESMColumnName>> result = await _cabDal.getESMColumnName();
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("/GetCustomViews_ESM/{trackingno}")]
        public async Task<IActionResult> GetCustomViews_ESM(string trackingno)
        {
            try
            {
                Response_New<List<ESMCustomView>> result = await _cabDal.getESMCustomViews(trackingno);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("/AddCustomViews")]
        public async Task<IActionResult> AddCustomViews([FromBody] ESMCustomView obj)
        {
            try
            {
                Response_New<int> response = await _cabDal.UpdateEsmTracking_data(obj);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
        [HttpDelete]
        [Route("/DeleteEsmCustomView/{id}")]
        public async Task<IActionResult> DeleteEsmCustomView(int id)
        {
            try
            {
                Response_New<int> response = await _cabDal.DeleteCustomEsmTracking_data(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("/GetColumnName_ESMByIDs/{ColumnIDs}")]
        public async Task<IActionResult> GetColumnName_ESMByIDs(int ColumnIDs)
        {
            try
            {
                Response_New<string> result = await _cabDal.getESMColumnNameByIds(ColumnIDs);
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("/GetColumnName_ESMByIDs_string/{ColumnIDs}")]
        public async Task<IActionResult> GetColumnName_ESMByIDs(string ColumnIDs)
        {
            try
            {
                Response_New<List<GetESMColumnName>> result = await _cabDal.getESMColumnNameByIdsString(ColumnIDs);
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion

    }
}
