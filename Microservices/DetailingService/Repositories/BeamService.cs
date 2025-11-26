using DetailingService.Context;
using DetailingService.Interfaces;
using DetailingService.Constants;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class BeamService : IBeamService
    {

       
        TactonHelper_New objTactonHelper_new = new TactonHelper_New();

        //string UserName = "" + (new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())).Identity.Name;
        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;


        public BeamService(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }
        public List<BeamStructure> GetStructureMarkingDetails(string groupMarkName, int projectID, int seDetailingID, int structureElementTypeID, int productTypeID, int groupMarkID, out string errorMessage)
        {
            BeamStructure objBeamStructure = new BeamStructure();
            errorMessage = "";
            try
            {
                return objBeamStructure.BCDStructureMarking_Get(groupMarkName, projectID, seDetailingID, structureElementTypeID, productTypeID, groupMarkID);
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                objBeamStructure = null;
            }
        }

        public List<ProductCode> PopulateProductCode(int structureElementTypeID, int productTypeID)
        {
            List<ProductCode> listBeamDetailinfo = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            string errorMessage = "";
            try
            {
                listBeamDetailinfo = objProductCode.ProductCodeForBeam_Get(structureElementTypeID, productTypeID);
                return listBeamDetailinfo;
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                listBeamDetailinfo = null;
                objProductCode = null;
            }
        }

        public List<ProductCode> FilterProductCode(int structureElementTypeID, int productTypeID, string enteredText, out string errorMessage)
        {
            List<ProductCode> listBeamDetailinfo = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            errorMessage = "";
            try
            {
                listBeamDetailinfo = objProductCode.FilterProductCodeForBeam_Get(structureElementTypeID, productTypeID, enteredText);
                return listBeamDetailinfo;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                //errorMessage = ex.Message;
                return null;
            }
            finally
            {
                listBeamDetailinfo = null;
                objProductCode = null;
            }
        }
        public List<ShapeCode> PopulateShapeCode()
        {
            List<ShapeCode> listShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();
            string errorMessage = "";
            try
            {
                listShapeCode = objShapeCode.ShapeCodeForBeam_Get();
                return listShapeCode;
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                objShapeCode = null;
                listShapeCode = null;
            }
        }

        public List<ShapeCode> FilterShapeCode(string enteredText, out string errorMessage)
        {
            List<ShapeCode> listShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();
            errorMessage = "";
            try
            {
                listShapeCode = objShapeCode.FilterShapeCodeForBeam_Get(enteredText);
                return listShapeCode;
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                objShapeCode = null;
                listShapeCode = null;
            }
        }

        public List<ProductCode> PopulateCapProductCode()
        {
            List<ProductCode> listcapProductCode = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            string errorMessage = "";
            try
            {
                listcapProductCode = objProductCode.CapProductForBeam_Get();
                return listcapProductCode;
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                objProductCode = null;
                listcapProductCode = null;
            }
        }

        public List<ProductCode> FilterCapProductCode(string enteredText, out string errorMessage)
        {
            List<ProductCode> listcapProductCode = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            errorMessage = "";
            try
            {
                listcapProductCode = objProductCode.FilterCapProductForBeam_Get(enteredText);
                return listcapProductCode;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
               errorMessage = ex.Message;
                return null;
            }
            finally
            {
                objProductCode = null;
                listcapProductCode = null;
            }
        }

        public List<BeamProduct> VerifyStructureMarkingInputsNew( BeamStructure objStructureMarking, out string errorMessage, int gap1, int gap2, int topCover, int bottomCover, int leftCover, int rightCover, int hook, int leg, int seDetailingID, out string bendingcheck)
        {
            string SHPCODE = "";
            bendingcheck = "";
           
            BeamProduct objProduct = new BeamProduct();
            BeamStructure structMark = new BeamStructure();
            List<BeamProduct> products = new List<BeamProduct>();
            errorMessage ="";
            try
            {
                //Added by aishwarya
                string shapecodename = objStructureMarking.Shape.ShapeCodeName;
                // string specialBeamShapes = "C;CR;K;KR"; // production deployment
                //string specialBeamShapes = "C;CR;K;KR;LH1;LH3;LH2";
                string specialBeamShapes = "C;CR;K;KR;LH;LH1;LH2;LH3;4M1B;4MR1B;C1;CR1;3M2B;LH5"; // second batch production deployment
                List<string> specialBeamShapesList = new List<string>(specialBeamShapes.Split(';'));

                if (specialBeamShapesList.Contains(shapecodename.ToUpper().Trim()))
                {
                    products = objTactonHelper_new.GenerateBeamStructureMarking(objStructureMarking, out errorMessage, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, out bendingcheck);
                }
                else
                {
                    products = objTactonHelper_new.GenerateBeamStructureMarking(objStructureMarking, out errorMessage, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, out bendingcheck);
                }
                //end



                if (bendingcheck == "")
                {
                    if (errorMessage == "")
                    {
                        objStructureMarking.ProductMark = products;
                        objStructureMarking.ProductGenerationStatus = true;
                        bool result = false;

                        structMark = objStructureMarking;
                        result = structMark.Save(seDetailingID, out errorMessage);

                        if (result == true)
                        {
                            if (structMark.StructureMarkId != 0)
                            {
                                objProduct.DeleteProductmark(structMark.StructureMarkId);
                            }
                            int intProductmarkSlNo = 1;

                            foreach (BeamProduct productMark in objStructureMarking.ProductMark)
                            {
                                bool productResult = false;
                                productMark.ProductMarkingName = structMark.StructureMarkName + "-" + intProductmarkSlNo;
                                productMark.StructureMarkID = structMark.StructureMarkId;
                                productMark.BOMDrawingPath = SystemConstant.DSSBOMPDF;// SystemConstant.DSSBOMPDF;

                                SHPCODE = productMark.ShapeCode; // CHANGE BY ANURAN CHG0031334
                                productResult = productMark.Save();
                                if (productResult == false)
                                {
                                    break;
                                }
                                // CHANGE BY ANURAN CHG0031334>>  
                                if ((SHPCODE == "5M1B") || (SHPCODE == "5MR1B"))
                                {
                                    BendingAdjusment objBendingAdjustment = new BendingAdjusment();
                                    //objBendingAdjustment.UpdateProductionBOMDetails(productMark.ProductMarkID, "Beam", 0);
                                    objBendingAdjustment.UpdateProductionBOMDetails(productMark.ProductMarkID, "Beam", 6, out errorMessage);
                                    if (errorMessage != "")
                                    {
                                        if (structMark.StructureMarkId != 0)
                                        {
                                            structMark.DeleteStructuremark(objStructureMarking.StructureMarkId,out errorMessage);
                                        }
                                    }
                                }

                                //if (objStructureMarking.BendingCheckInd == true)
                                //{
                                //    BendingAdjusment objBendingAdjustment = new BendingAdjusment();
                                //    //objBendingAdjustment.UpdateProductionBOMDetails(productMark.ProductMarkID, "Beam", 0);
                                //    objBendingAdjustment.UpdateProductionBOMDetails(productMark.ProductMarkID, "Beam", 6, out errorMessage);
                                //    if (errorMessage != "")
                                //    {
                                //        if (structMark.StructureMarkId != 0)
                                //        {
                                //            structMark.DeleteStructuremark(out errorMessage);
                                //        }
                                //    } 
                                //}

                                // CHANGE BY ANURAN CHG0031334>> 
                                intProductmarkSlNo++;
                            }
                            objStructureMarking.BendingCheckStatus = true;
                            //if (objStructureMarking.BendingCheckInd == true)
                            //{
                            List<BeamProduct> productMarkList = objProduct.ProductMarkingByGroupMarkId_Get(structMark.StructureMarkId);
                            if (productMarkList != null)
                            {
                                objStructureMarking.ProductMark = productMarkList;
                                products = productMarkList;
                            }
                            //}
                        }
                    }
                    else
                    {
                        objStructureMarking.ProductGenerationStatus = false;
                    }
                }
                return products;
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                objProduct = null;
                structMark = null;
                products = null;
            }

        }


        public List<BeamProduct> EditEnd( BeamStructure objStructureMarking, out string errorMessage, int gap1, int gap2, int topCover, int bottomCover, int leftCover, int rightCover, int hook, int leg, int seDetailingID, out string bendingcheck)
        {
            bendingcheck = "";
            List<BeamProduct> products = new List<BeamProduct>();
            BeamStructure structMark = new BeamStructure();
            BeamProduct objProduct = new BeamProduct();
            errorMessage = "";
            try
            {
                //Added by aishwarya
                string shapecodename = objStructureMarking.Shape.ShapeCodeName;
                //string specialBeamShapes = "C;CR;K;KR";
                string specialBeamShapes = "C;CR;K;KR;LH;LH1;LH2;LH3;4M1B;4MR1B;C1;CR1;3M2B;LH5"; // second batch production deployment
                List<string> specialBeamShapesList = new List<string>(specialBeamShapes.Split(';'));
                if (specialBeamShapesList.Contains(shapecodename.ToUpper().Trim()))
                {
                    products = objTactonHelper_new.GenerateBeamStructureMarking(objStructureMarking, out errorMessage, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, out bendingcheck);
                }
                else
                {
                    products = objTactonHelper_new.GenerateBeamStructureMarking(objStructureMarking, out errorMessage, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, out bendingcheck);
                }

                //End
                //original code
                // products = objTactonHelper.GenerateBeamStructureMarking(objStructureMarking, out errorMessage, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, out bendingcheck);

                if (errorMessage == "" && bendingcheck == "")
                {
                    objStructureMarking.ProductMark = products;
                    objStructureMarking.ProductGenerationStatus = true;
                    bool result = false;

                    structMark = objStructureMarking;
                    result = structMark.Save(seDetailingID, out errorMessage);
                    if (result == true)
                    {

                        if (structMark.StructureMarkId != 0)
                        {
                            objProduct.DeleteProductmark(structMark.StructureMarkId);
                        }
                        int intProductmarkSlNo = 1;

                        foreach (BeamProduct productMark in objStructureMarking.ProductMark)
                        {

                            bool productResult = false;
                            productMark.ProductMarkingName = structMark.StructureMarkName + "-" + intProductmarkSlNo;
                            productMark.StructureMarkID = structMark.StructureMarkId;
                            productMark.BOMDrawingPath = SystemConstant.DSSBOMPDF;
                            productResult = productMark.Save();
                            if (productResult == false)
                            {
                                break;
                            }
                            intProductmarkSlNo++;
                        }
                        objStructureMarking.BendingCheckStatus = true;
                        List<BeamProduct> productMarkList = objProduct.ProductMarkingByGroupMarkId_Get(structMark.StructureMarkId);
                        if (productMarkList != null)
                        {
                            objStructureMarking.ProductMark = productMarkList;
                            products = productMarkList;
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
              //  ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                structMark = null;
                objProduct = null;
                products = null;
            }

        }

        public List<BeamStructure> RegenerateValidation(List<BeamStructure> structureMarkList, int gap1, int gap2, int topCover, int bottomCover, int leftCover, int rightCover, int hook, int leg, int seDetailingID, int structureElementId, out string errorMessage, out string bendingcheck)
        {
            bendingcheck = "";
            BeamDetailinfo beamDetailInfo = new BeamDetailinfo();
            List<BeamProduct> products = new List<BeamProduct>();
            BeamStructure structMark = new BeamStructure();
            BeamProduct objProduct = new BeamProduct();
            errorMessage = "";
            try
            {
                beamDetailInfo.intStructureElementTypeId = structureElementId;
                beamDetailInfo.GroupMarkPPosting_Regenerate(structureMarkList[0].StructureMarkId, out errorMessage);
                if (errorMessage == "")
                {
                    foreach (BeamStructure objStructureMarking in structureMarkList)
                    {
                        string specialBeamShapes = "CR1;L1;RL1;LF;LH;LH3;LH4;2M11B;2MR11B;3M13B;3MR13B;4M13B;4MR13B;C1;3M2B";
                        List<string> specialBeamShapesList = new List<string>(specialBeamShapes.Split(';'));

                        if (specialBeamShapesList.Contains(objStructureMarking.Shape.ShapeCodeName.ToUpper().Trim())) continue;

                        //foreach (BeamProduct productMarkDetails in objStructureMarking.ProductMark)
                        //{
                        //    if (objStructureMarking.StructureMarkId != 0)
                        //    {
                        //        productMarkDetails.DeleteProductmark(objStructureMarking.StructureMarkId);
                        //    }
                        //}

                        //Added by aishwarya
                        string shapecodename = objStructureMarking.Shape.ShapeCodeName;
                        //string specialBeamShapeCodes = "C;CR;K;KR";
                        string specialBeamShapeCodes = "C;CR;K;KR;LH;LH1;LH2;LH3;4M1B;4MR1B;C1;CR1;3M2B;LH5"; // second batch production deployment
                        List<string> specialBeamShapesList1 = new List<string>(specialBeamShapeCodes.Split(';'));
                        if (specialBeamShapesList1.Contains(shapecodename.ToUpper().Trim()))
                        {
                            products = objTactonHelper_new.GenerateBeamStructureMarking(objStructureMarking, out errorMessage, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, out bendingcheck);
                        }
                        else
                        {
                            products = objTactonHelper_new.GenerateBeamStructureMarking(objStructureMarking, out errorMessage, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, out bendingcheck);
                        }
                        //end


                        //  products = objTactonHelper.GenerateBeamStructureMarking(objStructureMarking, out errorMessage, gap1, gap2, topCover, bottomCover, leftCover, rightCover, hook, leg, out bendingcheck);

                        if ((errorMessage == "" || errorMessage == null) && (bendingcheck == ""))
                        {
                            foreach (BeamProduct productMarkDetails in objStructureMarking.ProductMark)
                            {
                                if (objStructureMarking.StructureMarkId != 0)
                                {
                                    productMarkDetails.DeleteProductmark(objStructureMarking.StructureMarkId);
                                }
                            }
                            objStructureMarking.ProductGenerationStatus = true;
                            objStructureMarking.ProductMark = products;
                            structMark = objStructureMarking;

                            int intProductmarkSlNo = 1;

                            foreach (BeamProduct productMark in objStructureMarking.ProductMark)
                            {
                                bool productResult = false;
                                productMark.ProductMarkingName = structMark.StructureMarkName + "-" + intProductmarkSlNo;
                                productMark.StructureMarkID = structMark.StructureMarkId;
                                productMark.BOMDrawingPath = SystemConstant.DSSBOMPDF;
                                productResult = productMark.Save();
                                if (productResult == false)
                                {
                                    break;
                                }
                                intProductmarkSlNo++;
                            }
                        }
                        else
                        {
                            objStructureMarking.ProductMark = products;
                            if (errorMessage != "")
                            {
                                objStructureMarking.ProductGenerationStatus = false;
                            }
                            else
                            {
                                objStructureMarking.BendingCheckStatus = false;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                structMark = null;
                objProduct = null;
                products = null;
            }
            return structureMarkList;
        }

        public bool DeleteStructureMarking(int StructureMarkId, out string errorMessage)
        {
            bool isSuccess = false;
            errorMessage = "";
            BeamStructure objStructureMarking = new BeamStructure();
            try
            {
                isSuccess = objStructureMarking.DeleteStructuremark(StructureMarkId,out errorMessage);

            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }

            return isSuccess;
        }
        public bool UpdateGroupMarking(int SeDetailId, int ParamSetNumber, out string errorMessage)
        {
            GroupMark objGroupMark = new GroupMark();
            bool isSuccess = false;
            errorMessage = "";
            try
            {
                isSuccess = objGroupMark.GroupMarkingParamset_Update(SeDetailId, ParamSetNumber);

            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return false;
            }
            finally
            {
                objGroupMark = null;
            }
            return isSuccess;
        }


        public List<ShapeCodeParameterSet> ParameterSetByProjectProductTypeId(int ProjectId, int productTypeID)
        {
            List<ShapeCodeParameterSet> listParameterSet = new List<ShapeCodeParameterSet>();
            ShapeCodeParameterSet objShapeCodeParameterSet = new ShapeCodeParameterSet();
            string errorMessage = "";
            try
            {

                listParameterSet = objShapeCodeParameterSet.ParameterSetByProjectProductTypeId_Get(ProjectId, productTypeID);
                return listParameterSet;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                objShapeCodeParameterSet = null;
                listParameterSet = null;
            }
        }

        public List<ShapeCodeParameterSet> ParameterSetByProjectID(int GroupMarkId, out string errorMessage)
        
        {
            List<ShapeCodeParameterSet> listParameterSet = new List<ShapeCodeParameterSet>();
            ShapeCodeParameterSet objShapeCodeParameterSet = new ShapeCodeParameterSet();
            errorMessage = "";
            try
            {

                listParameterSet = objShapeCodeParameterSet.ParameterSetByProjectID_Get(GroupMarkId);
                return listParameterSet;
            }
            catch (Exception ex)
            {
               // ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                objShapeCodeParameterSet = null;
                listParameterSet = null;
            }
        }
        public int UpdateStructureMarkingDetails(int structureMarkID, string structuremarkingname, int qty, out string errormsg)
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
                    sqlConnection.QueryFirstOrDefault<int>(SystemConstant.Update_BeamStructureMark, dynamicParameters, commandType: CommandType.StoredProcedure);

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
