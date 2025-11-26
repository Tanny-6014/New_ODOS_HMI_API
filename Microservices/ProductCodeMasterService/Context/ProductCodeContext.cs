
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Net;
using System.Reflection.Emit;
using System.Security;
using ProductCodeMasterService.Model;
using Microsoft.EntityFrameworkCore;

namespace ProductCodeMasterService.Context
{
    public class ProductCodeContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ProductCodeContext(DbContextOptions options) : base(options)
        {
            // TODO: This seems a bit too long. Why has this been set this high?
            //Database.SetCommandTimeout(180);
        }

        public DbSet<ProductCodeMaster> ProductCodeMaster { get; set; }
        public DbSet<CABProductCodeMaster> CABProductCodeMaster { get; set; }
        



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProductCodeMaster>()
              .ToTable("ProductCodeMaster")
              .HasKey(x => x.intProductCodeId);

            builder.Entity<CABProductCodeMaster>()
                .ToTable("CABProductCodeMaster")
              .HasKey(x => x.intCABProductCodeID);

         


        }
    }
}

