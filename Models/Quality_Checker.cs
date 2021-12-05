namespace BiscuitQualityAssuaranceSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Quality_Checker
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Quality_Checker()
        {
            Complaints = new HashSet<Complaint>();
        }

        [Key]
        [Display(Name = "Quality Checker ID")]
        [StringLength(50)]
        public string Qc_id { get; set; }

        [Required]
        [Display(Name = "Quality Checker First Name")]
        [StringLength(50)]
        public string Qc_F_name { get; set; }

        [Display(Name = "Quality Checker Last Name")]
        [StringLength(50)]
        public string Qc_L_name { get; set; }
                
        [RegularExpression("^([0-9]{9}[x|X|v|V]|[0-9]{12})$")]
        [Required]
        [Display(Name = "Quality Checker NIC")]
        [StringLength(10)]
        public string Qc_NIC { get; set; }

        [Required]
        [Display(Name = "Product Code")]
        [StringLength(50)]
        public string Product_code { get; set; }

        [Required]
        [Display(Name = "Shift Manager ID")]
        [StringLength(50)]
        public string SM_ID { get; set; }

        [Required]
        [Display(Name = "Quality Checker User Name")]
        public string QC_User_Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint> Complaints { get; set; }

        public virtual Product Product { get; set; }

        public virtual Shift_Manager Shift_Manager { get; set; }
    }
}
