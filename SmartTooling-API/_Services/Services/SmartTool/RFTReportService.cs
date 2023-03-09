using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Models.SmartTool;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using Microsoft.Extensions.Configuration;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API._Services.Services.SmartTool
{
    public class RFTReportService : IRFTReportService
    {
        private readonly IModelRepository _repoModel;
        private readonly IViewRFTReportDetailRepository _repoViewRFTReport;
        private readonly IViewRFTAVGRepository _repoViewRFTAVG;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;

        public RFTReportService(IModelRepository repoModel,
                                IViewRFTReportDetailRepository rftreportdetailRepository,
                                IViewRFTAVGRepository repoViewRFTAVG,
                                IMapper mapper,
                                MapperConfiguration configMapper,
                                IConfiguration configuration)
        {
            _configMapper = configMapper;
            _mapper = mapper;
            _repoModel = repoModel;
            _repoViewRFTReport = rftreportdetailRepository;
            _repoViewRFTAVG = repoViewRFTAVG;
            _configuration = configuration;
        }

        public async Task<PagedList<Model>> SearchRFTReport(PaginationParams paginationParams, RFTReportParam rftReportParam)
        {
            if (!String.IsNullOrEmpty(rftReportParam.factory_id))
            {
                _configuration.GetSection("AppSettings:DataSeach").Value = rftReportParam.factory_id.Trim();
            }
            var data = await _repoModel.FindAll().ToListAsync();
            if (!String.IsNullOrEmpty(rftReportParam.factory_id))
            {
                data = data.Where(x => x.factory_id.Trim() == rftReportParam.factory_id.Trim()).OrderByDescending(x => x.prod_season).ThenByDescending(x => x.volume).ToList();
            }
            if (!String.IsNullOrEmpty(rftReportParam.model_no))
            {
                data = data.Where(x => x.model_no.Contains(rftReportParam.model_no) || x.model_name.Contains(rftReportParam.model_no)).OrderByDescending(x => x.prod_season).ThenByDescending(x => x.volume).ToList();
            }
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return PagedList<Model>.Create(data, paginationParams.PageNumber, paginationParams.PageSize);

            // LinqKit用法
            //var pred_Model = PredicateBuilder.New<Model>(true);
            //if (!String.IsNullOrEmpty(rftReportParam.model_no))
            //{
            //    pred_Model.And(a => a.model_no.Contains(rftReportParam.model_no) || a.model_name.Contains(rftReportParam.model_no));
            //}
            //var list = _rftreport.FindAll(pred_Model).ProjectTo<ModelDTO>(_configMapper).OrderByDescending(x => x.prod_season).ThenByDescending(x => x.volume);
            //return await PagedList<ModelDTO>.CreateAsync(list, paginationParams.PageNumber, paginationParams.PageSize);
        }

        // 搜尋RFT Report Detail(with page)
        //public async Task<PagedList<VW_RFTReportDetailDTO>> SearchRFTReportDetail(PaginationParams paginationParams, RFTReportParam rftReportParam)
        //{
        //    var query = _repoViewRFTReport.FindAll();
        //    // model_no input trim != empty
        //    if (!String.IsNullOrEmpty(rftReportParam.model_no))
        //    {
        //        query = query.Where(a => a.factory_id.Trim() == "SHC" && a.model_no == rftReportParam.model_no.Trim());
        //    }
        //    var list = query.ProjectTo<VW_RFTReportDetailDTO>(_configMapper).OrderBy(x => x.sequence);
        //    return await PagedList<VW_RFTReportDetailDTO>.CreateAsync(list, paginationParams.PageNumber, paginationParams.PageSize);
        //}

        // 搜尋RFT Report Detail
        public async Task<List<VW_RFTReportDetailDTO>> SearchRFTReportDetail(RFTReportParam rftReportParam)
        {

            _configuration.GetSection("AppSettings:DataSeach").Value = rftReportParam.factory_id.Trim();

            var data = await _repoViewRFTReport.FindAll(x => x.factory_id.Trim() == rftReportParam.factory_id.Trim() && x.model_no == rftReportParam.model_no.Trim())
                .OrderBy(x => x.sequence).ToListAsync();
            var result = _mapper.Map<List<VW_RFTReportDetail>, List<VW_RFTReportDetailDTO>>(data);
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return result;
        }

        // AVG
        public async Task<object> GetAVG(string factory_id, string model_no)
        {
            _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
            var rftavgall = await _repoViewRFTAVG.FindAll(a => a.factory_id.Trim() == factory_id && a.model_no == model_no).ToListAsync();
            var avgdata = (from all in rftavgall
                           select new
                           {
                               cr2 = all.CR2,
                               sms = all.SMS,
                               cs1 = all.CS1,
                               cs2 = all.CS2,
                               cs3 = all.CS3,
                               prod1 = all.PROD1,
                               prod2 = all.PROD2,
                               mp1 = all.MP1,
                               mp2 = all.MP2,
                               mp3 = all.MP3
                           }).FirstOrDefault();
            List<double?> avglist = new List<double?>() { avgdata.cr2, avgdata.sms, avgdata.cs1, avgdata.cs2, avgdata.cs3, avgdata.prod1, avgdata.prod2, avgdata.mp1, avgdata.mp2, avgdata.mp3 };
            var avg = avglist.Average();
            var data = (from a in rftavgall
                        select new
                        {
                            rftavg = a,
                            totalavg = avg
                        }).FirstOrDefault();
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return data;
        }

    }
}
