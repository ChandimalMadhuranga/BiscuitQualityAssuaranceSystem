namespace BiscuitQualityAssuaranceSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Complaint")]
    public partial class Complaint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Complaint()
        {
            Decisions = new HashSet<Decision>();
        }

        [Key]        
        [Display(Name = "Complaint ID")]
        [StringLength(50)]
        public string Com_ID { get; set; }

        [Required]
        [Display(Name = "Complaint Description")]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Complaint Date")]
        public DateTime Com_Date { get; set; }

        [Required]
        [Display(Name = "Quality Checker ID")]
        [StringLength(50)]
        public string QC_ID { get; set; }

        [Required]
        [Display(Name = "Shift Manager ID")]
        [StringLength(50)]
        public string SM_ID { get; set; }

        [Display(Name = "Complaint Status")]        
        public ComplaintStatus Com_Status { get; set; }

        public enum ComplaintStatus
        {
            Open,
            Inprogress,
            Completed
        }

        public enum DecisionAction
        {
            Accept,
            Reject           
        }

        [StringLength(50)]
        public string Product_code { get; set; }

        public virtual Product Product { get; set; }

        public virtual Quality_Checker Quality_Checker { get; set; }

        public virtual Shift_Manager Shift_Manager { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Decision> Decisions { get; set; }
    }
}
