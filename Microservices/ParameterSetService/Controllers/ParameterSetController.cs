using AutoMapper;
using ParameterSetService.Dtos;
using ParameterSetService.Interfaces;
using ParameterSetService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace ParameterSetService.Controllers
{
    
    public class ParameterSetController : ControllerBase
    {
   
        private readonly IParameterSet ParameterSetRepository;
        private readonly IMapper _mapper;
        private object ParameterSets;

        public ParameterSetController(IParameterSet parameterSet, IMapper mapper)
        {
            ParameterSetRepository = parameterSet;
              _mapper = mapper;

        }


        [HttpPost]
        [Route("/CommonParamterInsert")]
        public async Task<IActionResult> CommonParamterInsert([FromBody] AddParamDto addParameter)
        {
            try
            {
                var result = await ParameterSetRepository.CommonParamterInsert(addParameter);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }



        [HttpGet]
        [Route("/GetCommonParameterSet_Dropdown/{projectId}")]
        public async Task<IActionResult> GetParameterSet_Dropdown(int projectId)
        {
            try
            {
                var result = await ParameterSetRepository.GetParameterSetAsync(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }




        #region Mesh Parameter        

        [HttpGet]
        [Route("/GetMeshProductCode_Dropdown/{projectId}")]
        public async Task<IActionResult> GetMeshProductCode_Dropdown()
        {
            try
            {
                var result = await ParameterSetRepository.GetMeshProductCodeAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        // start added by vidya
        [HttpGet]
        [Route("/GetMeshList/{projectId}/{parameternumber}")]
        public async Task<IActionResult> GetMeshList(int projectId, int parameternumber)
        {
            try
            {
                IEnumerable<MeshListing> meshListingdtos = await ParameterSetRepository.GetMeshListAsync(projectId, parameternumber);
                var result = _mapper.Map<List<MeshListing>>(meshListingdtos);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("/GetWallList/{projectId}/{parameternumber}")]
        public async Task<IActionResult> GetWallList(int projectId, int parameternumber)
        {
            try
            {
                IEnumerable<MeshListing> meshListingdtos = await ParameterSetRepository.GetWallListAsync(projectId, parameternumber);
                var result = _mapper.Map<List<MeshListing>>(meshListingdtos);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpDelete]
        [Route("/DeleteMesh/{id}")]
        public async Task<IActionResult> DeleteMeshAsync(int id)
        {

            int result = await ParameterSetRepository.DeleteMesh(id);
            return Ok(result);

        }
        // END

        [HttpPost]
        [Route("/AddMeshProjectParamLap")]
        public async Task<IActionResult> MeshProjectParamLap([FromBody] AddMeshParamaterDto AddMeshParamaterDto)
        {
            try
            {
                int result = await ParameterSetRepository.MeshProjectParamLap(AddMeshParamaterDto);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPost]
        [Route("/UpdateMeshParamLap")]
        public async Task<IActionResult> UpdateMeshParamLap([FromBody] AddMeshParamaterDto AddMeshParamaterDto)
        {

            try
            {
                int result = await ParameterSetRepository.UpdateMeshParamLap(AddMeshParamaterDto);   
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        #endregion


        

        #region Column Parameter

        //added by vidhya 
        [HttpGet]
        [Route("/GetColumnList/{projectId}/{parameternumber}")]
        public async Task<IActionResult> GetColumnList(int projectId, int parameternumber)
        {
            try
            {
                IEnumerable<ColumnListingDto> columnListingDtos = await ParameterSetRepository.GetColumnListAsync(projectId, parameternumber);
                var result = _mapper.Map<List<ColumnListingDto>>(columnListingDtos);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        //end


        [HttpGet]
        [Route("/GetColumnParameterSet_Dropdown/{projectId}")]
        public async Task<IActionResult> GetColumnParameterSet_Dropdown(int projectId)
        {
            try
            {
                var result = await ParameterSetRepository.GetColumnParameterSetAsync(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpGet]
        [Route("/GetClinkLineItem/{projectId}/{parameternumber}")]
        public async Task<IActionResult> GetClinkLineItem(int projectId, int parameternumber)
        {
            try
            {
                IEnumerable<AddClinkLineDto> ClinkLineDto = await ParameterSetRepository.GetClinkLineItemAsync(projectId, parameternumber);
                var result = _mapper.Map<List<AddClinkLineDto>>(ClinkLineDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Route("/SaveColumnParameterSet")]
        public async Task<IActionResult> SaveColumnParameter([FromBody] AddColumnParameterSet addColumnParameterSet)
        {
            try
            {
                int result = await ParameterSetRepository.SaveColumnParameter(addColumnParameterSet);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPost]
        [Route("/DeleteCommonParamter")]
        public async Task<IActionResult> DeleteCommonParamter([FromBody] AddParamDto Deleteparamset)
        {
            try
            {
                int result = await ParameterSetRepository.DeleteCommonParamterInsert(Deleteparamset);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPost]
        [Route("/SaveClinkLineItem")]
        public async Task<IActionResult> SaveClinkLineItem([FromBody] AddClinkLineDto addClinkLineDto)
        {
            try
            {
                int result = await ParameterSetRepository.SaveClinkLineItem(addClinkLineDto);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }
        

        [HttpDelete]
        [Route("/DeleteColumnClinkItem/{id}")]
        public async Task<IActionResult> DeleteShapeGroupAsync(int id)
        {

            int result = await ParameterSetRepository.DeleteClinkLineItem(id);
            return Ok(result);

        }


        [HttpPost]
        [Route("/SaveCappingItem")]
        public async Task<IActionResult> SaveCappingItem([FromBody] AddCappingLineDto cappingLineDto)
        {
            try
            {
                int result = await ParameterSetRepository.SaveCappingLineItem(cappingLineDto);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        #endregion


        #region Beam Parameter
        //added by ajit

        [HttpPost]
        [Route("/SaveBeamParameterSet")]
        public async Task<IActionResult> SaveBeamParameter([FromBody] AddBeamParamterSetDto addBeamParameterSet)
        {
            try
            {
                int result = await ParameterSetRepository.SaveBeamParameter(addBeamParameterSet);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        [HttpGet]
        [Route("/GetBeamParameterSet_Dropdown/{projectId}")]
        public async Task<IActionResult> GetBeamParameterSet_Dropdown(int projectId)
        {
            try
            {
                var result = await ParameterSetRepository.GetBeamParameterSetAsync(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        // start added by vidya

        [HttpGet]
        [Route("/GetBeamList/{projectId}/{parameternumber}")]
        public async Task<IActionResult> GetBeamList(int projectId, int parameternumber)
        {
            try
            {
                IEnumerable<BeamListingDto> beamListingDtos = await ParameterSetRepository.GetBeamParamterTableListAsync(projectId, parameternumber);
                var result = _mapper.Map<List<BeamListingDto>>(beamListingDtos);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("/GetCappingLineItem/{projectId}/{parameternumber}")]
        public async Task<IActionResult> GetCappingLine(int projectId, int parameternumber)
        {
            try
            {
                IEnumerable<CappingLineDto> cappingLineDtos = await ParameterSetRepository.GetCappingLineItemAsync(projectId, parameternumber);
                var result = _mapper.Map<List<CappingLineDto>>(cappingLineDtos);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpDelete]
        [Route("/DeleteCappingItem/{id}")]        public async Task<IActionResult> DeleteCappingItem(int id)
        {

            int result = await ParameterSetRepository.DeleteCappingLineItem(id);
            return Ok(result);

        }


        [HttpGet]
        [Route("/GetCappingProductList/{cappingproduct}")]
        public async Task<IActionResult> GetCappingProductCodeListAsync(string cappingproduct)
        {
            try
            {
                IEnumerable<CappigProductListDto> cappingproductDtos = await ParameterSetRepository.GetCappingProductCodeListAsync(cappingproduct);
                var result = _mapper.Map<List<CappigProductListDto>>(cappingproductDtos);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }




        #endregion



        [HttpGet]
        [Route("/Get_TransportMode_Test")]
        public async Task<IActionResult> GetTransportMode()
        {
            try
            {
                var result =  ParameterSetRepository.Get_TransportMode();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

    }
}
