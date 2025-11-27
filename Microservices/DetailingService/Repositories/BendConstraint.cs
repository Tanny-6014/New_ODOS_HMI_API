using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class BendConstraint
    {

        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        public BendConstraint()
        {

        }
       

        #region "Variables"
        public Int32 WireDiaFrom { get; set; }
        public Int32 WireDiaTo { get; set; }
        public string BendType { get; set; }
        public Int32 Sequence { get; set; }
        public Int32 BendLength { get; set; }
        #endregion

        public List<BendConstraint> GetBendConstraint(double wireDia)
        {
            
            List<BendConstraint> listGetBendConstraint = new List<BendConstraint> { };
            DataSet dsGetBendConstriant = new DataSet();
            IEnumerable<GetBendConstraintDto> GetBendConstraintDtos;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add( "@WIREDIA", wireDia); //Added by vanita wireDia
                    //dsGetBendConstriant = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_Bend_BendConstraint_get");
                    GetBendConstraintDtos =sqlConnection.Query<GetBendConstraintDto>(SystemConstant.Bend_BendConstraint_get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (GetBendConstraintDtos.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(GetBendConstraintDtos);
                        dsGetBendConstriant.Tables.Add(dt);
                    }

                        if (dsGetBendConstriant != null && dsGetBendConstriant.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBendConstraint in dsGetBendConstriant.Tables[0].DefaultView)
                        {
                            BendConstraint bendConstriant = new BendConstraint
                            {
                                BendType = Convert.ToString(drvBendConstraint["CHRBENDTYPE"]),
                                Sequence = Convert.ToInt32(drvBendConstraint["TNTSEQUENCE"]),
                                BendLength = Convert.ToInt32(drvBendConstraint["SITBENDLENGTH"])
                            };
                            listGetBendConstraint.Add(bendConstriant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
            return listGetBendConstraint;
        }
    }

}
