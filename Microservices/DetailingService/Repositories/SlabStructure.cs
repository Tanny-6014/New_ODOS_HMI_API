using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SharedCache.WinServiceCommon.Provider.Cache;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tacton.Configurator.ObjectModel;
using static Dapper.SqlMapper;

namespace DetailingService.Repositories
{
    public class SlabStructure
    {

        // private DetailingApplicationContext _dbContext;
        // private IConfiguration _configuration;

        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";
    //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

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
        public List<SlabProduct> SlabProduct { get; set; }
        public bool ProductGenerationStatus { get; set; }
        public ShapeCodeParameterSet ParameterSet { get; set; }
        public string SideForCode { get; set; }
        public bool ProductSplitUp { get; set; }

        public SlabStructure()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DetailingApplicationContext>();
            optionsBuilder.UseSqlServer(connectionString);


            DetailingApplicationContext _dbContext = new DetailingApplicationContext(optionsBuilder.Options);

            // Or you can also instantiate inside using

          

        }
        //public SlabStructure(DetailingApplicationContext dbContext, IConfiguration configuration)
        //{
        //    _dbContext = dbContext;
        //    _configuration = configuration;
        //    connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        //}       
        public List<SlabStructure> SlabStructureMark_Get(int SeDetailId, int StructureElementTypeId)
        {
           
            List<SlabStructure> SlabStructureList = new List<SlabStructure>();
            DataSet dsSlabStructureMark = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand cmd = new SqlCommand(SystemConstant.SlabStructureMarking_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INTSEDETAILINGID", SeDetailId));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsSlabStructureMark);                  
                  
                   
                    bool BendingCheck = false;
                    bool MachineCheck = false;
                    bool ProduceIndicator = false;
                    bool TransportCheck = false;
                    bool MultiMesh = false;
                    if (dsSlabStructureMark != null && dsSlabStructureMark.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsSlabStructureMark.Tables[0].DefaultView)
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
                            string SlabProductCodeCache = "listSlabProductCode";
                            //if (IndexusDistributionCache.SharedCache.Get(SlabProductCodeCache) == null)
                            //{
                                listProductCode = objProduct.SlabProductCode();
                           
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

                            SlabProduct objSlabProductMark = new SlabProduct();

                            List<SlabProduct> listSlabProductMark = new List<SlabProduct>();


                            listSlabProductMark = objSlabProductMark.SlabProductByStructureMarkId_Get(Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]), StructureElementTypeId);


                            // listSlabProductMark = listSlabProductMark.FindAll(x => x.StructureMarkId == Convert.ToInt32((drvBeam["INTSTRUCTUREMARKID"])));


                            SlabStructure SlabStructure = new SlabStructure
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
                                SlabProduct = listSlabProductMark
                            };
                            //fix me
                            if (listSlabProductMark.Count == 0 ? SlabStructure.ProductGenerationStatus = false : SlabStructure.ProductGenerationStatus = true)
                                SlabStructureList.Add(SlabStructure);
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return SlabStructureList;
        }

        //public List<SlabStructure> SlabStructureMark_Get(int SeDetailId, int StructureElementTypeId)
        //{
        //   // DBManager dbManager = new DBManager();
        //    List<SlabStructure> SlabStructureList = new List<SlabStructure>();
        //    DataSet dsSlabStructureMark = new DataSet();
            
        //    IEnumerable<SlabStructureMarkingDto> slabStructureMarkingDto; //new IEnumerable<List<ProjectMaster>>();

        //    try
        //    {
        //       // var data = null;

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();

        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@INTSEDETAILINGID", SeDetailId);//SeDetailId  Currently added hard code value --Vanita
        //                                                                   //  dsSlabStructureMark = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SlabStructureMarking_Get");

        //            slabStructureMarkingDto = sqlConnection.Query<SlabStructureMarkingDto>(SystemConstant.SlabStructureMarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            //IEnumerable<int> enumerable = Enumerable.Range(1, 300);
                  

                  

        //            // SlabStructureList = sqlConnection.Query<SlabStructure>(SystemConstant.SlabStructureMarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);




        //            if (slabStructureMarkingDto.Count() > 0)
        //            {
        //                DataTable dt = ConvertToDataTable.ToDataTable(slabStructureMarkingDto);
        //                dsSlabStructureMark.Tables.Add(dt);

        //                bool BendingCheck = false;
        //                bool MachineCheck = false;
        //                bool ProduceIndicator = false;
        //                bool TransportCheck = false;
        //                bool MultiMesh = false;

        //                if (dsSlabStructureMark != null && dsSlabStructureMark.Tables.Count != 0)
        //                {
        //                    foreach (DataRowView drvBeam in dsSlabStructureMark.Tables[0].DefaultView)
        //                    {

        //                        if (drvBeam["BITBENDINGCHECK"].ToString().TrimEnd() == "1")
        //                        {
        //                            BendingCheck = true;
        //                        }
        //                        else if (drvBeam["BITBENDINGCHECK"].ToString().TrimEnd() == "0")
        //                        {
        //                            BendingCheck = false;
        //                        }

        //                        if (drvBeam["BITMACHINECHECK"].ToString().ToUpper() == "1")
        //                        {
        //                            MachineCheck = true;
        //                        }
        //                        else if (drvBeam["BITMACHINECHECK"].ToString().ToUpper() == "0")
        //                        {
        //                            MachineCheck = false;
        //                        }

        //                        if (drvBeam["BITTRANSPORTCHECK"].ToString().ToUpper() == "1")
        //                        {
        //                            TransportCheck = true;
        //                        }
        //                        else if (drvBeam["BITTRANSPORTCHECK"].ToString().ToUpper() == "0")
        //                        {
        //                            TransportCheck = false;
        //                        }

        //                        if (drvBeam["PRODUCEINDICATOR"].ToString().ToUpper() == "YES")
        //                        {
        //                            ProduceIndicator = true;
        //                        }
        //                        else if (drvBeam["PRODUCEINDICATOR"].ToString().ToUpper() == "NO")
        //                        {
        //                            ProduceIndicator = false;
        //                        }

        //                        if (drvBeam["BITSINGLEMESH"].ToString().Trim() == "1")
        //                        {
        //                            MultiMesh = false;
        //                        }
        //                        else if (drvBeam["BITSINGLEMESH"].ToString().Trim() == "0")
        //                        {
        //                            MultiMesh = true;
        //                        }

        //                        ProductCode objProduct = new ProductCode();
        //                        //objProduct.ProductCodeId = Convert.ToInt32((drvBeam["INTPRODUCTCODEID"]));
        //                        //objProduct.ProductCodeName = Convert.ToString((drvBeam["VCHPRODUCTCODE"]));

        //                        List<ProductCode> listProductCode = new List<ProductCode>();
        //                        string SlabProductCodeCache = "listSlabProductCode";
        //                        //if (IndexusDistributionCache.SharedCache.Get(SlabProductCodeCache) == null)
        //                        //{
        //                        //    listProductCode = objProduct.SlabProductCode();
        //                        //}
        //                        //else
        //                        //{
        //                        //    listProductCode = IndexusDistributionCache.SharedCache.Get(SlabProductCodeCache) as List<ProductCode>;
        //                        //}
        //                        //listProductCode = listProductCode.FindAll(x => x.ProductCodeId == Convert.ToInt32((drvBeam["INTPRODUCTCODEID"])));
        //                        //condition
        //                        listProductCode = objProduct.SlabProductCode();

        //                        if (listProductCode.Count != 0)
        //                        {
        //                            objProduct.ProductCodeId = Convert.ToInt32((drvBeam["INTPRODUCTCODEID"]));
        //                            objProduct.ProductCodeName = Convert.ToString((drvBeam["VCHPRODUCTCODE"]));
        //                            objProduct.MainWireDia = listProductCode[0].MainWireDia;
        //                            objProduct.MainWireSpacing = listProductCode[0].MainWireSpacing;
        //                            objProduct.WeightArea = listProductCode[0].WeightArea;
        //                            objProduct.WeightPerMeterRun = listProductCode[0].WeightPerMeterRun;
        //                            objProduct.CrossWireSpacing = listProductCode[0].CrossWireSpacing;
        //                            // objProduct.MinLinkFactor = listProductCode[0].MinLinkFactor;
        //                            //objProduct.MaxLinkFactor = listProductCode[0].MaxLinkFactor;
        //                            //objProduct.WeightArea = listProductCode[0].WeightArea;
        //                            objProduct.CwDia = listProductCode[0].CwDia;
        //                            objProduct.CwWeightPerMeterRun = listProductCode[0].CwWeightPerMeterRun;
        //                        }

        //                        SlabProduct objSlabProductMark = new SlabProduct();

        //                        List<SlabProduct> listSlabProductMark = new List<SlabProduct>();


        //                         listSlabProductMark = objSlabProductMark.SlabProductByStructureMarkId_Get(Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]), StructureElementTypeId);


        //                        // listSlabProductMark = listSlabProductMark.FindAll(x => x.StructureMarkId == Convert.ToInt32((drvBeam["INTSTRUCTUREMARKID"])));


        //                        SlabStructure SlabStructure = new SlabStructure
        //                        {
        //                            SEDetailingID = SeDetailId,
        //                            StructureMarkId = Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]),
        //                            StructureMarkingName = Convert.ToString(drvBeam["VCHSTRUCTUREMARKINGNAME"]),
        //                            ParamSetNumber = Convert.ToInt32(drvBeam["TNTPARAMSETNUMBER"]),
        //                            MainWireLength = Convert.ToInt32(drvBeam["DECTOTALMESHMAINLENGTH"]),
        //                            CrossWireLength = Convert.ToInt32(drvBeam["DECTOTALMESHCROSSLENGTH"]),
        //                            MemberQty = Convert.ToInt32(drvBeam["INTMEMBERQTY"]),
        //                            BendingCheck = BendingCheck,
        //                            MachineCheck = MachineCheck,
        //                            TransportCheck = TransportCheck,
        //                            ProductCode = objProduct,
        //                            MultiMesh = MultiMesh,
        //                            ProduceIndicator = ProduceIndicator,
        //                            PinSize = Convert.ToInt32(drvBeam["SITPINSIZE"]),
        //                            SideForCode = drvBeam["SIDEFORCODE"].ToString(),
        //                            ProductSplitUp = Convert.ToBoolean(drvBeam["PRODUCTSPLITUP"]),
        //                            SlabProduct = listSlabProductMark,
        //                            ProductGenerationStatus = listSlabProductMark.Count == 0 ? false : true
        //                        };
        //                        //fix me
        //                        // if (listSlabProductMark.Count == 0 ? SlabStructure.ProductGenerationStatus = false : SlabStructure.ProductGenerationStatus = true)
        //                        SlabStructureList.Add(SlabStructure);
        //                    }

        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return SlabStructureList;
        //}

        public bool Save(int UserId, out string errorMessage)
        {
            bool isSuccess = false;
            int intStructureMarkId = 0;
            errorMessage = "";
            // DBManager dbManager = new DBManager();
            SlabStructure slabStructure = new SlabStructure();
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", StructureMarkId);
                    dynamicParameters.Add("@intSEDetailingId", SEDetailingID);
                    dynamicParameters.Add("@intProductCodeId", ProductCode.ProductCodeId);                    
                    dynamicParameters.Add("@vchStructureMarkingName", StructureMarkingName);
                    dynamicParameters.Add("@tntParamSetNumber", ParamSetNumber);
                    dynamicParameters.Add("@decTotalMeshMainLength", MainWireLength);
                    dynamicParameters.Add("@decTotalMeshCrossLength", CrossWireLength);
                    dynamicParameters.Add("@intMemberQty", MemberQty);
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
                        //intStructureMarkId = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_SlabStructureMarking_InsertsUpdate_PRC");
                        intStructureMarkId = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.SlabStructureMarking_InsertsUpdate_PRC, dynamicParameters, commandType: CommandType.StoredProcedure);
                    }
                    else
                    { //Manager.ExecuteScalar(CommandType.StoredProcedure, "usp_SlabStructureMarking_InsertsUpdate");
                        intStructureMarkId = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.SlabStructureMarking_InsertsUpdate, dynamicParameters, commandType: CommandType.StoredProcedure);

                    }
                    // Result - 0 Posted
                    //          1 Duplicate
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
                        StructureMarkId = Convert.ToInt32(intStructureMarkId);
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

        public bool DeleteSlabStructureMark(int StructureMarkId)
        {
            bool isSuccess = false;
            
            
            int Postedvalidate;
             string errorMessage = "";
            try
            {
               
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);
                    Postedvalidate = sqlConnection.QueryFirstOrDefault <int>(SystemConstant.SlabStructureMark_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

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
                        throw new Exception("Error in deleting Slab Structure");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return isSuccess;
        }

        //public bool DeleteSlabStructureMark(int StructureMarkId, out string errorMessage)
        //{
        //    bool isSuccess = false;

        //    object Postedvalidate = null;
        //    errorMessage = "";
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {
        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);
        //            // Postedvalidate = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_SlabStructureMark_Delete");
        //            Postedvalidate = sqlConnection.QueryFirstOrDefault<Object>(SystemConstant.SlabStructureMark_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

        //            if (Convert.ToInt32(Postedvalidate) == 0)
        //            {
        //                errorMessage = "POSTED";
        //            }
        //            else if (Convert.ToInt32(Postedvalidate) == 1)
        //            {
        //                isSuccess = true;
        //            }
        //            else
        //            {
        //                throw new Exception("Error in deleting Slab Structure");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return isSuccess;
        //}

        public List<SlabStructure> SlabStructureMark_Get_byDto(SlabStructureMarkingDto obj, int SeDetailId)
        {

            List<SlabStructure> SlabStructureList = new List<SlabStructure>();
            DataSet dsSlabStructureMark = new DataSet();

            IEnumerable<SlabStructureMarkingDto> slabStructureMarkingDto;

            List<SlabStructureMarkingDto> test = new List<SlabStructureMarkingDto>();

            test.Add(obj);

            slabStructureMarkingDto = test.AsEnumerable();



            if (slabStructureMarkingDto.Count() > 0)
            {
                DataTable dt = ConvertToDataTable.ToDataTable(slabStructureMarkingDto);
                dsSlabStructureMark.Tables.Add(dt);

                bool BendingCheck = false;
                bool MachineCheck = false;
                bool ProduceIndicator = false;
                bool TransportCheck = false;
                bool MultiMesh = false;

                if (dsSlabStructureMark != null && dsSlabStructureMark.Tables.Count != 0)
                {
                    foreach (DataRowView drvBeam in dsSlabStructureMark.Tables[0].DefaultView)
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
                        string SlabProductCodeCache = "listSlabProductCode";
                        //if (IndexusDistributionCache.SharedCache.Get(SlabProductCodeCache) == null)
                        //{
                        //    listProductCode = objProduct.SlabProductCode();
                        //}
                        //else
                        //{
                        //    listProductCode = IndexusDistributionCache.SharedCache.Get(SlabProductCodeCache) as List<ProductCode>;
                        //}
                        //listProductCode = listProductCode.FindAll(x => x.ProductCodeId == Convert.ToInt32((drvBeam["INTPRODUCTCODEID"])));
                        //condition
                        listProductCode = objProduct.SlabProductCode();

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

                        SlabProduct objSlabProductMark = new SlabProduct();

                        List<SlabProduct> listSlabProductMark = new List<SlabProduct>();


                        // listSlabProductMark = objSlabProductMark.SlabProductByStructureMarkId_Get(Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]), StructureElementTypeId);


                        // listSlabProductMark = listSlabProductMark.FindAll(x => x.StructureMarkId == Convert.ToInt32((drvBeam["INTSTRUCTUREMARKID"])));


                        SlabStructure SlabStructure = new SlabStructure
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
                            SlabProduct = listSlabProductMark,
                            ProductGenerationStatus = listSlabProductMark.Count == 0 ? false : true
                        };
                        //fix me
                        // if (listSlabProductMark.Count == 0 ? SlabStructure.ProductGenerationStatus = false : SlabStructure.ProductGenerationStatus = true)
                        SlabStructureList.Add(SlabStructure);
                    }

                }

            }

            return SlabStructureList;
        }






      

    }



}




