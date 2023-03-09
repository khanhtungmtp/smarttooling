using AutoMapper;
using SmartTooling_API.DTO.Auth;
using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.DTO.BestLine.LayoutDesignOverall;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.DTO.ProductionBP.BondingProgramSetting;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Models.Auth;
using SmartTooling_API.Models.BestLine;
using SmartTooling_API.Models.ProductionBP;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<Model, ModelDTO>();
            CreateMap<VW_ModelKaizen, VW_ModelKaizen_Dto>();
            CreateMap<Users, UserDTO>();
            CreateMap<Measurement_RFT, Measurement_RFTDTO>();
            CreateMap<Stage, StageDTO>();
            CreateMap<Defect_Reason, DefectReasonDTO>();
            CreateMap<Model_Operation, ModelOperationDTO>();
            CreateMap<Kaizen, KaizenDTO>();
            CreateMap<Efficiency, ModelEfficiencyDTO>();
            CreateMap<VW_RFTReportDetail, VW_RFTReportDetailDTO>();
            CreateMap<VW_RFT_AVG, VW_RFT_AVGDTO>();
            CreateMap<Kaizen_Benefits_Application_Form, Kaizen_Benefits_Application_FormDTO>();

            //BestLine
            CreateMap<BL_Attachment_Type, BL_Attachment_TypeDTO>();
            CreateMap<BL_Attachments, BL_AttachmentsDTO>();
            CreateMap<BL_Critical_Process_Analysis, BL_Critical_Process_AnalysisDTO>();
            CreateMap<BL_Layout_Design_Overall, BL_Layout_Design_OverallDTO>();
            CreateMap<BL_Layout_Design_Process_Data, BL_Layout_Design_Process_DataDTO>();
            CreateMap<BL_Layout_Design_Process, BL_Layout_Design_ProcessDTO>();
            CreateMap<BL_Line_Type, BL_Line_TypeDTO>();
            CreateMap<BL_Lines, BL_LinesDTO>();
            CreateMap<BL_Rollout_Audit, BL_Rollout_AuditDTO>();
            CreateMap<BL_Rollout_Progress, BL_Rollout_ProgressDTO>();
            CreateMap<LayoutDesignOverallModelDTO, BL_Layout_Design_OverallDTO>();
            CreateMap<BL_Attachments, C2B_Layout_AttachmentDTO>();


            //Production BP
            CreateMap<PBP_Bonding_Program_Setting, BondingProgramSettingModelDTO>();            
            CreateMap<PBP_Adoption_Component_Type, PBP_Adoption_Component_TypeDTO>();
            CreateMap<PBP_Auto_Tech_Type, PBP_Auto_Tech_TypeDTO>();
            CreateMap<PBP_Bonding_Program_Setting, PBP_Bonding_Program_SettingDTO>();
            CreateMap<PBP_ComputerStitchingSetting, PBP_ComputerStitchingSettingDTO>();
            CreateMap<PBP_CS_Machine_Type, PBP_CS_Machine_TypeDTO>();
            CreateMap<PBP_Jig_Design_Type, PBP_Jig_Design_TypeDTO>();
            CreateMap<PBP_Machine_Vendor_Type, PBP_Machine_Vendor_TypeDTO>();
            CreateMap<PBP_Main_Bottom_Material_Type, PBP_Main_Bottom_Material_TypeDTO>();
            CreateMap<PBP_Main_Upper_Material_Type, PBP_Main_Upper_Material_TypeDTO>();
            CreateMap<PBP_Material_Type, PBP_Material_TypeDTO>();
            CreateMap<PBP_Pad_Shape_Type, PBP_Pad_Shape_TypeDTO>();
            CreateMap<PBP_Pad_Print_Setting, PBP_Pad_Print_SettingDTO>();
            CreateMap<PBP_Process_Adoption_Scope_Type, PBP_Process_Adoption_Scope_TypeDTO>();
            CreateMap<PBP_Chemical_Process_Type, PBP_Chemical_Process_TypeDTO>();
            CreateMap<PBP_CS_Type, PBP_CS_TypeDTO>();
            CreateMap<PBP_ComputerStitchingSetting, ComputerStitchingSettingViewDTO>();
        }
    }
}