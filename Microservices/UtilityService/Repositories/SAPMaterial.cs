using Dapper;

using Microsoft.Data.SqlClient;
using System.Data;
using UtilityService.Dtos;
using UtilityService.Constants;

namespace UtilityService.Repositories
{
    public class SAPMaterial
    {
       
        private readonly IConfiguration _configuration;
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";


        #region " Properties"

        public int MaterialCodeID { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public string BaseUOM { get; set; }
        public Int32 ExternalWidth { get; set; }
        public Int32 ExternalHeight { get; set; }
        public Int32 ExternalLength { get; set; }
        public double UnitWeight { get; set; }
        public string Material { get; set; }
        public string StructElement { get; set; }
        public string PRODH { get; set; }
        public bool BitACC { get; set; }
        public bool BitRM { get; set; }
        public bool BitStock { get; set; }
        public bool BitFG { get; set; }

        #endregion

        public SAPMaterial()
        {

        }
       
        #region "Get SAP Materials"

        public List<SAPMaterial> GetSAPMaterial(string sapMaterialCode)
        {
            
            List<SAPMaterial> listGetSAPMaterial = new List<SAPMaterial> { };
            DataSet dsGetSAPMaterial = new DataSet();
            IEnumerable<GetSapmaterialdto> GetSapmaterialdto;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SAPMaterialCode", sapMaterialCode);
                    //dsGetSAPMaterial = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SAPMaterialsMaster_Get");
                    GetSapmaterialdto = sqlConnection.Query<GetSapmaterialdto>(SystemConstant.SAPMaterialsMaster_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (GetSapmaterialdto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(GetSapmaterialdto);
                        dsGetSAPMaterial.Tables.Add(dt);
                    }

                    if (dsGetSAPMaterial != null && dsGetSAPMaterial.Tables.Count != 0)
                    {
                        foreach (DataRowView drvSAPMaterial in dsGetSAPMaterial.Tables[0].DefaultView)
                        {
                            SAPMaterial sapMaterial = new SAPMaterial
                            {
                                MaterialCodeID = Convert.ToInt32(drvSAPMaterial["MATERIALCODEID"]),
                                MaterialCode = drvSAPMaterial["MATERIALCODE"].ToString(),
                                MaterialDescription = drvSAPMaterial["MATERIALDESCRIPTION"].ToString(),
                                BaseUOM = drvSAPMaterial["BASE_UOM"].ToString(),
                                ExternalWidth = Convert.ToInt32(drvSAPMaterial["INTUNITWEIGHT"]),
                                ExternalHeight = Convert.ToInt32(drvSAPMaterial["NUMEXTERNALHEIGHT"]),
                                ExternalLength = Convert.ToInt32(drvSAPMaterial["NUMEXTERNALLENGTH"]),
                                UnitWeight = Convert.ToDouble(drvSAPMaterial["INTUNITWEIGHT"])
                            };
                            listGetSAPMaterial.Add(sapMaterial);
                        }
                    }
                }
                


            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return listGetSAPMaterial;
        }

        public List<SAPMaterial> GetSAPMaterialforPRCByStructureElementId(int intStructureElementId)
        {
            List<SAPMaterial> listSAPMaterialDetails = new List<SAPMaterial> { };
            IEnumerable<SAPMaterialforPRCDto> SAPMaterialforPRCDto;


            DataSet dsGetSAPMaterial = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@INTSTRUCTUREELEMENTID", intStructureElementId);
                    // dsGetSAPMaterial = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SAPMaterialByStructureElementId_Get");
                    SAPMaterialforPRCDto = sqlConnection.Query<SAPMaterialforPRCDto>(SystemConstant.SAPMaterialByStructureElementId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (SAPMaterialforPRCDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(SAPMaterialforPRCDto);
                        dsGetSAPMaterial.Tables.Add(dt);
                    }

                    if (dsGetSAPMaterial != null && dsGetSAPMaterial.Tables.Count != 0)
                    {
                        foreach (DataRowView drvSAPMaterial in dsGetSAPMaterial.Tables[0].DefaultView)
                        {
                            SAPMaterial sapMaterial = new SAPMaterial
                            {
                                MaterialCodeID = Convert.ToInt32(drvSAPMaterial["MATERIALCODEID"]),
                                MaterialCode = drvSAPMaterial["MATERIALCODE"].ToString()
                            };
                            listSAPMaterialDetails.Add(sapMaterial);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }          

            return listSAPMaterialDetails;
        }

        public List<SAPMaterial> GetSAPMaterialMasterDetailsForValidDatas()
        {
            
            List<object> objSAPMaterial = new List<object>();
            List<SAPMaterial> listGetCorrectedSAPMaterial = new List<SAPMaterial> { };
            DataSet dsGetSAPMaterial = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    // dsGetSAPMaterial = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SAPMaterialDetailsForValidDatas_Get");
                    dsGetSAPMaterial = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.SAPMaterialDetailsForValidDatas_Get,  commandType: CommandType.StoredProcedure);

                    if (dsGetSAPMaterial != null && dsGetSAPMaterial.Tables.Count != 0)
                    {
                        foreach (DataRowView drvSAPMaterial in dsGetSAPMaterial.Tables[0].DefaultView)
                        {
                            SAPMaterial sapMaterial = new SAPMaterial
                            {
                                MaterialCodeID = Convert.ToInt32(drvSAPMaterial["MATERIALCODEID"]),
                                MaterialCode = drvSAPMaterial["VCHMATERIALCODE"].ToString(),
                                Material = drvSAPMaterial["MATERIAL"].ToString(),
                                StructElement = drvSAPMaterial["VCHSTRUCTELEMENT"].ToString(),
                                BaseUOM = drvSAPMaterial["VCHUOM"].ToString(),
                                BitACC = Convert.ToBoolean(drvSAPMaterial["BITACC"]),
                                BitFG = Convert.ToBoolean(drvSAPMaterial["BITFG"]),
                                BitRM = Convert.ToBoolean(drvSAPMaterial["BITRM"]),
                                BitStock = Convert.ToBoolean(drvSAPMaterial["BITSTOCK"]),
                                ExternalWidth = Convert.ToInt32(drvSAPMaterial["NUMEXTERNALWIDTH"]),
                                ExternalHeight = Convert.ToInt32(drvSAPMaterial["NUMEXTERNALHEIGHT"]),
                                ExternalLength = Convert.ToInt32(drvSAPMaterial["NUMEXTERNALLENGTH"]),
                                UnitWeight = Convert.ToDouble(drvSAPMaterial["INTUNITWEIGHT"]),
                                PRODH = drvSAPMaterial["PRODH"].ToString()
                            };
                            listGetCorrectedSAPMaterial.Add(sapMaterial);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGetCorrectedSAPMaterial;
        }

        public List<SAPMaterial> GetSAPMaterialMasterDetailsForInValidDatas()
        {
            
            List<SAPMaterial> listGetInvalidSAPMaterial = new List<SAPMaterial> { };
            DataSet dsGetSAPMaterial = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    //dsGetSAPMaterial = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_SAPMaterialDetailsForInvalidDatas_Get");
                    dsGetSAPMaterial = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.SAPMaterialDetailsForInvalidDatas_Get,  commandType: CommandType.StoredProcedure);

                    if (dsGetSAPMaterial != null && dsGetSAPMaterial.Tables.Count != 0)
                    {
                        foreach (DataRowView drvSAPMaterial in dsGetSAPMaterial.Tables[0].DefaultView)
                        {
                            SAPMaterial sapMaterial = new SAPMaterial
                            {
                                MaterialCodeID = Convert.ToInt32(drvSAPMaterial["MATERIALCODEID"]),
                                MaterialCode = drvSAPMaterial["VCHMATERIALCODE"].ToString(),
                                Material = drvSAPMaterial["MATERIAL"].ToString(),
                                StructElement = drvSAPMaterial["VCHSTRUCTELEMENT"].ToString(),
                                BaseUOM = drvSAPMaterial["VCHUOM"].ToString(),
                                BitACC = Convert.ToBoolean(drvSAPMaterial["BITACC"]),
                                BitFG = Convert.ToBoolean(drvSAPMaterial["BITFG"]),
                                BitRM = Convert.ToBoolean(drvSAPMaterial["BITRM"]),
                                BitStock = Convert.ToBoolean(drvSAPMaterial["BITSTOCK"]),
                                ExternalWidth = Convert.ToInt32(drvSAPMaterial["NUMEXTERNALWIDTH"]),
                                ExternalHeight = Convert.ToInt32(drvSAPMaterial["NUMEXTERNALHEIGHT"]),
                                ExternalLength = Convert.ToInt32(drvSAPMaterial["NUMEXTERNALLENGTH"]),
                                UnitWeight = Convert.ToDouble(drvSAPMaterial["INTUNITWEIGHT"]),
                                PRODH = drvSAPMaterial["PRODH"].ToString()
                            };
                            listGetInvalidSAPMaterial.Add(sapMaterial);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return listGetInvalidSAPMaterial;
        }

        public List<SAPMaterial> GetStructureElementForSAPMaterial()
        {
            List<SAPMaterial> listStructElement = new List<SAPMaterial>();
            
            DataSet dsSAPMaterial = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    // dsSAPMaterial = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_StructureElementforSAPMaterial_Get");
                    dsSAPMaterial = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.StructureElementforSAPMaterial_Get,  commandType: CommandType.StoredProcedure);

                    if ((dsSAPMaterial != null) && (dsSAPMaterial.Tables.Count != 0))
                    {
                        foreach (DataRowView drvSAPMaterial in dsSAPMaterial.Tables[0].DefaultView)
                        {
                            SAPMaterial StruElement = new SAPMaterial
                            {
                                StructElement = drvSAPMaterial["STRUCTUREELEMENT"].ToString()
                            };
                            listStructElement.Add(StruElement);
                        }
                    }
                }
                return listStructElement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public List<SAPMaterial> GetMaterialForSAPMaterial()
        {
            List<SAPMaterial> listMaterial = new List<SAPMaterial>();
            
            DataSet dsSAPMaterial = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    // dsSAPMaterial = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_MaterialforSAPMaterial_Get");
                    dsSAPMaterial = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.MaterialforSAPMaterial_Get,  commandType: CommandType.StoredProcedure);

                    if ((dsSAPMaterial != null) && (dsSAPMaterial.Tables.Count != 0))
                    {
                        foreach (DataRowView drvSAPMaterial in dsSAPMaterial.Tables[0].DefaultView)
                        {
                            SAPMaterial Material = new SAPMaterial
                            {
                                Material = drvSAPMaterial["MATERIAL"].ToString()
                            };
                            listMaterial.Add(Material);
                        }
                    }
                }
                return listMaterial;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public List<SAPMaterial> GetProDHForSAPMaterial()
        {
            List<SAPMaterial> listPRODH = new List<SAPMaterial>();
            
            DataSet dsSAPMaterial = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    // dsSAPMaterial = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_PRODHforSAPMaterial_Get");
                    dsSAPMaterial = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.PRODHforSAPMaterial_Get,  commandType: CommandType.StoredProcedure);

                    if ((dsSAPMaterial != null) && (dsSAPMaterial.Tables.Count != 0))
                    {
                        foreach (DataRowView drvSAPMaterial in dsSAPMaterial.Tables[0].DefaultView)
                        {
                            SAPMaterial Prodh = new SAPMaterial
                            {
                                PRODH = drvSAPMaterial["PRODH"].ToString()
                            };
                            listPRODH.Add(Prodh);
                        }
                    }
                }
                return listPRODH;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        #endregion

        public bool UpdateSAPMaterialBySAPMaterialCodeId(int UserId)
        {
            bool isSuccess = false;
            
            object Output = null;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTMATERIALCODEID", this.MaterialCodeID);
                    dynamicParameters.Add("@MATERIAL", this.Material);
                    dynamicParameters.Add("@VCHSTRUCTELEMENT", this.StructElement);
                    dynamicParameters.Add("@BITACC", this.BitACC);
                    dynamicParameters.Add("@BITRM", this.BitRM);
                    dynamicParameters.Add("@BITFG", this.BitFG);
                    dynamicParameters.Add("@BITSTOCK", this.BitStock);
                    dynamicParameters.Add("@PRODH", this.PRODH);
                    dynamicParameters.Add("@STRHEIGHT", this.ExternalHeight);
                    dynamicParameters.Add("@STRLENGTH", this.ExternalLength);
                    dynamicParameters.Add("@STRWIDTH", this.ExternalWidth);
                    dynamicParameters.Add("@INTWEIGHT", this.UnitWeight);
                    dynamicParameters.Add("@USERID", UserId);
                    //Output = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_SAPMaterialDetails_Update");
                    Output = sqlConnection.QueryFirstOrDefault<object>(SystemConstant.SAPMaterialDetails_Update, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (Convert.ToInt32(Output) == 1)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false;
                    }
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
    }
}
