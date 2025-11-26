using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using DetailingService.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class ColumnService : IColumnService
    {
       // TactonHelper objTactonHelper = new TactonHelper();
        TactonHelper_New objTactonHelper_new = new TactonHelper_New();
        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;


        public ColumnService(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }

        #region "Get Methods"

        public List<ColumnStructure> GetStructureMarkingDetails(int projectID, int seDetailingID, int structureElementTypeID, int productTypeID)
        {
            string errorMsg = "";
            ColumnStructure structuremarkEntryCollection = new ColumnStructure();
            try
            {
                return structuremarkEntryCollection.ColumnStructureMark_Get(projectID, seDetailingID, structureElementTypeID, productTypeID);
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMsg = ex.Message;
                return null;
            }
            finally
            {
                structuremarkEntryCollection = null;
            }
        }

        public List<ShapeCodeParameterSet> ColumnParameterSetbyProjIdProdType(int projectId, int productTypeId)
        {
            List<ShapeCodeParameterSet> listParameterSet = new List<ShapeCodeParameterSet>();
            ShapeCodeParameterSet objShapeCodeParameterSet = new ShapeCodeParameterSet();
            string errorMsg = "";
            try
            {
                listParameterSet = objShapeCodeParameterSet.ColumnParameterSetbyProjIdProdType(projectId, productTypeId);
                return listParameterSet;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMsg = ex.Message;
                return null;
            }
            finally
            {
                objShapeCodeParameterSet = null;
                listParameterSet = null;
            }
        }

        public List<ShapeCode> PopulateShapeCode()
        {
            List<ShapeCode> listColumnShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();
            ///errorMsg = "";
            try
            {
                listColumnShapeCode = objShapeCode.ColumnShapeCodeBind_Get();
                return listColumnShapeCode;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
               // errorMsg = ex.Message;
                return null;
            }
            finally
            {
                objShapeCode = null;
                listColumnShapeCode = null;
            }

        }

        public List<ShapeCode> FilterShapeCode(string enteredText, out string errorMsg)
        {
            List<ShapeCode> listColumnShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();
            errorMsg = "";
            try
            {
                listColumnShapeCode = objShapeCode.ColumnShapeCode_Get(enteredText);
                return listColumnShapeCode;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMsg = ex.Message;
                return null;
            }
            finally
            {
                objShapeCode = null;
                listColumnShapeCode = null;
            }

        }

        public List<ProductCode> PopulateProductCode(int structureElementTypeID, int productTypeID)
        {
            List<ProductCode> listColumnProductCode = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            //errorMsg = "";
            try
            {
                listColumnProductCode = objProductCode.ColumnProductCodeBind(structureElementTypeID, productTypeID);
                return listColumnProductCode;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
               // errorMsg = ex.Message;
                return null;
            }
            finally
            {
                objProductCode = null;
                listColumnProductCode = null;
            }

        }

        public List<ProductCode> FilterProductCode(int structureElementTypeID, int productTypeID, string enteredText, out String errorMsg)
        {
            List<ProductCode> listColumnProductCode = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            errorMsg = "";
            try
            {
                listColumnProductCode = objProductCode.ColumnProductCode(structureElementTypeID, productTypeID, enteredText);
                return listColumnProductCode;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMsg = ex.Message;
                return null;
            }
            finally
            {
                objProductCode = null;
                listColumnProductCode = null;
            }

        }

        public List<ProductCode> PopulateClinkProductLength(out string errorMsg)
        {
            List<ProductCode> listClinkProductLength = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            errorMsg = "";
            try
            {
                listClinkProductLength = objProductCode.ColumnCLinkProducttLengthBind_Get();
                return listClinkProductLength;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMsg = ex.Message;
                return null;
            }
            finally
            {
                objProductCode = null;
                listClinkProductLength = null;
            }
        }

        public List<ProductCode> FilterClinkProduct(string enteredText)
        {
            List<ProductCode> listClinkProductLength = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            //errorMsg = "";
            try
            {
                listClinkProductLength = objProductCode.ColumnCLinkProductLength_Get(enteredText);
                return listClinkProductLength;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                //errorMsg = ex.Message;
                return null;
            }
            finally
            {
                objProductCode = null;
                listClinkProductLength = null;
            }

        }

        #endregion

        #region "Save"

        public List<ColumnProduct> InsertColumnStructureMarking( ColumnStructure objStructureMarking,  int topCover, int bottomCover, int leftCover, int rightCover, int leg, int seDetailingID, int userId, out string errorMessage)
        {

            List<ColumnProduct> products = new List<ColumnProduct>();
            ColumnStructure structMark = new ColumnStructure();
            ColumnProduct objProduct = new ColumnProduct();
             errorMessage = "";
            try
            {
                //Added by aihswarya
                string shapecodename = objStructureMarking.Shape.ShapeCodeName;
                string specialColumnShapeCodes = "CO;COI;2M1C;2MR1C;4M1C;4MR1C;COIH;COH;"; // SNEHAT_TTL 02092022
                List<string> specialColumnShapesList = new List<string>(specialColumnShapeCodes.Split(';'));
                if (specialColumnShapesList.Contains(shapecodename.ToUpper().Trim()))
                {
                    products = objTactonHelper_new.GenerateColumnStructureMarking(objStructureMarking, out errorMessage, topCover, bottomCover, leftCover, rightCover, leg);
                }
                else
                {
                    products = objTactonHelper_new.GenerateColumnStructureMarking(objStructureMarking, out errorMessage, topCover, bottomCover, leftCover, rightCover, leg);

                }
                if (errorMessage == "")
                {
                    objStructureMarking.ColumnProducts = products;
                    objStructureMarking.ProductGenerationStatus = true;
                    bool result = false;
                    structMark = objStructureMarking;
                    result = structMark.Save(userId, out errorMessage);
                    if (result == true)
                    {
                        if (structMark.StructureMarkId != 0)
                        {
                            objProduct.DeleteColumnProductmark(structMark.StructureMarkId);
                        }
                        int intProductmarkSlNo = 1;

                        foreach (ColumnProduct productMark in objStructureMarking.ColumnProducts)
                        {
                            bool productResult = false;
                            productMark.ProductMarkingName = structMark.StructureMarkingName + "-" + intProductmarkSlNo;
                            productMark.StructureMarkId = structMark.StructureMarkId;
                            productMark.BOMDrawingPath = SystemConstant.DSSBOMPDF;
                            productResult = productMark.Save(userId);
                            if (productResult == false)
                            {
                                break;
                            }
                            if (objStructureMarking.BendingCheckInd == true && objStructureMarking.Shape.ShapeCodeName.Trim().ToUpper() != "2M1C" && objStructureMarking.Shape.ShapeCodeName.Trim().ToUpper() != "2MR1C")
                            {
                                BendingAdjusment objBendingAdjustment = new BendingAdjusment();
                                objBendingAdjustment.UpdateProductionBOMDetails(productMark.ProductMarkId, "Column", userId, out errorMessage);

                                if (errorMessage != "")
                                {
                                    if (structMark.StructureMarkId != 0)
                                    {
                                        structMark.DeleteColumnStructuremark(structMark.StructureMarkId,out errorMessage);
                                    }
                                }
                            }
                            intProductmarkSlNo++;
                        }
                        if (objStructureMarking.BendingCheckInd == true)
                        {
                            if (errorMessage == "")
                            {
                                List<ColumnProduct> productMarkList = objProduct.ColumnProductByStructureMarkId_Get(structMark.StructureMarkId);
                                if (productMarkList != null)
                                {
                                    objStructureMarking.ColumnProducts = productMarkList;
                                    products = productMarkList;
                                }
                            }
                        }
                    }
                }
                else
                {
                    objStructureMarking.ProductGenerationStatus = false;
                }
                return products;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return products;
            }
            finally
            {
                products = null;
                structMark = null;
                objProduct = null;
            }

        }

        public List<ColumnProduct> UpdateColumnStructureMarking(ColumnStructure objStructureMarking,  int topCover, int bottomCover, int leftCover, int rightCover, int leg, int seDetailingID, int userId, out string errorMessage)
        {
            ColumnProduct objProduct = new ColumnProduct();
            ColumnStructure structMark = new ColumnStructure();
            List<ColumnProduct> products = new List<ColumnProduct>();
             errorMessage = "";
            try
            {
                //Added by aishwarya
                string shapecodename = objStructureMarking.Shape.ShapeCodeName;
                string specialColumnShapeCodes = "CO;COI;2M1C;2MR1C;4M1C;4MR1C;COIH;COH;"; // SNEHAT_TTL 02092022
                List<string> specialColumnShapesList = new List<string>(specialColumnShapeCodes.Split(';'));
                if (specialColumnShapesList.Contains(shapecodename.ToUpper().Trim()))
                {
                    products = objTactonHelper_new.GenerateColumnStructureMarking(objStructureMarking, out errorMessage, topCover, bottomCover, leftCover, rightCover, leg);
                }
                else
                {
                    products = objTactonHelper_new.GenerateColumnStructureMarking(objStructureMarking, out errorMessage, topCover, bottomCover, leftCover, rightCover, leg);
                }
                //end


                //products = objTactonHelper.GenerateColumnStructureMarking(objStructureMarking, out errorMessage, topCover, bottomCover, leftCover, rightCover, leg);

                if (errorMessage == "")
                {
                    objStructureMarking.ColumnProducts = products;
                    objStructureMarking.ProductGenerationStatus = true;
                    bool result = false;

                    structMark = objStructureMarking;
                    result = structMark.Update(userId, out errorMessage);
                    if (result == true)
                    {
                        if (structMark.StructureMarkId != 0)
                        {
                            objProduct.DeleteColumnProductmark(structMark.StructureMarkId);
                        }
                        int intProductmarkSlNo = 1;

                        foreach (ColumnProduct productMark in objStructureMarking.ColumnProducts)
                        {

                            bool productResult = false;
                            productMark.ProductMarkingName = structMark.StructureMarkingName + "-" + intProductmarkSlNo;
                            productMark.StructureMarkId = structMark.StructureMarkId;
                            productMark.BOMDrawingPath = SystemConstant.DSSBOMPDF;
                            productResult = productMark.Save(userId);
                            if (productResult == false)
                            {
                                break;
                            }

                            if (objStructureMarking.BendingCheckInd == true && objStructureMarking.Shape.ShapeCodeName.Trim().ToUpper() != "2M1C" && objStructureMarking.Shape.ShapeCodeName.Trim().ToUpper() != "2MR1C")
                            {
                                BendingAdjusment objBendingAdjustment = new BendingAdjusment();
                                objBendingAdjustment.UpdateProductionBOMDetails(productMark.ProductMarkId, "Column", userId, out errorMessage);

                                if (errorMessage != "")
                                {
                                    if (structMark.StructureMarkId != 0)
                                    {
                                        structMark.DeleteColumnStructuremark(structMark.StructureMarkId,out errorMessage);
                                    }
                                }
                            }
                            intProductmarkSlNo++;
                        }
                        if (objStructureMarking.BendingCheckInd == true)
                        {
                            if (errorMessage == "")
                            {
                                List<ColumnProduct> productMarkList = objProduct.ColumnProductByStructureMarkId_Get(structMark.StructureMarkId);
                                if (productMarkList != null)
                                {
                                    objStructureMarking.ColumnProducts = productMarkList;
                                    products = productMarkList;
                                }
                            }
                        }
                    }
                }
                else
                {
                    objStructureMarking.ProductGenerationStatus = false;
                }
                return products;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return products;
            }
            finally
            {
                objProduct = null;
                structMark = null;
                products = null;
            }

        }

        public List<ColumnStructure> RegenerateValidation(List<ColumnStructure> structureMarkList, int topCover, int bottomCover, int leftCover, int rightCover, int leg, int seDetailingID, int userId, int structureElementId,out string errorMessage)
        {
            BeamDetailinfo beamDetailInfo = new BeamDetailinfo();
            List<ColumnProduct> products = new List<ColumnProduct>();
            ColumnStructure structMark = new ColumnStructure();
            ColumnProduct colProduct = new ColumnProduct();
            errorMessage = "";
            try
            {

                beamDetailInfo.intStructureElementTypeId = structureElementId;
                beamDetailInfo.GroupMarkPPosting_Regenerate(structureMarkList[0].StructureMarkId, out errorMessage);
                if (errorMessage == "")
                {
                    foreach (ColumnStructure objStructureMarking in structureMarkList)
                    {
                        foreach (ColumnProduct productMarkDetails in objStructureMarking.ColumnProducts)
                        {
                            if (objStructureMarking.StructureMarkId != 0)
                            {
                                productMarkDetails.DeleteColumnProductmark(objStructureMarking.StructureMarkId);
                            }
                        }

                        //Added by aishwarya
                        string shapecodename = objStructureMarking.Shape.ShapeCodeName;
                        string specialColumnShapeCodes = "CO;COI;2M1C;2MR1C;4M1C;4MR1C;COIH;COH;"; // SNEHAT_TTL 02092022
                        List<string> specialColumnShapesList = new List<string>(specialColumnShapeCodes.Split(';'));
                        if (specialColumnShapesList.Contains(shapecodename.ToUpper().Trim()))
                        {
                            products = objTactonHelper_new.GenerateColumnStructureMarking(objStructureMarking, out errorMessage, topCover, bottomCover, leftCover, rightCover, leg);
                        }
                        else
                        {
                            products = objTactonHelper_new.GenerateColumnStructureMarking(objStructureMarking, out errorMessage, topCover, bottomCover, leftCover, rightCover, leg);
                        }
                        //end


                        //products = objTactonHelper.GenerateColumnStructureMarking(objStructureMarking, out errorMessage, topCover, bottomCover, leftCover, rightCover, leg);
                        if (errorMessage == "" || errorMessage == null)
                        {
                            objStructureMarking.ProductGenerationStatus = true;
                            objStructureMarking.ColumnProducts = products;

                            structMark = objStructureMarking;
                            int intProductmarkSlNo = 1;

                            foreach (ColumnProduct productMark in objStructureMarking.ColumnProducts)
                            {
                                bool productResult = false;
                                productMark.ProductMarkingName = structMark.StructureMarkingName + "-" + intProductmarkSlNo;
                                productMark.StructureMarkId = structMark.StructureMarkId;
                                productMark.BOMDrawingPath = SystemConstant.DSSBOMPDF;
                                productResult = productMark.Save(userId);
                                if (productResult == false)
                                {
                                    break;
                                }
                                if (objStructureMarking.BendingCheckInd == true && objStructureMarking.Shape.ShapeCodeName.Trim().ToUpper() != "2M1C" && objStructureMarking.Shape.ShapeCodeName.Trim().ToUpper() != "2MR1C")
                                {
                                    BendingAdjusment objBendingAdjustment = new BendingAdjusment();
                                    objBendingAdjustment.UpdateProductionBOMDetails(productMark.ProductMarkId, "Column", userId, out errorMessage);
                                }
                                intProductmarkSlNo++;
                            }
                            List<ColumnProduct> productMarkList = colProduct.ColumnProductByStructureMarkId_Get(structMark.StructureMarkId);
                            if (productMarkList != null)
                            {
                                objStructureMarking.ColumnProducts = productMarkList;
                            }
                        }
                        else
                        {
                            objStructureMarking.ColumnProducts = products;
                            objStructureMarking.ProductGenerationStatus = false;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return structureMarkList;
            }
            finally
            {
                products = null;
                structMark = null;
            }
            return structureMarkList;
        }

        public bool UpdateGroupMarking(int SeDetailId, int ParamSetNumber)
        {
            GroupMark objGroupMark = new GroupMark();
            bool isSuccess = false;
            string errorMsg = "";
            try
            {
                isSuccess = objGroupMark.GroupMarkingParamset_Update(SeDetailId, ParamSetNumber);

            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMsg = ex.Message;
                return false;
            }
            finally
            {
                objGroupMark = null;
            }
            return isSuccess;
        }

        #endregion

        #region "Delete"

        public bool DeleteStructureMarking(int StructureMarkId, out string errorMsg)
        {
            bool isSuccess = false;
            errorMsg = "";
            try
            {
                ColumnStructure objStructureMarking = new ColumnStructure();
                isSuccess = objStructureMarking.DeleteColumnStructuremark(StructureMarkId,out errorMsg);

            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMsg = ex.Message;
                return false;
            }
            return isSuccess;
        }

        #endregion

        #region CoreCage Detailing
        public List<ProductCode> CoreCagePopulateProductCode()
        {
            List<ProductCode> listCoreCageProductCode = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            //errorMsg = "";
            try
            {
                listCoreCageProductCode = objProductCode.CarpetProductCode();
                return listCoreCageProductCode;
            }
            catch (Exception ex)
            {
                // ExceptionLog.LogException(ex, UserName);
                // errorMsg = ex.Message;
                return null;
            }
            finally
            {
                objProductCode = null;
                listCoreCageProductCode = null;
            }

        }

        public List<ProductCode> CoreCageSelectdProductCode(int GroupMarkId)
        {
            List<ProductCode> listCoreCageProductCode = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            GroupMarkDAL objGroupmark=new GroupMarkDAL();

            try
            {
                listCoreCageProductCode = objGroupmark.GetProductCodeIdForGroupMark(GroupMarkId);
                return listCoreCageProductCode;
            }
            catch (Exception ex)
            {
                // ExceptionLog.LogException(ex, UserName);
                // errorMsg = ex.Message;
                return null;
            }
            finally
            {
                objProductCode = null;
                listCoreCageProductCode = null;
            }

        }
        #endregion


        public List<ColumnLegDto> GetColumnLeg(int tntparameter, int productID)
        {
            List<ColumnLegDto> listColumnProductCode = new List<ColumnLegDto>();
            ProductCode objProductCode = new ProductCode();

            //ierrorMsg = "";
            try
            {
                listColumnProductCode = objProductCode.ColumnLeg_Get(tntparameter, productID);
                return listColumnProductCode;
            }
            catch (Exception ex)
            {
                // ExceptionLog.LogException(ex, UserName);
                //errorMsg = ex.Message;
                return null;
            }
            finally
            {
                objProductCode = null;
                listColumnProductCode = null;
            }

        }


        public int UpdateStructureMarkingDetails(int structureMarkID, string structuremarkingname,int qty, out string errormsg)
        {
            int intReturnValue = 0;
            errormsg = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkID", structureMarkID);
                    dynamicParameters.Add("@vchStructureMarkName", structuremarkingname);
                    dynamicParameters.Add("@qty", qty);
                    dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.QueryFirstOrDefault<int>(SystemConstant.Update_ColumnStructureMark, dynamicParameters, commandType: CommandType.StoredProcedure);

                    intReturnValue = dynamicParameters.Get<int>("@intOutput");
                    sqlConnection.Close();
                    return intReturnValue;

                }


            }
            catch (Exception ex)
            {
                errormsg = "Failed to update the marking name";
                return intReturnValue;
            }
        }

    }
}
