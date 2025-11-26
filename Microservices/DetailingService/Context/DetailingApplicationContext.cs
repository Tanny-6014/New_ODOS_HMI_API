using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Net;
using System.Reflection.Emit;
using System.Security;


using Microsoft.EntityFrameworkCore;

namespace DetailingService.Context
{
    public class DetailingApplicationContext : DbContext
    {

        protected readonly IConfiguration Configuration;

        public DetailingApplicationContext(DbContextOptions options) : base(options)
        {
            // TODO: This seems a bit too long. Why has this been set this high?
            //Database.SetCommandTimeout(180);
        }

        //public DbSet<> Shapegroup { get; set; }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<Shapegroup>()
        //      .ToTable("SCM_Shape_Group_Master")
        //      .HasKey(x => x.SG_IDENT);
        //}
    }

}