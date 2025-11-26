using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class BendSpacingConstraint
    {
        #region "Variables"
        public Int32 WireDiaFrom { get; set; }
        public Int32 WireDiaTo { get; set; }
        public Int32 WireSpace { get; set; }
        public Int32 WireShiftedSpace { get; set; }
        #endregion

        public BendSpacingConstraint()
        {

        }
        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;

        public BendSpacingConstraint(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }

        public List<BendSpacingConstraint> GetBendSpacingConstraint(double wireDia)
        {
           
            List<BendSpacingConstraint> listGetBendSpacingConstraint = new List<BendSpacingConstraint> { };
            DataSet dsGetBendSpaceConstriant = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add( "@WIREDIA", wireDia);
                    dynamicParameters.Add( "@WIRESPACE", this.WireSpace);
                   // dsGetBendSpaceConstriant = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "Usp_bend_bendspacingconstraint_get");
                    dsGetBendSpaceConstriant = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.bend_bendspacingconstraint_get, dynamicParameters, commandType: CommandType.StoredProcedure);


                    if (dsGetBendSpaceConstriant != null && dsGetBendSpaceConstriant.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBendSpaceConstraint in dsGetBendSpaceConstriant.Tables[0].DefaultView)
                        {
                            BendSpacingConstraint bendSpacingConstriant = new BendSpacingConstraint
                            {
                                WireShiftedSpace = Convert.ToInt32(drvBendSpaceConstraint["INTSHIFTEDSPACE"])
                            };
                            listGetBendSpacingConstraint.Add(bendSpacingConstriant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
            return listGetBendSpacingConstraint;
        }
    }

}
