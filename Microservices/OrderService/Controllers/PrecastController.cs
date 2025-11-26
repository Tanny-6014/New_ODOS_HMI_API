using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Interfaces;
using OrderService.Dtos;
using System.Data.SqlClient;
using OrderService.Repositories;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrecastController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPrecastService _precastService;

        public PrecastController(IMapper mapper, IPrecastService precast )
        {

            _mapper = mapper;
            _precastService = precast;

        }

        #region Precast

        [HttpGet]
        [Route("/GetPrecastDetails")]
        public async Task<IActionResult> GetPrecastDetails()
        {
            string errorMessage = "";

            //var result = _orderProcessingService.GetESMTrackingDetails(structureElementTypeId, productTypeId, projectId,out errorMessage);

            var result = _precastService.GetPrecastDetails();

            return Ok(result);
        }

        [HttpPost]
        [Route("/InsertPrecastDetails")]
        public async Task<IActionResult> InsertPrecastDetails([FromBody] getPrecastDto getPrecastDto)
        {
            string errorMessage = "";

            var result = _precastService.InsertPrecastDetails(getPrecastDto);

            return Ok(result);
        }

        [HttpPost]
        [Route("/UpdatePrecastDetails")]
        public async Task<IActionResult> UpdatePrecastDetails([FromBody] getPrecastDto getPrecastDto)
        {
            string errorMessage = "";

            var result = _precastService.UpdatePrecastDetails(getPrecastDto);

            return Ok(result);
        }

        [HttpGet]

        [Route("/UpdatePrecastFlag/{PostHeaderID}/{Flag}")]

        public async Task<IActionResult> UpdatePrecastFlag(int PostHeaderID, int Flag)

        {

            string errorMessage = "";

            var result = _precastService.UpdatePrecastFlag(PostHeaderID, Flag);

            return Ok(result);

        }

        [HttpDelete]

        [Route("/DeletePrecastDetailsById/{precastId}")]

        public IActionResult DeletePrecastDetailsById(int precastId)

        {

            try

            {

                bool isDeleted = _precastService.DeletePrecastDetailsById(precastId);



                if (isDeleted)

                {

                    return Ok(true);

                }

                else

                {

                    return NotFound(false);

                }

            }

            catch (Exception ex)

            {

                return BadRequest();

            }

        }



        #endregion


        #region BarShapecode

        [HttpGet]
        [Route("/GetDistinctBarShapeCodes")]
        public async Task<IActionResult> GetDistinctBarShapeCodes()
        {
            string errorMessage = "";

            //var result = _orderProcessingService.GetESMTrackingDetails(structureElementTypeId, productTypeId, projectId,out errorMessage);

            var result = _precastService.GetDistinctBarShapeCodes();

            return Ok(result);
        }

        [HttpPost]
        [Route("/UpdateBarShapeCodeDetails")]
        public async Task<IActionResult> UpdateBarShapeCodeDetails([FromBody] getBarShapeCodeDto getBarShape)

        {
            string errorMessage = "";

            var result = _precastService.UpdateBarShapeCodeDetails(getBarShape);

            return Ok(result);
        }

        #endregion


        [HttpGet]
        [Route("/GetIsPrecast/{customerCode}/{projectCode}")]
        public async Task<ActionResult<bool>> GetIsPrecast(string customerCode, string projectCode)
        {
            try
            {
                var result = await _precastService.GetIsPrecast(customerCode, projectCode);
                if (result.HasValue)
                    return Ok(result.Value);
                else
                {
                    return Ok(false);
                }
            }
            catch(Exception ex) {

                return BadRequest(ex.Message);
                    
                    }
         
            
        }


    }
}
