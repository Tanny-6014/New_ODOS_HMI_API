
using Dapper;
using Microsoft.Extensions.Configuration;
using SAP_API.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace SAP_API.Modelss
{
    public class StructureElement
    {
        private WbsServiceContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;

        public StructureElement(WbsServiceContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }

        public StructureElement()
        {

        }

        #region Properties

        public string StructureElementType { get; set; }
        public int StructureElementTypeId { get; set; }
        public string Prefix { get; set; }

        #endregion

        

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
                    dsStructureElement = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.usp_StructureElement_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

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
