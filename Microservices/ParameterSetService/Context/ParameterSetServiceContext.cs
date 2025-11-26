using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Net;
using System.Reflection.Emit;
using System.Security;
//using ParameterSetService.Models;
using Microsoft.EntityFrameworkCore;

namespace ParameterSetService.Context
{
    public class ParameterSetServiceContext : DbContext
    {

        protected readonly IConfiguration Configuration;

        public ParameterSetServiceContext(DbContextOptions options) : base(options)
        {
            // TODO: This seems a bit too long. Why has this been set this high?
            //Database.SetCommandTimeout(180);
        }

       
       

        protected override void OnModelCreating(ModelBuilder builder)
        {
           
        }
        }

}