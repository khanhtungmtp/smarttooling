using SmartTooling_API.Models.BestLine;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API.Models.Auth;
using SmartTooling_API.Models.SmartTool;
using SmartTooling_API.Models.ProductionBP;

#pragma warning disable CS0114
namespace SmartTooling_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Defect_Reason> Defect_Reason { get; set; }
        public DbSet<Model> Model { get; set; }
        public DbSet<Factory> Factory { get; set; }
        public DbSet<Efficiency> Efficiency { get; set; }
        public DbSet<Kaizen> Kaizen { get; set; }
        public DbSet<Model_Type> Model_Type { get; set; }
        public DbSet<Model_Operation> Model_Operation { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<RoleUser> RoleUser { get; set; }
        public DbSet<Process_Type> Process_Type { get; set; }
        public DbSet<VW_ModelKaizen> VW_ModelKaizen { get; set; }
        public DbSet<VW_RFTReportDetail> VW_RFTReportDetail { get; set; }
        public DbSet<VW_RFT_AVG> VW_RFT_AVG { get; set; }
        public DbSet<Pilot_Line_Setup_Summary_Tracking> Pilot_Line_Setup_Summary_Tracking { get; set; }
        public DbSet<Measurement_RFT> Measurement_RFT { get; set; }
        public virtual DbSet<Stage> Stage { get; set; }
        public DbSet<Kaizen_Benefits_Application_Form> Kaizen_Benefits_Application_Form { get; set; }

        //BestLine
        public virtual DbSet<BL_Attachment_Type> BL_Attachment_Type { get; set; }
        public virtual DbSet<BL_Attachments> BL_Attachments { get; set; }
        public virtual DbSet<BL_Critical_Process_Analysis> BL_Critical_Process_Analysis { get; set; }
        public virtual DbSet<BL_Layout_Design_Overall> BL_Layout_Design_Overall { get; set; }
        public virtual DbSet<BL_Layout_Design_Process> BL_Layout_Design_Process { get; set; }
        public virtual DbSet<BL_Layout_Design_Process_Data> BL_Layout_Design_Process_Data { get; set; }
        public virtual DbSet<BL_Line_Type> BL_Line_Type { get; set; }
        public virtual DbSet<BL_Lines> BL_Lines { get; set; }
        public virtual DbSet<BL_Rollout_Audit> BL_Rollout_Audit { get; set; }
        public virtual DbSet<BL_Rollout_Progress> BL_Rollout_Progress { get; set; }

        //Production BP
        public virtual DbSet<PBP_Adoption_Component_Type> PBP_Adoption_Component_Type { get; set; }
        public virtual DbSet<PBP_Auto_Tech_Type> PBP_Auto_Tech_Type { get; set; }
        public virtual DbSet<PBP_Bonding_Program_Setting> PBP_Bonding_Program_Setting { get; set; }
        public virtual DbSet<PBP_CS_Machine_Type> PBP_CS_Machine_Type { get; set; }
        public virtual DbSet<PBP_CS_Type> PBP_CS_Type { get; set; }
        public virtual DbSet<PBP_Chemical_Process_Type> PBP_Chemical_Process_Type { get; set; }
        public virtual DbSet<PBP_ComputerStitchingSetting> PBP_ComputerStitchingSetting { get; set; }
        public virtual DbSet<PBP_Jig_Design_Type> PBP_Jig_Design_Type { get; set; }
        public virtual DbSet<PBP_Machine_Vendor_Type> PBP_Machine_Vendor_Type { get; set; }
        public virtual DbSet<PBP_Main_Bottom_Material_Type> PBP_Main_Bottom_Material_Type { get; set; }
        public virtual DbSet<PBP_Main_Upper_Material_Type> PBP_Main_Upper_Material_Type { get; set; }
        public virtual DbSet<PBP_Material_Type> PBP_Material_Type { get; set; }
        public virtual DbSet<PBP_Pad_Print_Setting> PBP_Pad_Print_Setting { get; set; }
        public virtual DbSet<PBP_Pad_Shape_Type> PBP_Pad_Shape_Type { get; set; }
        public virtual DbSet<PBP_Process_Adoption_Scope_Type> PBP_Process_Adoption_Scope_Type { get; set; }
        public virtual DbSet<PBP_Chemical_Supplier_Type> PBP_Chemical_Supplier_Type { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Defect_Reason>().HasKey(x => new { x.factory_id, x.defect_reason_id });
            modelBuilder.Entity<Model>().HasKey(x => new { x.factory_id, x.model_no });
            modelBuilder.Entity<Model_Type>().HasKey(x => new { x.factory_id, x.model_type_id });
            modelBuilder.Entity<RoleUser>().HasKey(x => new { x.user_account, x.role_unique });
            modelBuilder.Entity<VW_ModelKaizen>().HasNoKey();
            modelBuilder.Entity<VW_RFTReportDetail>().HasNoKey();
            modelBuilder.Entity<VW_RFT_AVG>().HasNoKey();
            modelBuilder.Entity<Pilot_Line_Setup_Summary_Tracking>().HasNoKey();
            modelBuilder.Entity<Efficiency>().HasKey(x => new
            {
                x.factory_id,
                x.upper_id,
                x.season,
                x.month
            });
            modelBuilder.Entity<Kaizen_Benefits_Application_Form>().HasKey(x => new
            {
                x.factory_id,
                x.model_no,
                x.serial_no,
                x.to_factory_id
            });
            modelBuilder.Entity<Kaizen>().HasKey(x => new
            {
                x.factory_id,
                x.model_no,
                x.serial_no
            });
            modelBuilder.Entity<Measurement_RFT>().HasKey(x => new
            {
                x.factory_id,
                x.model_no,
                x.stage_id,
                x.operation_id
            });
            modelBuilder.Entity<Stage>().HasKey(x => new
            {
                x.factory_id,
                x.stage_id

            });
            modelBuilder.Entity<Model_Operation>().HasKey(x => new
            {
                x.factory_id,
                x.model_no,
                x.stage_id,
                x.operation_id
            });
            modelBuilder.Entity<Process_Type>().HasKey(x => new
            {
                x.factory_id,
                x.process_type_id,
            });


            modelBuilder.Entity<BL_Attachment_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.attachment_type_id });

                entity.HasIndex(e => e.attachment_type_id).IsUnique();
            });

            modelBuilder.Entity<BL_Attachments>(entity =>
            {
                entity.HasOne(d => d.attachment_type_)
                    .WithMany(p => p.BL_Attachments)
                    .HasPrincipalKey(p => p.attachment_type_id)
                    .HasForeignKey(d => d.attachment_type_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BL_Attachments_BL_Attachment_Type");

                entity.HasOne(d => d.layout_design_overall_)
                    .WithMany(p => p.BL_Attachments)
                    .HasForeignKey(d => d.layout_design_overall_id)
                    .HasConstraintName("FK_BL_Attachments_BL_Layout_Design_Overall");
            });

            modelBuilder.Entity<BL_Critical_Process_Analysis>(entity =>
            {
                entity.HasIndex(e => new { e.layout_design_overall_id, e.stage_id, e.operation_id }).IsUnique();

                entity.HasOne(d => d.layout_design_overall_)
                    .WithMany(p => p.BL_Critical_Process_Analysis)
                    .HasForeignKey(d => d.layout_design_overall_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BL_Critical_Process_Analysis_BL_Layout_Design_Overall");
            });

            modelBuilder.Entity<BL_Layout_Design_Overall>(entity =>
            {
                entity.HasIndex(e => new { e.factory_id, e.line_id, e.line_type_id, e.model_no, e.prod_season }).IsUnique();

                entity.HasOne(d => d.line_)
                    .WithMany(p => p.BL_Layout_Design_Overall)
                    .HasPrincipalKey(p => p.line_id)
                    .HasForeignKey(d => d.line_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BL_Layout_Design_Overall_BL_Lines");
            });

            modelBuilder.Entity<BL_Layout_Design_Process>(entity =>
            {
                entity.HasIndex(e => new { e.layout_design_overall_id, e.process_type_id }).IsUnique();

                entity.Property(e => e.id).ValueGeneratedNever();

                entity.HasOne(d => d.layout_design_overall_)
                    .WithMany(p => p.BL_Layout_Design_Process)
                    .HasForeignKey(d => d.layout_design_overall_id)
                    .HasConstraintName("FK_BL_Layout_Design_Process_BL_Layout_Design_Overall");
            });

            modelBuilder.Entity<BL_Layout_Design_Process_Data>(entity =>
            {
                entity.HasOne(d => d.layout_design_process_)
                    .WithMany(p => p.BL_Layout_Design_Process_Data)
                    .HasForeignKey(d => d.layout_design_process_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BL_Layout_Design_Process_Data_BL_Layout_Design_Process");
            });

            modelBuilder.Entity<BL_Line_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.line_type_id });

                entity.HasIndex(e => e.line_type_id).IsUnique();
            });

            modelBuilder.Entity<BL_Lines>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.line_id });

                entity.HasIndex(e => e.line_id).IsUnique();
            });

            modelBuilder.Entity<BL_Rollout_Audit>(entity =>
            {
                entity.HasOne(d => d.rollout_progress_)
                    .WithMany(p => p.BL_Rollout_Audit)
                    .HasForeignKey(d => d.rollout_progress_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BL_Rollout_Audit_BL_Rollout_Progress");
            });

            modelBuilder.Entity<BL_Rollout_Progress>(entity =>
            {
                entity.HasIndex(e => new { e.critical_process_analysis_id, e.rollout_line_id }).IsUnique();

                entity.HasOne(d => d.critical_process_analysis_)
                    .WithMany(p => p.BL_Rollout_Progress)
                    .HasForeignKey(d => d.critical_process_analysis_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BL_Rollout_Progress_BL_Critical_Process_Analysis");
            });


            //Production BP
            modelBuilder.Entity<PBP_Adoption_Component_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.adoption_component_id });
            });

            modelBuilder.Entity<PBP_Auto_Tech_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.auto_tech_id });
            });

            modelBuilder.Entity<PBP_Bonding_Program_Setting>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.model_no, e.chemical_process_type_id, e.auto_tech_id, e.chemical_name, e.adoption_component_id });
            });

            modelBuilder.Entity<PBP_CS_Machine_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.cs_machine_type_id });
            });

            modelBuilder.Entity<PBP_CS_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.cs_type_id });
            });

            modelBuilder.Entity<PBP_Chemical_Process_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.chemical_process_type_id });
            });

            modelBuilder.Entity<PBP_ComputerStitchingSetting>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.model_no, e.stage_id, e.operation_id, e.cs_type_id, e.cs_machine_type_id });
            });

            modelBuilder.Entity<PBP_Jig_Design_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.jig_design_id });
            });

            modelBuilder.Entity<PBP_Machine_Vendor_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.machine_vendor_id });
            });

            modelBuilder.Entity<PBP_Main_Bottom_Material_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.main_bottom_material_type_id });
            });

            modelBuilder.Entity<PBP_Main_Upper_Material_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.main_upper_material_type_id });
            });

            modelBuilder.Entity<PBP_Material_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.material_type_id });
            });

            modelBuilder.Entity<PBP_Pad_Print_Setting>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.model_no, e.component_name, e.material_type_id, e.chemical_ink });
            });

            modelBuilder.Entity<PBP_Pad_Shape_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.pad_shape_id });
            });

            modelBuilder.Entity<PBP_Process_Adoption_Scope_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.process_adoption_scope_id });
            });
            modelBuilder.Entity<PBP_Chemical_Supplier_Type>(entity =>
            {
                entity.HasKey(e => new { e.factory_id, e.chemical_supplier_id });
            });

        }
    }
}