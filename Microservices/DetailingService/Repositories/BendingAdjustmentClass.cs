using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class BendingAdjustmentClass
    {

        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";


        public int ProductionMO1 { get; set; }
        public int ProductionMO2 { get; set; }
        public int PinSize { get; set; }
        public int ProductionMWLength { get; set; }
        public int ProductionCWLength { get; set; }
        public double MWDiameter { get; set; }
        public double CWDiameter { get; set; }
        public int MWSpacing { get; set; }
        public int CWSpacing { get; set; }
        public string WireType { get; set; }
        public string WireSpacing { get; set; }
        public int StuctureElementTypeId { get; set; }

        public BendingAdjustmentClass()
        {

        }
       
        public string ParamValuesByProductMarkId_Get(int ProductMarkId, string StructureElementType)
        {
          
            string paramValues = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add( "@ProductMarkId", ProductMarkId);
                    dynamicParameters.Add( "@StructureElementType", StructureElementType);
                   // paramValues = Convert.ToString(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_ParamValuesByProductMarkId_Get"));
                    paramValues = sqlConnection.QueryFirstOrDefault<string>(SystemConstant.ParamValuesByProductMarkId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return paramValues;
        }

        public object ProductMarkingDetailsForIrregularBOMByProductMarkId_Get(int ProductMarkId, string StructureElementType)
        {
          
            BendingAdjustmentClass objProductMarkDetails = null;
            DataSet dsProductMarkDetails = new DataSet();
            IEnumerable<ProductMarkingDetailsForIrregularBOMDto>productMarkingDetails;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@ProductMarkId", ProductMarkId);
                    dynamicParameters.Add("@StructureElementType", StructureElementType);
                    // dsProductMarkDetails = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ProductDetailsForIrregularBOMByProductMarkId_Get");
                    productMarkingDetails = sqlConnection.Query<ProductMarkingDetailsForIrregularBOMDto>(SystemConstant.ProductDetailsForIrregularBOMByProductMarkId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsProductMarkDetails != null && dsProductMarkDetails.Tables.Count != 0)
                    {
                        foreach (DataRowView drvProductMarkDetails in dsProductMarkDetails.Tables[0].DefaultView)
                        {
                            objProductMarkDetails = new BendingAdjustmentClass
                            {
                                ProductionMO1 = Convert.ToInt32(drvProductMarkDetails["INTPRODUCTIONMO1"]),
                                ProductionMO2 = Convert.ToInt32(drvProductMarkDetails["INTPRODUCTIONMO2"]),
                                PinSize = Convert.ToInt32(drvProductMarkDetails["INTPINSIZEID"]),
                                ProductionMWLength = Convert.ToInt32(drvProductMarkDetails["NUMPRODUCTIONMWLENGTH"]),
                                ProductionCWLength = Convert.ToInt32(drvProductMarkDetails["NUMPRODUCTIONCWLENGTH"]),
                                MWDiameter = Convert.ToInt32(drvProductMarkDetails["DECMWDIAMETER"]),
                                CWDiameter = Convert.ToInt32(drvProductMarkDetails["DECCWDIAMETER"]),
                                MWSpacing = Convert.ToInt32(drvProductMarkDetails["NUMMWSPACING"]),
                                CWSpacing = Convert.ToInt32(drvProductMarkDetails["NUMCWSPACING"]),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objProductMarkDetails;
        }

        public List<BendingAdjustmentClass> WireTypeAndWireSpacingforIrregularBOM_Get(int ProductMarkId, string StructureElementType)
        {
          
            List<BendingAdjustmentClass> listBOMDetails = new List<BendingAdjustmentClass> { };
            DataSet dsBOMDetails = new DataSet();
            IEnumerable<WireTypeAndWireSpacingforIrregularBOMDto>wireTypeAndWireSpaceDto;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add( "@ProductMarkId", ProductMarkId);
                    dynamicParameters.Add( "@StructureElementType", StructureElementType);
                    //dsBOMDetails = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_WireTypeAndSpacingByProductMarkId_Get");
                    wireTypeAndWireSpaceDto =sqlConnection.Query<WireTypeAndWireSpacingforIrregularBOMDto>(SystemConstant.WireTypeAndSpacingByProductMarkId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    if (wireTypeAndWireSpaceDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(wireTypeAndWireSpaceDto);
                        dsBOMDetails.Tables.Add(dt);
                    }

                    if (dsBOMDetails != null && dsBOMDetails.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBOMDetails in dsBOMDetails.Tables[0].DefaultView)
                        {
                            BendingAdjustmentClass bomDetails = new BendingAdjustmentClass
                            {
                                WireSpacing = drvBOMDetails["WireSpacing"].ToString(),
                                WireType = drvBOMDetails["WireType"].ToString()
                            };
                            listBOMDetails.Add(bomDetails);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listBOMDetails;
        }

        public int StructureElementTypeIdByStructureElementType_Get(string StructureElementType)
        {
          
            int StructureElementTypeId = 0;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@StructureElementType", StructureElementType);
                    //StructureElementTypeId = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_StructureElementTypeIdByStructureElementType_Get"));
                    StructureElementTypeId = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.StructureElementTypeIdByStructureElementType_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StructureElementTypeId;
        }

        public int BOMDetailsforIrregularSpacing_Insert(int ProductMarkId, int StructureElementTypeId, string MWPitch, int MWFlag, string CWPitch, int CWFlag, int UserId, string bomType)
        {
          
            List<BendingAdjustmentClass> listBOMDetails = new List<BendingAdjustmentClass> { };
            DataSet dsBOMDetails = new DataSet();
           
            int output = 0;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTPRODUCTMARKINGID", ProductMarkId);
                    dynamicParameters.Add("@INTSTRUCTUREELEMENTTYPEID", StructureElementTypeId);
                    dynamicParameters.Add("@MWPITCH", MWPitch);
                    dynamicParameters.Add("@MWFLAG", MWFlag);
                    dynamicParameters.Add("@CWPITCH", CWPitch);
                    dynamicParameters.Add("@CWFLAG", CWFlag);
                    dynamicParameters.Add("@NVCHBOMTYPE", 'P');
                    dynamicParameters.Add("@INTSTATUSID", 1);
                    dynamicParameters.Add("@INTUSERID", UserId);
                    dynamicParameters.Add("@BOMTYPE", bomType);
                  //  output = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_GenerateBOMDetailsBendCheck_Insert"));
                    output = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.GenerateBOMDetailsBendCheck_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return output;
        }

        public int BendingCheckFailed_Insert(int ProductMarkId, int StructureElementTypeId)
        {
          
            List<BendingAdjustmentClass> listBOMDetails = new List<BendingAdjustmentClass> { };
            DataSet dsBOMDetails = new DataSet();
            int output = 0;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add( "@INTPRODUCTMARKINGID", ProductMarkId);
                    dynamicParameters.Add( "@INTSTRUCTUREELEMENTTYPEID", StructureElementTypeId);
                   // output = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_BendingCheckFail_Insert"));
                    output = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.BendingCheckFail_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return output;
        }
    }

}
