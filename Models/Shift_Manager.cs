namespace BiscuitQualityAssuaranceSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Shift_Manager
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shift_Manager()
        {
            Complaints = new HashSet<Complaint>();
            Decisions = new HashSet<Decision>();    
            Products = new HashSet<Product>();
            Quality_Checker = new HashSet<Quality_Checker>();
        }

        [Key]
        [Display(Name = "Shift Manager ID")]
        [StringLength(50)]
        public string SM_id { get; set; }

        [Required]
        [Display(Name = "Shift Manager First Name")]
        [StringLength(50)]
        public string SM_F_name { get; set; }

        [Display(Name = "Shift Manager Last Name")]
        [StringLength(50)]
        public string SM_L_name { get; set; }

        [Required]      
        [Display(Name = "Shift Manager Contact Number")]
        //[StringLength(10)]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,10}$", ErrorMessage = "Please enter valid phone no.")]
        public string Tel_no { get; set; }
                
        [Required]
        [Display(Name = "Shift Manager User name")]
        [StringLength(10)]
        public string SM_User_Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint> Complaints { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Decision> Decisions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quality_Checker> Quality_Checker { get; set; }
    }
}
