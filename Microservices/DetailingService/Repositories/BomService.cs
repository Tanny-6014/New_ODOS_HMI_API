
using DetailingService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using System.Collections.Generic;
using DetailingService.Context;
using DetailingService.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Dapper;
using DetailingService.Dtos;

namespace DetailingService.Repositories
{
    public class BomService : IBOM
    {
        //private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";

        public int intMO1 { get; set; }
        public int intMO2 { get; set; }
        public int intCO1 { get; set; }
        public int intCO2 { get; set; }

        public decimal MWSequence { get; set; }

        public decimal CWSequence { get; set; }

        public decimal countMW { get; set; }

        public decimal countCW { get; set; }


        public decimal Theoritical { get; set; }
        public decimal NetTon { get; set; }
        public decimal ActualTon { get; set; }
        public decimal Area { get; set; }
        public int numInvoiceMWLength { get; set; }
        public int numInvoiceCWLength { get; set; }
        public int numProductionMWLength { get; set; }
        public int numProductionWeight { get; set; }
        public decimal Actual { get; set; }
        public decimal Net { get; set; }
        public decimal InvoiceTheoriticalWeight { get; set; }
        public decimal ProductionTheoritialWeight { get; set; }
        public string vchProductCode { get; set; }
        public string MarkingName { get; set; }
        public string MeshType { get; set; }
        public string ReleasedStatus { get; set; }
        public decimal decMWDiameter { get; set; }
        public int intMWSpace { get; set; }
        public int intMWLength { get; set; }
        public decimal decMWLength { get; set; }
        public string BOMIndicator { get; set; }
        public string TwinIndicator { get; set; }
        public bool bitStaggeredIndicator { get; set; }
        public int countProduction { get; set; }
        public string ParamValues { get; set; }
        public decimal intCWSpace { get; set; }
        public decimal decCWDiameter { get; set; }
        public decimal decCWLength { get; set; }

        public async Task<IEnumerable<Get_bomDto>> GetBomDetails(int intProductMarkingId, string BOMType, string StructureElement)
        {
            IEnumerable<Get_bomDto> BomDetails; //new IEnumerable<List<ProjectMaster>>();
            List<Get_bomDto> BomList;


            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intProductMarkingId", intProductMarkingId);
                dynamicParameters.Add("@nvchBOMType", BOMType);

                dynamicParameters.Add("@strStructureElement", StructureElement);


                BomDetails = sqlConnection.Query<Get_bomDto>(SystemConstant.Get_bom, dynamicParameters, commandType: CommandType.StoredProcedure);
               
                //BomList = BomDetails.ToList();

                sqlConnection.Close();
                return BomDetails;

            }

        }

        public bool InsertBOM(BOMInsert ObjBOM)
        {
            bool isSuccess = false;

            IEnumerable<int> ReturnValue;
            string errorMessage = "";



            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();



                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", ObjBOM.intProductMarkingId);
                    dynamicParameters.Add("@intDetailingBOMDetailId", ObjBOM.intDetailingBOMDetailId);
                    dynamicParameters.Add("@strStructureElement", ObjBOM.strStructureElement);
                    dynamicParameters.Add("@chrWireType", ObjBOM.chrWireType);
                    dynamicParameters.Add("@strLineNo", ObjBOM.strLineNo);
                    dynamicParameters.Add("@strStartPos", ObjBOM.strStartPos);
                    dynamicParameters.Add("@strNoPths", ObjBOM.strNoPths);
                    dynamicParameters.Add("@strWireSpace", ObjBOM.strWireSpace);
                    dynamicParameters.Add("@strWireLen", ObjBOM.strWireLen);
                    dynamicParameters.Add("@strWireDia", ObjBOM.strWireDia);
                    dynamicParameters.Add("@strRawMaterial", ObjBOM.strRawMaterial);
                    dynamicParameters.Add("@strRepFrom", ObjBOM.strRepFrom);
                    dynamicParameters.Add("@strRepTo", ObjBOM.strRepTo);
                    dynamicParameters.Add("@strRep", ObjBOM.strRep);
                    dynamicParameters.Add("@bitTwinWire", ObjBOM.bitTwinWire);
                    dynamicParameters.Add("@intMO1", ObjBOM.intMO1);
                    dynamicParameters.Add("@intMO2", ObjBOM.intMO2);
                    dynamicParameters.Add("@intCO1", ObjBOM.intCO1);
                    dynamicParameters.Add("@intCO2", ObjBOM.intCO2);
                    dynamicParameters.Add("@intUserId", ObjBOM.intUserId);
                    
                    dynamicParameters.Add("@strBOMType", ObjBOM.BomType);

                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);





                    ReturnValue = sqlConnection.Query<int>(SystemConstant.BOM_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

                    var result = ReturnValue.ToList();

                    var a = Convert.ToInt32(result[0]);

                    //if (ReturnValue != null && Convert.ToInt32(ReturnValue) == 0)
                    //{
                    //    errorMessage = "POSTED";
                    //}
                    //else if (ReturnValue != null && Convert.ToInt32(ReturnValue) == 1)
                    //{
                    //    errorMessage = "DUPLICATE";
                    //}
                    if (ReturnValue != null && Convert.ToInt32(result[0]) == 1)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        throw new Exception("Could not insert/update");
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
        //public async Task<IEnumerable<BOMHeaderDto>> GetBOMHeader(int intProductMarkingId, string nvchBOMType, string strStructureElement)
        //{

        //    IEnumerable<BOMHeaderDto> bOMHeaderDtos;



        //    using (var sqlConnection = new SqlConnection(connectionString))
        //    {
        //        sqlConnection.Open();
        //        var dynamicParameters = new DynamicParameters();
        //        dynamicParameters.Add("@intProductMarkingId", intProductMarkingId);
        //        dynamicParameters.Add("@nvchBOMType", nvchBOMType);
        //        dynamicParameters.Add("@strStructureElement", strStructureElement);

        //        bOMHeaderDtos = sqlConnection.Query<BOMHeaderDto>(SystemConstant.BOMHeader_Select, dynamicParameters, commandType: CommandType.StoredProcedure);
        //        sqlConnection.Close();

        //        SqlConnection con = new SqlConnection(connectionString);
        //        SqlCommand cmd = new SqlCommand();
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        DataSet ds = new DataSet();
        //        cmd = new SqlCommand(SystemConstant.BOMHeader_Select, con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        //dynamicParameters.Add("@intProductMarkingId", intProductMarkingId);
        //        //dynamicParameters.Add("@nvchBOMType", nvchBOMType);
        //        //dynamicParameters.Add("@strStructureElement", strStructureElement);
        //        cmd.Parameters.AddWithValue("@intProductMarkingId", intProductMarkingId);
        //        cmd.Parameters.AddWithValue("@nvchBOMType", nvchBOMType);
        //        cmd.Parameters.AddWithValue("@strStructureElement", strStructureElement);//if you have parameters.
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //       var a =  ds.Tables[0];
        //        var b  = ds.Tables[1];
        //        con.Close();
        //        return bOMHeaderDtos;

        //    }





        //}


        public  List<get_ShapeParamDetailsDTO> GetShapeParamDetails(int ShapeId)
        {

            IEnumerable<get_ShapeParamDetailsDTO> GetShapeParamDetails;

            List<get_ShapeParamDetailsDTO> ShapeParamList;


            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intShapeID", ShapeId);
         
                  GetShapeParamDetails = sqlConnection.Query<get_ShapeParamDetailsDTO>(SystemConstant.Shape_ParamDetails, dynamicParameters, commandType: CommandType.StoredProcedure);

                ShapeParamList = GetShapeParamDetails.ToList();

                sqlConnection.Close();
                return ShapeParamList;

            }

        }


        public bool UpdateBOM_PROD(UpdateBomProd_dto ObjBOMupdate)
        {
            bool isSuccess = false;



            IEnumerable<int> ReturnValue;
            string errorMessage = "";





            try
            {



                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();





                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", ObjBOMupdate.intProductMarkingId);



                    dynamicParameters.Add("@strStructureElement", ObjBOMupdate.strStructureElement);



                    dynamicParameters.Add("@intUserId", ObjBOMupdate.intUserId);





                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);







                    ReturnValue = sqlConnection.Query<int>(SystemConstant.Bom_Production_Update, dynamicParameters, commandType: CommandType.StoredProcedure);





                    //if (ReturnValue != null && Convert.ToInt32(ReturnValue) == 0)
                    //{
                    //    errorMessage = "POSTED";
                    //}
                    //else if (ReturnValue != null && Convert.ToInt32(ReturnValue) == 1)
                    //{
                    //    errorMessage = "DUPLICATE";
                    //}
                    var Return = ReturnValue.FirstOrDefault();

                    if (Return != null && Convert.ToInt32(Return) == 1)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        throw new Exception("Could not insert/update");
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

        

        public List<BomService> GetBOMHeader(int intProductMarkingId, string nvchBOMType, string strStructureElement)
        {



            IEnumerable<BOMHeaderDto> bOMHeaderDtos;
            BomService BOMServiceHeader = new BomService();
            List<BomService> listHeaderDetails;


            List<BomService> listBomheader1 = new List<BomService> { };
            listHeaderDetails = new List<BomService> { };



            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {





                    sqlConnection.Open();





                    DataSet dsGetBomHeader = new DataSet();

                    SqlCommand cmd = new SqlCommand(SystemConstant.BOMHeader_Select, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;



                    cmd.Parameters.Add(new SqlParameter("@intProductMarkingId", intProductMarkingId));
                    cmd.Parameters.Add(new SqlParameter("@nvchBOMType", nvchBOMType));
                    cmd.Parameters.Add(new SqlParameter("@strStructureElement", strStructureElement));





                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsGetBomHeader);

                    if (dsGetBomHeader != null && dsGetBomHeader.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsGetBomHeader.Tables[0].DefaultView)
                        {
                            BOMServiceHeader.intMO1 = Convert.ToInt32(drvBeam["intMO1"]);
                            BOMServiceHeader.intMO2 = Convert.ToInt32(drvBeam["intMO2"]);
                            BOMServiceHeader.intCO1 = Convert.ToInt32(drvBeam["intCO1"]);
                            BOMServiceHeader.intCO2 = Convert.ToInt32(drvBeam["intCO2"]);
                            BOMServiceHeader.MWSequence = Convert.ToDecimal(drvBeam["MWSequence"]);
                            BOMServiceHeader.CWSequence = Convert.ToDecimal(drvBeam["CWSequence"]);
                            BOMServiceHeader.countMW = Convert.ToDecimal(drvBeam["countMW"]);
                            BOMServiceHeader.countCW = Convert.ToDecimal(drvBeam["countCW"]);
                            BOMServiceHeader.NetTon = Convert.ToDecimal(drvBeam["NetTon"]);
                            BOMServiceHeader.ActualTon = Convert.ToDecimal(drvBeam["ActualTon"]);
                            BOMServiceHeader.Area = Convert.ToDecimal(drvBeam["Area"]);
                            BOMServiceHeader.numInvoiceMWLength = Convert.ToInt32(drvBeam["numInvoiceMWLength"]);
                            BOMServiceHeader.numProductionMWLength = Convert.ToInt32(drvBeam["numProductionMWLength"]);
                            BOMServiceHeader.Actual = Convert.ToDecimal(drvBeam["Actual"]);
                            BOMServiceHeader.Net = Convert.ToDecimal(drvBeam["Net"]);
                            BOMServiceHeader.InvoiceTheoriticalWeight = Convert.ToDecimal(drvBeam["InvoiceTheoriticalWeight"]);
                            BOMServiceHeader.ProductionTheoritialWeight = Convert.ToDecimal(drvBeam["ProductionTheoritialWeight"]);
                            BOMServiceHeader.ParamValues = (drvBeam["ParamValues"]).ToString();
                        }
                        //var dynamicParameters = new DynamicParameters();
                        //dynamicParameters.Add("@intProductMarkingId", intProductMarkingId);
                        //dynamicParameters.Add("@nvchBOMType", nvchBOMType);
                        //dynamicParameters.Add("@strStructureElement", strStructureElement);
                        //bOMHeaderDtos = sqlConnection.Query<BOMHeaderDto>(SystemConstant.BOMHeader_Select, dynamicParameters, commandType: CommandType.StoredProcedure);
                        //sqlConnection.Close();
                        //return bOMHeaderDtos;
                    }
                    if (dsGetBomHeader != null && dsGetBomHeader.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsGetBomHeader.Tables[1].DefaultView)
                        {
                            BOMServiceHeader.vchProductCode = (drvBeam["vchProductCode"]).ToString();
                            BOMServiceHeader.MarkingName = (drvBeam["MarkingName"]).ToString();
                            BOMServiceHeader.MeshType = (drvBeam["MeshType"]).ToString();
                            BOMServiceHeader.ReleasedStatus = (drvBeam["ReleasedStatus"]).ToString();
                            BOMServiceHeader.decMWDiameter = Convert.ToDecimal(drvBeam["decMWDiameter"]);
                            BOMServiceHeader.intMWSpace = Convert.ToInt32(drvBeam["intMWSpace"]);
                            BOMServiceHeader.BOMIndicator = (drvBeam["BOMIndicator"]).ToString();
                            BOMServiceHeader.TwinIndicator = (drvBeam["TwinIndicator"]).ToString();
                            BOMServiceHeader.bitStaggeredIndicator = Convert.ToBoolean(drvBeam["bitStaggeredIndicator"]);
                            BOMServiceHeader.countProduction = Convert.ToInt32(drvBeam["countProduction"]);
                            BOMServiceHeader.InvoiceTheoriticalWeight = Convert.ToDecimal(drvBeam["InvoiceTheoriticalWeight"]);
                            BOMServiceHeader.ProductionTheoritialWeight = Convert.ToDecimal(drvBeam["ProductionTheoritialWeight"]);
                            BOMServiceHeader.intCWSpace = Convert.ToInt32(drvBeam["intCWSpace"]);
                            BOMServiceHeader.decCWDiameter = Convert.ToInt32(drvBeam["decCWDiameter"]);
                            BOMServiceHeader.decCWLength = Convert.ToInt32(drvBeam["decCWLength"]);
                            BOMServiceHeader.decMWLength = Convert.ToInt32(drvBeam["decMWLength"]);
                        }
                    }
                    listBomheader1.Add(BOMServiceHeader);
                    //var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("@intProductMarkingId", intProductMarkingId);
                    //dynamicParameters.Add("@nvchBOMType", nvchBOMType);
                    //dynamicParameters.Add("@strStructureElement", strStructureElement);
                    //bOMHeaderDtos = sqlConnection.Query<BOMHeaderDto>(SystemConstant.BOMHeader_Select, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //sqlConnection.Close();
                    //return bOMHeaderDtos;

                }
                //     catch (Exception ex)
                //{
                //    throw ex;
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listBomheader1;


        }

        public async Task<int> BOMDelete(int BOMDetailId)
        {
            string ErrorMsg = "";
            //IEnumerable<GetGroupMarkListDto> getGroupMarkLists;
            try
            {
                int Output = 0;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intDetailingBOMDetailId", BOMDetailId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);



                    sqlConnection.Query<int>(SystemConstant.BOM_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                    Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();

                }

                return Output;

            }

            catch (Exception e)
            {
                throw e;
            }
        }


        public  List<Get_BendingGroup> BendingGroup_Get(int ShapeId)
        {
            string ErrorMsg = "";
            IEnumerable<Get_BendingGroup> Get_BendingGroup;
            try
            {
                int Output = 0;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intShapeID", ShapeId);




                    Get_BendingGroup = sqlConnection.Query<Get_BendingGroup>(SystemConstant.BendingGroup_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                 

                    sqlConnection.Close();

                }

                return Get_BendingGroup.ToList();

            }

            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
