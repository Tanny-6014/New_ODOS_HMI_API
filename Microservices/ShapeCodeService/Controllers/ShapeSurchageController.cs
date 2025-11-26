using AutoMapper;
using ShapeCodeService.Dtos;
using ShapeCodeService.Interfaces;
using ShapeCodeService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Linq;

namespace ShapeCodeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShapeSurchageController : ControllerBase
    {
        private readonly IShapeSurchage shapeSurchageRepository;
        private readonly IMapper _mapper;
        public ShapeSurchageController(IShapeSurchage surchageObj, IMapper mapper)
        {
            shapeSurchageRepository = surchageObj;
            _mapper = mapper;

        }

        [HttpGet]
        [Route("/GetShapeSurchageList")]
        public async Task<IActionResult> GetShapeSurchageList()
        {
            List<Shapesurchage> shapesurchages = await shapeSurchageRepository.GetShapeSurchageListAsync();
            var result = _mapper.Map<List<ShapeSurchageDto>>(shapesurchages);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetShapeCodesDropdown")]
        public async Task<IActionResult> GetShapeCodesDropdown()
        {
            List<ShapeCodes> shapeCodes = await shapeSurchageRepository.GetShapeCodesAsync();
            var result = _mapper.Map<List<ShapeCodesDto>>(shapeCodes);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetSurchargesDropdown")]
        public async Task<IActionResult> GetSurchargesDropdown()
        {
            IEnumerable<SurchargeDropdown> surchargelist = await shapeSurchageRepository.GetSurchargesAsync();
            var result = _mapper.Map<List<SurchargeDropdownDto>>(surchargelist);
            return Ok(result);
        }

        [HttpPost]
        [Route("/AddShapeSurchage")]
        public async Task<IActionResult> AddShapeSurchage([FromBody] ShapeSurchageDto[] shapeSurchageDto)
        {
            //   var surchobj = _mapper.Map<Shapesurchage>(shapeSurchageDto);

            List<string> duplicateshapecode = new List<string>();
            List<string> newlyaddedshapecode = new List<string>();
            List<Shapesurchage> shapesurchageListobj = new List<Shapesurchage>();

            for (int i = 0; i < shapeSurchageDto.Length; i++)
            {
                Shapesurchage shapesurchage = new Shapesurchage();
                shapesurchage.IDENTITYNO = shapeSurchageDto[i].ID;
                shapesurchage.INTSHAPECODE = shapeSurchageDto[i].ShapeCode_Id;
                shapesurchage.CHRSHAPECODE = shapeSurchageDto[i].ShapeCode;
                shapesurchage.BARDIA = shapeSurchageDto[i].Bar_Dia;
                shapesurchage.INVLEN = shapeSurchageDto[i].Invoice_Length;
                shapesurchage.CHRSURCHARGE = shapeSurchageDto[i].Surcharge;
                shapesurchage.INTSURCHARGECODE = shapeSurchageDto[i].Surchage_Code;
                shapesurchage.INTCONDITION = shapeSurchageDto[i].Condition_Id;
                shapesurchage.CHRCONDITION = shapeSurchageDto[i].Dia_Condition;
                shapesurchage.Updated_By = shapeSurchageDto[i].User_Id;
                shapesurchage.Updated_Date = DateTime.Now; // Convert.ToDateTime(shapeSurchageDto[i].Updated_Date);
                var shaprgroup = await shapeSurchageRepository.CheckduplicateShapeGroupAsync(shapesurchage.CHRSHAPECODE);

                //duplicateshaprgroup = duplicateshaprgroup +' '+ shaprgroup;
                if (!shaprgroup)
                {
                    shapesurchageListobj.Add(shapesurchage);
                    newlyaddedshapecode.Add(shapesurchage.CHRSHAPECODE.Trim());
                }
                else
                {
                    duplicateshapecode.Add(shapesurchage.CHRSHAPECODE.Trim());
                }

            }
            var result = await shapeSurchageRepository.AddShapeSurchageAsync(shapesurchageListobj);
            if (duplicateshapecode.Count > 0 && newlyaddedshapecode.Count > 0)
            {
                return BadRequest(string.Join(",", duplicateshapecode) + ":" + string.Join(",", newlyaddedshapecode));
            }
            else if (duplicateshapecode.Count > 0)
            {
                return BadRequest(":" + string.Join(",", duplicateshapecode));
            }
            if (newlyaddedshapecode.Count > 0)
            {
                var newaddedshape = string.Join(",", newlyaddedshapecode);
                 //return BadRequest(newaddedshape + " added successfully");
                return Ok(result);
            }
            //var result = _mapper.Map<ShapeSurchageDto>(shapesurchageListobj);
            
            return Ok(result);

        }

        [HttpPost]
        [Route("/UpdateShapeSurchage")]
        public async Task<IActionResult> UpdateShapeSurchage([FromBody] ShapeSurchageDto shapeSurchagedto)
        {
            try
            {
                Shapesurchage Shape_surchage = new Shapesurchage();
                shapeSurchagedto.Updated_Date = DateTime.Now.ToString();
                var Shapesurchageobj = _mapper.Map<Shapesurchage>(shapeSurchagedto);

                Shape_surchage = await shapeSurchageRepository.UpdateShapeSurchageAsync(Shapesurchageobj);

                var result = _mapper.Map<ShapegroupDto>(Shape_surchage);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok("");
            }

        }


        [HttpDelete]
        [Route("/deleteShapeSurchage/{id}")]
        public async Task<IActionResult> DeleteShapeSurchageAsync(int id)
        {

            var shapesurchage = await shapeSurchageRepository.DeleteShapeSurchageAsync(id);
            return Ok(new { Shapegroup = shapesurchage, response = "success" });

        }
    }
}
