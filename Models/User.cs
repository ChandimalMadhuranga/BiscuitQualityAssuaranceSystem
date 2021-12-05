namespace BiscuitQualityAssuaranceSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Xunit;
    using Xunit.Sdk;

    public partial class User
    {
        [Key]
        [Display(Name = "Employee ID")]
        [StringLength(20)]
        public string User_Employee_ID { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [StringLength(50)]
        public string User_Name { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50)]
        public string User_F_Name { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string User_L_Name { get; set; }
                
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(50)]
        public string User_Email { get; set; }
                
        [Display(Name = "Contact Number")]
        //[StringLength(int.MaxValue, MinimumLength = 10)]
        //[StringLength(10)]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,10}$", ErrorMessage = "Please enter valid phone no.")]
        public string User_Mob_Number { get; set; }

        [Display(Name = "User Role ID")]
        [StringLength(50)]
        public string User_Role_ID { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(50)]
        public string User_Password { get; set; }

        //[Compare(nameof(User_Password), ErrorMessage = "Make sure both passwords are the same")]
        //public string User_Password2 { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? User_Registration_Date { get; set; }

        public virtual User_Roles User_Roles { get; set; }
    }
}
