namespace BiscuitQualityAssuaranceSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product_Plan
    {
        [Key]        
        [Display(Name = "Product Plan ID")]
        [StringLength(50)]
        public string Product_Plane_Id { get; set; }
                
        [Display(Name = "Product Code")]
        [StringLength(50)]
        public string Product_code { get; set; }

        [Required]
        [Display(Name = "Product Date")]        
        public DateTime? Product_Date { get; set; }

        [Required]
        [Display(Name = "Product Expected Qty (Kg)")]
        public int? Product_Expected_qty { get; set; }

        [Required]
        [Display(Name = "Produced Qty (Kg) ")]
        public int? Product_Qty { get; set; }

        public virtual Product Product { get; set; }
    }
}
