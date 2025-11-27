using Dapper;
using DetailingService.Constants;
using DetailingService.Dtos;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DetailingService.Repositories
{
    public class Accessory
    {
        #region "Properties .."
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        public int SEDetailingID { get; set; }
        public int AccProductMarkID { get; set; }
        public int SAPMaterialCodeID { get; set; }
        public int NoOfPieces { get; set; }
        public int CABProductMarkID { get; set; }
        public int GroupMarkId { get; set; }
        public int ActualWeight { get; set; }
        public int ExternalWidth { get; set; }
        public int ExternalHeight { get; set; }
        public int ExternalLength { get; set; }
        public int IsCoupler { get; set; }
        public int OrderQty { get; set; }
        public int CentralizerFlag { get; set; }
        public int Length { get; set; }
        public int BitIsCoupler { get; set; }

        public double UnitWeight { get; set; }
        public double InvoiceWeight { get; set; }

        public string UOM { get; set; }
        public string CouplerType { get; set; }
        public string SAPMaterialCode { get; set; }
        public string CABProductMarkName { get; set; }
        public string AccProductMarkingName { get; set; }
        public string standard { get; set; }
        public string MaterialType { get; set; }

        public string ProductCodeName { get; set; }

        //public CABItem CABParentItem { get; set; }
        //public List<Accessory> AccessoriesList { get; set; }

        #endregion

        #region "Get Accessories"

        public List<Accessory> GetACCProductMarkDetailsBySEDetailingID(int intSEDetailingID)
        {
            List<Accessory> listGetACCProductMarkDetail = new List<Accessory> ();
            DataSet dsGetACCProductMarkDetail = new DataSet();
            IEnumerable<AccessorieesGetDto> AccessorieesGetDto;
            DataSet dsSlabStructureMark = new DataSet();

            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSEDETAILINGID", intSEDetailingID);

                    AccessorieesGetDto = sqlConnection.Query<AccessorieesGetDto>(SystemConstant.Accessories_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (AccessorieesGetDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(AccessorieesGetDto);
                        dsGetACCProductMarkDetail.Tables.Add(dt);
                    }

                    if (dsGetACCProductMarkDetail != null && dsGetACCProductMarkDetail.Tables.Count != 0)
                    {
                        foreach (DataRowView drvACCProductMarkDetail in dsGetACCProductMarkDetail.Tables[0].DefaultView)
                        {
                            Accessory accProductMark = new Accessory
                            {
                                AccProductMarkID = Convert.ToInt32(drvACCProductMarkDetail["INTACCPRODUCTMARKID"]),
                                SEDetailingID = Convert.ToInt32(drvACCProductMarkDetail["INTSEDETAILINGID"]),
                                SAPMaterialCodeID = Convert.ToInt32(drvACCProductMarkDetail["INTSAPMATERIALCODEID"]),
                                NoOfPieces = Convert.ToInt32(drvACCProductMarkDetail["INTNOOFPIECES"]),
                                CABProductMarkID = Convert.ToInt32(drvACCProductMarkDetail["INTCABPRODUCTMARKID"]),
                                GroupMarkId = Convert.ToInt32(drvACCProductMarkDetail["INTGROUPMARKID"]),
                                InvoiceWeight = Convert.ToDouble(drvACCProductMarkDetail["NUMINVOICEWEIGHTPERPC"]),
                                ActualWeight = Convert.ToInt32(drvACCProductMarkDetail["NUMACTUALWEIGHTPERPC"]),
                                ExternalWidth = Convert.ToInt32(drvACCProductMarkDetail["NUMEXTERNALWIDTH"]),
                                ExternalHeight = Convert.ToInt32(drvACCProductMarkDetail["NUMEXTERNALHEIGHT"]),
                                ExternalLength = Convert.ToInt32(drvACCProductMarkDetail["NUMEXTERNALLENGTH"]),
                                OrderQty = Convert.ToInt32(drvACCProductMarkDetail["NUMORDERQTY"]),
                                Length = Convert.ToInt32(drvACCProductMarkDetail["NUMLENGTH"]),
                                UOM = drvACCProductMarkDetail["BASE_UOM"].ToString(),
                                UnitWeight = Convert.ToDouble(drvACCProductMarkDetail["INTUNITWEIGHT"]),
                                AccProductMarkingName = drvACCProductMarkDetail["VCHACCPRODUCTMARKINGNAME"].ToString(),
                                SAPMaterialCode = drvACCProductMarkDetail["MATERIALCODE"].ToString(),
                                CABProductMarkName = drvACCProductMarkDetail["VCHCABPRODUCTMARKNAME"].ToString(),
                                ProductCodeName = drvACCProductMarkDetail["ProductCodeName"].ToString()
                            };
                            listGetACCProductMarkDetail.Add(accProductMark);
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
                //dbManager.Dispose();
            }
            return listGetACCProductMarkDetail;
        }


        public List<Accessory> GetCABItemForAccessoryBySEDetailingID(int intSEDetailingID)
        {
            //DBManager dbManager = new DBManager();
            List<Accessory> listGetCABItem = new List<Accessory> { };
            DataSet dsGetCABItem = new DataSet();
            try
            {
                //dbManager.Open();
                //dbManager.CreateParameters(1);
                //dbManager.AddParameters(0, "@INTSEDETAILINGID", intSEDetailingID);
                //dsGetCABItem = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "USP_CABITEMBYSEDETAILING_GET");
                if (dsGetCABItem != null && dsGetCABItem.Tables.Count != 0)
                {
                    foreach (DataRowView drvCABItem in dsGetCABItem.Tables[0].DefaultView)
                    {
                        Accessory cabItem = new Accessory
                        {
                            CABProductMarkID = Convert.ToInt32(drvCABItem["INTCABPRODUCTMARKID"]),
                            CABProductMarkName = drvCABItem["VCHCABPRODUCTMARKNAME"].ToString()
                        };
                        listGetCABItem.Add(cabItem);
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
            return listGetCABItem;
        }
        #endregion

        #region "Save Accessories"

        public bool Save(out string errorMessage)
        {
            bool isSuccess = false;
            //DBManager dbManager = new DBManager();
            object ReturnValue=null;
            errorMessage = "";

            List<Accessory> listGetACCProductMarkDetail = new List<Accessory>();
            DataSet dsGetACCProductMarkDetail = new DataSet();
            IEnumerable<AccessorieesGetDto> AccessorieesGetDto;
            DataSet dsSlabStructureMark = new DataSet();

            try
            {
                //dbManager.Open();
                //dbManager.CreateParameters(11);
                //dbManager.AddParameters(0, "@INTSEDETAILINGID", SEDetailingID);
                //dbManager.AddParameters(1, "@VCHACCPRODUCTMARKINGNAME", AccProductMarkingName);
                //dbManager.AddParameters(2, "@INTSAPMATERIALCODEID", SAPMaterialCodeID);
                //dbManager.AddParameters(3, "@NUMLENGTH", Length);
                //dbManager.AddParameters(4, "@INTQTY", NoOfPieces);
                //dbManager.AddParameters(5, "@INTCABPRODUCTMARKID", CABProductMarkID);
                //dbManager.AddParameters(6, "@NUMEXTERNALWIDTH", ExternalWidth);
                //dbManager.AddParameters(7, "@NUMEXTERNALHEIGHT", ExternalHeight);
                //dbManager.AddParameters(8, "@NUMEXTERNALLENGTH", ExternalLength);
                //dbManager.AddParameters(9, "@NUMINVOICEWEIGHTPERPC", InvoiceWeight);
                //dbManager.AddParameters(10, "@INTACCPRODUCTMARKID", AccProductMarkID);
                //ReturnValue = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_AccProductMarkingDetails_InsUpd");
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSEDETAILINGID", SEDetailingID);
                    dynamicParameters.Add("@VCHACCPRODUCTMARKINGNAME", AccProductMarkingName);
                    dynamicParameters.Add("@INTSAPMATERIALCODEID", SAPMaterialCodeID);
                    dynamicParameters.Add("@NUMLENGTH", Length);
                    dynamicParameters.Add("@INTQTY", OrderQty);
                    dynamicParameters.Add("@INTCABPRODUCTMARKID", CABProductMarkID);
                    dynamicParameters.Add("@NUMEXTERNALWIDTH", ExternalWidth);
                    dynamicParameters.Add("@NUMEXTERNALHEIGHT", ExternalHeight);
                    dynamicParameters.Add("@NUMEXTERNALLENGTH", ExternalLength);
                    dynamicParameters.Add("@NUMINVOICEWEIGHTPERPC", InvoiceWeight);
                    dynamicParameters.Add("@INTACCPRODUCTMARKID", AccProductMarkID);
                    dynamicParameters.Add("@INTACCPRODUCTMARKID", AccProductMarkID);
                    dynamicParameters.Add("@vchProductCodename", ProductCodeName);


                    dynamic result = sqlConnection.Query<int>(SystemConstant.Accessories_Insert, dynamicParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();

                    ReturnValue = result;

                    if (ReturnValue != null && Convert.ToInt32(ReturnValue) == 0)
                    {
                        errorMessage = "POSTED";
                    }
                    else if (ReturnValue != null && Convert.ToInt32(ReturnValue) == 1)
                    {
                        errorMessage = "DUPLICATE";
                    }
                    else if (ReturnValue != null && Convert.ToInt32(ReturnValue) > 1)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        throw new Exception("Could not insert/update Accessories");
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

        #region "Delete Accessories"

        public bool Delete(out string errorMessage)
        {
            bool isSuccess = false;
            //DBManager dbManager = new DBManager();
            object Postedvalidate = null;
            errorMessage = "";

            List<Accessory> listGetACCProductMarkDetail = new List<Accessory>();
            DataSet dsGetACCProductMarkDetail = new DataSet();
            IEnumerable<AccessorieesGetDto> AccessorieesGetDto;
            DataSet dsSlabStructureMark = new DataSet();

            try
            {
                //dbManager.Open();
                //dbManager.CreateParameters(1);
                //dbManager.AddParameters(0, "@ACCPRODUCTMARKID", this.AccProductMarkID);
                //Postedvalidate = dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_ACCProductMarkDetails_Del");

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ACCPRODUCTMARKID", AccProductMarkID);
                   
                    Postedvalidate = sqlConnection.Query<int>(SystemConstant.Accessories_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

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
                        throw new Exception("Error in deleting Accessory");
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

        #region "Get Arma Couplertype"

        //public ArrayList GetArmaCouplerTypeMaster()
        //{
        //    DBManager dbManager = new DBManager();
        //    ArrayList armaCouplerTypeList = new ArrayList();
        //    DataSet dsArmaCouplerType = new DataSet();
        //    try
        //    {
        //        dbManager.Open();
        //        dsArmaCouplerType = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_ArmaCouplerTypeMaster_Get");
        //        if (dsArmaCouplerType != null && dsArmaCouplerType.Tables.Count != 0)
        //        {
        //            foreach (DataRowView drvACCProductMarkDetail in dsArmaCouplerType.Tables[0].DefaultView)
        //            {
        //                armaCouplerTypeList.Add(drvACCProductMarkDetail["COUPLER_TYPE"]);
        //            }
        //        }
        //        return armaCouplerTypeList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //}

        #endregion
    }
}
