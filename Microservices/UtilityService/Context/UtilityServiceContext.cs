using Microsoft.EntityFrameworkCore;


namespace UtilityService.Context
{
    public class UtilityServiceContext : DbContext
    {
        protected readonly IConfiguration Configuration;



        public UtilityServiceContext(DbContextOptions options) : base(options)
        {
           
        }
    }
}
