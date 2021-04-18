using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TheLamboProject.Data.DataBases.DataWharehouse.Models
{
    [Table("CoinspotPrice")]
    [Index(nameof(CurrencyCode), Name = "CoinspotPrice_Index_CurrencyCode")]
    [Index(nameof(CurrencyCode), nameof(CurrentDate), nameof(CurrentTime), Name = "CoinspotPrice_Index_CurrencyCodeDateTime", IsUnique = true)]
    public partial class CoinspotPrice
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
        public double PriceBid { get; set; }
        public double PriceAsk { get; set; }
        public double PriceLatest { get; set; }
        public long CurrentDate { get; set; }
        public long CurrentTime { get; set; }
    }
}
