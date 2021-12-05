namespace BiscuitQualityAssuaranceSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Decision")]
    public partial class Decision
    {
        [Key]        
        [Display(Name = "Descision ID")]
        [StringLength(50)]
        public string Des_ID { get; set; }

        [Required]
        [Display(Name = "Descision Description")]
        [StringLength(1000)]
        public string Des_Description { get; set; }

        [Required]
        [Display(Name = "Descision Date")]
        public DateTime Dec_date { get; set; }
                
        [Required]
        [Display(Name = "Shift Manager ID")]
        [StringLength(50)]
        public string SM_id { get; set; }

        [Required]
        [Display(Name = "Complaint ID")]
        [StringLength(50)]
        public string Com_ID { get; set; }

        [Display(Name = "Decision Status")]
        public DecisionStatus Dec_Status { get; set; }

        public enum DecisionStatus
        {
            Open,
            Inprogress,
            Completed
        }

        public virtual Complaint Complaint { get; set; }

        public virtual Shift_Manager Shift_Manager { get; set; }
    }
}
