using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace OrderService.Models
{
    public class DBContextModels : DbContext
    {
        public DBContextModels()
            : base("Server=NSPRDDB19\\MSSQL2022,1433;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS")
        {
        }
        public DBContextModels Create()
        {
            return new DBContextModels();
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
        //    modelBuilder.Conventions.Add(new DecimalPropertyConvention(10, 3));

        //    modelBuilder.Entity<OrderProjectModels>()
        //    .ToTable("OESProjOrder")
        //    .HasKey(x => x.OrderNumber);
        //}

        //public DbSet<OrderProjectModels> OrderProject { get; set; }

        public DbSet<UserAccessModels> UserAccess { get; set; }
        //public DbSet<UserNameModels> UserName { get; set; }
        //public DbSet<ShapeModels> Shape { get; set; }
        //public DbSet<CustomerShapeModels> CustomerShape { get; set; }
        //public DbSet<JobAdviceModels> JobAdvice { get; set; }
        //public DbSet<BBSModels> BBS { get; set; }
        //public DbSet<OrderDetailsModels> OrderDetails { get; set; }
        //public DbSet<OrderDetailsDoubleModels> OrderDetailsDouble { get; set; }
        //public DbSet<CustomerModels> Customer { get; set; }
        public DbSet<ProjectModels> Project { get; set; }
        //public DbSet<ProjectListModels> ProjectList { get; set; }
        //public DbSet<ProjectStageModels> ProjectStage { get; set; }
        //public DbSet<WBSModels> WBSList { get; set; }
        //public DbSet<ProcessModels> Process { get; set; }
        //public DbSet<ProcessCancelModels> ProcessCancel { get; set; }
        //public DbSet<PODocsModels> PODoc { get; set; }
        //public DbSet<ProdCodeModels> ProdCode { get; set; }
        //public DbSet<StdSheetMasterModels> StdSheetMaster { get; set; }
        //public DbSet<StdSheetDetailsModels> StdSheetDetails { get; set; }
        //public DbSet<StdSheetJobAdviceModels> StdSheetJobAdvice { get; set; }
        //public DbSet<StdSheetPODocsModels> StdSheetPODoc { get; set; }
        //public DbSet<PinModels> PinMaster { get; set; }
        //public DbSet<PinAddModels> PinAdd { get; set; }
        //public DbSet<CTSMESHJobAdviceModels> CTSMESHJobAdvice { get; set; }
        //public DbSet<CTSMESHBBSNSHModels> CTSMESHBBSNSH { get; set; }
        //public DbSet<CTSMESHBBSModels> CTSMESHBBS { get; set; }
        //public DbSet<CTSMESHPODocsModels> CTSMESHPODoc { get; set; }
        //public DbSet<CTSMESHBeamDetailsModels> CTSMESHBeamDetails { get; set; }

        //public DbSet<CTSMESHColumnDetailsModels> CTSMESHColumnDetails { get; set; }
        //public DbSet<CTSMESHStdSheetDetailsModels> CTSMESHStdSheetDetails { get; set; }
        //public DbSet<CTSMESHOthersDetailsModels> CTSMESHOthersDetails { get; set; }
        //public DbSet<CTSMESHDrawingsModels> CTSMESHDrawings { get; set; }
        //public DbSet<CTSMESHDrawingsRevModels> CTSMESHDrawingsRev { get; set; }
        //public DbSet<CTSMESHShapeModels> CTSMESHShape { get; set; }
        //public DbSet<BPCDetailsModels> BPCDetails { get; set; }
        //public DbSet<BPCJobAdviceModels> BPCJobAdvice { get; set; }
        //public DbSet<BPCPODocsModels> BPCPODoc { get; set; }
        //public DbSet<BPCLoadModels> BPCLoad { get; set; }
        //public DbSet<BPCCageBarsModels> BPCCageBars { get; set; }
        //public DbSet<BPCTemplateModels> BPCTemplate { get; set; }
        //public DbSet<BPCCagesLoadModels> BPCCagesLoad { get; set; }
        //public DbSet<BPCTemplateMasterModels> BPCTemplateMaster { get; set; }
        //public DbSet<BPCTemplateHeaderModels> BPCTemplateHeader { get; set; }
        //public DbSet<BPCDetailsProcModels> BPCDetailsProc { get; set; }
        //public DbSet<PRCJobAdviceModels> PRCJobAdvice { get; set; }
        //public DbSet<PRCBBSModels> PRCBBS { get; set; }
        //public DbSet<PRCBBSNSHModels> PRCBBSNSH { get; set; }
        //public DbSet<PRCPODocsModels> PRCPODoc { get; set; }
        //public DbSet<PRCCABDetailsModels> PRCCABDetails { get; set; }
        //public DbSet<PRCBeamMESHDetailsModels> PRCBeamMESHDetails { get; set; }
        //public DbSet<PRCColumnMESHDetailsModels> PRCColumnMESHDetails { get; set; }
        //public DbSet<PRCCTSMESHDetailsModels> PRCCTSMESHDetails { get; set; }
        //public DbSet<PRCDrawingsModels> PRCDrawings { get; set; }
        //public DbSet<PRCDrawingsRevModels> PRCDrawingsRev { get; set; }
        //public DbSet<StdProductsModels> StdProducts { get; set; }
        //public DbSet<StdProdDetailsModels> StdProdDetails { get; set; }

        //public DbSet<OrderProjectSEModels> OrderProjectSE { get; set; }
        //public DbSet<OrderDocsModels> OrderDocs { get; set; }
        //public DbSet<OrderDocsRevModels> OrderDocsRev { get; set; }
        //public DbSet<DrawingModels> Drawings { get; set; }
        //public DbSet<DrawingOrderModels> DrawingsOrder { get; set; }
        //public DbSet<DrawingWBSModels> DrawingsWBS { get; set; }
        //public DbSet<BBSAddnLimitModels> BBSAddnLimit { get; set; }
        //public DbSet<BBSAddnLimitShapeModels> BBSAddnLimitShape { get; set; }
        //public DbSet<BBSAddnParLimitModels> BBSAddnParLimit { get; set; }

        //public DbSet<ComponentModel> OESComponent { get; set; }
        //public DbSet<ComponentSetModel> ComponentSetOrder { get; set; }
        //public DbSet<ComponentWBSModel> ComponentWBS { get; set; }
        //public DbSet<ComponentSplitModel> ComponentSplit { get; set; }
        //public DbSet<OfflineRebarDBModels> OfflineRebarDB { get; set; }
        //public DbSet<ShapePrintModels> ShapePrint { get; set; }

        //public DbSet<CacheTableModels> CacheTable { get; set; }

        //public DbSet<UpcomingOrderDto> OESUpcomingOrders { get; set; }


        ////---------------added by vishal--------------------
        //public DbSet<PrecastJobAdviceModels> PrecastJobAdvice { get; set; }
        //public DbSet<PreCastDetailsModel> OESPrecastDetails { get; set; }

        //------------------------------------------------


    }

}
