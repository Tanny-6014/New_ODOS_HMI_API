using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WBSService.Constants;

namespace WBSService.Repositories
{
    public class Project
    {
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=NDSWebApps;Password=DBAdmin4*NDS";
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectDescription { get; set; }
        public string SAPProjectCode { get; set; }
        public int SAPProjectId { get; set; }
        public string SAPProjectDescription { get; set; }
        public string BBSSAPProjectDescription { get; set; }
        public string ShipToParty { get; set; }   //added for ship to party
        public string PhysicalProjectName { get; set; }  //added for ship to party

        public List<Project> GetProject(int ContractId)
        {
            
            List<Project> listGetProject = new List<Project> { };
            DataSet dsGetProject = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTCONTRACTID", ContractId);

                    listGetProject = (List<Project>)sqlConnection.Query<Project>(SystemConstant.usp_Project_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return listGetProject;
                }
              
                if (dsGetProject != null && dsGetProject.Tables.Count != 0)
                {
                    foreach (DataRowView drvProject in dsGetProject.Tables[0].DefaultView)
                    {
                        Project psProject = new Project
                        {
                            ProjectName = drvProject["VCHPROJECTCODE"].ToString(),
                            ProjectId = Convert.ToInt32(drvProject["INTProjectID"].ToString()),
                            ProjectDescription = drvProject["VCHPROJECTNAME"].ToString(),
                            SAPProjectCode = drvProject["SAPPROJECTCODE"].ToString(),
                            //SAPProjectId = Convert.ToInt32(drvProject["SAPPROJECTID"].ToString())
                            //SAPProjectDescription = drvProject["SAPDESCRIPTION"].ToString(),
                            //BBSSAPProjectDescription = drvProject["SAPPROJDESCRIPTION"].ToString()

                        };
                        listGetProject.Add(psProject);
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
            return listGetProject;
        }

        //added for ship to party
        public List<Project> GetProject_new(int CustomerId)
        {
            
            List<Project> listGetProject = new List<Project> { };
            DataSet dsGetProject = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intCustomerID", CustomerId);

                    listGetProject = (List<Project>)sqlConnection.Query<Project>(SystemConstant.Project_Get_new, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return listGetProject;
                }
                
               
                if (dsGetProject != null && dsGetProject.Tables.Count != 0)
                {
                    foreach (DataRowView drvProject in dsGetProject.Tables[0].DefaultView)
                    {
                        Project psProject = new Project
                        {
                            ProjectName = drvProject["vchProjectCode"].ToString(),
                            ProjectId = Convert.ToInt32(drvProject["INTProjectID"].ToString()),
                            ProjectDescription = drvProject["VCHPROJECTNAME"].ToString(),
                            SAPProjectCode = drvProject["SAPPROJECTCODE"].ToString(),
                            //ShipToParty = drvProject["vchshiptoparty"].ToString(),
                            //SAPProjectId = Convert.ToInt32(drvProject["SAPPROJECTID"].ToString())
                            //SAPProjectDescription = drvProject["SAPDESCRIPTION"].ToString(),
                            //BBSSAPProjectDescription = drvProject["SAPPROJDESCRIPTION"].ToString()

                        };
                        listGetProject.Add(psProject);
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
            return listGetProject;
        }

        public List<Project> GetDestProject_new(int CustomerId)
        {
            
            List<Project> listGetProject = new List<Project> { };
            DataSet dsGetProject = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intCustomerID", CustomerId);
                    listGetProject = (List<Project>)sqlConnection.Query<Project>(SystemConstant.DestProject_Get_new, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return listGetProject;
                }
               
                if (dsGetProject != null && dsGetProject.Tables.Count != 0)
                {
                    foreach (DataRowView drvProject in dsGetProject.Tables[0].DefaultView)
                    {
                        Project psProject = new Project
                        {
                            ProjectName = drvProject["vchProjectCode"].ToString(),
                            ProjectId = Convert.ToInt32(drvProject["INTProjectID"].ToString()),
                            ProjectDescription = drvProject["VCHPROJECTNAME"].ToString(),
                            SAPProjectCode = drvProject["SAPPROJECTCODE"].ToString(),
                            //ShipToParty = drvProject["vchshiptoparty"].ToString(),
                            //SAPProjectId = Convert.ToInt32(drvProject["SAPPROJECTID"].ToString())
                            //SAPProjectDescription = drvProject["SAPDESCRIPTION"].ToString(),
                            //BBSSAPProjectDescription = drvProject["SAPPROJDESCRIPTION"].ToString()

                        };
                        listGetProject.Add(psProject);
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
            return listGetProject;
        }

        public String GetProjectName(int ProjectId)
        {
            

            DataSet dsGetProject = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProjectId", ProjectId);
                    dsGetProject = (DataSet)sqlConnection.Query<string>(SystemConstant.PhysicalProject_get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return dsGetProject.ToString();
                }
                
                if (dsGetProject != null && dsGetProject.Tables.Count != 0)
                {
                    foreach (DataRowView drvProject in dsGetProject.Tables[0].DefaultView)
                    {

                        PhysicalProjectName = drvProject["Name1"].ToString();


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
            return PhysicalProjectName;
        }

        //end added for ship to party
    }
}
