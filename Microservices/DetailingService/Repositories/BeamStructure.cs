
using Dapper;
using DetailingService.Constants;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using SharedCache.WinServiceCommon.Provider.Cache;
using System.Data;

namespace DetailingService.Repositories
{
    public class BeamStructure
    {
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";


        #region "Variables"
        private string _structureMarkName;
        private ProductCode _productCode;
        private ShapeCode _shape;
        private Int32 _width;
        private Int32 _depth;
        private Int32 _slope;
        private Int32 _stirupps;
        private Int32 _qty;
        private Int32 _span;
        private bool _iscap;
        private ProductCode _capProduct;
        private bool _produceInd;
        private Int32 _pinSize;
        private List<BeamProduct> _productMark;
        private Int32 _structureMarkId;
        private bool _productGenerationStatus;
        private Int32 _parentStructureMarkId; //Added for PRC
        public string _sideForCode;    // Added for SideFor
        public bool _bendCheckStatus;
        public bool _bendingCheckInd;
        #endregion

        public BeamStructure()
        {
        }

        # region "Properties"
        public BeamStructure(string structureMarkName, ProductCode productCode, ShapeCode shape, int width, int depth, int slope, int stirupps, int qty, int span, bool Iscap, ProductCode capProduct, bool produceInd, int pinSize, List<BeamProduct> productMark, int structureMarkId, bool productGenerationStatus, string sideForCode, bool bendCheckStatus, bool bendingCheckInd)
        {
            _structureMarkName = structureMarkName;
            _productCode = productCode;
            _shape = shape;
            _width = width;
            _depth = depth;
            _slope = slope;
            _stirupps = stirupps;
            _qty = qty;
            _span = span;
            _iscap = Iscap;
            _capProduct = capProduct;
            _produceInd = produceInd;
            _pinSize = pinSize;
            _productMark = productMark;
            _structureMarkId = structureMarkId;
            _productGenerationStatus = productGenerationStatus;
            _sideForCode = sideForCode;
            _bendCheckStatus = bendCheckStatus;
            _bendingCheckInd = bendingCheckInd;
        }
        public Int32 ParentStructureMarkId
        {
            get { return _parentStructureMarkId; }
            set { _parentStructureMarkId = value; }
        }
        public string StructureMarkName
        {
            get { return _structureMarkName; }
            set { _structureMarkName = value; }
        }

        public ProductCode ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; }
        }

        public ShapeCode Shape
        {
            get { return _shape; }
            set { _shape = value; }
        }

        public Int32 Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public Int32 Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        public Int32 Slope
        {
            get { return _slope; }
            set { _slope = value; }
        }

        public Int32 Stirupps
        {
            get { return _stirupps; }
            set { _stirupps = value; }
        }

        public Int32 Qty
        {
            get { return _qty; }
            set { _qty = value; }
        }

        public Int32 Span
        {
            get { return _span; }
            set { _span = value; }
        }

        public bool IsCap
        {
            get { return _iscap; }
            set { _iscap = value; }
        }

        public ProductCode CapProduct
        {
            get { return _capProduct; }
            set { _capProduct = value; }
        }

        public bool ProduceInd
        {
            get { return _produceInd; }
            set { _produceInd = value; }
        }

        public Int32 PinSize
        {
            get { return _pinSize; }
            set { _pinSize = value; }
        }
        public List<BeamProduct> ProductMark
        {
            get { return _productMark; }
            set { _productMark = value; }
        }
        public Int32 StructureMarkId
        {
            get { return _structureMarkId; }
            set { _structureMarkId = value; }
        }

        public string SideForCode
        {
            get { return _sideForCode; }
            set { _sideForCode = value; }
        }

        public bool ProductGenerationStatus
        {
            get { return _productGenerationStatus; }
            set { _productGenerationStatus = value; }
        }

        public bool BendingCheckStatus
        {
            get { return _bendCheckStatus; }
            set { _bendCheckStatus = value; }
        }

        public bool BendingCheckInd
        {
            get { return _bendingCheckInd; }
            set { _bendingCheckInd = value; }
        }

        #endregion

        #region "Beam Structure"

        public List<BeamStructure> BCDStructureMarking_Get(string GroupMarkName, int ProjectId, int SeDetailingId, int StructureElementTypeId, int ProductTyprId, int GroupMarkId)
        {
           // DBManager dbManager = new DBManager();
            List<BeamStructure> listStructureMarking = new List<BeamStructure>();

            DataSet dsStructureMark = new DataSet();
            IEnumerable<BeamStructureMarkListDto> beamStructureMarkListDto;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();


                    //Hardcoded value//
                    dynamicParameters.Add("@vchGroupMarkingName", GroupMarkName);
                    dynamicParameters.Add("@intProjectId", ProjectId);
                    dynamicParameters.Add("@inSEDetailingId", SeDetailingId);
                    beamStructureMarkListDto = sqlConnection.Query<BeamStructureMarkListDto>(SystemConstant.StructureMarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    // dsStructureMark.Tables[0].DefaultView.Sort = "intstructuremarkid DESC"; 
                    bool boolIsCap = false;
                    bool boolProduceInd = false;
                    if (beamStructureMarkListDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(beamStructureMarkListDto);
                        dsStructureMark.Tables.Add(dt);
                    }
                        if (dsStructureMark != null && dsStructureMark.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsStructureMark.Tables[0].DefaultView)
                        {

                            if (drvBeam["ProduceIndicator"].ToString().TrimEnd().ToUpper() == "YES")
                            {
                                boolProduceInd = true;
                            }
                            else if (drvBeam["ProduceIndicator"].ToString().TrimEnd().ToUpper() == "NO")
                            {
                                boolProduceInd = false;
                            }

                            if (drvBeam["ISCap"].ToString() == "Yes")
                            {
                                boolIsCap = true;
                            }
                            else if (drvBeam["ISCap"].ToString() == "No")
                            {
                                boolIsCap = false;
                            }
                            ProductCode objProduct = new ProductCode();
                            //objProduct.ProductCodeId = Convert.ToInt32((drvBeam["intBeamProductCodeId"]));
                            //objProduct.ProductCodeName = Convert.ToString((drvBeam["BeamProduct"]));

                            List<ProductCode> listProductCode = new List<ProductCode>();

                           // if (IndexusDistributionCache.SharedCache.Get("CacheProuctCode") == null)
                           // {
                                listProductCode = objProduct.ProductCodeForBeam_Get(StructureElementTypeId, ProductTyprId);

                            //}
                            //else
                            //{
                            //    listProductCode = IndexusDistributionCache.SharedCache.Get("CacheProuctCode") as List<ProductCode>;
                            //}
                            listProductCode = listProductCode.FindAll(x => x.ProductCodeId == Convert.ToInt32((drvBeam["intBeamProductCodeId"])));
                            //condition
                            if (listProductCode.Count > 0)
                            {
                                objProduct.ProductCodeId = Convert.ToInt32((drvBeam["intBeamProductCodeId"]));
                                objProduct.ProductCodeName = Convert.ToString((drvBeam["BeamProduct"]));
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
                            objShapeCode.ShapeID = Convert.ToInt32(drvBeam["intBeamShapeId"]);
                            objShapeCode.ShapeCodeName = Convert.ToString(drvBeam["BeamShape"]);

                            //ShapeParameter objShapeParam = new ShapeParameter();
                            ShapeParameterCollection shapeParameterCollection = new ShapeParameterCollection();
                            ShapeParameter listShapeParameterForBeam = new ShapeParameter { };
                           // if (IndexusDistributionCache.SharedCache.Get("ShapeparamCache") == null)
                           // {
                                shapeParameterCollection = shapeParameterCollection.ShapeParameterForBeam_Get();

                            //}
                            //else
                            //{
                            //    shapeParameterCollection = (ShapeParameterCollection)IndexusDistributionCache.SharedCache.Get("ShapeparamCache");
                            //}
                            ShapeParameterCollection strucParamCollFilter = new ShapeParameterCollection();



                            foreach (ShapeParameter shapeParamCollection in shapeParameterCollection)
                            {
                                if (shapeParamCollection.ShapeId == Convert.ToInt32(drvBeam["intBeamShapeId"]))
                                {
                                    strucParamCollFilter.Add(shapeParamCollection);
                                }
                            }

                            objShapeCode.ShapeParam = strucParamCollFilter;


                            ProductCode objCapProduct = new ProductCode();
                            objCapProduct.ProductCodeId = Convert.ToInt32((drvBeam["intCappingProductCodeId"]));
                            objCapProduct.ProductCodeName = Convert.ToString((drvBeam["CapProduct"]));


                            BeamProduct objProductMark = new BeamProduct();

                            List<BeamProduct> listProductMark = new List<BeamProduct>();

                            int structureMarkId;
                            structureMarkId = Convert.ToInt32((drvBeam["intStructureMarkId"]));

                            listProductMark = objProductMark.ProductMarkingByGroupMarkId_Get(structureMarkId);

                            /* Commented By Prem */
                            //if (IndexusDistributionCache.SharedCache.Get("CacheProuctMark") == null)
                            //{
                            //    listProductMark = objProductMark.ProductMarkingByGroupMarkId_Get(GroupMarkId, ProjectId, SeDetailingId);

                            //}
                            //else
                            //{
                            //    listProductMark = IndexusDistributionCache.SharedCache.Get("CacheProuctMark") as List<BeamProduct>;
                            //}
                            //listProductMark = listProductMark.FindAll(x => x.StructureMarkID == Convert.ToInt32((drvBeam["intStructureMarkId"])));

                            List<BeamStructure> structureMarkList = new List<BeamStructure>();

                            listStructureMarking.Add(new BeamStructure(Convert.ToString(drvBeam["vchStructureMarkingName"]), objProduct, objShapeCode, Convert.ToInt32(drvBeam["numBeamWidth"]), Convert.ToInt32(drvBeam["numBeamDepth"]), Convert.ToInt32(drvBeam["numBeamSlope"]), Convert.ToInt32(drvBeam["intTotalStirrups"]), Convert.ToInt32(drvBeam["intMemberQty"]), Convert.ToInt32(drvBeam["intClearSpan"]), boolIsCap, objCapProduct, boolProduceInd, Convert.ToInt32(drvBeam["PinSize"]), listProductMark, Convert.ToInt32(drvBeam["intStructureMarkId"]), true, "0", true, false));
                            //listStructureMarking.Add(new StructureMarkEntry(Convert.ToString(drvBeam["vchStructureMarkingName"]), Convert.ToInt32(drvBeam["intBeamProductCodeId"]), Convert.ToInt32(drvBeam["intBeamShapeId"]), Convert.ToInt32(drvBeam["numBeamWidth"]), Convert.ToInt32(drvBeam["numBeamDepth"]), Convert.ToInt32(drvBeam["numBeamSlope"]), Convert.ToInt32(drvBeam["intTotalStirrups"]), Convert.ToInt32(drvBeam["intMemberQty"]), Convert.ToInt32(drvBeam["intClearSpan"]), boolIsCap, Convert.ToInt32(drvBeam["CapProduct"]), boolProduceInd, Convert.ToInt32(drvBeam["PinSize"])));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               
            }
            return listStructureMarking;

        }

        public bool Save(int SeDetailingId, out string errorMessage)
        {
            bool isSuccess = false;
            
            object intStructureMarkId = null;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", StructureMarkId);
                    dynamicParameters.Add("@intSEDetailingId", SeDetailingId);
                    dynamicParameters.Add("@vchStructureMarkingName", this.StructureMarkName);
                    dynamicParameters.Add("@intMemberQty", this.Qty);
                    dynamicParameters.Add("@numBeamWidth", this.Width);
                    dynamicParameters.Add("@numBeamDepth", this.Depth);
                    dynamicParameters.Add("@numBeamSlope", this.Slope);
                    dynamicParameters.Add("@intClearSpan", this.Span);
                    dynamicParameters.Add("@intTotalStirrups", this.Stirupps);
                    dynamicParameters.Add("@intBeamProductCodeId", this.ProductCode.ProductCodeId);
                    dynamicParameters.Add( "@intBeamShapeId", this.Shape.ShapeID);
                    dynamicParameters.Add( "@bitIsCapping", this.IsCap);
                    dynamicParameters.Add( "@intCappingProductCodeId", this.CapProduct.ProductCodeId);
                    dynamicParameters.Add( "@sitPinSize", this.PinSize);
                    dynamicParameters.Add( "@AssemblyIndicator", 1);
                    if (ProduceInd == true)
                    {
                        dynamicParameters.Add( "@ProduceIndicator", "Yes");
                    }
                    else if (ProduceInd == false)
                    {
                        dynamicParameters.Add( "@ProduceIndicator", "No");
                    }


                    dynamicParameters.Add( "@INTPARENTSTRUCTUREMARKID", this.ParentStructureMarkId);
                    //added by anuran CHG0031334 >>>
                    if (this.BendingCheckInd == true)
                    {
                        dynamicParameters.Add( "@BENDINGCHECK", 1);
                    }
                    else
                    {
                        if ((Shape.ShapeCodeName == "5M1B") || (Shape.ShapeCodeName == "5M1RB"))
                        {
                            dynamicParameters.Add( "@BENDINGCHECK", 1);
                        }
                        else
                        {
                            dynamicParameters.Add( "@BENDINGCHECK", 0);
                        }
                    }
                    //added by anuran CHG0031334 <<<

                    if (ParentStructureMarkId != 0)
                    {
                        object syncLock = new object();
                        lock (syncLock)
                        {
                            //   dsStructureMark = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.StructureMarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                            dynamic result = sqlConnection.Query<int>(SystemConstant.StructureMarkingDetailsInsUpd_PRC, dynamicParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                            intStructureMarkId = result;
                        }
                    }
                    else
                    {
                        object syncLock = new object();
                        lock (syncLock)
                        {
                            dynamic result = sqlConnection.Query<int>(SystemConstant.StructureMarkingDetailsInsert, dynamicParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                            intStructureMarkId = result;
                        }
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //dbManager.Dispose();
            }
            return isSuccess;
        }

        public bool DeleteStructuremark(int StructureMarkId, out string errorMessage)
        {
            bool isSuccess = false;
           // DBManager dbManager = new DBManager();
            object Postedvalidate = null;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSTRUCTUREMARKID", StructureMarkId);                    
                    
                     dynamic result= sqlConnection.Query<int>(SystemConstant.StrucutreMarkByStructId_Delete, dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                   
                      Postedvalidate = result;
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
                        throw new Exception("Error in deleting Beam Product");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //dbManager.Dispose();
            }
            return isSuccess;
        }

        #endregion
    }
}
