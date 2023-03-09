using AutoMapper;
using SmartTooling_API.DTO.Auth;
using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.DTO.BestLine.LayoutDesignOverall;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Models.Auth;
using SmartTooling_API.Models.BestLine;
using SmartTooling_API.Models.ProductionBP;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<ModelDTO, Model>();
            CreateMap<UserDTO, Users>();
            CreateMap<RoleUserDTO, RoleUser>();
            CreateMap<StageDTO, Stage>();
            CreateMap<ModelOperationDTO, Model_Operation>();
            CreateMap<Measurement_RFTDTO, Measurement_RFT>();
            CreateMap<DefectReasonDTO, Defect_Reason>();
            CreateMap<KaizenDTO, Kaizen>();
            CreateMap<ModelEfficiencyDTO, Efficiency>();
            CreateMap<VW_RFTReportDetailDTO, VW_RFTReportDetail>();
            CreateMap<VW_RFT_AVGDTO, VW_RFT_AVG>();
            CreateMap<Kaizen_Benefits_Application_FormDTO, Kaizen_Benefits_Application_Form>();

            //BestLine
            CreateMap<BL_Attachment_TypeDTO, BL_Attachment_Type>();
            CreateMap<BL_AttachmentsDTO, BL_Attachments>();
            CreateMap<BL_Critical_Process_AnalysisDTO, BL_Critical_Process_Analysis>();
            CreateMap<BL_Layout_Design_OverallDTO, BL_Layout_Design_Overall>();
            CreateMap<BL_Layout_Design_Process_DataDTO, BL_Layout_Design_Process_Data>();
            CreateMap<BL_Layout_Design_ProcessDTO, BL_Layout_Design_Process>();
            CreateMap<BL_Line_TypeDTO, BL_Line_Type>();
            CreateMap<BL_LinesDTO, BL_Lines>();
            CreateMap<BL_Rollout_AuditDTO, BL_Rollout_Audit>();
            CreateMap<BL_Rollout_ProgressDTO, BL_Rollout_Progress>();
            CreateMap<BL_Layout_Design_OverallDTO, LayoutDesignOverallModelDTO>();
            CreateMap<C2B_Layout_AttachmentDTO, BL_Attachments>();


            //Production BP
            CreateMap<PBP_Adoption_Component_TypeDTO, PBP_Adoption_Component_Type>();
            CreateMap<PBP_Auto_Tech_TypeDTO, PBP_Auto_Tech_Type>();
            CreateMap<PBP_Bonding_Program_SettingDTO, PBP_Bonding_Program_Setting>();
            CreateMap<PBP_ComputerStitchingSettingDTO, PBP_ComputerStitchingSetting>();
            CreateMap<PBP_CS_Machine_TypeDTO, PBP_CS_Machine_Type>();
            CreateMap<PBP_Jig_Design_TypeDTO, PBP_Jig_Design_Type>();
            CreateMap<PBP_Machine_Vendor_TypeDTO, PBP_Machine_Vendor_Type>();
            CreateMap<PBP_Main_Bottom_Material_TypeDTO, PBP_Main_Bottom_Material_Type>();
            CreateMap<PBP_Main_Upper_Material_TypeDTO, PBP_Main_Upper_Material_Type>();
            CreateMap<PBP_Material_TypeDTO, PBP_Material_Type>();
            CreateMap<PBP_Pad_Shape_TypeDTO, PBP_Pad_Shape_Type>();
            CreateMap<PBP_Pad_Print_SettingDTO, PBP_Pad_Print_Setting>();
            CreateMap<PBP_Process_Adoption_Scope_TypeDTO, PBP_Process_Adoption_Scope_Type>();
            CreateMap<PBP_Chemical_Process_TypeDTO, PBP_Chemical_Process_Type>();
            CreateMap<PBP_CS_TypeDTO, PBP_CS_Type>();
            CreateMap<ComputerStitchingSettingViewDTO, PBP_ComputerStitchingSetting>();
        }
    }
}