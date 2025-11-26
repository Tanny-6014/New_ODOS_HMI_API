using AutoMapper;
using ShapeCodeService.Dtos;
using ShapeCodeService.Interfaces;
using ShapeCodeService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using ShapeCodeService.Repositories;
using System.Text;

namespace ShapeCodeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShapeGroupController : ControllerBase
    {
        private readonly IShapeGroup _ShapeGroupRepository;
        private readonly IMapper _mapper;
        private readonly ICABDAL _cABDAL;
        public ShapeGroupController(IShapeGroup ShapeGroup, IMapper mapper,ICABDAL cABDAL)
        {
            _ShapeGroupRepository = ShapeGroup;
            _mapper = mapper;
            _cABDAL = cABDAL;

        }        
        [HttpGet]
        [Route("/GetShapegroupList")]
        public async Task<IActionResult> GetShapegroupList()
        {
            List<Shapegroup> shapegroups = await _ShapeGroupRepository.GetShapeGroupListAsync();
            var result = _mapper.Map<List<ShapegroupDto>>(shapegroups);
            return Ok(result);
        }

        [HttpPost]
        [Route("/AddShapegroup")]
        public async Task<IActionResult> AddShapegroup([FromBody] ShapegroupDto shapegroupdto)
        {
            Shapegroup shape_group = new Shapegroup();
            var shapegroupobj = _mapper.Map<Shapegroup>(shapegroupdto);
            if (shapegroupdto.Id == 0)
            { 
            var duplicateshaprgroup = await _ShapeGroupRepository.CheckduplicateShapeGroupAsync(shapegroupdto.ShapeGroupName);
            if (duplicateshaprgroup)
            {
                return BadRequest("Shape Group : " + shapegroupdto.ShapeGroupName + " is already exist!");
            }
           }
            shape_group = await  _ShapeGroupRepository.AddShapeGroupAsync(shapegroupobj);
            var result = _mapper.Map<ShapegroupDto>(shape_group);
            return Ok(result);
            
        }

        [HttpDelete]
        [Route("/deleteShapegroup/{id}")]
        public async Task<IActionResult> DeleteShapeGroupAsync(int id)
        {

            var shapegroup = await _ShapeGroupRepository.DeleteShapeGroupAsync(id);
            return Ok(new { Shapegroup = shapegroup, response = "success" });

        }

        #region AddedByVidhya

        [HttpGet]
        [Route("/PopulateShapes/{intCatalog}")]
        public async Task<IActionResult> GetExistingCABShapeDetails(Int16 intCatalog)
        {
            string errorMessage = "";


            var result = _cABDAL.GetExistingCABShapeDetails(intCatalog);

            return Ok(result);

        }

        [HttpGet]
        [Route("/PopulateCatalogs")]
        public async Task<IActionResult> GetCABShapeCatalogDetails()
        {
            string errorMessage = "";
            var result = _cABDAL.GetCABShapeCatalogDetails();

            return Ok(result);

        }

        [HttpGet]
        [Route("/PreviewImage/{StrShapeID}")]
        public  byte[] GetShapeImage(string StrShapeID)
        {
            string errorMessage = "";
            byte[] result = _cABDAL.GetShapeImage(StrShapeID);

            return result;

        }

        [HttpGet]
        [Route("/PreviewAllImage")]
        public async Task<IActionResult> GetAllShapeImage()
        {
            string errorMessage = "";
            var result = _cABDAL.GetAllShapeImage();

            return Ok(result);

        }


        [HttpGet]
        [Route("/GetShapeDetails/{StrShapeID}")]
        public async Task<IActionResult> GetShapeCoordinates(string StrShapeID)
        {
            string errorMessage = "";
            
            var result = _cABDAL.GetShapeCoordinates(StrShapeID);

            return Ok(result);

        }

        [HttpGet]
        [Route("/ShapeToEdit/{shapeEdit}")]
        public async Task<IActionResult> GetShapeEditable(string shapeEdit)
        {
            string errorMessage = "";
            CABDAL objCab = new CABDAL();
            var result = objCab.GetShapeEditable(shapeEdit);

            return Ok(result);

        }

        [HttpPost]
        [Route("/UpdateShapeParameters")]
        public async Task<IActionResult> UpdateShapeParameters([FromBody] UpdateShapeParamDto updateShapeParam)
        {
           
            var success = _cABDAL.UpdateShapeParameters(updateShapeParam);
            return Ok(success);

        }

        //[HttpPost]
        //[Route("/ImportImageToDB/{ShapeCode}")]
        //public async Task<IActionResult> ImportImage([FromForm] IFormFile file,string ShapeCode)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        if(memoryStream!=null)
        //        {
        //            file.CopyTo(memoryStream);
        //            ImportImageDto importImageDto = new ImportImageDto();
        //            importImageDto.imag = memoryStream.ToArray();
        //            importImageDto.shapeCode = ShapeCode;
        //            var success = _cABDAL.ImportImageToDB(importImageDto);
        //            return Ok(success);
        //        }
               
        //    }
          
           
        //}

        [HttpDelete]
        [Route("/DeleteShapeCode/{shapeCode}")]
        public async Task<IActionResult> DeleteShapeCode(string shapeCode)
        {
            string errorMessage = "";
           
            var result = _cABDAL.DeleteShapeCode(shapeCode);

            return Ok(result);

        }

        #region DrawShape

        [HttpPost]
        [Route("/InsertCabShapeMaster/{ShapeID}/{createdUser}/{description}")]
        public async Task<IActionResult> InsertCabShapeMaster(string ShapeID, string createdUser, string description)
        {
            string errorMessage = "";

            var result = _cABDAL.InsertCabShapeMaster(ShapeID, createdUser, description);

            return Ok(result);

        }

        [HttpPost]
        [Route("/InsertCabShapeDetails/{ShapeID}")]
        public async Task<IActionResult> InsertCabShapeDetails(string ShapeID)
        {
            string errorMessage = "";

            CABInfo cABInfo = new CABInfo
            {
                ShapeID = ShapeID,
            };

            var result = _cABDAL.InsertCabShapeDetails(cABInfo);

            return Ok(result);

        }

        [HttpPost]
        [Route("/InsertShapeGroupDetails")]
        public async Task<IActionResult> InsertShapeGroupDetails([FromBody] addShapeGrpDetailDto addShapeGrp)
        {
            string errorMessage = "";

            CABDAL cABDAL = new CABDAL();


            var result = cABDAL.InsertShapeGroupDetails(addShapeGrp);

            return Ok(result);

        }

        [HttpDelete]
        [Route("/DeleteCabShapeMaster/{ShapeID}")]
        public async Task<IActionResult> DeleteCabShapeMaster(string ShapeID)
        {
            string errorMessage = "";

            CABInfo cABInfo = new CABInfo
            {
                ShapeID = ShapeID
            };

            var result = _cABDAL.DeleteCabShapeMaster(cABInfo);

            return Ok(result);

        }

        [HttpDelete]
        [Route("/DeleteCabShapeDetails/{ShapeID}")]
        public async Task<IActionResult> DeleteCabShapeDetails(string ShapeID)
        {
            string errorMessage = "";

            CABInfo cABInfo = new CABInfo
            {
                ShapeID = ShapeID
            };

            var result = _cABDAL.DeleteCabShapeDetails(cABInfo);

            return Ok(result);

        }


        [HttpGet]
        [Route("/ShapeCodeDetails_Grid/{shapeId}")]
        public async Task<IActionResult> ShapeCodeDetails_Grid(string shapeId)
        {
            string errorMessage = "";
            CABDAL objCab = new CABDAL();
            var result = objCab.ShapeCodeDetails_Grid(shapeId);

            return Ok(result);

        }
        [HttpPost]
        [Route("/InsertCabShapeDetails")]
        public async Task<IActionResult> InsertCabShapeDetails([FromBody] List<GridListDto> addShapeDetails)
        {
            string errorMessage = "";

            CABDAL cABDAL = new CABDAL();


            var result = cABDAL.ShapeCodeDetails_Insert(addShapeDetails);

            return Ok(result);

        }
        [HttpGet]
        [Route("/CabShapeStatusChange/{ShapeID}/{Status}")]
        public async Task<IActionResult> CabShapeStatusChange(string ShapeID,int Status)
        {
            string errorMessage = "";
            try
            {
                var result = _cABDAL.CABShape_Status_Update(ShapeID, Status, out errorMessage);
                return Ok(result);
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return BadRequest(errorMessage);
            }
        }
        #endregion


        #endregion





    }
}
