namespace BiscuitQualityAssuaranceSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Complaints = new HashSet<Complaint>();
            Product_Plan = new HashSet<Product_Plan>();
            Quality_Checker = new HashSet<Quality_Checker>();
            Quality_Parameter = new HashSet<Quality_Parameter>();
        }

        [Key]        
        [Display(Name = "Product Code")]
        [StringLength(50)]
        public string Product_code { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        [StringLength(50)]
        public string Product_name { get; set; }
               
        [Display(Name = "Product Category Name")]
        [StringLength(50)]
        public string ProductCat_name { get; set; }

        [Display(Name = "Shift Managet ID")]
        [Required]
        [StringLength(50)]
        public string SM_ID { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint> Complaints { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Plan> Product_Plan { get; set; }

        public virtual Shift_Manager Shift_Manager { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quality_Checker> Quality_Checker { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quality_Parameter> Quality_Parameter { get; set; }
    }
}
