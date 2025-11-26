using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Net;
using System.Reflection.Emit;
using System.Security;
using WBSService.Model;
using Microsoft.EntityFrameworkCore;

namespace WBSService.Context
{
    public class WbsServiceContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public WbsServiceContext(DbContextOptions options) : base(options)
        {
            // TODO: This seems a bit too long. Why has this been set this high?
            //Database.SetCommandTimeout(180);
        }

        public DbSet<WBSElements> WBSElements { get; set; }
        public DbSet<WBSAtCollapseLevel> WBSAtCollapseLevel { get; set; }
        public DbSet<ProductTypeMaster> ProductTypeMaster { get; set; }
        public DbSet<WBS> WBS { get; set; }
        public DbSet<WBSMaintainence> WBSMaintainence { get; set; }

        public DbSet<StructureElementMaster> StructureElementMaster { get; set; }
        //public DbSet<StoryToFrom> storeyToFrom { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<WBS>()
                .ToTable("WBS")
                .HasKey(x => x.intWBSId);

            builder.Entity<WBSElements>()
                .ToTable("WBSElements")
              .HasKey(x => x.intWBSElementId);

            builder.Entity<WBSAtCollapseLevel>()
                .ToTable("WBSAtCollapseLevel")
              .HasKey(x => x.intStoreyLevelWBSId);

            builder.Entity<ProductTypeMaster>()
                .ToTable("ProductTypeMaster")
              .HasKey(x => x.sitProductTypeID);

            builder.Entity<StructureElementMaster>()
                   .ToTable("StructureElementMaster")
                 .HasKey(x => x.intStructureElementTypeId);

            builder.Entity<WBSMaintainence>()
                   .ToTable("WBSmaintenance")
                 .HasKey(x => x.intWBSMTNCId);


        }
    }
}

