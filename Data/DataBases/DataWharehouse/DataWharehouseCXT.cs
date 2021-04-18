using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TheLamboProject.Data.DataBases.DataWharehouse.Models;

#nullable disable

namespace TheLamboProject.Data.DataBases.DataWharehouse
{
    public partial class DataWharehouseCXT : DbContext
    {
        public DataWharehouseCXT(DbContextOptions<DataWharehouseCXT> options)
            : base(options)
        {
        }

        public virtual DbSet<CoinspotPrice> CoinspotPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

