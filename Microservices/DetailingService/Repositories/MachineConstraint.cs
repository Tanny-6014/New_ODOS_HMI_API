using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class MachineConstraint
    {
        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";


        #region "Variables"
        public int MinMO1 { get; set; }
        public int MaxMO1 { get; set; }
        public int MinMO2 { get; set; }
        public int MaxMO2 { get; set; }
        public int MinCO1 { get; set; }
        public int MaxCO1 { get; set; }
        public int MinCO2 { get; set; }
        public int MaxCO2 { get; set; }
        public int MinMWSpace { get; set; }
        public int MaxMWSpace { get; set; }
        public int MinCWSpace { get; set; }
        public int MaxCWSpace { get; set; }
        #endregion

        public MachineConstraint()
        {

        }      


       
        public List<MachineConstraint> GetMachineConstraint()
        {

            List<MachineConstraint> listGetMachineConstraint = new List<MachineConstraint> { };
            DataSet dsGetMachineConstriant = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();                    
                    SqlCommand cmd = new SqlCommand(SystemConstant.bend_machineconstraint_get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure; 
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsGetMachineConstriant);
                   
                    if (dsGetMachineConstriant != null && dsGetMachineConstriant.Tables.Count != 0)
                    {
                        foreach (DataRowView drvMachineConstraint in dsGetMachineConstriant.Tables[0].DefaultView)
                        {
                            MachineConstraint machineConstriant = new MachineConstraint
                            {
                                MinMO1 = Convert.ToInt32(drvMachineConstraint["INTMINMO1"]),
                                MaxMO1 = Convert.ToInt32(drvMachineConstraint["INTMAXMO1"]),
                                MinMO2 = Convert.ToInt32(drvMachineConstraint["INTMINMO2"]),
                                MaxMO2 = Convert.ToInt32(drvMachineConstraint["INTMAXMO2"]),
                                MinCO1 = Convert.ToInt32(drvMachineConstraint["INTMINCO1"]),
                                MaxCO1 = Convert.ToInt32(drvMachineConstraint["INTMAXCO1"]),
                                MinCO2 = Convert.ToInt32(drvMachineConstraint["INTMINCO2"]),
                                MaxCO2 = Convert.ToInt32(drvMachineConstraint["INTMAXCO2"]),
                                MinMWSpace = Convert.ToInt32(drvMachineConstraint["INTMINMWSPACE"]),
                                MaxMWSpace = Convert.ToInt32(drvMachineConstraint["INTMAXMWSPACE"]),
                                MinCWSpace = Convert.ToInt32(drvMachineConstraint["INTMINCWSPACE"]),
                                MaxCWSpace = Convert.ToInt32(drvMachineConstraint["INTMAXCWSPACE"])
                            };
                            listGetMachineConstraint.Add(machineConstriant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listGetMachineConstraint;
        }
    }

}
