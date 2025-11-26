using DetailingService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using System.Collections.Generic;
using DetailingService.Context;
using DetailingService.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Dapper;
using DetailingService.Dtos;



namespace DetailingService.Repositories
{
    public class CarpetService : ICarpetService
    {
        TactonHelper_New objTactonHelper = new TactonHelper_New();
        TactonHelper_New objTactonHelper_new = new TactonHelper_New();
        string UserName = "" + (new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())).Identity.Name;
        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;


        public CarpetService(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }

      
        
        
        public List<ShapeCodeParameterSet> CarpetParameterSetbyProjIdProdType(int projectId, int productTypeId,out string errorMessage)
        {
            errorMessage = "";
            List<ShapeCodeParameterSet> listParameterSet = new List<ShapeCodeParameterSet>();
            ShapeCodeParameterSet objShapeCodeParameterSet = new ShapeCodeParameterSet();
            try
            {
                listParameterSet = objShapeCodeParameterSet.CarpetParameterSetbyProjIdProdType(projectId, productTypeId);
            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message.ToString();
            }
            finally
            {
                objShapeCodeParameterSet = null;
            }
            return listParameterSet;
        }

        public List<ProductCode> PopulateProductCode(out string errorMessage)
        {
            errorMessage = "";
            List<ProductCode> listCarpetProductCode = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            try
            {
                listCarpetProductCode = objProductCode.CarpetProductCode_New();
            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            finally
            {
                objProductCode = null;
            }
            return listCarpetProductCode;

        }

        public List<ProductCode> FilterProductCode(string enteredText, out string errorMessage)
        {
            errorMessage = "";
            List<ProductCode> listCarpetProductCode = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            try
            {
                listCarpetProductCode = objProductCode.CarpetProductCodeFilter(enteredText);
            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            finally
            {
                objProductCode = null;
            }
            return listCarpetProductCode;
        }

        public List<ShapeCode> PopulateShapeCode(out string errorMessage)
        {
            errorMessage = "";
            List<ShapeCode> listCarpetShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();
            try
            {
                listCarpetShapeCode = objShapeCode.CarpetShapeCode_Get();
            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            finally
            {
                objShapeCode = null;
            }
            return listCarpetShapeCode;

        }

        public List<ShapeCode> FilterShapeCode(string enteredText, out string errorMessage)
        {
            errorMessage = "";
            List<ShapeCode> listCarpetShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();
            try
            {
                listCarpetShapeCode = objShapeCode.CarpetShapeCodeFilter_Get(enteredText);
            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            finally
            {
                objShapeCode = null;
            }
            return listCarpetShapeCode;
        }

        public List<CarpetStructure> GetStructureMarkingDetails(int seDetailingID, int structureElementId, out string errorMessage)
        {
            errorMessage = "";
            CarpetStructure structureMarkEntryCollection = new CarpetStructure();
            try
            {
                return structureMarkEntryCollection.CarpetStructureMark_Get(seDetailingID, structureElementId);
            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                structureMarkEntryCollection = null;
            }

        }

        public List<CarpetProduct> InsertCarpetStructureMarking(CarpetStructure objStructureMarking, int structureElementId, int projectTypeId, int projectId, int productTypeId, out string errorMessage, int userId)
        {
            errorMessage = "";
            CarpetProduct objProduct = new CarpetProduct();
            CarpetStructure structMark = new CarpetStructure();
            List<CarpetProduct> products = new List<CarpetProduct>();
            try
            {

                bool IsSingleMesh = false;
                string meshSplitting = "";
                if (objStructureMarking.MainWireLength <= objStructureMarking.ParameterSet.MaxMWLength && objStructureMarking.CrossWireLength <= objStructureMarking.ParameterSet.MaxCWLength)
                {
                    IsSingleMesh = true;
                }
                else if (objStructureMarking.MainWireLength <= objStructureMarking.ParameterSet.MaxCWLength && objStructureMarking.CrossWireLength <= objStructureMarking.ParameterSet.MaxMWLength)
                {
                    IsSingleMesh = true;
                }



                products = objTactonHelper.GenerateCarpetStructureMarking(objStructureMarking, projectTypeId, IsSingleMesh, structureElementId, projectId, productTypeId, out errorMessage, out meshSplitting);

                //LOG THE Carpet Logic    Surendar --18-Dec-2012
                //CarpetSplitLog.LogCarpetLogic(Convert.ToString(objStructureMarking.SEDetailingID), objStructureMarking.StructureMarkingName, meshSplitting, UserName);
                if (errorMessage == "")
                {
                    objStructureMarking.CarpetProduct = products;
                    objStructureMarking.ProductGenerationStatus = true;
                    bool result = false;
                    structMark = objStructureMarking;
                    //if (products[0].ProductionMO1 < objStructureMarking.ParameterSet.MinMo1 || products[0].ProductionMO2 < objStructureMarking.ParameterSet.MinMo2 || products[0].ProductionCO1 < objStructureMarking.ParameterSet.MinCo1 || products[0].ProductionCO2 < objStructureMarking.ParameterSet.MinCo2)
                    //{
                    //    errorMessage = "Machine check failed. Please check the inputs.";
                    //    return null;
                    //}

                    result = structMark.Save(userId, out errorMessage);
                    if (result == true)
                    {
                        if (structMark.StructureMarkId != 0)
                        {
                            // objProduct.DeleteColumnProductmark(structMark.StructureMarkId);
                        }
                        int intProductmarkSlNo = 1;

                        foreach (CarpetProduct productMark in objStructureMarking.CarpetProduct)
                        {

                            bool productResult = false;
                            productMark.ProductMarkingName = structMark.StructureMarkingName + "-" + intProductmarkSlNo;
                            productMark.StructureMarkId = structMark.StructureMarkId;
                            productMark.BOMDrawingPath = SystemConstant.DSSBOMPDF;
                            productMark.MWPitch = "";
                            productMark.CWPitch = "";
                            productMark.MWFlag = 0;
                            productMark.CWFlag = 0;
                            productResult = productMark.Save(userId, structureElementId);
                            if (productResult == false)
                            {
                                break;
                            }
                            intProductmarkSlNo++;
                        }

                    }
                }
                else
                {
                    objStructureMarking.ProductGenerationStatus = false;
                }

            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            finally
            {
                objProduct = null;
                structMark = null;
            }
            return objStructureMarking.CarpetProduct;
        }

        public bool UpdateCarpetStructureMarkingWithoutProductGeneration(ref CarpetStructure objStructureMarking, int structureElementId, int projectTypeId, out string errorMessage, int userId)
        {
            bool result = false;
            errorMessage = "";
            try
            {
                result = objStructureMarking.Save(userId, out errorMessage);
            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            return result;
        }

        public List<CarpetProduct> UpdateCarpetStructureMarkingWithProductGeneration(ref CarpetStructure objStructureMarking, int structureElementId, int projectTypeId, int projectId, int productTypeId, out string errorMessage, int userId)
        {
            CarpetStructure structMark = new CarpetStructure();
            BeamDetailinfo beamDetailInfo = new BeamDetailinfo();
            List<CarpetProduct> products = new List<CarpetProduct>();
            CarpetProduct objProduct = new CarpetProduct();
            errorMessage = "";
            try
            {
                bool IsSingleMesh = false;
                string meshSplitting = "";
                beamDetailInfo.GroupMarkPostedValidate_Get(objStructureMarking.SEDetailingID, out errorMessage);
                if (errorMessage == "")
                {
                    if (objStructureMarking.MainWireLength <= objStructureMarking.ParameterSet.MaxMWLength && objStructureMarking.CrossWireLength <= objStructureMarking.ParameterSet.MaxCWLength)
                    {
                        IsSingleMesh = true;
                    }
                    else if (objStructureMarking.MainWireLength <= objStructureMarking.ParameterSet.MaxCWLength && objStructureMarking.CrossWireLength <= objStructureMarking.ParameterSet.MaxMWLength)
                    {
                        IsSingleMesh = true;
                    }
                    products = objTactonHelper.GenerateCarpetStructureMarking(objStructureMarking, projectTypeId, IsSingleMesh, structureElementId, projectId, productTypeId, out errorMessage, out meshSplitting);
                    //LOG THE Carpet Logic    Surendar --18-Dec-2012
                   // CarpetSplitLog.LogCarpetLogic(Convert.ToString(objStructureMarking.SEDetailingID), objStructureMarking.StructureMarkingName, meshSplitting, UserName);
                    if (errorMessage == "")
                    {
                        if (objStructureMarking.StructureMarkId != 0)
                        {
                            foreach (CarpetProduct productMarkDetails in objStructureMarking.CarpetProduct)
                            {
                                productMarkDetails.DeleteCarpetProductMarkByStructureMarkId(out errorMessage);
                            }
                        }
                        objStructureMarking.CarpetProduct = products;
                        objStructureMarking.ProductGenerationStatus = true;
                        bool result = false;

                        structMark = objStructureMarking;
                        result = structMark.Save(userId, out errorMessage);
                        if (result == true)
                        {
                            int intProductmarkSlNo = 1;

                            foreach (CarpetProduct productMark in objStructureMarking.CarpetProduct)
                            {

                                bool productResult = false;
                                productMark.ProductMarkingName = structMark.StructureMarkingName + "-" + intProductmarkSlNo;
                                productMark.StructureMarkId = structMark.StructureMarkId;
                                productMark.BOMDrawingPath = SystemConstant.DSSBOMPDF;
                                productResult = productMark.Save(userId, structureElementId);
                                if (productResult == false)
                                {
                                    break;
                                }
                                if (objStructureMarking.BendingCheck == true)
                                {
                                    BendingAdjusment objBendingAdjustment = new BendingAdjusment();
                                    StructureElement objSE = new StructureElement();
                                    List<StructureElement> listSE = new List<StructureElement>();
                                    listSE = objSE.GetStructureElement();
                                    string structureElementType = (from StructureElement se in listSE
                                                                   where se.StructureElementTypeId == structureElementId
                                                                   select se.StructureElementType).ToString();
                                    objBendingAdjustment.UpdateProductionBOMDetails(productMark.ProductMarkId, structureElementType, userId, out errorMessage);
                                    if (errorMessage != "")
                                    {
                                        productMark.DeleteCarpetProductMark(out errorMessage);
                                    }
                                }
                                intProductmarkSlNo++;
                            }
                            if (objStructureMarking.BendingCheck == true)
                            {
                                List<CarpetProduct> productMarkList = objProduct.CarpetProductByStructureMarkId_Get(structMark.StructureMarkId, structureElementId);
                                if (productMarkList != null)
                                {
                                    objStructureMarking.CarpetProduct = productMarkList;
                                }
                            }
                        }
                    }
                    else
                    {
                        objStructureMarking.ProductGenerationStatus = false;
                    }
                }
            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            finally
            {
                structMark = null;
            }
            return objStructureMarking.CarpetProduct;
        }

        public bool DeleteStructureMarking(CarpetStructure objStructureMarking, out string errorMessage)
        {
            bool isSuccess = false;
            errorMessage = "";
            try
            {
                isSuccess = objStructureMarking.DeleteCarpetStructureMark(out errorMessage);

            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }

            return isSuccess;
        }

        public CarpetProduct UpdateCarpetProductMarking(CarpetStructure structureMarking, CarpetProduct productMark,  int currentIndex, int structureElementId,  bool machineCheck,  string bendingCheck,  bool transportCheck, out string errorMessage, int userId)
        {
            CarpetProduct newProductMark = new CarpetProduct();
            BeamDetailinfo beamDetailInfo = new BeamDetailinfo();
            errorMessage = "";
            machineCheck = false;
            bendingCheck = "";
            transportCheck = false;
            int oldProductMarkId = 0;
            try
            {
                newProductMark = productMark;
                productMark.BendingCheckInd = false;

                beamDetailInfo.GroupMarkPostedValidate_Get(structureMarking.SEDetailingID, out errorMessage);

                if (errorMessage == "")
                {

                    if (productMark.MWLength <= structureMarking.ParameterSet.MachineMaxMWLength && productMark.CWLength <= structureMarking.ParameterSet.MachineMaxCWLength)
                    {
                        machineCheck = true;
                    }
                    else if (productMark.MWLength <= structureMarking.ParameterSet.MachineMaxCWLength && productMark.CWLength <= structureMarking.ParameterSet.MachineMaxMWLength)
                    {
                        machineCheck = true;
                    }


                    //Defining BOM type (Change for Carpet - TCS (12th Nov,2014) - Subhankar
                    string bom_type = string.Empty;
                    CarpetProduct objCarpet = new CarpetProduct();
                    bom_type = objCarpet.GetCarpetBOMType(newProductMark.ProductCodeId, newProductMark.shapecode.ShapeCodeName);




                    if (bom_type == "C_BOM" || bom_type == "D_BOM" || bom_type == "E_BOM" || bom_type == "F_BOM" || bom_type == "G_BOM")
                    {
                        machineCheck = true;
                    }


                    if (machineCheck == false)
                    {
                        return newProductMark;
                    }
                    //newProductMark = objTactonHelper.GenerateCarpetProductMarking(structureMarking, ref productMark, out machineCheck, out bendingCheck, out transportCheck, out errorMessage);

                    transportCheck = true;

                    //Defining BOM type (Change for Carpet - TCS (12th Nov,2014) - Subhankar 


                    if (bom_type != "C_BOM" && bom_type != "D_BOM" && bom_type != "E_BOM" && bom_type != "F_BOM" && bom_type != "G_BOM")
                    {
                        string shapecodename = productMark.shapecode.ShapeCodeName;

                        string specialSlabShapes = "1M1;1MR1;1M2;2MR1;2M2;1C1;2C1;1CR1;2MR12;2M1;N";
                        List<string> specialSlabShapesList = new List<string>(specialSlabShapes.Split(';'));

                        if (specialSlabShapes.Contains(shapecodename.ToUpper().Trim()))
                        {
                            newProductMark = objTactonHelper_new.GenerateCarpetProductMarking(structureMarking,  productMark, out machineCheck, out bendingCheck, out transportCheck, out errorMessage);
                        }
                        else
                        {
                            newProductMark = objTactonHelper.GenerateCarpetProductMarking(structureMarking,  productMark, out machineCheck, out bendingCheck, out transportCheck, out errorMessage);
                        }


                        // newProductMark = objTactonHelper.GenerateCarpetProductMarking(structureMarking, ref productMark, out machineCheck, out bendingCheck, out transportCheck, out errorMessage);
                    }
                    else
                    {
                        newProductMark.ProductionCO1 = newProductMark.CO1;
                        newProductMark.ProductionCO2 = newProductMark.CO2;
                        newProductMark.ProductionMO1 = newProductMark.MO1;
                        newProductMark.ProductionMO2 = newProductMark.MO2;
                    }
                    //--------------------------------------

                    if (machineCheck == true && bendingCheck == "" && transportCheck == true)
                    {
                        if (errorMessage == "")
                        {
                            oldProductMarkId = productMark.ProductMarkId;
                            if (productMark.BendingCheckInd != true)
                            {
                                if (productMark.ProductMarkId != 0)
                                {
                                    productMark.DeleteCarpetProductMark(out errorMessage);
                                }
                            }
                            // int intProductmarkSlNo = 1;
                            bool productResult = false;
                            //productMark.ProductMarkingName = structureMarking.StructureMarkingName + "-" + intProductmarkSlNo;
                            newProductMark.StructureMarkId = structureMarking.StructureMarkId;
                            newProductMark.BOMDrawingPath = SystemConstant.DSSBOMPDF;
                            productResult = newProductMark.Save(userId, structureElementId);

                            if (productMark.BendingCheckInd == true)
                            {
                                BendingAdjusment objBendingAdjustment = new BendingAdjusment();
                                StructureElement objSE = new StructureElement();
                                List<StructureElement> listSE = new List<StructureElement>();
                                listSE = objSE.GetStructureElement();

                                var seList = (from StructureElement se in listSE
                                              where se.StructureElementTypeId == structureElementId
                                              select se).ToList();

                                objBendingAdjustment.UpdateProductionBOMDetails(productMark.ProductMarkId, seList[0].StructureElementType, userId, out errorMessage);

                                if (errorMessage != "")
                                {
                                    productMark.DeleteCarpetProductMark(out errorMessage);
                                }
                                else
                                {
                                    productMark.ProductMarkId = oldProductMarkId;
                                    productMark.DeleteCarpetProductMark(out errorMessage);
                                }
                            }

                            List<CarpetProduct> productMarkList = productMark.CarpetProductByStructureMarkId_Get(structureMarking.StructureMarkId, structureElementId);
                            //if (productMarkList != null)
                            //{
                            //    newProductMark = productMarkList.FirstOrDefault(item => item.ProductMarkId == newProductMark.ProductMarkId);
                            //    productMark = newProductMark;
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            return newProductMark;
        }

        public CarpetProduct SaveProductMarkafterConfirmation(CarpetStructure structureMarking, CarpetProduct productMark, ref int currentIndex, int structureElementId, int userId, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (productMark.ProductMarkId != 0)
                {
                    productMark.DeleteCarpetProductMark(out errorMessage);
                }
                // int intProductmarkSlNo = 1;
                bool productResult = false;
                //productMark.ProductMarkingName = structureMarking.StructureMarkingName + "-" + intProductmarkSlNo;
                productMark.StructureMarkId = structureMarking.StructureMarkId;
                productMark.BOMDrawingPath =SystemConstant.DSSBOMPDF;
                productResult = productMark.Save(userId, structureElementId);
            }

            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            return productMark;
        }

        public List<CarpetStructure> RegenerateValidation(List<CarpetStructure> structureMarkList, ShapeCodeParameterSet parameterSet, int structureElementId, int projectTypeId, int projectId, int productTypeId, out string errorMessage, int userId)
        {
            BeamDetailinfo beamDetailInfo = new BeamDetailinfo();
            List<CarpetProduct> products = new List<CarpetProduct>();
            CarpetStructure structMark = new CarpetStructure();
            CarpetProduct objProduct = new CarpetProduct();
            errorMessage = "";
            try
            {
                beamDetailInfo.intStructureElementTypeId = structureElementId;
                beamDetailInfo.GroupMarkPPosting_Regenerate_CARPET(structureMarkList[0].StructureMarkId, out errorMessage);
                if (errorMessage == "")
                {
                    foreach (CarpetStructure objStructureMarking in structureMarkList)
                    {
                        bool IsSingleMesh = false;
                        string meshSplitting = "";
                        if (objStructureMarking.SideForCode == "0" || objStructureMarking.SideForCode == null)
                        {
                            objStructureMarking.ParameterSet = parameterSet;
                            objStructureMarking.ParamSetNumber = parameterSet.ParameterSetNumber;
                            if (objStructureMarking.MainWireLength <= objStructureMarking.ParameterSet.MaxMWLength && objStructureMarking.CrossWireLength <= objStructureMarking.ParameterSet.MaxCWLength)
                            {
                                IsSingleMesh = true;
                            }
                            else if (objStructureMarking.MainWireLength <= objStructureMarking.ParameterSet.MaxCWLength && objStructureMarking.CrossWireLength <= objStructureMarking.ParameterSet.MaxMWLength)
                            {
                                IsSingleMesh = true;
                            }

                            products = objTactonHelper.GenerateCarpetStructureMarking(objStructureMarking, projectTypeId, IsSingleMesh, structureElementId, projectId, productTypeId, out errorMessage, out meshSplitting);
                            //LOG THE Carpet Logic    Surendar --18-Dec-2012
                            //CarpetSplitLog.LogCarpetLogic(Convert.ToString(objStructureMarking.SEDetailingID), objStructureMarking.StructureMarkingName, meshSplitting, UserName);
                            if (errorMessage == "")
                            {
                                if (objStructureMarking.StructureMarkId != 0)
                                {
                                    foreach (CarpetProduct productMarkDetails in objStructureMarking.CarpetProduct)
                                    {

                                        productMarkDetails.DeleteCarpetProductMarkByStructureMarkId(out errorMessage);
                                    }

                                }
                                objStructureMarking.CarpetProduct = products;
                                objStructureMarking.ProductGenerationStatus = true;
                                bool result = false;
                                structMark = objStructureMarking;
                                result = structMark.Save(userId, out errorMessage);
                                if (result == true)
                                {
                                    int intProductmarkSlNo = 1;

                                    foreach (CarpetProduct productMark in objStructureMarking.CarpetProduct)
                                    {

                                        bool productResult = false;
                                        productMark.ProductMarkingName = structMark.StructureMarkingName + "-" + intProductmarkSlNo;
                                        productMark.StructureMarkId = structMark.StructureMarkId;
                                        productMark.BOMDrawingPath =SystemConstant.DSSBOMPDF;
                                        productResult = productMark.Save(userId, structureElementId);
                                        if (productResult == false)
                                        {
                                            break;
                                        }
                                        intProductmarkSlNo++;
                                    }

                                }
                            }
                            else
                            {
                                objStructureMarking.ProductGenerationStatus = false;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            finally
            {
                structMark = null;
                objProduct = null;
            }
            return structureMarkList;
        }

        public bool DeleteProductMarking(CarpetProduct productMark, int seDetailingId, out string errorMessage)
        {
            BeamDetailinfo beamDetailInfo = new BeamDetailinfo();
            bool isSuccess = false;
            errorMessage = "";

            try
            {
                beamDetailInfo.GroupMarkPostedValidate_Get(seDetailingId, out errorMessage);

                if (errorMessage == "")
                {
                    productMark.Deleteflag = 1;
                    isSuccess = productMark.DeleteCarpetProductMark(out errorMessage);
                }

            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }

            return isSuccess;
        }

        public CarpetProduct InsertCarpetProductMarking(CarpetProduct objProductMarkingMarking, int structureElementId, out string errorMessage, int userId)
        {
            errorMessage = "";
            try
            {
                bool productResult = false;
                objProductMarkingMarking.ProductMarkId = 0;
                objProductMarkingMarking.ProductMarkingName = objProductMarkingMarking.ProductMarkingName;
                objProductMarkingMarking.BOMDrawingPath =SystemConstant.DSSBOMPDF;
                productResult = objProductMarkingMarking.Save(userId, structureElementId);
            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            return objProductMarkingMarking;
        }

        public List<CarpetProduct> GetOverHang(int parameterSetNumber, int projectId, int structureElementId, int productTypeId, int mwLength, int cwLength, int mwSpace, int cwSpace, out string errorMessage)
        {
            errorMessage = "";
            List<CarpetProduct> listCarpetOH = new List<CarpetProduct>();
            CarpetProduct objCarpetOH = new CarpetProduct();
            try
            {
                objCarpetOH.ProductionMWLength = mwLength;
                objCarpetOH.ProductionCWLength = cwLength;
                objCarpetOH.MWSpacing = mwSpace;
                objCarpetOH.CWSpacing = cwSpace;
                listCarpetOH = objCarpetOH.GetOverHang(parameterSetNumber, projectId, structureElementId, productTypeId);
            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            finally
            {
                objCarpetOH = null;
            }
            return listCarpetOH;
        }


        #region "Load Header Details"

        public List<BeamDetailinfo> GetCustomerContractProjectByGroupMarkId(int GroupMarkId, out List<GroupMark> listHeaderDetails, out string errorMessage)        // Get PRC Detailing Header Details By Group Mark Id
        {
            List<BeamDetailinfo> listBeamDetailInfo = new List<BeamDetailinfo>();
            BeamDetailinfo objBeamDetailInfo = new BeamDetailinfo();
            listHeaderDetails = null;
            errorMessage = "";
            try
            {
                objBeamDetailInfo.intGroupMarkId = GroupMarkId;
                listBeamDetailInfo = objBeamDetailInfo.GetCustContProjByGroupMarkID(out listHeaderDetails);
            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            finally
            {
                objBeamDetailInfo = null;
            }
            return listBeamDetailInfo;
        }

        public List<BeamDetailinfo> GetCustomerContractProjectByProjectId(int ProjectId, out string errorMessage)            // Get PRC Detailing Header Details By Project Id
        {
            List<BeamDetailinfo> listBeamDetailInfo = new List<BeamDetailinfo>();
            BeamDetailinfo objBeamDetailInfo = new BeamDetailinfo();
            errorMessage = "";
            try
            {
                listBeamDetailInfo = objBeamDetailInfo.GetCustContProjByProjectID(ProjectId);

            }
            catch (Exception ex)
            {
               
                errorMessage = ex.Message;
            }
            finally
            {
                objBeamDetailInfo = null;
            }
            return listBeamDetailInfo;
        }

        #endregion

    }
}
