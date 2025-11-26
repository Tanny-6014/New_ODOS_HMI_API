using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;
using DrainService.Constants;

namespace DrainService.Repositories
{

    public class BorePileParameterSet
    {
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";
        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        public int ParameterSetNo { get; set; }
        public int ParameterSetId { get; set; }
        public int ProductTypeL2Id { get; set; }
        public int TransportModeId { get; set; }
        public int LapLength { get; set; }
        public int EndLength { get; set; }
        public int AdjFactor { get; set; }
        public int CoverToLink { get; set; }
        public int UserId { get; set; }
        public string ParameterType { get; set; }
        public string Description { get; set; }

        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }

        public List<BorePileParameterSet> GetBorePileParameterSetDetailsByProjectId(int ProjectId)
        {
          
            List<BorePileParameterSet> listBorePilePS = new List<BorePileParameterSet>();
            DataSet dsBorePilePS = new DataSet();
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {


                    SqlCommand cmd = new SqlCommand(SystemConstants.usp_BorePileParameterSet_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INTPROJECTID", ProjectId));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsBorePilePS);

                    if (dsBorePilePS != null && dsBorePilePS.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBorePileParameterSet in dsBorePilePS.Tables[0].DefaultView)
                        {
                            BorePileParameterSet borePilePS = new BorePileParameterSet
                            {
                                ParameterSetNo = Convert.ToInt32(drvBorePileParameterSet["INTPARAMETESET"]),
                                ParameterSetId = Convert.ToInt32(drvBorePileParameterSet["TNTPARAMSETNUMBER"]),
                                ProductType = drvBorePileParameterSet["VCHPRODUCTTYPE"].ToString(),
                                ProductTypeL2Id = Convert.ToInt32(drvBorePileParameterSet["SITPRODUCTTYPEL2ID"]),
                                TransportModeId = Convert.ToInt32(drvBorePileParameterSet["TNTTRANSPORTMODEID"]),
                                LapLength = Convert.ToInt32(drvBorePileParameterSet["INTLAPLENGTH"]),
                                EndLength = Convert.ToInt32(drvBorePileParameterSet["INTENDLENGTH"]),
                                AdjFactor = Convert.ToInt32(drvBorePileParameterSet["INTADJFACTOR"]),
                                CoverToLink = Convert.ToInt32(drvBorePileParameterSet["INTCOVERTOLINK"]),
                                ParameterType = drvBorePileParameterSet["VCHPARAMETERTYPE"].ToString(),
                                Description = drvBorePileParameterSet["VCHDESCRIPTION"].ToString()
                            };
                            listBorePilePS.Add(borePilePS);
                        }
                    }
                }
                return listBorePilePS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateParameterSet(int ProjectId)
        {
          
            int Output=0;
            try
            {
                DataSet outputData = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {


                    SqlCommand cmd = new SqlCommand(SystemConstants.usp_BorePileParameterSet_InsUpd_PV, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INTPROJECTID", ProjectId));
                    cmd.Parameters.Add(new SqlParameter("@TNTPARAMSETNUMBER", ParameterSetId));
                    cmd.Parameters.Add(new SqlParameter("@TNTTRANSPORTMODEID", TransportModeId));
                    cmd.Parameters.Add(new SqlParameter("@INTLAPLENGTH", LapLength));
                    cmd.Parameters.Add(new SqlParameter("@INTENDLENGTH", EndLength));
                    cmd.Parameters.Add(new SqlParameter("@INTADJFACTOR", AdjFactor));
                    cmd.Parameters.Add(new SqlParameter("@INTCOVERTOLINK", CoverToLink));
                    cmd.Parameters.Add(new SqlParameter("@PRODUCTTYPEL2ID", ProductTypeL2Id));
                    cmd.Parameters.Add(new SqlParameter("@INTUSERID", UserId));
                    cmd.Parameters.Add(new SqlParameter("@VCHDESCRIPTION", Description));


                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(outputData);

                    if (outputData != null && outputData.Tables.Count != 0)
                    {
                        foreach (DataRowView drvtProductCode in outputData.Tables[0].DefaultView)
                        {

                            Output = Convert.ToInt32(drvtProductCode["Result"]);

                        }
                    }

                  
                }
                return Output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }

        public int InsertParameterSet(int ProjectId,int ProductTypeId,int UserId)
        {
          
            int Output=-1;
            try
            {
               DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {


                    SqlCommand cmd = new SqlCommand(SystemConstants.usp_BorePileParameterSet_InsUpd_PV, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INTPROJECTID", ProjectId));
                    cmd.Parameters.Add(new SqlParameter("@PRODUCTTYPEL2ID", ProductTypeId));
                    cmd.Parameters.Add(new SqlParameter("@INTUSERID", UserId));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);


                    if (ds != null && ds.Tables.Count != 0)
                    {
                        foreach (DataRowView drvtProductCode in ds.Tables[0].DefaultView)
                        {

                            Output = Convert.ToInt32(drvtProductCode["Result"]);
                       
                        }
                    }
             



                   
                    return Output;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public List<BorePileParameterSet> GetProductCode()
        {
          
            List<BorePileParameterSet> listProductCode = new List<BorePileParameterSet>();
            DataSet dstProductCode = new DataSet();
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(SystemConstants.usp_BorePileProductCode_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dstProductCode);
                    if (dstProductCode != null && dstProductCode.Tables.Count != 0)
                    {
                        foreach (DataRowView drvtProductCode in dstProductCode.Tables[0].DefaultView)
                        {
                            BorePileParameterSet borePS = new BorePileParameterSet
                            {
                                ProductTypeId = Convert.ToInt32(drvtProductCode["SITPRODUCTTYPEL2ID"]),
                                ProductType = drvtProductCode["VCHPRODUCTTYPE"].ToString()
                            };
                            listProductCode.Add(borePS);
                        }
                    }
                    return listProductCode;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

    }
}
