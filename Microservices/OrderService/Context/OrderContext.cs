using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Context
{
    public partial class OrderContext : DbContext
    {

        protected readonly IConfiguration Configuration;

        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
            // TODO: This seems a bit too long. Why has this been set this high?
            //Database.SetCommandTimeout(180);
         
        }

        public virtual DbSet<ExtendedHeader> OrderAssignment{ get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<ExtendedHeader>(entity => entity.HasNoKey());
            OnModelCreatingPartial(builder);

        }


        partial void OnModelCreatingPartial(ModelBuilder builder);

    }

}