using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TheLamboProject.Data.DataBases.DataWharehouse.Models
{
    [Table("ErrorLog")]
    public partial class ErrorLog
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Required]
        public string ErrorDescription { get; set; }
        [Required]
        public string ErrorMessage { get; set; }
        [Required]
        public string CurrentDate { get; set; }
    }
}
