using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class StructureElement
    {
        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;

        public string StructureElementType { get; set; }
        public int StructureElementTypeId { get; set; }
        public string Prefix { get; set; }
        public StructureElement()
        {

        }
        public StructureElement(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }

        public List<StructureElement> GetStructureElement()
        {
            
            List<StructureElement> listStructureElement = new List<StructureElement>();
            DataSet dsStructureElement = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    //dsStructureElement = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_StructureElement_Get");
                    dsStructureElement = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.StructureElement_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (dsStructureElement != null && dsStructureElement.Tables.Count != 0)
                    {
                        foreach (DataRowView drvStructElement in dsStructureElement.Tables[0].DefaultView)
                        {
                            StructureElement structureElement = new StructureElement
                            {
                                StructureElementTypeId = Convert.ToInt32(drvStructElement["INTSTRUCTUREELEMENTTYPEID"]),
                                StructureElementType = drvStructElement["VCHSTRUCTUREELEMENTTYPE"].ToString()
                                //Prefix = drvStructElement["CHRPREFIX"].ToString()
                            };
                            listStructureElement.Add(structureElement);
                        }
                    }
                    return listStructureElement;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

    }
}
