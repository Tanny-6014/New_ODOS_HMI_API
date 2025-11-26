using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using DetailingService.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class SlabService : ISlabService
    {
        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;


        //   TactonHelper objTactonHelper = new TactonHelper();

        TactonHelper_New objTactonHelper_new = new TactonHelper_New();

        string UserName = ""; //+ (new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())).Identity.Name;
        public SlabService(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }
     
        public  List<ShapeCodeParameterSet> SlabParameterSetbyProjIdProdType(int projectId, int productTypeId)
        {
           // errorMessage = "";
            List<ShapeCodeParameterSet> listParameterSet = new List<ShapeCodeParameterSet>();
            ShapeCodeParameterSet objShapeCodeParameterSet = new ShapeCodeParameterSet();
            try
            {
                listParameterSet = objShapeCodeParameterSet.SlabParameterSetbyProjIdProdType(projectId, productTypeId);
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                // = ex.Message.ToString();
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
            List<ProductCode> listSlabProductCode = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            try
            {
                listSlabProductCode = objProductCode.SlabProductCode();
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objProductCode = null;
            }
            return listSlabProductCode;

        }

        public List<ProductCode> FilterProductCode(string enteredText)
        {
            
            List<ProductCode> listSlabProductCode = new List<ProductCode>();
            ProductCode objProductCode = new ProductCode();
            try
            {
               
                listSlabProductCode = objProductCode.ACSProductCodeFilter(enteredText);
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                
            }
            finally
            {
                objProductCode = null;
            }
            return listSlabProductCode;
        }

        public List<ShapeCode> PopulateSlabShapeCode()
        {
            //string errorMessage = "";
            List<ShapeCode> listSlabShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();
            try
            {
                listSlabShapeCode = objShapeCode.SlabShapeCode_Get();
                return listSlabShapeCode;
            }
            catch (Exception ex)
            {
                
                //errorMessage = ex.Message;
                return null;
            }
            finally
            {
                objShapeCode = null;
                listSlabShapeCode = null;
            }

        }

        public List<ShapeCode> FilterShapeCode(string enteredText, out string errorMessage)
        {
            errorMessage = "";
            List<ShapeCode> listSlabShapeCode = new List<ShapeCode>();
            ShapeCode objShapeCode = new ShapeCode();
            try
            {
                listSlabShapeCode = objShapeCode.SlabShapeCodeFilter_Get(enteredText);
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objShapeCode = null;
            }
            return listSlabShapeCode;
        }

        public List<SlabStructure> GetStructureMarkingDetails(int seDetailingID, int structureElementId, out string errorMessage)
        {
            errorMessage = "";
            SlabStructure structureMarkEntryCollection = new SlabStructure();
            try
            {
                return structureMarkEntryCollection.SlabStructureMark_Get(seDetailingID, structureElementId);
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
                return null;
            }
            finally
            {
                structureMarkEntryCollection = null;
            }
            ////DBManager dbManager = new DBManager();
            //List<SlabStructure> SlabStructureList = new List<SlabStructure>();
            //DataSet dsSlabStructureMark = new DataSet();
            //List<SlabStructureMarkingDto> slabStructureMarkingTest;
            //IEnumerable<SlabStructureMarkingDto> slabStructureMarkingDto; //new IEnumerable<List<ProjectMaster>>();

            //try
            //{
            //    // var data = null;

            //    using (var sqlConnection = new SqlConnection(connectionString))
            //    {
            //        sqlConnection.Open();

            //        var dynamicParameters = new DynamicParameters();
            //        dynamicParameters.Add("@INTSEDETAILINGID", seDetailingID);//SeDetailId  Currently added hard code value --Vanita
            //                                                                  //  dsSlabStructureMark = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SlabStructureMarking_Get");

            //        slabStructureMarkingDto = sqlConnection.Query<SlabStructureMarkingDto>(SystemConstant.SlabStructureMarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

            //        //IEnumerable<int> enumerable = Enumerable.Range(1, 300);
            //        slabStructureMarkingTest = slabStructureMarkingDto.ToList();




            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return slabStructureMarkingTest;

        }

        public List<SlabProduct> InsertSlabStructureMarking( SlabStructure  objStructureMarking, int structureElementId, int projectTypeId, int projectId, int productTypeId, out string errorMessage, int userId)
        {
            errorMessage = "";
            SlabProduct objProduct = new SlabProduct();
            SlabStructure structMark = new SlabStructure();
            List<SlabProduct> products = new List<SlabProduct>();
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

                products = objTactonHelper_new.GenerateSlabStructureMarking(objStructureMarking, projectTypeId, IsSingleMesh, structureElementId, projectId, productTypeId, out errorMessage, out meshSplitting);

                //LOG THE Slab Logic    Surendar --18-Dec-2012
               // SlabSplitLog.LogSlabLogic(Convert.ToString(objStructureMarking.SEDetailingID), objStructureMarking.StructureMarkingName, meshSplitting, UserName);
                if (errorMessage == "")
                {  //Pankaj Commented start
                    objStructureMarking.SlabProduct = products;
                    objStructureMarking.ProductGenerationStatus = true;
                    bool result = false;
                    structMark = objStructureMarking;
                    // Pankaj Commented End

                    //objStructureMarking.SlabProduct = products;
                    //objStructureMarking.MainWireLength = products[0].MWLength;
                    //objStructureMarking.CrossWireLength = products[0].CWLength;
                    //objStructureMarking.MemberQty = products[0].MemberQty;

                    //objStructureMarking.ProductGenerationStatus = true;
                    //bool result = false;
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

                        foreach (SlabProduct productMark in objStructureMarking.SlabProduct)
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
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objProduct = null;
                structMark = null;
            }
            
            return objStructureMarking.SlabProduct;

            
        }

        public bool UpdateSlabStructureMarkingWithoutProductGeneration(SlabStructure objStructureMarking, int structureElementId, int projectTypeId, out string errorMessage, int userId)
        {
            bool result = false;
            errorMessage = "";
            try
            {
                result = objStructureMarking.Save(userId, out errorMessage);
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            return result;
        }
        public List<SlabProduct> UpdateSlabStructureMarkingWithProductGeneration( SlabStructure objStructureMarking, int structureElementId, int projectTypeId, int projectId, int productTypeId, out string errorMessage, int userId)
        {
            SlabStructure structMark = new SlabStructure();
            BeamDetailinfo beamDetailInfo = new BeamDetailinfo();
            List<SlabProduct> products = new List<SlabProduct>();
            SlabProduct objProduct = new SlabProduct();
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
                    products = objTactonHelper_new.GenerateSlabStructureMarking(objStructureMarking, projectTypeId, IsSingleMesh, structureElementId, projectId, productTypeId, out errorMessage, out meshSplitting);
                    //LOG THE Slab Logic    Surendar --18-Dec-2012
                    //SlabSplitLog.LogSlabLogic(Convert.ToString(objStructureMarking.SEDetailingID), objStructureMarking.StructureMarkingName, meshSplitting, UserName);
                    if (errorMessage == "")
                    {
                        if (objStructureMarking.StructureMarkId != 0)
                        {
                            foreach (SlabProduct productMarkDetails in objStructureMarking.SlabProduct)
                            {
                                productMarkDetails.DeleteSlabProductMarkByStructureMarkId(objStructureMarking.StructureMarkId, out errorMessage);
                            }
                        }
                        objStructureMarking.SlabProduct = products;
                        objStructureMarking.ProductGenerationStatus = true;
                        bool result = false;

                        structMark = objStructureMarking;
                        result = structMark.Save(userId, out errorMessage);
                        if (result == true)
                        {
                            int intProductmarkSlNo = 1;

                            foreach (SlabProduct productMark in objStructureMarking.SlabProduct)
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
                                        productMark.DeleteSlabProductMark(productMark.ProductMarkId, out errorMessage);
                                    }
                                }
                                intProductmarkSlNo++;
                            }
                            if (objStructureMarking.BendingCheck == true)
                            {
                                List<SlabProduct> productMarkList = objProduct.SlabProductByStructureMarkId_Get(structMark.StructureMarkId, structureElementId);
                                if (productMarkList != null)
                                {
                                    objStructureMarking.SlabProduct = productMarkList;
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
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                structMark = null;
            }
            return objStructureMarking.SlabProduct;
        }

        public bool DeleteStructureMarking(int StructureMarkId)
        {
            bool isSuccess = false;
            
            SlabStructure structure = new SlabStructure();
            try
            {
                isSuccess = structure.DeleteSlabStructureMark(StructureMarkId);



            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                // errorMessage = ex.Message;
            }



            return isSuccess;
        }
        //public bool DeleteStructureMarking(SlabStructure objStructureMarking, out string errorMessage)
        //{
        //    bool isSuccess = false;
        //    errorMessage = "";
        //    try
        //    {
        //        isSuccess = objStructureMarking.DeleteSlabStructureMark(objStructureMarking.StructureMarkId, out errorMessage);

        //    }
        //    catch (Exception ex)
        //    {
        //        //ExceptionLog.LogException(ex, UserName);
        //        errorMessage = ex.Message;
        //    }

        //    return isSuccess;
        //}


        public SlabProduct UpdateSlabProductMarking(SlabStructure structureMarking, SlabProduct productMark, int currentIndex, int structureElementId, bool machineCheck, string bendingCheck, bool transportCheck, int userId,out string errorMessage)
        {
            SlabProduct newProductMark = new SlabProduct();
            BeamDetailinfo beamDetailInfo = new BeamDetailinfo();
            errorMessage = "";
            machineCheck = false;
            bendingCheck = "";
            transportCheck = true;
            int oldProductMarkId = 0;
            machineCheck = true;
            try
            {
                newProductMark = productMark;





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



                    if (machineCheck == false)
                    {
                        return newProductMark;
                    }



                    //Original code
                    // newProductMark = objTactonHelper_new.GenerateSlabProductMarking(structureMarking, ref productMark, out machineCheck, out bendingCheck, out transportCheck, out errorMessage);



                    //Changed by aishwarya
                    int shapeId = productMark.shapecode.ShapeID;

                    string shapecodename = productMark.shapecode.ShapeCodeName;
                    //string shapecodename = "Test";



                    //string specialSlabShapes = "1M1;1MR1;1M2;2M2;2MR12;2M1;2MR1;1C1;2C1;1CR1"; // production deployed
                    //string specialSlabShapes = "1M1;1MR1;1M2;2M2;2MR12;2M1;1C1;1CR1;3M1;1C2;3C2;3C4;DN1;4M1";
                    //string specialSlabShapes = "1M1;1MR1;1M2;2M2;2MR12;2M1;2MR1;1C1;2C1;1CR1;2MR2;3M1;1M3;1MR3;1C2;1CR2;1C3;1CR3;3C2;3CR2;3C4;2C3;2C4;2C5;2C8"; //second batch
                    //string specialSlabShapes = "1M1;1MR1;1M2;2M2;2MR12;2M1;2MR1;1C1;2C1;1CR1;2MR2;3M1;1M3;1MR3;1C2;1CR2;1C3;1CR3;3C2;3CR2;3C4;2C3;2C4;2C5;2C8;DN1;DS1;2C6;EN5;2M8;4C2;1MR2;2MR8;ENR5;2M4;2M12;DN10;DS10;ENR6;DN8;DS8;DN9;DS9;2MR9;DN4;EN8;DS4;ES8;EN17;ES17;EN6;2MR4;DN2;DS2;DS6;DN5;2M5;DS5;DN6;2M9;EN10;ES10;2MR3;ENR12;DS3;EN12;DN3;DS13;2M3;2MR5;DN13;DN12;DS12;4M1;4M2;4M4;4M5;4M9;4MR1;4MR2;4MR3;4MR4;4MR9;4M3;4M6;4MR6"; //third batch
                    //string specialSlabShapes = "1M1;1MR1;1M2;2M2;2MR12;2M1;2MR1;1C1;2C1;1CR1;2MR2;3M1;1M3;1MR3;1C2;1CR2;1C3;1CR3;3C2;3CR2;3C4;2C3;2C4;2C5;2C8;DN1;DS1;2C6;EN5;2M8;4C2;1MR2;2MR8;ENR5;2M4;2M12;DN10;DS10;ENR6;DN8;DS8;DN9;DS9;2MR9;DN4;EN8;DS4;ES8;EN17;ES17;EN6;2MR4;DN2;DS2;DS6;DN5;2M5;DS5;DN6;2M9;EN10;ES10;2MR3;ENR12;DS3;EN12;DN3;DS13;2M3;2MR5;DN13;DN12;DS12;4M1;4M2;4M4;4M5;4M9;4MR1;4MR2;4MR3;4MR4;4MR9;4M3;4M6;4MR6;2CR1;EN7;5M1;3M4;FB1;2M6;LM1;N;2MR6;3M10;T1;3M20;3MR8;3M9;3M8;TN1;TS1;2C2;2CR8;3MR4;ENR7;T4;5MR1;T5;ENR15;FBR1;EN15;3MR1;3M2;T3;2CR2;3M5;T1K1;2CR4;T2;2CR9;T7;2C9;T8;3M3;NK2;P1;NK1;3MR5;2CR3;2CR5;3MR2;3MR3;A2;T2K1;T2K2;T1K2;A3;T6;T3K2;A1;T3K1;JB1"; //Fourth batch
                    //string specialSlabShapes = "1M1;1MR1;1M2;2M2;2MR12;2M1;2MR1;1C1;2C1;1CR1;2MR2;3M1;1M3;1MR3;1C2;1CR2;1C3;1CR3;3C2;3CR2;3C4;2C3;2C4;2C5;2C8;DN1;DS1;2C6;EN5;2M8;4C2;1MR2;2MR8;ENR5;2M4;2M12;DN10;DS10;ENR6;DN8;DS8;DN9;DS9;2MR9;DN4;EN8;DS4;ES8;EN17;ES17;EN6;2MR4;DN2;DS2;DS6;DN5;2M5;DS5;DN6;2M9;EN10;ES10;2MR3;ENR12;DS3;EN12;DN3;DS13;2M3;2MR5;DN13;DN12;DS12;4M1;4M2;4M4;4M5;4M9;4MR1;4MR2;4MR3;4MR4;4MR9;4M3;4M6;4MR6;2CR1;EN7;5M1;3M4;FB1;2M6;LM1;N;2MR6;3M10;T1;3M20;3MR8;3M9;3M8;TN1;TS1;2C2;2CR8;3MR4;ENR7;T4;5MR1;T5;ENR15;FBR1;EN15;3MR1;3M2;T3;2CR2;3M5;T1K1;2CR4;T2;2CR9;T7;2C9;T8;3M3;NK2;P1;NK1;3MR5;2CR3;2CR5;3MR2;3MR3;A2;T2K1;T2K2;T1K2;A3;T6;T3K2;A1;T3K1;JB1;F;2C7;2CR6;2CR7;2M10;2M11;2M13;2M7;2MR10;2MR13;2MR7;3C3;4M10;4M14;4M7;6B2;CF1;DF1;DF3;DF4;DN11;DN7;DS11;DS7;EC1;EC1N;EC1S;EC2N;EC2S;F1;F2;TB1;TB15;TB2;TB29;TB3;TB31;TB32;TB4;TB5;TB8;TN12;TN25;TN26;TN32;TN6;TS12;TS25;TS26;TS32;TS6;2M14;TB19";//Fifth batch
                    // string specialSlabShapes = "1M1;1MR1;1M2;2M2;2MR12;2M1;2MR1;1C1;2C1;1CR1;2MR2;3M1;1M3;1MR3;1C2;1CR2;1C3;1CR3;3C2;3CR2;3C4;2C3;2C4;2C5;2C8;DN1;DS1;2C6;EN5;2M8;4C2;1MR2;2MR8;ENR5;2M4;2M12;DN10;DS10;ENR6;DN8;DS8;DN9;DS9;2MR9;DN4;EN8;DS4;ES8;EN17;ES17;EN6;2MR4;DN2;DS2;DS6;DN5;2M5;DS5;DN6;2M9;EN10;ES10;2MR3;ENR12;DS3;EN12;DN3;DS13;2M3;2MR5;DN13;DN12;DS12;4M1;4M2;4M4;4M5;4M9;4MR1;4MR2;4MR3;4MR4;4MR9;4M3;4M6;4MR6;2CR1;EN7;5M1;3M4;FB1;2M6;LM1;N;2MR6;3M10;T1;3M20;3MR8;3M9;3M8;TN1;TS1;2C2;2CR8;3MR4;ENR7;T4;5MR1;T5;ENR15;FBR1;EN15;3MR1;3M2;T3;2CR2;3M5;T1K1;2CR4;T2;2CR9;T7;2C9;T8;3M3;NK2;P1;NK1;3MR5;2CR3;2CR5;3MR2;3MR3;A2;T2K1;T2K2;T1K2;A3;T6;T3K2;A1;T3K1;JB1;F;2C7;2CR6;2CR7;2M10;2M11;2M13;2M7;2MR10;2MR13;2MR7;3C3;4M10;4M14;4M7;6B2;CF1;DF1;DF3;DF4;DN11;DN7;DS11;DS7;EC1;EC1N;EC1S;EC2N;EC2S;F1;F2;TB1;TB15;TB2;TB29;TB3;TB31;TB32;TB4;TB5;TB8;TN12;TN25;TN26;TN32;TN6;TS12;TS25;TS26;TS32;TS6;2M14;TB19;HM5;6B7;FR13;FN13;4CR2;4C5;6S6;TB30;3C13;HM3;6N6;CM1;CMR1;CM2;CMR2;6N5;8B6;3CR13;4C14;6N4;6S4;FN12;FS12;8N5;FM1;8N2;8S2;8S4;8S3;TB28;7B2;FN11;FS11;7N2;7S2;EN19;FS8;FS7;FS6;ES4;3M21";//Sixth batch
                    string specialSlabShapes = "1M1;1MR1;1M2;2M2;2MR12;2M1;2MR1;1C1;2C1;1CR1;2MR2;3M1;1M3;1MR3;1C2;1CR2;1C3;1CR3;3C2;3CR2;3C4;2C3;2C4;2C5;2C8;DN1;DS1;2C6;EN5;2M8;4C2;1MR2;2MR8;ENR5;2M4;2M12;DN10;DS10;ENR6;DN8;DS8;DN9;DS9;2MR9;DN4;EN8;DS4;ES8;EN17;ES17;EN6;2MR4;DN2;DS2;DS6;DN5;2M5;DS5;DN6;2M9;EN10;ES10;2MR3;ENR12;DS3;EN12;DN3;DS13;2M3;2MR5;DN13;DN12;DS12;4M1;4M2;4M4;4M5;4M9;4MR1;4MR2;4MR3;4MR4;4MR9;4M3;4M6;4MR6;2CR1;EN7;5M1;3M4;FB1;2M6;LM1;N;2MR6;3M10;T1;3M20;3MR8;3M9;3M8;TN1;TS1;2C2;2CR8;3MR4;ENR7;T4;5MR1;T5;ENR15;FBR1;EN15;3MR1;3M2;T3;2CR2;3M5;T1K1;2CR4;T2;2CR9;T7;2C9;T8;3M3;NK2;P1;NK1;3MR5;2CR3;2CR5;3MR2;3MR3;A2;T2K1;T2K2;T1K2;A3;T6;T3K2;A1;T3K1;JB1;F;2C7;2CR6;2CR7;2M10;2M11;2M13;2M7;2MR10;2MR13;2MR7;3C3;4M10;4M14;4M7;6B2;CF1;DF1;DF3;DF4;DN11;DN7;DS11;DS7;EC1;EC1N;EC1S;EC2N;EC2S;F1;F2;TB1;TB15;TB2;TB29;TB3;TB31;TB32;TB4;TB5;TB8;TN12;TN25;TN26;TN32;TN6;TS12;TS25;TS26;TS32;TS6;2M14;TB19;HM5;6B7;FR13;FN13;4CR2;4C5;6S6;TB30;3C13;HM3;6N6;CM1;CMR1;CM2;CMR2;6N5;8B6;3CR13;4C14;6N4;6S4;FN12;FS12;8N5;FM1;8N2;8S2;8S4;8S3;TB28;7B2;FN11;FS11;7N2;7S2;EN19;FS8;FS7;FS6;ES4;3M21;3C7;3M6;3MR6;EN21;ES21;FN15;4C8;TB34;2C10;CC1;CCR1;5N6;5S6;5N7;LCR1;7C1;4MR7;6S8;4M8";//Sixth batch
                    List<string> specialSlabShapesList = new List<string>(specialSlabShapes.Split(';'));
                    if (specialSlabShapesList.Contains(shapecodename.ToUpper().Trim()))
                    {
                        newProductMark = objTactonHelper_new.GenerateSlabProductMarking(structureMarking,  productMark, out machineCheck, out bendingCheck, out transportCheck, out errorMessage);
                    }
                    else
                    {
                        newProductMark = objTactonHelper_new.GenerateSlabProductMarking(structureMarking, productMark, out machineCheck, out bendingCheck, out transportCheck, out errorMessage);
                    }
                    //end

                    if (machineCheck == false && errorMessage == "")
                    {
                        throw new Exception("MachineCheck is false,can not able to insert");
                    }
                    if (transportCheck == false && errorMessage == "")
                    {
                        throw new Exception("Transportcheck is false,can not able to insert");
                    }


                    if (machineCheck == true && bendingCheck == "" && transportCheck == true)
                    {
                        if (errorMessage == "")
                        {
                            oldProductMarkId = productMark.ProductMarkId;
                            productMark.BendingCheckInd = false;  //CARPET DEVELOPMENT
                            if (productMark.BendingCheckInd != true)
                            {
                                if (productMark.ProductMarkId != 0)
                                {
                                    productMark.DeleteSlabProductMark(productMark.ProductMarkId, out errorMessage);
                                }
                            }
                            int intProductmarkSlNo = 1;
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
                                    productMark.DeleteSlabProductMark(productMark.ProductMarkId, out errorMessage);
                                }
                                else
                                {
                                    productMark.ProductMarkId = oldProductMarkId;
                                    productMark.DeleteSlabProductMark(productMark.ProductMarkId, out errorMessage);
                                }
                            }



                            List<SlabProduct> productMarkList = productMark.SlabProductByStructureMarkId_Get(structureMarking.StructureMarkId, structureElementId);
                            if (productMarkList != null)
                            {
                                newProductMark = productMarkList.FirstOrDefault(item => item.ProductMarkId == newProductMark.ProductMarkId);
                                productMark = newProductMark;
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
            return newProductMark;
        }
        public SlabProduct SaveProductMarkafterConfirmation(SlabStructure structureMarking, SlabProduct productMark, ref int currentIndex, int structureElementId, int userId, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (productMark.ProductMarkId != 0)
                {
                    productMark.DeleteSlabProductMark(productMark.ProductMarkId, out errorMessage);
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
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            return productMark;
        }

        public List<SlabStructure> RegenerateValidation(List<SlabStructure> structureMarkList, ShapeCodeParameterSet parameterSet, int structureElementId, int projectTypeId, int projectId, int productTypeId, int userId,out string errorMessage)
        {
            
            BeamDetailinfo beamDetailInfo = new BeamDetailinfo();
            List<SlabProduct> products = new List<SlabProduct>();
            SlabStructure structMark = new SlabStructure();
            SlabProduct objProduct = new SlabProduct();
            try
            {
                beamDetailInfo.intStructureElementTypeId = structureElementId;
                beamDetailInfo.GroupMarkPPosting_Regenerate(structureMarkList[0].StructureMarkId, out errorMessage);
                if (errorMessage == "")
                {
                    foreach (SlabStructure objStructureMarking in structureMarkList)
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

                            products = objTactonHelper_new.GenerateSlabStructureMarking(objStructureMarking, projectTypeId, IsSingleMesh, structureElementId, projectId, productTypeId, out errorMessage, out meshSplitting);
                            //LOG THE Slab Logic    Surendar --18-Dec-2012
                            //SlabSplitLog.LogSlabLogic(Convert.ToString(objStructureMarking.SEDetailingID), objStructureMarking.StructureMarkingName, meshSplitting, UserName);
                            if (errorMessage == "")
                            {
                                if (objStructureMarking.StructureMarkId != 0)
                                {
                                    foreach (SlabProduct productMarkDetails in objStructureMarking.SlabProduct)
                                    {

                                        productMarkDetails.DeleteSlabProductMarkByStructureMarkId(objStructureMarking.StructureMarkId,out errorMessage);
                                    }

                                }
                                objStructureMarking.SlabProduct = products;
                                objStructureMarking.ProductGenerationStatus = true;
                                bool result = false;
                                structMark = objStructureMarking;
                                result = structMark.Save(userId, out errorMessage);
                                if (result == true)
                                {
                                    int intProductmarkSlNo = 1;

                                    foreach (SlabProduct productMark in objStructureMarking.SlabProduct)
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
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                structMark = null;
                objProduct = null;
            }
            return structureMarkList;
        }

        public bool DeleteProductMarking(int ProductMarkId, int seDetailingId, out string errorMessage)
        {
            BeamDetailinfo beamDetailInfo = new BeamDetailinfo();
            bool isSuccess = false;
            errorMessage = "";
            SlabProduct _slabprod = new SlabProduct();

            try
            {
                beamDetailInfo.GroupMarkPostedValidate_Get(seDetailingId, out errorMessage);

                if (errorMessage == "")
                {
                   // productMark.Deleteflag = 1;
                    isSuccess = _slabprod.DeleteSlabProductMark(ProductMarkId, out errorMessage);
                }

            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }

            return isSuccess;
        }

        public SlabProduct InsertSlabProductMarking(SlabProduct objProductMarkingMarking, int structureElementId, int userId)
        {
            string errorMessage = "";
            try
            {
                bool productResult = false;
                objProductMarkingMarking.ProductMarkId = 0;
                objProductMarkingMarking.ProductMarkingName = objProductMarkingMarking.ProductMarkingName;
                objProductMarkingMarking.BOMDrawingPath = SystemConstant.DSSBOMPDF;
                productResult = objProductMarkingMarking.Save(userId, structureElementId);
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            return objProductMarkingMarking;
        }

        public List<SlabProduct> GetOverHang(int parameterSetNumber, int projectId, int structureElementId, int productTypeId, int mwLength, int cwLength, int mwSpace, int cwSpace, out string errorMessage)
        {
            errorMessage = "";
            List<SlabProduct> listSlabOH = new List<SlabProduct>();
            SlabProduct objSlabOH = new SlabProduct();
            try
            {
                objSlabOH.ProductionMWLength = mwLength;
                objSlabOH.ProductionCWLength = cwLength;
                objSlabOH.MWSpacing = mwSpace;
                objSlabOH.CWSpacing = cwSpace;
                listSlabOH = objSlabOH.GetOverHang(parameterSetNumber, projectId, structureElementId, productTypeId, objSlabOH);
            }
            catch (Exception ex)
            {
                //ExceptionLog.LogException(ex, UserName);
                errorMessage = ex.Message;
            }
            finally
            {
                objSlabOH = null;
            }
            return listSlabOH;
        }

        //added by vidya 
        public bool DeleteProductMarking(SlabProduct productMark, int seDetailingId, out string errorMessage)
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
                    isSuccess = productMark.DeleteSlabProductMark(out errorMessage);
                }

            }
            catch (Exception ex)
            {
                
                errorMessage = ex.Message;
            }

            return isSuccess;
        }


    }


}
