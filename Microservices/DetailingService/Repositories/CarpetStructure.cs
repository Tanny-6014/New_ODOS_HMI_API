using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Data;

namespace DetailingService.Repositories
{
    public class CarpetStructure
    {
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;

        #region Properties

        public int SEDetailingID { get; set; }
        public int StructureMarkId { get; set; }
        public int ParentStructureMarkId { get; set; }
        public string StructureMarkingName { get; set; }
        public int ParamSetNumber { get; set; }
        public int MainWireLength { get; set; }
        public int CrossWireLength { get; set; }
        public int MemberQty { get; set; }
        public bool BendingCheck { get; set; }
        public bool MachineCheck { get; set; }
        public bool TransportCheck { get; set; }
        public ProductCode ProductCode { get; set; }
        public bool MultiMesh { get; set; }
        public bool ProduceIndicator { get; set; }
        public int PinSize { get; set; }
        public List<CarpetProduct> ?CarpetProduct { get; set; }
        public bool ProductGenerationStatus { get; set; }
        public ShapeCodeParameterSet ?ParameterSet { get; set; }
        public string SideForCode { get; set; }
        public bool ProductSplitUp { get; set; }

        #endregion


        public CarpetStructure()
        {

        }
        //public CarpetStructure(DetailingApplicationContext dbContext, IConfiguration configuration)
        //{   _dbContext = dbContext;
        //    _configuration = configuration;
        //    connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        //}

        public List<CarpetStructure> CarpetStructureMark_Get(int SeDetailId, int StructureElementTypeId)
        {
            //DBManager dbManager = new DBManager();
            List<CarpetStructure> CarpetStructureList = new List<CarpetStructure>();
            DataSet dsCarpetStructureMark = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    
                    SqlCommand cmd = new SqlCommand(SystemConstant.CarpetStructureMarking_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INTSEDETAILINGID", SeDetailId));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsCarpetStructureMark);


                    bool BendingCheck = false;
                    bool MachineCheck = false;
                    bool ProduceIndicator = false;
                    bool TransportCheck = false;
                    bool MultiMesh = false;
                    if (dsCarpetStructureMark != null && dsCarpetStructureMark.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsCarpetStructureMark.Tables[0].DefaultView)
                        {

                            if (drvBeam["BITBENDINGCHECK"].ToString().TrimEnd() == "1")
                            {
                                BendingCheck = true;
                            }
                            else if (drvBeam["BITBENDINGCHECK"].ToString().TrimEnd() == "0")
                            {
                                BendingCheck = false;
                            }

                            if (drvBeam["BITMACHINECHECK"].ToString().ToUpper() == "1")
                            {
                                MachineCheck = true;
                            }
                            else if (drvBeam["BITMACHINECHECK"].ToString().ToUpper() == "0")
                            {
                                MachineCheck = false;
                            }

                            if (drvBeam["BITTRANSPORTCHECK"].ToString().ToUpper() == "1")
                            {
                                TransportCheck = true;
                            }
                            else if (drvBeam["BITTRANSPORTCHECK"].ToString().ToUpper() == "0")
                            {
                                TransportCheck = false;
                            }

                            if (drvBeam["PRODUCEINDICATOR"].ToString().ToUpper() == "YES")
                            {
                                ProduceIndicator = true;
                            }
                            else if (drvBeam["PRODUCEINDICATOR"].ToString().ToUpper() == "NO")
                            {
                                ProduceIndicator = false;
                            }

                            if (drvBeam["BITSINGLEMESH"].ToString().Trim() == "1")
                            {
                                MultiMesh = false;
                            }
                            else if (drvBeam["BITSINGLEMESH"].ToString().Trim() == "0")
                            {
                                MultiMesh = true;
                            }

                            ProductCode objProduct = new ProductCode();
                            //objProduct.ProductCodeId = Convert.ToInt32((drvBeam["INTPRODUCTCODEID"]));
                            //objProduct.ProductCodeName = Convert.ToString((drvBeam["VCHPRODUCTCODE"]));

                            List<ProductCode> listProductCode = new List<ProductCode>();
                            string CarpetProductCodeCache = "listCarpetProductCode";
                            
                            listProductCode = objProduct.CarpetProductCode_New(); 
                           
                            listProductCode = listProductCode.FindAll(x => x.ProductCodeId == Convert.ToInt32((drvBeam["INTPRODUCTCODEID"])));
                            //condition
                            if (listProductCode.Count != 0)
                            {
                                objProduct.ProductCodeId = Convert.ToInt32((drvBeam["INTPRODUCTCODEID"]));
                                objProduct.ProductCodeName = Convert.ToString((drvBeam["VCHPRODUCTCODE"]));
                                objProduct.MainWireDia = listProductCode[0].MainWireDia;
                                objProduct.MainWireSpacing = listProductCode[0].MainWireSpacing;
                                objProduct.WeightArea = listProductCode[0].WeightArea;
                                objProduct.WeightPerMeterRun = listProductCode[0].WeightPerMeterRun;
                                objProduct.CrossWireSpacing = listProductCode[0].CrossWireSpacing;
                                // objProduct.MinLinkFactor = listProductCode[0].MinLinkFactor;
                                //objProduct.MaxLinkFactor = listProductCode[0].MaxLinkFactor;
                                //objProduct.WeightArea = listProductCode[0].WeightArea;
                                objProduct.CwDia = listProductCode[0].CwDia;
                                objProduct.CwWeightPerMeterRun = listProductCode[0].CwWeightPerMeterRun;
                            }

                            CarpetProduct objCarpetProductMark = new CarpetProduct();

                            List<CarpetProduct> listCarpetProductMark = new List<CarpetProduct>();


                            listCarpetProductMark = objCarpetProductMark.CarpetProductByStructureMarkId_Get(Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]), StructureElementTypeId);


                            // listCarpetProductMark = listCarpetProductMark.FindAll(x => x.StructureMarkId == Convert.ToInt32((drvBeam["INTSTRUCTUREMARKID"])));


                            CarpetStructure CarpetStructure = new CarpetStructure
                            {
                                SEDetailingID = SeDetailId,
                                StructureMarkId = Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]),
                                StructureMarkingName = Convert.ToString(drvBeam["VCHSTRUCTUREMARKINGNAME"]),
                                ParamSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
                                MainWireLength = Convert.ToInt32(drvBeam["DECTOTALMESHMAINLENGTH"]),
                                CrossWireLength = Convert.ToInt32(drvBeam["DECTOTALMESHCROSSLENGTH"]),
                                MemberQty = Convert.ToInt32(drvBeam["INTMEMBERQTY"]),
                                BendingCheck = BendingCheck,
                                MachineCheck = MachineCheck,
                                TransportCheck = TransportCheck,
                                ProductCode = objProduct,
                                MultiMesh = MultiMesh,
                                ProduceIndicator = ProduceIndicator,
                                PinSize = Convert.ToInt32(drvBeam["SITPINSIZE"]),
                                SideForCode = drvBeam["SIDEFORCODE"].ToString(),
                                ProductSplitUp = Convert.ToBoolean(drvBeam["PRODUCTSPLITUP"]),
                                CarpetProduct = listCarpetProductMark
                            };
                            //fix me
                            if (listCarpetProductMark.Count == 0 ? CarpetStructure.ProductGenerationStatus = false : CarpetStructure.ProductGenerationStatus = true)
                                CarpetStructureList.Add(CarpetStructure);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return CarpetStructureList;
        }

        public bool Save(int UserId, out string errorMessage)
        {
            bool isSuccess = false;
            int intStructureMarkId = 0;
            errorMessage = "";
            //  DBManager dbManager = new DBManager();
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", StructureMarkId);
                    dynamicParameters.Add("@intSEDetailingId", SEDetailingID);
                    dynamicParameters.Add("@intProductCodeId", this.ProductCode.ProductCodeId);
                    dynamicParameters.Add("@vchStructureMarkingName", this.StructureMarkingName);
                    dynamicParameters.Add("@tntParamSetNumber", this.ParamSetNumber);
                    dynamicParameters.Add("@decTotalMeshMainLength", this.MainWireLength);
                    dynamicParameters.Add("@decTotalMeshCrossLength", this.CrossWireLength);
                    dynamicParameters.Add("@intMemberQty", this.MemberQty);
                    if (BendingCheck == true)
                    {
                        dynamicParameters.Add("@bitBendingCheck", 1);
                    }
                    else
                    {
                        dynamicParameters.Add("@bitBendingCheck", 0);
                    }
                    if (MachineCheck == true)
                    {
                        dynamicParameters.Add("@bitMachineCheck", 1);
                    }
                    else
                    {
                        dynamicParameters.Add("@bitMachineCheck", 0);
                    }
                    if (TransportCheck == true)
                    {
                        dynamicParameters.Add("@bitTransportCheck", 1);
                    }
                    else
                    {
                        dynamicParameters.Add("@bitTransportCheck", 0);
                    }
                    if (MultiMesh == true)
                    {
                        dynamicParameters.Add("@bitSingleMesh", 0);
                    }
                    else
                    {
                        dynamicParameters.Add("@bitSingleMesh", 1);
                    }
                    if (ProduceIndicator == true)
                    {
                        dynamicParameters.Add("@chProduceIndicator", "Yes");
                    }
                    else
                    {
                        dynamicParameters.Add("@chProduceIndicator", "No");
                    }
                    dynamicParameters.Add("@INTUSERID", UserId);

                    dynamicParameters.Add("@INTPARENTSTRUCTUREMARKID", ParentStructureMarkId);

                    if (ProductSplitUp == true)
                    {
                        dynamicParameters.Add("@PRODUCTSPLITUP", 1);
                    }
                    else
                    {
                        dynamicParameters.Add("@PRODUCTSPLITUP", 0);
                    }

                    if (ParentStructureMarkId != 0)
                    {
                        // CARPET
                        // intStructureMarkId = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_CarpetStructureMarking_InsertsUpdate");
                        intStructureMarkId =sqlConnection.QueryFirstOrDefault<int>(SystemConstant.CarpetStructureMarking_InsertsUpdate, dynamicParameters, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        // intStructureMarkId = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_CarpetStructureMarking_InsertsUpdate");
                        intStructureMarkId = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.CarpetStructureMarking_InsertsUpdate, dynamicParameters, commandType: CommandType.StoredProcedure);

                    }
                    // Result - 0 Posted
                    //          1 Duplicate

                    //CARPET

                    if (Convert.ToInt32(intStructureMarkId) == 15)
                    {
                        throw new Exception("Please check MAX Mainwire length.");
                    }

                    if (Convert.ToInt32(intStructureMarkId) == 16)
                    {
                        throw new Exception("Please check MIN Mainwire length.");
                    }

                    if (Convert.ToInt32(intStructureMarkId) == 13)
                    {
                        throw new Exception("Please check MAX Crosswire length.");
                    }

                    if (Convert.ToInt32(intStructureMarkId) == 11)
                    {
                        throw new Exception("Please check MIN Crosswire length.");
                    }

                    if (Convert.ToInt32(intStructureMarkId) == 14)
                    {
                        throw new Exception("Please check Roll mesh diameter.");
                    }

                    if (intStructureMarkId != null && Convert.ToInt32(intStructureMarkId) == 0)
                    {
                        errorMessage = "POSTED";
                    }
                    else if (intStructureMarkId != null && Convert.ToInt32(intStructureMarkId) == 1)
                    {
                        errorMessage = "DUPLICATE";
                    }
                    else if (intStructureMarkId != null && Convert.ToInt32(intStructureMarkId) > 1)
                    {
                        this.StructureMarkId = Convert.ToInt32(intStructureMarkId);
                        isSuccess = true;
                    }
                    else
                    {
                        throw new Exception("Could not insert/update structure marking");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return isSuccess;
        }

        public bool DeleteCarpetStructureMark(out string errorMessage)
        {
            bool isSuccess = false;
           // DBManager dbManager = new DBManager();
            int Postedvalidate = 0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", this.StructureMarkId);
                    //Postedvalidate = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_CarpetStructureMark_Delete");
                    Postedvalidate =sqlConnection.QueryFirstOrDefault<int>(SystemConstant.CarpetStructureMark_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (Convert.ToInt32(Postedvalidate) == 0)
                    {
                        errorMessage = "POSTED";
                    }
                    else if (Convert.ToInt32(Postedvalidate) == 1)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        throw new Exception("Error in deleting Carpet Structure");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }           
            return isSuccess;
        }
    }
}
