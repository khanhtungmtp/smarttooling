using SmartTooling_API._Repositories.Interfaces.Auth;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Interfaces.ProductionBP;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Repositories.Repositories.Auth;
using SmartTooling_API._Repositories.Repositories.BestLine;
using SmartTooling_API._Repositories.Repositories.ProductionBP;
using SmartTooling_API._Repositories.Repositories.SmartTool;
using SmartTooling_API._Services.Interfaces;
using SmartTooling_API._Services.Interfaces.Auth;
using SmartTooling_API._Services.Interfaces.BestLine;
using SmartTooling_API._Services.Interfaces.ProductionBP;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API._Services.Services;
using SmartTooling_API._Services.Services.Auth;
using SmartTooling_API._Services.Services.BestLine;
using SmartTooling_API._Services.Services.ProductionBP;
using SmartTooling_API._Services.Services.SmartTool;
using SmartTooling_API.Helpers.Utilities;
using SmartTooling_API.Helpers.Params.BestLine;

namespace SmartTooling_API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //Repository

            //smart-tool
            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<IViewModelKaizenRepository, ViewModelKaizenRepository>();
            services.AddScoped<IModelTypeRepository, ModelTypeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleUserRepository, RoleUserRepository>();
            services.AddScoped<IEfficiencyRepository, EfficiencyRepository>();
            services.AddScoped<IKaizenRepository, KaizenRepository>();
            services.AddScoped<IModelOperationRepository, ModelOperationRepository>();
            services.AddScoped<IFactoryRepository, FactoryRepository>();
            services.AddScoped<IDefectReasonRepository, DefectReasonRepository>();
            services.AddScoped<IMeasurement_RFTRepository, Measurement_RFTRepository>();
            services.AddScoped<IStageRepository, StageRepository>();
            services.AddScoped<IProcessTypeRepository, ProcessTypeRepository>();
            services.AddScoped<IViewRFTReportDetailRepository, ViewRFTReportDetailRepository>();
            services.AddScoped<IViewRFTAVGRepository, ViewRFTAVGRepository>();
            services.AddScoped<IKaizenBenefitsApplicationFormRepository, KaizenBenefitsApplicationFormRepository>();
            services.AddScoped<IPilotLineSetupSummaryTrackingRepository, PilotLineSetupSummaryTrackingRepository>();

            //best-line
            services.AddScoped<IBLAttachmentsRepository, BLAttachmentsRepository>();
            services.AddScoped<IBLAttachmentTypeRepository, BLAttachmentTypeRepository>();
            services.AddScoped<IBLCriticalProcessAnalysisRepository, BLCriticalProcessAnalysisRepository>();
            services.AddScoped<IBLLayoutDesignOverallRepository, BLLayoutDesignOverallRepository>();
            services.AddScoped<IBLLayoutDesignProcessDataRepository, BLLayoutDesignProcessDataRepository>();
            services.AddScoped<IBLLayoutDesignProcessRepository, BLLayoutDesignProcessRepository>();
            services.AddScoped<IBLLinesRepository, BLLinesRepository>();
            services.AddScoped<IBLLineTypeRepository, BLLineTypeRepository>();
            services.AddScoped<IBLRolloutAuditRepository, BLRolloutAuditRepository>();
            services.AddScoped<IBLRolloutProgressRepository, BLRolloutProgressRepository>();

            //production-bp
            services.AddScoped<IPBPAdoptionComponentTypeRepository, PBPAdoptionComponentTypeRepository>();
            services.AddScoped<IPBPAutoTechTypeRepository, PBPAutoTechTypeRepository>();
            services.AddScoped<IPBPBondingProgramSettingRepository, PBPBondingProgramSettingRepository>();
            services.AddScoped<IPBPChemicalProcessTypeRepository, PBPChemicalProcessTypeRepository>();
            services.AddScoped<IPBPComputerStitchingSettingRepository, PBPComputerStitchingSettingRepository>();
            services.AddScoped<IPBPCSMachineTypeRepository, PBPCSMachineTypeRepository>();
            services.AddScoped<IPBPCSTypeRepository, PBPCSTypeRepository>();
            services.AddScoped<IPBPJigDesignTypeRepository, PBPJigDesignTypeRepository>();
            services.AddScoped<IPBPMainBottomMaterialTypeRepository, PBPMainBottomMaterialTypeRepository>();
            services.AddScoped<IPBPMachineVendorTypeRepository, PBPMachineVendorTypeRepository>();
            services.AddScoped<IPBPMainUpperMaterialTypeRepository, PBPMainUpperMaterialTypeRepository>();
            services.AddScoped<IPBPMaterialTypeRepository, PBPMaterialTypeRepository>();
            services.AddScoped<IPBPPadPrintSettingRepository, PBPPadPrintSettingRepository>();
            services.AddScoped<IPBPPadShapeTypeRepository, PBPPadShapeTypeRepository>();
            services.AddScoped<IPBPProcessAdoptionScopeTypeRepository, PBPProcessAdoptionScopeTypeRepository>();
            services.AddScoped<IPBPChemicalSupplierTypeRepository, PBPChemicalSupplierTypeRepository>();



            //Services

            //smart-tool
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IModelService, ModelService>();
            services.AddScoped<IKaizenReportService, KaizenReportService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRFTService, RFTService>();
            services.AddScoped<IGroupKaizenReportService, GroupKaizenReportService>();
            services.AddScoped<IDefectReasonService, DefectReasonService>();
            services.AddScoped<IModelOperationService, ModelOperationService>();
            services.AddScoped<IModelEfficiencyService, ModelEfficiencyService>();
            services.AddScoped<IRFTReportService, RFTReportService>();
            services.AddScoped<IKaizenService, KaizenService>();
            services.AddScoped<ICrossSiteSharingService, CrossSiteSharingService>();
            // services.AddScoped<IPilotLineSetupSummaryTrackingServices, PilotLineSetupSummaryTrackingServices>();

            //best-line
            services.AddScoped<ILayoutDesignOverallService, LayoutDesignOverallService>();
            services.AddScoped<IC2BLayoutAttachmentService, C2BLayoutAttachmentService>();
            // services.AddScoped<IC2BOverallLayoutReportService, C2BOverallLayoutReportService>();
            // services.AddScoped<IC2BLayoutByProcessReportService, C2BLayoutByProcessReportService>();
            // services.AddScoped<ICriticalProcessReportService, CriticalProcessReportService>();
            // services.AddScoped<IRolloutProgressService, RolloutProgressService>();
            // services.AddScoped<IRolloutAuditService, RolloutAuditService>();
            // services.AddScoped<IBLCriticalProcessAnalysisService, BLCriticalProcessAnalysisService>();
            // services.AddScoped<IC2BLayoutByProcessService, C2BLayoutByProcessService>();
            // services.AddScoped<ILineBalancingService, LineBalancingService>();
            // services.AddScoped<IBLRolloutReportService, BLRolloutReportService>();
            // services.AddScoped<IBLAuditReportService, BLAuditReportService>();
            //  services.AddScoped<IPBPPadPrintSettingReportService, PBPPadPrintSettingReportService>();

            //Production
            services.AddScoped<IPBPBondingProgramSettingService, PBPBondingProgramSettingService>();
            services.AddScoped<IBPComputerStitchingSettingService, BPComputerStitchingSettingService>();
            services.AddScoped<IBondingReportService, BondingReportService>();
            services.AddScoped<IComputerReportService, ComputerReportService>();
            services.AddScoped<IPadPrintSettingService, PadPrintSettingService>();
            services.AddScoped<ISharedResourcesService, SharedResourcesService>();

            //Utilities
            services.AddScoped<IImageUrlUtility, ImageUrlUtility>();
            services.AddScoped<IFunctionUtility, FunctionUtility>();
        }
    }
}