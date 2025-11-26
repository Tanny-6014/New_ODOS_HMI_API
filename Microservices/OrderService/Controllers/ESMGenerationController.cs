using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using OrderService.Dtos;
using OrderService.Interfaces;
using OrderService.Models;
using OrderService.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ESMGenerationController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IESMOrderProcessingService _orderProcessingService;

        public ESMGenerationController(IMapper mapper, IESMOrderProcessingService eSMOrderProcessingService)
        {

            _mapper = mapper;
            _orderProcessingService = eSMOrderProcessingService;

        }


        //By vidhya for ESM Tracking 

        [HttpGet]
        [Route("/ESMTrackingDetailsGet/{structureElementTypeId}/{productTypeId}/{projectId}")]
        public async Task<IActionResult> GetESMTrackingDetails(int structureElementTypeId, int productTypeId, int projectId)
        {
            string errorMessage = "";

            //var result = _orderProcessingService.GetESMTrackingDetails(structureElementTypeId, productTypeId, projectId,out errorMessage);

            ESMTrackerGenerator adminInfo = new ESMTrackerGenerator
            {
                StructureElementTypeId = structureElementTypeId,
                ProductTypeId = productTypeId,
                ProjectId = projectId
            };
            List<ESMTrackerGenerator> listESMTrackerGenerator = _orderProcessingService.GetESMTrackingDetails(structureElementTypeId, productTypeId, projectId, out errorMessage);
            var result = listESMTrackerGenerator;
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetESMTrackingDetailsByTrackNum/{TrakingId}")]
        public async Task<IActionResult> GetESMTrackingDetailsByTrackNum(string TrakingId)
        {
            string errorMessage = "";

            //var result = _orderProcessingService.GetESMTrackingDetails(structureElementTypeId, productTypeId, projectId,out errorMessage);

            List<ESMTrackerGenerator> listESMTrackerGenerator = _orderProcessingService.GetESMTrackingDetailsByTrackNum(TrakingId, out errorMessage);
            var result = listESMTrackerGenerator;
            return Ok(result);
        }


        [HttpPost]
        [Route("/SaveESMTrackingDetails")]
        public async Task<IActionResult> SaveESMTrackingDetails([FromBody] SaveESMTrackingDetailsDto saveESMTrackingDetails)
        {
            string errorMessage = "";
            string Resultvalue = "";

            try
            {
                ESMTrackerGenerator eSMTrackerGenerator = new ESMTrackerGenerator();
                eSMTrackerGenerator.TrakingId = saveESMTrackingDetails.TrakingId;
                eSMTrackerGenerator.ProjectId = saveESMTrackingDetails.ProjectId;
                eSMTrackerGenerator.ContractNo = saveESMTrackingDetails.ContractNo;
                eSMTrackerGenerator.PONumber = saveESMTrackingDetails.PONumber;
                eSMTrackerGenerator.WBSElementId = saveESMTrackingDetails.WBSElementId;
                eSMTrackerGenerator.StructureElementTypeId = saveESMTrackingDetails.StructureElementTypeId;
                eSMTrackerGenerator.ProductTypeId = saveESMTrackingDetails.ProductTypeId;
                eSMTrackerGenerator.BBSNO = saveESMTrackingDetails.BBSNO;
                eSMTrackerGenerator.BBSSDesc = saveESMTrackingDetails.BBSSDesc;
                eSMTrackerGenerator.ReqDate = saveESMTrackingDetails.ReqDate.ToString();
                eSMTrackerGenerator.IntRemark = saveESMTrackingDetails.IntRemark;
                eSMTrackerGenerator.ExtRemark = saveESMTrackingDetails.ExtRemark;
                eSMTrackerGenerator.OrdDate = saveESMTrackingDetails.OrdDate.ToString();
                eSMTrackerGenerator.ProdDate = saveESMTrackingDetails.ProdDate.ToString();
                eSMTrackerGenerator.OrderType = saveESMTrackingDetails.OrderType;
                eSMTrackerGenerator.Location = saveESMTrackingDetails.Location;
                eSMTrackerGenerator.OverDelTolerance = saveESMTrackingDetails.OverDelTolerance;
                eSMTrackerGenerator.UnderDelTolerance = saveESMTrackingDetails.UnderDelTolerance;
                eSMTrackerGenerator.ContactPerson = saveESMTrackingDetails.ContactPerson;
                eSMTrackerGenerator.EstimatedWeight = saveESMTrackingDetails.EstimatedWeight.ToString();


                var result = _orderProcessingService.SaveESMTrackingDetails(saveESMTrackingDetails, out Resultvalue, out errorMessage);

                return Ok(result);
            }

            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
                throw ex;

            }


        }



        [HttpPost]
        [Route("/UpdateESMTrackingDetails")]
        public async Task<IActionResult> UpdateESMTrackingDetails([FromBody] SaveESMTrackingDetailsDto saveESMTrackingDetails)
        {
            string errorMessage = "";
            string Resultvalue = "";

            try
            {
                ESMTrackerGenerator eSMTrackerGenerator = new ESMTrackerGenerator();
                eSMTrackerGenerator.TrakingId = saveESMTrackingDetails.TrakingId;
                eSMTrackerGenerator.ProjectId = saveESMTrackingDetails.ProjectId;
                eSMTrackerGenerator.ContractNo = saveESMTrackingDetails.ContractNo;
                eSMTrackerGenerator.PONumber = saveESMTrackingDetails.PONumber;
                eSMTrackerGenerator.WBSElementId = saveESMTrackingDetails.WBSElementId;
                eSMTrackerGenerator.StructureElementTypeId = saveESMTrackingDetails.StructureElementTypeId;
                eSMTrackerGenerator.ProductTypeId = saveESMTrackingDetails.ProductTypeId;
                eSMTrackerGenerator.BBSNO = saveESMTrackingDetails.BBSNO;
                eSMTrackerGenerator.BBSSDesc = saveESMTrackingDetails.BBSSDesc;
                eSMTrackerGenerator.ReqDate = saveESMTrackingDetails.ReqDate.ToString();
                eSMTrackerGenerator.IntRemark = saveESMTrackingDetails.IntRemark;
                eSMTrackerGenerator.ExtRemark = saveESMTrackingDetails.ExtRemark;
                eSMTrackerGenerator.OrdDate = saveESMTrackingDetails.OrdDate.ToString();
                eSMTrackerGenerator.ProdDate = saveESMTrackingDetails.ProdDate.ToString();
                eSMTrackerGenerator.OrderType = saveESMTrackingDetails.OrderType;
                eSMTrackerGenerator.Location = saveESMTrackingDetails.Location;
                eSMTrackerGenerator.OverDelTolerance = saveESMTrackingDetails.OverDelTolerance;
                eSMTrackerGenerator.UnderDelTolerance = saveESMTrackingDetails.UnderDelTolerance;
                eSMTrackerGenerator.ContactPerson = saveESMTrackingDetails.ContactPerson;

                eSMTrackerGenerator.EstimatedWeight = saveESMTrackingDetails.EstimatedWeight.ToString();


                var result = _orderProcessingService.UpdateESMTrackingDetails(saveESMTrackingDetails, out Resultvalue, out errorMessage);
                return Ok(result);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }


    }


}
