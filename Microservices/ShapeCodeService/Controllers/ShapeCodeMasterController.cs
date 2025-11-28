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
    [Route("api/[controller]")]
    [ApiController]
    public class ShapeCodeMasterController : ControllerBase
    {
        private readonly IShapeGroup _ShapeGroupRepository;
        private readonly IMapper _mapper;
        private readonly ICABDAL _cABDAL;
        private readonly IAdminDal _adminDal;
        public ShapeCodeMasterController(IShapeGroup ShapeGroup, IMapper mapper, ICABDAL cABDAL, IAdminDal adminDal)
        {
            _ShapeGroupRepository = ShapeGroup;
            _mapper = mapper;
            _cABDAL = cABDAL;
            _adminDal = adminDal;
        }

        [HttpGet]
        [Route("/PopulateShapeCode")]
        public async Task<IActionResult> GetShape()
        {
            List<getShapeDetailsDto> getShapes = _adminDal.GetShape();
            return Ok(getShapes);
        }

        [HttpGet]
        [Route("/PreviewAllImage_MSH")]
        public async Task<IActionResult> GetAllShapeImage()
        {
            string errorMessage = "";
            AdminDal adminDal = new AdminDal();
            var result = adminDal.GetAllShapeImage();

            return Ok(result);

        }
        [HttpGet]
        [Route("/PopulateShapeParamDetails/{ShapeId}")]
        public async Task<IActionResult> GetShapeParamDetails(int ShapeId)
        {
            List<GetShapeParamDetailsDto> getShapes = _adminDal.GetShapeParamDetails(ShapeId);
            return Ok(getShapes);
        }

        [HttpGet]
        [Route("/GetStatusDetails")]
        public async Task<IActionResult> GetStatusDetails()
        {
            List<StatusDetailsDto>getShapeDetails = _adminDal.GetStatusDetails();
            return Ok(getShapeDetails);
        }

        [HttpGet]
        [Route("/GetShapeCodeDetails/{shapeid}")]
        public async Task<IActionResult> GetShapeCodeDetails(int shapeid)
        {
            List<ChkShapeExistDto> shapeCodeDetails = _adminDal.GetShapeCodeDetails(shapeid);
            return Ok(shapeCodeDetails);
        }
        [HttpGet]
        [Route("/CheckShapeExists/{ShapeCode}")]
        public async Task<IActionResult> CheckShapeExists(string ShapeCode)
        {
            AdminDal adminDal = new AdminDal();
            List<ChkShapeExistDto> chkShapeExists = adminDal.CheckShapeExists(ShapeCode);
            return Ok(chkShapeExists);
        }
        [HttpGet]
        [Route("/CheckMeshShapeGroupExists/{meshShapeGroup}")]
        public async Task<IActionResult> CheckMeshShapeGroupExists(string meshShapeGroup)
        {
            AdminDal adminDal = new AdminDal();
            List<ChkShapeExistDto> chkShapeExists = adminDal.CheckMeshShapeGroupExists(meshShapeGroup);
            return Ok(chkShapeExists);
        }
        [HttpPost]
        [Route("/InsUpdShapeParamDetails/{UserId}")]
        public async Task<IActionResult> InsUpdShapeParamDetails(int UserId,[FromBody] GetShapeParamDetailsDto getShapeParamDetails)
        {
            var isInsert = _adminDal.InsUpdShapeParamDetails(UserId,getShapeParamDetails);
            return Ok(isInsert);
        }

        [HttpDelete]
        [Route("/DeleteShapeParamDetails/{ShapeDetailsId}")]
        public async Task<IActionResult> DeleteShapeParamDetails(int ShapeDetailsId)
        {
            var isdelete = _adminDal.DeleteShapeParamDetails(ShapeDetailsId);
            return Ok(isdelete);
        }

        [HttpPost]
        [Route("/InsUpdShapeHeaderDetails/{UserID}")]
        public async Task<IActionResult> InsUpdShapeHeaderDetails(int UserID,[FromBody] InsUpdShapeHeaderDto insUpdShapeHeader)
        {
            var isInsert = _adminDal.InsUpdShapeHeaderDetails(UserID, insUpdShapeHeader);
            return Ok(isInsert);
        }

        [HttpPost]
        [Route("/UpdateShapeHeaderDetails/{UserID}")]
        public async Task<IActionResult> UpdateShapeHeaderDetails(int UserID, [FromBody] UpdShapeHeaderDto UpdShapeHeader)
        {
            var isInsert = _adminDal.UpdateShapeHeaderDetails(UserID ,UpdShapeHeader);
            return Ok(isInsert);
        }


        #region Add Validation 

        [HttpGet]
        [Route("/GetAttributes/{shapeid}")]
        public async Task<IActionResult> GetAttributes(int shapeid)
        {
            AdminDal adminDal = new AdminDal();
            List<GetAttributeDto> chkShapeExists = adminDal.GetAttributes(shapeid);
            return Ok(chkShapeExists);
        }

        [HttpGet]
        [Route("/GetValidationConstraintList/{shapeid}")]
        public async Task<IActionResult> GetValidationConstraint(int shapeid)
        {
            AdminDal adminDal = new AdminDal();
            List<GetAttributeDto> chkShapeExists = adminDal.GetValidationConstraint(shapeid);
            return Ok(chkShapeExists);
        }

        [HttpPost]
        [Route("/InsertIpOpValidationConstraints")]
        public async Task<IActionResult> InsertIpOpValidationConstraints([FromBody] GetAttributeDto getAttribute)
        {
            AdminDal adminDal = new AdminDal();
            var isInsert = adminDal.InsertIpOpValidationConstraints(getAttribute);
            return Ok(isInsert);
        }

        [HttpPost]
        [Route("/UpdateIpOpValidationConstraints")]
        public async Task<IActionResult> UpdateIpOpValidationConstraints([FromBody] GetAttributeDto getAttribute)
        {
            AdminDal adminDal = new AdminDal();
            var isInsert = adminDal.UpdateIpOpValidationConstraints(getAttribute);
            return Ok(isInsert);
        }

        [HttpDelete]
        [Route("/DeleteIpOpValidationConstraints/{Id}")]
        public async Task<IActionResult> DeleteIpOpValidationConstraints(int Id)
        {
            AdminDal adminDal = new AdminDal();
            var isdelete = adminDal.DeleteIpOpValidationConstraints(Id);
            return Ok(isdelete);
        }
        #endregion


        #region Add Product MArking

        [HttpGet]
        [Route("/GetFormulaList/{shapeid}")]
        public async Task<IActionResult> GetProductMArkingFormulas(int shapeid)
        {
            AdminDal adminDal = new AdminDal();
            List<getFormulasDto>getFormulas = adminDal.GetProductMArkingFormulas(shapeid);
            return Ok(getFormulas);
        }

        [HttpGet]
        [Route("/GetProductMArkingFormulasById/{structele}")]
        public async Task<IActionResult> GetProductMArkingFormulasById(string structele)
        {
            AdminDal adminDal = new AdminDal();
            List<getFormulasDto> getFormulas = adminDal.GetProductMArkingFormulasById(structele);
            return Ok(getFormulas);
        }
        [HttpGet]
        [Route("/GetLibraryId/{structele}/{FormulaName}")]
        public async Task<IActionResult> GetLibraryId(string structele,string FormulaName)
        {
            AdminDal adminDal = new AdminDal();
            List<getFormulasDto> getFormulas = adminDal.GetLibraryId(structele, FormulaName);
            return Ok(getFormulas);
        }

        [HttpPost]
        [Route("/InsertProductMArkingFormula")]
        public async Task<IActionResult> InsertProductMArkingFormula([FromBody] getFormulasDto getFormulas)
        {
            AdminDal adminDal = new AdminDal();
            var isInsert = adminDal.InsertProductMArkingFormula(getFormulas);
            return Ok(isInsert);
        }

        [HttpPost]
        [Route("/UpdateProductMArkingFormula")]
        public async Task<IActionResult> UpdateProductMArkingFormula([FromBody] getFormulasDto getFormulas)
        {
            AdminDal adminDal = new AdminDal();
            var isInsert = adminDal.UpdateProductMArkingFormula(getFormulas);
            return Ok(isInsert);
        }


        #endregion

        //create directory
        [HttpPost]
        [Route("/CheckandCreateDirectory")]
        public IActionResult CheckOrCreateDirectory(createpathDto createpath)
        {
            if (string.IsNullOrEmpty(createpath.Path))
            {
                return BadRequest("Directory path is required.");
            }

            try

            {
                string decodedPath = Uri.UnescapeDataString(createpath.Path);

                // Replace '/' with '\\' for Windows file system compatibility
                //string correctedPath = decodedPath.Replace("/", "\\");
                if (!Directory.Exists(decodedPath))
                {
                    Directory.CreateDirectory(decodedPath);
                    //return Ok("Directory created successfully.");
                    return Ok(1);

                }
                else
                {
                    //return Ok("Directory already exists.");
                    return Ok(2);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //move file 

        [HttpPost]
        [Route("/CheckfileExist/")]
        public IActionResult MoveFile([FromBody] FileMoveRequest request)
        {
            if (string.IsNullOrEmpty(request.strdirPath_tcx) || string.IsNullOrEmpty(request.strExsitingdirPath_tcx))
            {
                return BadRequest("Source and destination paths are required.");
            }

            try
            {
                var filePath = request.strdirPath_tcx;
                Console.WriteLine($"Checking if file exists at: {filePath}");

                if (System.IO.File.Exists(request.strdirPath_tcx))
                {

                    var fileName = Path.GetFileName(request.strdirPath_tcx);

                    // Ensure the destination directory exists
                    var destinationDirectory = request.strExsitingdirPath_tcx;
                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                    }

                    // Combine destination path with the file name
                    var destinationFilePath = Path.Combine(request.strExsitingdirPath_tcx, fileName);

                    // Move the file
                    System.IO.File.Move(request.strdirPath_tcx, destinationFilePath);

                    //tcxFile.SaveAs(strdirPath_tcx);

                    return Ok(1);
                }
                else
                {
                    return Ok(2);
                }
              
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
      //test
    }
    

}
