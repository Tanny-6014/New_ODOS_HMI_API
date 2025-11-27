using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using SAP_API.Modelss;

namespace SAP_API.Models
{
    public class DBContextModels : DbContext
    {
        public DBContextModels()
            : base("Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS")
        {
        }
        public DBContextModels Create()
        {
            return new DBContextModels();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(10, 3));

            modelBuilder.Entity<OrderProjectModels>()
            .ToTable("OESProjOrder")
            .HasKey(x => x.OrderNumber);
        }

        public DbSet<OrderProjectModels> OrderProject { get; set; }

        public DbSet<UserAccessModels> UserAccess { get; set; }
        public DbSet<UserNameModels> UserName { get; set; }
        public DbSet<DrawingModels> Drawings { get; set; }
        public DbSet<OrderDetailsModels> OrderDetails { get; set; }
        public DbSet<OrderDetailsDoubleModels> OrderDetailsDouble { get; set; }
        public DbSet<CustomerModels> Customer { get; set; }
        public DbSet<ProjectModels> Project { get; set; }
        public DbSet<ProjectListModels> ProjectList { get; set; }

        public DbSet<ProdCodeModels> ProdCode { get; set; }

        public DbSet<StdSheetDetailsModels> StdSheetDetails { get; set; }
      

     
        public DbSet<StdProdDetailsModels> StdProdDetails { get; set; }

        public DbSet<OrderProjectSEModels> OrderProjectSE { get; set; }

        public DbSet<JobAdviceModels> JobAdvice { get; set; }

        public DbSet<ProcessModels> Process { get; set; }

        public DbSet<BBSModels> BBS { get; set; }

        public DbSet<StdSheetJobAdviceModels> StdSheetJobAdvice { get; set; }

        public DbSet<CTSMESHBBSModels> CTSMESHBBS { get; set; }

        public DbSet<CTSMESHJobAdviceModels> CTSMESHJobAdvice { get; set; }

        public DbSet<CTSMESHBBSNSHModels> CTSMESHBBSNSH { get; set; }

        public DbSet<ComponentSetModel> ComponentSetOrder { get; set; }

        public DbSet<BPCJobAdviceModels> BPCJobAdvice { get; set; }

        public DbSet<BPCDetailsProcModels> BPCDetailsProc { get; set; }

        public DbSet<BPCDetailsModels> BPCDetails { get; set; }

        public DbSet<ProcessCancelModels> ProcessCancel { get; set; }

        public DbSet<Drawing_postingModels> Drawings_posting { get; set; }
    }
}
