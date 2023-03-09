using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API.Models.BestLine;
using AutoMapper;
using SmartTooling_API.Data;
using Microsoft.Data.SqlClient;
using SmartTooling_API._Services.Interfaces.BestLine;
using SmartTooling_API._Repositories.Interfaces.BestLine;

namespace SmartTooling_API._Services.Services.BestLine
{
    public class PilotLineSetupSummaryTrackingServices : IPilotLineSetupSummaryTrackingServices
    {
        private readonly IPilotLineSetupSummaryTrackingRepository _pilotLineRepo;
        private readonly IModelRepository _modelRepo;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly SHCDataContext _shcContext;
        private readonly CBDataContext _cbContext;
        private readonly TSHDataContext _tshContext;
        private readonly SPCDataContext _spcContext;
        public PilotLineSetupSummaryTrackingServices(
            IPilotLineSetupSummaryTrackingRepository pilotLineRepo,
            IModelRepository modelRepo,
            MapperConfiguration configMapper,
            IMapper mapper,
            SHCDataContext shcContext,
            CBDataContext cbContext,
            TSHDataContext tshContext,
            SPCDataContext spcContext)
        {
            _pilotLineRepo = pilotLineRepo;
            _modelRepo = modelRepo;
            _configMapper = configMapper;
            _mapper = mapper;
            _shcContext = shcContext;
            _cbContext = cbContext;
            _tshContext = tshContext;
            _spcContext = spcContext;
        }


        /// <summary>
        /// Lấy data từ 4 nhà máy SHC, CB, TSH, SPC theo prodSeason
        /// </summary>
        /// <param name="prodSeason"></param>
        /// <returns></returns>
        public async Task<List<Pilot_Line_Setup_Summary_Tracking_Report>> GetAllPilots(string prodSeason)
        {
            var result = new List<Pilot_Line_Setup_Summary_Tracking_Report>();
            var factorys = new List<string> { "SHC", "CB", "TSH", "SPC" };
            var strExecStore = "EXEC [dbo].[Pilot_Line_Setup_Summary_Tracking] @p_Prod_Season";
            foreach (var factory in factorys)
            {
                var dataFactory = new Pilot_Line_Setup_Summary_Tracking_Report();
                var dataInStore = new List<Pilot_Line_Setup_Summary_Tracking>();
                var dataEmpty = new Pilot_Line_Setup_Summary_Tracking
                {
                    factory = factory,
                    prod_season = prodSeason,
                };
                try
                {
                    if (factory == "SHC")
                    {

                        dataInStore = await (_shcContext.Pilot_Line_Setup_Summary_Tracking
                                    .FromSqlRaw(strExecStore, new SqlParameter("@p_Prod_Season", prodSeason))).ToListAsync();
                    }
                    else if (factory == "CB")
                    {
                        dataInStore = await (_cbContext.Pilot_Line_Setup_Summary_Tracking
                                   .FromSqlRaw(strExecStore, new SqlParameter("@p_Prod_Season", prodSeason))).ToListAsync();
                    }
                    else if (factory == "TSH")
                    {
                        dataInStore = await (_tshContext.Pilot_Line_Setup_Summary_Tracking
                                   .FromSqlRaw(strExecStore, new SqlParameter("@p_Prod_Season", prodSeason))).ToListAsync();
                    }
                    else
                    {
                        dataInStore = await (_spcContext.Pilot_Line_Setup_Summary_Tracking
                                   .FromSqlRaw(strExecStore, new SqlParameter("@p_Prod_Season", prodSeason))).ToListAsync();
                    }

                    if (!dataInStore.Any())
                    {
                        dataInStore.Add(dataEmpty);
                    }
                }
                catch
                {
                    dataInStore.Add(dataEmpty);
                }

                dataFactory.Factory = factory;
                dataFactory.Data = dataInStore;
                result.Add(dataFactory);
            }
            return result;
        }

        public async Task<List<string>> GetProdSeasons()
        {
            return await _modelRepo.FindAll().Select(x => x.prod_season).Distinct().ToListAsync();
        }
    }
}