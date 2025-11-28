using AutoMapper;
using ProductCodeMasterService.Dtos;
using ProductCodeMasterService.Interfaces;
using ProductCodeMasterService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace ProductCodeMasterService.Controllers
{
    public class ProductCodeController : ControllerBase
    {
        private readonly IProductCode productRepository;
        private readonly IMapper _mapper;
        private object ProductCodeMasters;

        public ProductCodeController(IProductCode productCode, IMapper mapper)
        {
            productRepository = productCode;
            _mapper = mapper;

        }

        [HttpGet]
        [Route("/GetProductType_Dropdown")]
        public async Task<IActionResult> GetProductType_Dropdown()
        {
            try
            {
                var result = await productRepository.GetProductTypeAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        #region MESH
        [HttpGet]
        [Route("/GetSAPMaterialDropdown_Mesh")]
        public async Task<IActionResult> GetSAPMaterialDropdown_Mesh()
        {
            IEnumerable<SAPMaterialDropdown_Dto_Mesh> SAPMaterial = await productRepository.GetSAPMaterialAsync();
            var result = _mapper.Map<List<SAPMaterialDropdown_Dto_Mesh>>(SAPMaterial);
            return Ok(result);
        }
        [HttpGet]
        [Route("/GetSAPMaterialDropdown_Raw")]
        public async Task<IActionResult> GetSAPMaterialDropdown_Raw()
        {
            IEnumerable<SAPMaterialDropdown_Dto_Mesh> SAPMaterial = await productRepository.GetSAPMaterialRawAsync();
            var result = _mapper.Map<List<SAPMaterialDropdown_Dto_Mesh>>(SAPMaterial);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetMeshData/{SAPMaterialid}")]
        public async Task<IActionResult> GetMeshData(int SAPMaterialid)
        {
            IEnumerable<GetMeshData_Dto> Meshdata = await productRepository.GetMeshDataAsync(SAPMaterialid);
            var result = _mapper.Map<List<GetMeshData_Dto>>(Meshdata);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetProductCodeList")]
        public async Task<IActionResult> GetProductCodeList()
        {
            List<ProductCodeMaster> ProductCodes = await productRepository.GetProductcodeList();
            var result = _mapper.Map<List<ProductCodeMasterDto>>(ProductCodes);
            return Ok(result);
        }
        [HttpGet]
        [Route("/GetMeshWireProductList")]
        public async Task<IActionResult> GetMeshWireProductList()
        {
            List<ProductCodeMaster> ProductCodes = await productRepository.GetMeshWireProductList();
            var result = _mapper.Map<List<ProductCodeMasterDto>>(ProductCodes);
            return Ok(result);
        }
        //GetMeshWireProductList

        [HttpGet]
        [Route("/GetTwinIndicator_Dropdown_Mesh")]
        public async Task<IActionResult> GetTwinIndicator_Dropdown_Mesh()
        {
            IEnumerable<TwinIndicator_Dropdown_Mesh> twinIndicator_Dropdown_Meshes = await productRepository.GetTwinIndicatorAsync();
            var result = _mapper.Map<List<TwinIndicator_Dropdown_Mesh>>(twinIndicator_Dropdown_Meshes);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("/GetBOMIndicator_Dropdown_Mesh")]
        public async Task<IActionResult> GetBOMIndicator_Dropdown_Mesh()
        {
            IEnumerable<BOMIndicator_Dropdown_Mesh> BOMIndicator_Dropdowns = await productRepository.GetBOMIndicatorAsync();
            var result = _mapper.Map<List<BOMIndicator_Dropdown_Mesh>>(BOMIndicator_Dropdowns);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetStructureElement_Dropdown_Mesh")]
        public async Task<IActionResult> GetStructureElement_Dropdown_Mesh()
        {
            IEnumerable<StructureElement_Dropdown_Mesh> structureElement_Dropdown_Meshes = await productRepository.GetStructureElementAsync();
            var result = _mapper.Map<List<StructureElement_Dropdown_Mesh>>(structureElement_Dropdown_Meshes);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetStructureElement_Dropdown_Mesh_Get")]
        public async Task<IActionResult> GetStructureElement_Dropdown_Mesh_Get()
        {
            IEnumerable<StructureElement_Dropdown_Mesh> structureElement_Dropdown_Meshes = await productRepository.GetStructureElementMeshAsync();
            var result = _mapper.Map<List<StructureElement_Dropdown_Mesh>>(structureElement_Dropdown_Meshes);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetMeshGridList")]
        public async Task<IActionResult> GetMeshGridList()
        {
            IEnumerable<MeshGetListDto> getMeshLists = await productRepository.GetMeshProductListAsync();
            var result = _mapper.Map<List<MeshGetListDto>>(getMeshLists);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetMeshById/{prodcodeId}")]
        public async Task<IActionResult> GetMeshById(int prodcodeId)
        {
            IEnumerable<MeshListByIdDto> getMeshLists = await productRepository.GetMeshProductListByIdAsync(prodcodeId);
            var result = _mapper.Map<List<MeshListByIdDto>>(getMeshLists);
            return Ok(result);
        }

        [HttpPost]
        [Route("/AddMeshProductCode")]
        public async Task<IActionResult> AddMeshProductCode([FromBody] AddMeshDto AddMeshDtos)
        {

            try
            {
                int result = await productRepository.AddMeshProductCodeAsync(AddMeshDtos);

                if (result == 1)
                {
                    return Ok(result);
                }
                else if (result == 2)
                {
                    return Ok(result);

                }
                else if (result == 3)
                {
                    return Ok("The Product Code already Exists.");
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPost]
        [Route("/UpdateMeshProductCode")]
        public async Task<IActionResult> UpdateMeshProductCode([FromBody] AddMeshDto AddMeshDtos)
        {

            try
            {
                int result = await productRepository.UpdateMeshProductCodeAsync(AddMeshDtos);

                if (result == 1)
                {
                    return Ok(result);
                }
                else if (result == 2)
                {
                    return Ok(result);

                }
                else if (result == 3)
                {
                    return Ok("The Product Code already Exists.");
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        [HttpDelete]
        [Route("/deleteMESHProductCode/{MESHProductCodeID}")]
        public async Task<IActionResult> deleteMESHProductCode(int MESHProductCodeID)
        {

            var Meshdelete = await productRepository.DeleteMESHPRoductAsync(MESHProductCodeID);
            return Ok(new { Cab = Meshdelete, response = "success" });

        }

        [HttpGet]
        [Route("/GetMWData/{ProductCodeId}")]
        public async Task<IActionResult> GetMWData(int ProductCodeId)
        {
            IEnumerable<GetMWDataDto> MainWireData = await productRepository.GetMainWireDataAsync(ProductCodeId);
            var result = _mapper.Map<List<GetMWDataDto>>(MainWireData);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetCWData/{ProductCodeId}")]
        public async Task<IActionResult> GetCWData(int ProductCodeId)
        {
            IEnumerable<GetCWDataDto> crossWireData = await productRepository.GetCrossWireDataAsync(ProductCodeId);
            var result = _mapper.Map<List<GetCWDataDto>>(crossWireData);
            return Ok(result);
        }
        #endregion

        #region Row Material

        [HttpGet]
        [Route("/GetGrade_Raw_Mat")]
        public async Task<IActionResult> GetGrade_Raw_Mat()
        {
            try
            {
                var result = await productRepository.GetGradeTypeAsync_Raw();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("/GetMaterialType_Raw_Mat")]
        public async Task<IActionResult> GetMaterialType_Raw_Mat()
        {
            try
            {
                var result = await productRepository.GetMaterialTypeAsync_Raw();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Route("/AddRawMaterial")]
        public async Task<IActionResult> AddRawMaterial([FromBody] AddRawMaterialDto addRawMaterialDto)
        {
            try
            {
                var result = await productRepository.AddRawMaterialAsync(addRawMaterialDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("/GetRawMaterialList")]
        public async Task<IActionResult> GetRawMaterialList()
        {
            IEnumerable<GetRawMaterialList> getRawMaterialLists = await productRepository.GetRawMaterialListAsync();
            var result = _mapper.Map<List<GetRawMaterialList>>(getRawMaterialLists);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetRawMaterialListById/{prodcodeId}")]
        public async Task<IActionResult> GetRawMaterialListById(int prodcodeId)
        {
            IEnumerable<GetRawMaterialList> getRawMaterialLists = await productRepository.GetRawMaterialListbyIdAsync(prodcodeId);
            var result = _mapper.Map<List<GetRawMaterialList>>(getRawMaterialLists);
            return Ok(result);
        }

        [HttpPost]
        [Route("/UpdateRawMaterial")]
        public async Task<IActionResult> UpdateRawMaterial([FromBody] AddRawMaterialDto updaterawmaterial)
        {
            try
            {

                AddRawMaterialDto addRawMaterialDto = new AddRawMaterialDto();

                var cab = _mapper.Map<AddRawMaterialDto>(updaterawmaterial);
                var result = await productRepository.UpdateRawMaterialAsync(updaterawmaterial);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        [HttpDelete]
        [Route("/deleteRawMaterialProduct/{ProductCodeID}")]
        public async Task<IActionResult> deleteRawMaterialProduct(int ProductCodeID)
        {

            var deletrawmaterial = await productRepository.DeleteRawMaterialAsync(ProductCodeID);
            return Ok(new { deleteACSProductCode = deletrawmaterial, response = "success" });

        }


        #endregion

        #region CAB Product Code

        [HttpGet]
        [Route("/GetGradeTypeDropdown_Cab")]
        public async Task<IActionResult> GetGradeTypeDropdown_Cab()
        {
            IEnumerable<GradeTypeDropdownDto> gradeTypes = await productRepository.GetGradeTypeAsync();
            var result = _mapper.Map<List<GradeTypeDropdownDto>>(gradeTypes);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetFGSA_MaterialDropdown_Cab")]
        public async Task<IActionResult> GetFGSA_MaterialDropdown_Cab()
        {
            IEnumerable<SAPMaterialDropdown_Dto_Cab> fgsaSapMaterial = await productRepository.GetCabSAPMaterialAsync();
            var result = _mapper.Map<List<SAPMaterialDropdown_Dto_Cab>>(fgsaSapMaterial);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetRMSA_MaterialDropdown_Cab")]
        public async Task<IActionResult> GetRMSA_MaterialDropdown_Cab()
        {
            IEnumerable<SAPMaterialDropdown_Dto_Cab> RmsaSapMaterial = await productRepository.GetCabSAPMaterialAsync();
            var result = _mapper.Map<List<SAPMaterialDropdown_Dto_Cab>>(RmsaSapMaterial);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetCABProductCodeList")]
        public async Task<IActionResult> GetCABProductCodeList()
        {
            IEnumerable<CABProductCodeMasterDto> productCodes = await productRepository.GetCABProductCodeListAsync();
            var result = _mapper.Map<List<CABProductCodeMasterDto>>(productCodes);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetCABProductCodebyIdAsync/{capproductId}")]
        public async Task<IActionResult> GetCABProductCodebyIdAsync(int capproductId)
        {
            IEnumerable<CABProductCodeMasterDto> cabProductCodeMaster = await productRepository.GetCABProductCodebyIdAsync(capproductId);
            var result = _mapper.Map<List<CABProductCodeMasterDto>>(cabProductCodeMaster);
            return Ok(result);
        }


        [HttpPost]
        [Route("/AddCABProductCode")]
        public async Task<IActionResult> AddCABProductCode([FromBody] AddCABProductDto CABProductDto)
        {

            try
            {
                var result = await productRepository.AddCABProductCodeAsync(CABProductDto);
                if (result == 1)
                {
                    return Ok(result);
                }
                else if (result == 2)
                {
                    return Ok(result);

                }
                else if (result == 3)
                {
                    return Ok("The grade,diameter,coupler indicator and coupler type combination already exist.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        [HttpPost]
        [Route("/UpdateCABProduct")]
        public async Task<IActionResult> UpdateCABProduct([FromBody] AddCABProductDto CABProductDto)
        {
            try
            {

                AddCABProductDto cABProduct = new AddCABProductDto();

                var cab = _mapper.Map<AddCABProductDto>(CABProductDto);
                var result = await productRepository.UpdateCABProductAsync(CABProductDto);


                if (result == 1)
                {
                    return Ok(result);
                }
                else if (result == 2)
                {
                    return Ok(result);

                }
                else if (result == 3)
                {
                    return Ok("The grade,diameter,coupler indicator and coupler type combination already exist.");
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        [HttpDelete]
        [Route("/deleteCABProductCode/{CABProductCodeID}")]
        public async Task<IActionResult> deleteCABProductCode(int CABProductCodeID)
        {

            var Cabdelete = await productRepository.DeleteCABPRoductAsync(CABProductCodeID);
            return Ok(new { Cab = Cabdelete, response = "success" });

        }


        #endregion
        

        #region CORECAGE Product Code
        [HttpGet]
        [Route("/GetSAPMaterialDropdown_Corecage")]
        public async Task<IActionResult> GetSAPMaterialDropdown_Corecage()
        {
            IEnumerable<SAPMaterialDropdown_Dto_Corecage> sAPMaterials = await productRepository.GetCoreCageSAPMaterialAsync();
            var result = _mapper.Map<List<SAPMaterialDropdown_Dto_Corecage>>(sAPMaterials);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetCORECAGEProductCodeList")]
        public async Task<IActionResult> GetCORECAGEProductCodeList()
        {
            IEnumerable<CARProductCodeListDto> CARProductCodeMaster = await productRepository.GetCORECAGEProductCodeListAsync();
            var result = _mapper.Map<List<CARProductCodeListDto>>(CARProductCodeMaster);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetCARProductCodebyIdAsync/{productId}")]
        public async Task<IActionResult> GetCARProductCodebyIdAsync(int productId)
        {
            IEnumerable<CARProductCodeListDto> ProductCodeMaster = await productRepository.GetCARProductCodebyIdAsync(productId);
            var result = _mapper.Map<List<CARProductCodeListDto>>(ProductCodeMaster);
            return Ok(result);
        }


        [HttpPost]
        [Route("/AddCARProductCode")]
        public async Task<IActionResult> AddCARProductCode([FromBody] AddCoreCageDto AddCoreCageDto)
        {

            try
            {
                int result = await productRepository.AddCARProductCodeAsync(AddCoreCageDto);
                
                if (result == 1)
                {
                    return Ok(result);
                }
                else if (result == 2)
                {
                    return Ok(result);

                }
                else if (result == 3)
                {
                    return Ok("The Product Code already Exists.");
                }
                return Ok(result);
                
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }
        
        [HttpPost]
        [Route("/UpdateCARProduct")]
        public async Task<IActionResult> UpdateCARProduct([FromBody] AddCoreCageDto CARProductDto)
        {
            try
            {

                AddCoreCageDto AddCoreCageDtos = new AddCoreCageDto();

                var cab = _mapper.Map<AddCoreCageDto>(CARProductDto);
                var result = await productRepository.UpdateCARProductAsync(CARProductDto);
                if (result == 1)
                {
                    return Ok(result);
                }
                else if (result == 2)
                {
                    return Ok(result);

                }
                else if (result == 3)
                {
                    return Ok("The Product Code already Exists.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }
        [HttpDelete]
        [Route("/deleteCOREProductCode/{Id}")]
        public async Task<IActionResult> deleteCOREProductCode(int Id)
        {

            var deletproduct = await productRepository.DeleteCORECAGEProductCodeAsync(Id);
            return Ok(new { ProductCode = deletproduct, response = "success" });

        }

        #endregion
                

        
        #region ACS
        [HttpGet]
        [Route("/GetGradeTypeDropdown_Acs")]
        public async Task<IActionResult> GetGradeTypeDropdown_Acs()
        {
            IEnumerable<GradeTypeDropdownDto> gradeTypes = await productRepository.GetACSGradeTypeAsync();
            var result = _mapper.Map<List<GradeTypeDropdownDto>>(gradeTypes);
            return Ok(result);
        }


        [HttpGet]
        [Route("/GetFGSA_MaterialDropdown_Acs")]
        public async Task<IActionResult> GetFGSA_MaterialDropdown_Acs()
        {
            IEnumerable<SAPMaterialDropdown_Dto_Cab> fgsaSapMaterial = await productRepository.GetACSSAPMaterialAsync();
            var result = _mapper.Map<List<SAPMaterialDropdown_Dto_Cab>>(fgsaSapMaterial);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetRMSA_MaterialDropdown_Acs")]
        public async Task<IActionResult> GetRMSA_MaterialDropdown_Acs()
        {
            IEnumerable<SAPMaterialDropdown_Dto_Cab> RmsaSapMaterial = await productRepository.GetACSSAPMaterialAsync();
            var result = _mapper.Map<List<SAPMaterialDropdown_Dto_Cab>>(RmsaSapMaterial);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetACSProductCodeList")]
        public async Task<IActionResult> GetACSProductCodeList()
        {
            IEnumerable<AccessoriesProductCodeMaster_Dto> AcsProductCodeMaster = await productRepository.GetACSProductCodeListAsync();
            var result = _mapper.Map<List<AccessoriesProductCodeMaster_Dto>>(AcsProductCodeMaster);
            return Ok(result);
        }

        [HttpGet]
        [Route("/GetACSProductCodebyIdAsync/{acsproductId}")]
        public async Task<IActionResult> GetACSProductCodebyIdAsync(int acsproductId)
        {
            IEnumerable<AccessoriesProductCodeMaster_Dto> AcsProductCodeMaster = await productRepository.GetACSProductCodebyIdAsync(acsproductId);
            var result = _mapper.Map<List<AccessoriesProductCodeMaster_Dto>>(AcsProductCodeMaster);
            return Ok(result);
        }

        [HttpPost]
        [Route("/AddACSProductCode")]
        public async Task<IActionResult> AddACSProductCode([FromBody] AddAccessoriesDto addAccessoriesDto)
        {

            try
            {
                var result = await productRepository.AddACSProductCodeAsync(addAccessoriesDto);
                if (result == 1)
                {
                    return Ok(result);
                }
                else if (result == 2)
                {
                    return Ok(result);

                }
                else if (result == 3)
                {
                    return Ok("The grade,diameter combination already exist.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        [HttpPost]
        [Route("/UpdateACSProduct")]
        public async Task<IActionResult> UpdateACSProduct([FromBody] AddAccessoriesDto ACSProductDto)
        {
            try
            {

                AddAccessoriesDto cABProduct = new AddAccessoriesDto();

                var cab = _mapper.Map<AddAccessoriesDto>(ACSProductDto);
                var result = await productRepository.UpdateACSProductAsync(ACSProductDto);
                if (result == 1)
                {
                    return Ok(result);
                }
                else if (result == 2)
                {
                    return Ok(result);

                }
                else if (result == 4)
                {
                    return Ok("The grade,diameter type combination already exist.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        [HttpDelete]
        [Route("/deleteACSProductCode/{ACSProductCodeID}")]
        public async Task<IActionResult> deleteACSProductCode(int ACSProductCodeID)
        {

            var deleteaccessories = await productRepository.DeleteACSProductAsync(ACSProductCodeID);
            return Ok(new { deleteACSProductCode = deleteaccessories, response = "success" });

        }

        #endregion

        #region Common Product Code

        [HttpGet]
        [Route("/GetCommonProductCodeListAsync/{producttypes}")]
        public async Task<IActionResult> GetCommonProductCodeListAsync(string producttypes)
        {
            IEnumerable<ProductCodeCommonListDto> ProductCodeMaster = await productRepository.GetCommonProductCodeListAsync(producttypes);
            var result = _mapper.Map<List<ProductCodeCommonListDto>>(ProductCodeMaster);
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("/DeleteCommonProductAsync/{ProductCodeID}/{ProductTypeId}")]
        public async Task<IActionResult> DeleteCommonProductAsync(int ProductCodeID, int ProductTypeId)
        {
            var deleteproductcode= await productRepository.DeleteCommonProductAsync(ProductCodeID, ProductTypeId);
            return Ok(new { deleteProductCode = deleteproductcode, response = "success" });

        }


        #endregion
        //test

    }
}
