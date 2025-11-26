
using Microsoft.EntityFrameworkCore;


namespace DrainService.Context
{
    public class DrainServiceContext : DbContext
    {
        protected readonly IConfiguration Configuration;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DrainServiceContext(DbContextOptions options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

        }
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
           
        }



    }
}


    




