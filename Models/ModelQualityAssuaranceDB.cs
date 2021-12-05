using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BiscuitQualityAssuaranceSystem.Models
{
    public partial class ModelQualityAssuaranceDB : DbContext
    {
        public ModelQualityAssuaranceDB()
            : base("name=ModelQualityAssuaranceDB1")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<Decision> Decisions { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Product_Plan> Product_Plan { get; set; }
        public virtual DbSet<Quality_Checker> Quality_Checker { get; set; }
        public virtual DbSet<Quality_Parameter> Quality_Parameter { get; set; }
        public virtual DbSet<Shift_Manager> Shift_Manager { get; set; }
        public virtual DbSet<User_Roles> User_Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(e => e.cat_name)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.cat_desc)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.ProductCat_name);

            modelBuilder.Entity<Complaint>()
                .Property(e => e.Com_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Complaint>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Complaint>()
                .Property(e => e.QC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Complaint>()
                .Property(e => e.SM_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Complaint>()
                .Property(e => e.Com_Status);

            modelBuilder.Entity<Complaint>()
                .Property(e => e.Product_code)
                .IsUnicode(false);

            modelBuilder.Entity<Decision>()
                .Property(e => e.Des_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Decision>()
                .Property(e => e.Des_Description)
                .IsUnicode(false);

            modelBuilder.Entity<Decision>()
                .Property(e => e.SM_id)
                .IsUnicode(false);

            modelBuilder.Entity<Decision>()
                .Property(e => e.Com_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Decision>()
                .Property(e => e.Dec_Status);

            modelBuilder.Entity<Product>()
                .Property(e => e.Product_code)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Product_name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductCat_name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.SM_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Quality_Checker)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Quality_Parameter)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product_Plan>()
                .Property(e => e.Product_Plane_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Product_Plan>()
                .Property(e => e.Product_code)
                .IsUnicode(false);

            modelBuilder.Entity<Quality_Checker>()
                .Property(e => e.Qc_id)
                .IsUnicode(false);

            modelBuilder.Entity<Quality_Checker>()
                .Property(e => e.Qc_F_name)
                .IsUnicode(false);

            modelBuilder.Entity<Quality_Checker>()
                .Property(e => e.Qc_L_name)
                .IsUnicode(false);

            modelBuilder.Entity<Quality_Checker>()
                .Property(e => e.Qc_NIC)
                .IsUnicode(false);

            modelBuilder.Entity<Quality_Checker>()
                .Property(e => e.Product_code)
                .IsUnicode(false);

            modelBuilder.Entity<Quality_Checker>()
                .Property(e => e.SM_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Quality_Checker>()
                .Property(e => e.QC_User_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Quality_Checker>()
                .HasMany(e => e.Complaints)
                .WithRequired(e => e.Quality_Checker)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quality_Parameter>()
                .Property(e => e.QP_code)
                .IsUnicode(false);

            modelBuilder.Entity<Quality_Parameter>()
                .Property(e => e.Taste)
                .IsUnicode(false);

            modelBuilder.Entity<Quality_Parameter>()
                .Property(e => e.Moisture)
                .IsUnicode(false);

            modelBuilder.Entity<Quality_Parameter>()
                .Property(e => e.Product_code)
                .IsUnicode(false);

            modelBuilder.Entity<Shift_Manager>()
                .Property(e => e.SM_id)
                .IsUnicode(false);

            modelBuilder.Entity<Shift_Manager>()
                .Property(e => e.SM_F_name)
                .IsUnicode(false);

            modelBuilder.Entity<Shift_Manager>()
                .Property(e => e.SM_L_name)
                .IsUnicode(false);

            modelBuilder.Entity<Shift_Manager>()
                .Property(e => e.Tel_no)
                .IsUnicode(false);

            modelBuilder.Entity<Shift_Manager>()
                .Property(e => e.SM_User_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Shift_Manager>()
                .HasMany(e => e.Complaints)
                .WithRequired(e => e.Shift_Manager)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shift_Manager>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Shift_Manager)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shift_Manager>()
                .HasMany(e => e.Quality_Checker)
                .WithRequired(e => e.Shift_Manager)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User_Roles>()
                .Property(e => e.User_Role_ID)
                .IsUnicode(false);

            modelBuilder.Entity<User_Roles>()
                .Property(e => e.User_Role_Type)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.User_Employee_ID)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.User_Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.User_F_Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.User_L_Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.User_Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.User_Mob_Number)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.User_Role_ID)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.User_Password)
                .IsUnicode(false);

            //modelBuilder.Entity<User>()
            //    .Property(e => e.User_Password2)
            //    .IsUnicode(false);
        }
    }
}
