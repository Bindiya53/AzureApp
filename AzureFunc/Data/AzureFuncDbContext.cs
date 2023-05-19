using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctions.AzureFunc.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureFunc.Data
{
    public class AzureFuncDbContext : DbContext
    {
        public AzureFuncDbContext(DbContextOptions<AzureFuncDbContext> options) : base(options)
        {
            
        }
        public DbSet<SalesRequest> SalesRequests{get; set;}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<SalesRequest>(entity => 
            {
                entity.HasKey(c => c.Id);
            });
            
        }
    }
}