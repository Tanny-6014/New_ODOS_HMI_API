using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Net;
using System.Reflection.Emit;
using System.Security;


using CommonServices.Models;
using Microsoft.EntityFrameworkCore;

namespace CommonServices.Context
{
    public class CommonServicesContext : DbContext
    {

        protected readonly IConfiguration Configuration;

        public CommonServicesContext(DbContextOptions options) : base(options)
        {
            // TODO: This seems a bit too long. Why has this been set this high?
            //Database.SetCommandTimeout(180);
        }

        public DbSet<CustomerMaster> CustomerMaster { get; set; }
        public DbSet<ProjectMaster> ProjectMaster { get; set; }

        public DbSet<ProductType> ProductType { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CustomerMaster>()
              .ToTable("CustomerMaster")
              .HasKey(x => x.intCustomerCode);

            builder.Entity<ProjectMaster>()
              .ToTable("SAPProjectMaster")
              .HasKey(x => x.intProjectId);

            builder.Entity<ProductType>()
            .ToTable("ProductTypeMaster")
            .HasKey(x => x.sitProductTypeID);
        }


        //protected override void OnModelCreating(ModelBuilder builder)
       
    }

}