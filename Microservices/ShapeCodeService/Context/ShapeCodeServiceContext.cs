using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Net;
using System.Reflection.Emit;
using System.Security;
using ShapeCodeService.Models;
using Microsoft.EntityFrameworkCore;

namespace ShapeCodeService.Context
{
    public class ShapeCodeServiceContext : DbContext
    {

        protected readonly IConfiguration Configuration;

        public ShapeCodeServiceContext(DbContextOptions options) : base(options)
        {
            // TODO: This seems a bit too long. Why has this been set this high?
            //Database.SetCommandTimeout(180);
        }

        public DbSet<Shapegroup> Shapegroup { get; set; }
        public DbSet<Shapesurchage> shapesurchages { get; set; }
        public DbSet<ShapeCodes> shapecodes { get; set; }
       

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Shapegroup>()
              .ToTable("SCM_Shape_Group_Master")
              .HasKey(x => x.SG_IDENT);

            //for Shapesurchage table
            builder.Entity<Shapesurchage>()
              .ToTable("sap_surcharge_master")
              .HasKey(x => x.IDENTITYNO);


            //for shapecode dropdown
            builder.Entity<ShapeCodes>()
              .ToTable("ShapeMaster")
              .HasKey(x => x.intShapeID);
        }
        }

}