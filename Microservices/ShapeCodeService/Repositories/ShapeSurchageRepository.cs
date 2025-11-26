using ShapeCodeService.Context;
using ShapeCodeService.Interfaces;
using ShapeCodeService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.Data.SqlClient;
using ShapeCodeService.Constants;
using System.Data;
using Dapper;

namespace ShapeCodeService.Repositories
{
    public class ShapeSurchageRepository : IShapeSurchage 
    {
        private readonly IConfiguration _configuration;
        private ShapeCodeServiceContext _dbContext;
        private string connectionString;


        public ShapeSurchageRepository(ShapeCodeServiceContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstants.DefaultDBConnection);
            // connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123";

        }   

        public async Task<List<Shapesurchage>> GetShapeSurchageListAsync()
        {

           return await _dbContext.shapesurchages.OrderByDescending(a => a.IDENTITYNO).ToListAsync();

        }

        public async Task<List<ShapeCodes>> GetShapeCodesAsync()
        {

            return await _dbContext.shapecodes.ToListAsync();

        }
        public async Task<bool> CheckduplicateShapeGroupAsync(string ShapeSurchangeName)
        {            
            try
            {
                
                var shapeexist = await _dbContext.shapesurchages.Where(x => x.CHRSHAPECODE == ShapeSurchangeName)
                    .Select(x => x.CHRSHAPECODE).AnyAsync();
                if(shapeexist)
                { 
                    return true;
                }
                else
                {
                    return false;
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<Shapesurchage>> AddShapeSurchageAsync(List<Shapesurchage> shapesurchage)
        {
            try
            {
                foreach(var item in  shapesurchage)
                {
                    if (item.IDENTITYNO != 0)
                    {
                        _dbContext.shapesurchages.Update(item);
                    }
                    else
                    {

                        item.IDENTITYNO = null;
                        _dbContext.shapesurchages.Add(item);
                    }
                    await _dbContext.SaveChangesAsync();

                }

             
                return shapesurchage.ToList();
            }
            catch (Exception e)
            {
                return shapesurchage;

            }

        }

        //public async Task<int> DeleteShapeSurchageAsync(int id)
        //{
        //    Shapesurchage itemToRemove = await _dbContext.shapesurchages.SingleOrDefaultAsync(x => x.IDENTITYNO == id); //returns a single item.
        //    _dbContext.shapesurchages.Remove(itemToRemove);
        //    return await _dbContext.SaveChangesAsync();

        //}
        public async Task<int> DeleteShapeSurchageAsync(int id)
        {
            try
            {
                string sqlQuery = "DELETE FROM sap_surcharge_master WHERE IDENTITYNO = @id";
                var parameters = new[] { new SqlParameter("@id", id) };

                return await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the record.", ex);
            }
        }

        public async Task<IEnumerable<SurchargeDropdown>> GetSurchargesAsync()
        {
            //return await _dbContext.ProjectMaster.ToListAsync();
            IEnumerable<SurchargeDropdown> Surchargelist; //new IEnumerable<List<ProjectMaster>>();



            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                // dynamicParameters.Add("@intCustomerID", customerid);
                Surchargelist = sqlConnection.Query<SurchargeDropdown>(SystemConstants.usp_ShapeSurcharge_get, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return Surchargelist;




            }




        }

        public async Task<Shapesurchage> UpdateShapeSurchageAsync(Shapesurchage shapesurchage)
        {
            try
            {
                if (shapesurchage.IDENTITYNO != 0)
                {
                    shapesurchage.Updated_Date = DateTime.Now;
                    _dbContext.shapesurchages.Update(shapesurchage);
                }
               
                await _dbContext.SaveChangesAsync();
                return shapesurchage;
            }
            catch (Exception e)
            {
                return shapesurchage;

            }

        }
    }


}
