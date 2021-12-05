namespace BiscuitQualityAssuaranceSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Quality_Parameter
    {
        [Key]
        [Display(Name = "Quality Parameter Code")]
        [StringLength(50)]
        public string QP_code { get; set; }

        [Required]
        [Display(Name = "Weight (g)")]
        public int Weight { get; set; }

        [Required]
        [Display(Name = "Water Level 1 (ml)")]
        public int Waterlevel1 { get; set; }

        [Display(Name = "Water Level 2 (ml)")]
        public int Waterlevel2 { get; set; }

        [Required]
        [Display(Name = "Taste")]
        [StringLength(50)]
        public string Taste { get; set; }

        [Required]
        [Display(Name = "Moisture")]
        [StringLength(50)]
        public string Moisture { get; set; }

        [Required]
        [Display(Name = "Product Code")]
        [StringLength(50)]
        public string Product_code { get; set; }

        public virtual Product Product { get; set; }
    }
}
