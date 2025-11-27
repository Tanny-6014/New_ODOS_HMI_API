using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Data;

namespace DetailingService.Repositories
{
    public class ColumnStructure
    {
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        public int SEDetailingID { get; set; }
        public int StructureMarkId { get; set; }
        public string StructureMarkingName { get; set; }
        public int ParamSetNumber { get; set; }
        public int MemberQty { get; set; }
        public int ColumnWidth { get; set; }
        public int ColumnLength { get; set; }
        public int ColumnHeight { get; set; }
        public int TotalNoOfLinks { get; set; }
        public int ColumnShapeId { get; set; }
        public bool IsCLink { get; set; }
        public int ClinkProductCodeIdAtLength { get; set; }
        public int ClinkShapeCodeIdAtLength { get; set; }
        public int ClinkProductCodeIdAtWidth { get; set; }
        public int ClinkShapeCodeIdAtWidth { get; set; }
        public bool CLOnly { get; set; }
        public double Area { get; set; }
        public int TotalQty { get; set; }
        public int ColumnProductCodeId { get; set; }
        public int PinSize { get; set; }
        public bool ProduceIndicator { get; set; }
        public List<ColumnProduct> ColumnProducts { get; set; }
        public ProductCode ProductCode { get; set; }
        public ShapeCode Shape { get; set; }
        public int RowatLength { get; set; }
        public int RowatWidth { get; set; }
        //public ShapeCode shapeCode { get; set; }
        public ProductCode ClinkProductLength { get; set; }
        public ProductCode ClinkProductWidth { get; set; }
        public bool ProductGenerationStatus { get; set; }
        private Int32 _parentStructureMarkId; //Added for PRC
        public Int32 ParentStructureMarkId
        {
            get { return _parentStructureMarkId; }
            set { _parentStructureMarkId = value; }
        }

        public string SideForCode { get; set; }
        public bool BendingCheckInd { get; set; }

        public ColumnStructure()
        {          

        }

        public List<ColumnStructure> ColumnStructureMark_Get(int ProjectId, int SeDetailId, int StructureElementTypeId, int ProductTypeId)
        {
            //DBManager dbManager = new DBManager();
            List<ColumnStructure> columnStructureList = new List<ColumnStructure>();
            DataSet dsColumnStructureMark = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();                    
                    IEnumerable<ColumnStructureMarkDto> columnStructureMarkDto;
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProjectId", ProjectId);
                    dynamicParameters.Add("@intSEDetailingId", SeDetailId);
                    // dsColumnStructureMark = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ColumnStructureMark_Get");
                    columnStructureMarkDto = sqlConnection.Query<ColumnStructureMarkDto>(SystemConstant.ColumnStructureMark_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    // dsStructureMark.Tables[0].DefaultView.Sort = "intstructuremarkid DESC"; 
                    bool boolCLOnly = false;
                    bool boolIsCLink = false;
                    bool boolProduceIndicator = false;
                    if (columnStructureMarkDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(columnStructureMarkDto);
                        dsColumnStructureMark.Tables.Add(dt);

                        if (dsColumnStructureMark != null && dsColumnStructureMark.Tables.Count != 0)
                        {
                            foreach (DataRowView drvBeam in dsColumnStructureMark.Tables[0].DefaultView)
                            {

                                if (drvBeam["CLOnly"].ToString().TrimEnd() == "1")
                                {
                                    boolCLOnly = true;
                                }
                                else if (drvBeam["CLOnly"].ToString().TrimEnd() == "0")
                                {
                                    boolCLOnly = false;
                                }

                                if (drvBeam["ISCLINK"].ToString().Trim().ToUpper() == "YES")
                                {
                                    boolIsCLink = true;
                                }
                                else if (drvBeam["ISCLINK"].ToString().Trim().ToUpper() == "NO")
                                {
                                    boolIsCLink = false;
                                }
                                if (drvBeam["PRODUCEINDICATOR"].ToString().Trim().ToUpper() == "YES")
                                {
                                    boolProduceIndicator = true;
                                }
                                else if (drvBeam["PRODUCEINDICATOR"].ToString().Trim().ToUpper() == "NO")
                                {
                                    boolProduceIndicator = false;
                                }


                                ProductCode objProduct = new ProductCode();
                                //objProduct.ProductCodeId = Convert.ToInt32((drvBeam["intBeamProductCodeId"]));
                                //objProduct.ProductCodeName = Convert.ToString((drvBeam["BeamProduct"]));

                                List<ProductCode> listProductCode = new List<ProductCode>();
                                string ColumnProductCodeCache = "ColumnProductCodeCache" + StructureElementTypeId.ToString() + ProductTypeId.ToString();
                                //if (IndexusDistributionCache.SharedCache.Get(ColumnProductCodeCache) == null)
                                //{
                                listProductCode = objProduct.ColumnProductCodeBind(StructureElementTypeId, ProductTypeId);
                                //}
                                //else
                                //{
                                //    listProductCode = IndexusDistributionCache.SharedCache.Get(ColumnProductCodeCache) as List<ProductCode>;
                                //}
                                listProductCode = listProductCode.FindAll(x => x.ProductCodeId == Convert.ToInt32((drvBeam["INTCOLUMNPRODUCTCODEID"])));
                                //condition
                                if (listProductCode.Count > 0)
                                {
                                    objProduct.ProductCodeId = Convert.ToInt32((drvBeam["INTCOLUMNPRODUCTCODEID"]));
                                    objProduct.ProductCodeName = Convert.ToString((drvBeam["COLUMNPRODUCT"]));
                                    objProduct.MainWireDia = listProductCode[0].MainWireDia;
                                    objProduct.MainWireSpacing = listProductCode[0].MainWireSpacing;
                                    objProduct.WeightArea = listProductCode[0].WeightArea;
                                    objProduct.WeightPerMeterRun = listProductCode[0].WeightPerMeterRun;
                                    objProduct.MinLinkFactor = listProductCode[0].MinLinkFactor;
                                    objProduct.MaxLinkFactor = listProductCode[0].MaxLinkFactor;
                                    objProduct.WeightArea = listProductCode[0].WeightArea;
                                    objProduct.CwDia = listProductCode[0].CwDia;
                                    objProduct.CwWeightPerMeterRun = listProductCode[0].CwWeightPerMeterRun;
                                }
                                ShapeCode objShapeCode = new ShapeCode();
                                objShapeCode.ShapeID = Convert.ToInt32(drvBeam["INTCOLUMNSHAPEID"]);
                                objShapeCode.ShapeCodeName = Convert.ToString(drvBeam["COLUMNSHAPE"]);

                                //ShapeParameter objShapeParam = new ShapeParameter();
                                ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();
                                ShapeParameter listShapeParameterForBeam = new ShapeParameter { };
                                // if (IndexusDistributionCache.SharedCache.Get("ColumnShapeparamCache") == null)
                                // {
                                shapeParameterCollection = shapeParameterCollection.ColumnShapeParameter_Get();

                                //}
                                //else
                                //{
                                //    shapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("ColumnShapeparamCache");
                                //}
                                ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();



                                foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
                                {
                                    if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["INTCOLUMNSHAPEID"]))
                                    {

                                        strucParamCollFilter.Add(shapeParamCollection);
                                    }

                                }

                                objShapeCode.ShapeParam = strucParamCollFilter;


                                ProductCode objClinkProductLength = new ProductCode();
                                objClinkProductLength.ProductCodeId = Convert.ToInt32((drvBeam["INTCLINKPRODUCTCODEIDATLENGTH"]));
                                objClinkProductLength.ProductCodeName = Convert.ToString((drvBeam["CLINKPRODUCTLEN"]));

                                ProductCode objClinkProductWidth = new ProductCode();
                                objClinkProductWidth.ProductCodeId = Convert.ToInt32((drvBeam["INTCLINKPRODUCTCODEIDATWIDTH"]));
                                objClinkProductWidth.ProductCodeName = Convert.ToString((drvBeam["CLINKPRODUCTWIDTH"]));


                                ColumnProduct objColumnProductMark = new ColumnProduct();

                                List<ColumnProduct> listColumnProductMark = new List<ColumnProduct>();


                                listColumnProductMark = objColumnProductMark.ColumnProductByStructureMarkId_Get(Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]));


                                // listColumnProductMark = listColumnProductMark.FindAll(x => x.StructureMarkId == Convert.ToInt32((drvBeam["intStructureMarkId"])));


                                ColumnStructure columnStructure = new ColumnStructure
                                {
                                    StructureMarkId = Convert.ToInt32(drvBeam["INTSTRUCTUREMARKID"]),
                                    StructureMarkingName = Convert.ToString(drvBeam["VCHSTRUCTUREMARKINGNAME"]),
                                    MemberQty = Convert.ToInt32(drvBeam["INTMEMBERQTY"]),
                                    ColumnWidth = Convert.ToInt32(drvBeam["NUMCOLUMNWIDTH"]),
                                    ColumnLength = Convert.ToInt32(drvBeam["NUMCOLUMNLENGTH"]),
                                    ColumnHeight = Convert.ToInt32(drvBeam["NUMCOLUMNHEIGHT"]),
                                    RowatLength = Convert.ToInt32(drvBeam["INTROWSATLENGTH"]),
                                    RowatWidth = Convert.ToInt32(drvBeam["INTROWSATWIDTH"]),
                                    CLOnly = boolCLOnly,
                                    PinSize = Convert.ToInt32(drvBeam["PINSIZE"]),
                                    IsCLink = boolIsCLink,
                                    ProduceIndicator = boolProduceIndicator,
                                    ProductCode = objProduct,
                                    Shape = objShapeCode,
                                    ClinkProductLength = objClinkProductLength,
                                    ClinkProductWidth = objClinkProductWidth,
                                    ColumnProducts = listColumnProductMark,
                                    TotalNoOfLinks = Convert.ToInt32(drvBeam["INTNOOFLINKS"]),
                                    SideForCode = drvBeam["SIDEFORCODE"].ToString(),
                                    BendingCheckInd = Convert.ToBoolean(drvBeam["BITBENDINGCHECK"])
                                };
                                if (listColumnProductMark.Count == 0 ? columnStructure.ProductGenerationStatus = false : columnStructure.ProductGenerationStatus = true)
                                    columnStructureList.Add(columnStructure);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return columnStructureList;
        }

        public bool DeleteColumnStructuremark( int StructureMarkId,out string errorMessage)
        {
            bool isSuccess = false;
           // DBManager dbManager = new DBManager();
            object Postedvalidate = null;
            errorMessage = "";
            int Output = 0;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    //Postedvalidate = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_ColumnStructureMarkByStructId_Delete");
                    sqlConnection.Query<int>(SystemConstant.ColumnStructureMarkByStructId_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();

                    if (Convert.ToInt32(Output) == 0)
                    {
                        errorMessage = "POSTED";
                    }
                    else if (Convert.ToInt32(Output) == 1)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        throw new Exception("Error in deleting Column Product");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return isSuccess;
        }

        public bool Save(int UserId, out string errorMessage)
        {
            bool isSuccess = false;
            //DBManager dbManager = new DBManager();
            int intStructureMarkId =0;
            errorMessage = "";
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intSEDetailingId", SEDetailingID);
                    dynamicParameters.Add("@vchStructureMarkingName", StructureMarkingName);
                    dynamicParameters.Add("@tntParamSetNumber", this.ParamSetNumber);
                    dynamicParameters.Add("@intMemberQty", this.MemberQty);
                    dynamicParameters.Add("@numColumnWidth", this.ColumnWidth);
                    dynamicParameters.Add("@numColumnLength", this.ColumnLength);
                    dynamicParameters.Add("@numColumnHeight", this.ColumnHeight);
                    dynamicParameters.Add("@intNoofLinks", this.TotalNoOfLinks);
                    dynamicParameters.Add("@intRowsAtLength", this.RowatLength);
                    dynamicParameters.Add("@intRowsAtWidth", this.RowatWidth);
                    dynamicParameters.Add("@intColumnProductCodeId", this.ProductCode.ProductCodeId);
                    dynamicParameters.Add("@intColumnShapeId", this.Shape.ShapeID);
                    dynamicParameters.Add("@bitIsCLink", this.IsCLink);
                    dynamicParameters.Add("@intCLinkProductCodeIdatLen", this.ClinkProductLength.ProductCodeId);
                    dynamicParameters.Add("@intCLinkProductCodeIdatWidth", this.ClinkProductWidth.ProductCodeId);
                    if (this.ProduceIndicator == true)
                    {
                        dynamicParameters.Add("@chProduceIndicator", "Yes");
                    }
                    else
                    {
                        dynamicParameters.Add("@chProduceIndicator", "No");
                    }

                    dynamicParameters.Add("@intPinsize", this.PinSize);
                    dynamicParameters.Add("@intuserid", UserId);

                    dynamicParameters.Add("@INTPARENTSTRUCTUREMARKID", this.ParentStructureMarkId);
                    if (this.BendingCheckInd == true)
                    {
                        dynamicParameters.Add("@BENDINGCHECK", 1);
                    }
                    else
                    {
                        dynamicParameters.Add("@BENDINGCHECK", 0);
                    }
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    if (ParentStructureMarkId != 0)
                    {
                        //intStructureMarkId = dbManager.ExecuteScalar(CommandType.StoredProcedure, "Usp_ColumnStructureMarking_InsUpd_PRC");
                        //intStructureMarkId = sqlConnection.Query<int>(SystemConstant.ColumnStructureMarking_InsUpd_PRC, dynamicParameters, commandType: CommandType.StoredProcedure);
                        sqlConnection.Query<int>(SystemConstant.ColumnStructureMarking_InsUpd_PRC, dynamicParameters, commandType: CommandType.StoredProcedure);

                        intStructureMarkId = dynamicParameters.Get<int>("@Output");
                        sqlConnection.Close();
                    }
                    else
                    {
                        //intStructureMarkId = dbManager.ExecuteScalar(CommandType.StoredProcedure, "Usp_columnstructuremarking_insert");
                       // intStructureMarkId = sqlConnection.Query<int>(SystemConstant.columnstructuremarking_insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                         sqlConnection.Query<int>(SystemConstant.columnstructuremarking_insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                        intStructureMarkId = dynamicParameters.Get<int>("@Output");
                        sqlConnection.Close();
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
                        this.StructureMarkId = Convert.ToInt32(intStructureMarkId);
                        isSuccess = true;
                    }
                    else
                    {
                        throw new Exception("Could not insert/update structure marking");
                    }

                    // object intStructureMarkId = dbManager.ExecuteScalar(CommandType.StoredProcedure, "Usp_columnstructuremarking_insert");
                    //this.StructureMarkId = Convert.ToInt32(intStructureMarkId);
                    //isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return isSuccess;
        }

        public bool Update(int UserId, out string errorMessage)
        {
            bool isSuccess = false;
           // DBManager dbManager = new DBManager();
            object intStructureMarkId = null;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", this.StructureMarkId);
                    dynamicParameters.Add("@tntParamSetNumber", this.ParamSetNumber);
                    dynamicParameters.Add("@intMemberQty", this.MemberQty);
                    dynamicParameters.Add("@numColumnWidth", this.ColumnWidth);
                    dynamicParameters.Add("@numColumnLength", this.ColumnLength);
                    dynamicParameters.Add("@numColumnHeight", this.ColumnHeight);
                    dynamicParameters.Add("@intNoofLinks", this.TotalNoOfLinks);
                    dynamicParameters.Add("@intRowsAtLength", this.RowatLength);
                    dynamicParameters.Add("@intRowsAtWidth", this.RowatWidth);
                    dynamicParameters.Add("@intColumnProductCodeId", this.ProductCode.ProductCodeId);
                    dynamicParameters.Add("@intColumnShapeId", this.Shape.ShapeID);
                    dynamicParameters.Add("@bitIsCLink", this.IsCLink);
                    dynamicParameters.Add("@intCLinkProductCodeIdatLen", this.ClinkProductLength.ProductCodeId);
                    dynamicParameters.Add("@intCLinkProductCodeIdatWidth", this.ClinkProductWidth.ProductCodeId);
                    if (this.ProduceIndicator == true)
                    {
                        dynamicParameters.Add("@chProduceIndicator", "Yes");
                    }
                    else
                    {
                        dynamicParameters.Add("@chProduceIndicator", "No");
                    }
                    dynamicParameters.Add("@intPinsize", this.PinSize);
                    dynamicParameters.Add("@intuserid", UserId);
                    dynamicParameters.Add("@VCHSTRUCTUREMARKINGNAME", StructureMarkingName);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    if (this.BendingCheckInd == true)
                    {
                        dynamicParameters.Add("@BENDINGCHECK", 1);
                    }
                    else
                    {
                        dynamicParameters.Add("@BENDINGCHECK", 0);
                    }
                    // intStructureMarkId = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_ColumnStructureMarking_Update");
                     sqlConnection.Query<int>(SystemConstant.ColumnStructureMarking_Update, dynamicParameters, commandType: CommandType.StoredProcedure);
                    intStructureMarkId = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();
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
                        intStructureMarkId = Convert.ToInt32(intStructureMarkId);
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



    }
}
