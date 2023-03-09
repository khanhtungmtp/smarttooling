using LinqKit;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.BestLine;
using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;
using SmartTooling_API.Helpers.Utilities;
using SmartTooling_API.Models.BestLine;

namespace SmartTooling_API._Services.Services.BestLine
{
    public class C2BOverallLayoutReportService : IC2BOverallLayoutReportService
    {
        private readonly IBLLayoutDesignOverallRepository _bLLayoutDesignOverallRepository;
        private readonly IBLLinesRepository _bLLinesRepository;
        private readonly IBLLineTypeRepository _bLLineTypeRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IFactoryRepository _factoryRepository;
        private readonly IBLAttachmentsRepository _blAttachmentsRepository;
        private readonly IConfiguration _configuration;
        private readonly IImageUrlUtility _imageUrlUtility;

        public C2BOverallLayoutReportService(
            IBLLayoutDesignOverallRepository BLLayoutDesignOverallRepository,
            IBLLinesRepository BLLinesRepository,
            IBLLineTypeRepository BLLineTypeRepository,
            IModelRepository modelRepository,
            IFactoryRepository factoryRepository,
            IBLAttachmentsRepository blAttachmentsRepository,
            IConfiguration configuration,
            IImageUrlUtility imageUrlUtility)
        {
            _blAttachmentsRepository = blAttachmentsRepository;
            _configuration = configuration;
            _imageUrlUtility = imageUrlUtility;
            _bLLayoutDesignOverallRepository = BLLayoutDesignOverallRepository;
            _bLLinesRepository = BLLinesRepository;
            _bLLineTypeRepository = BLLineTypeRepository;
            _modelRepository = modelRepository;
            _factoryRepository = factoryRepository;
        }


        // public async Task<List<KeyValuePair<string, string>>> GetAllFactory()
        // {
        //     var data = await _factoryRepository.FindAll().Select(x => new KeyValuePair<string, string>(x.factory_id.Trim(), x.factory_name.Trim())).Distinct().ToListAsync();
        //     return data;
        // }

        // public async Task<C2BOverallLayoutReportDetailDTO> GetDetailDTO(C2BOverallLayoutReportParam searchParam)
        // {
        //     var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(searchParam.factory.Trim());
        //     var pred = PredicateBuilder.New<BL_Layout_Design_Overall>(true);
        //     _configuration.GetSection("AppSettings:DataSeach").Value = searchParam.factory.Trim();
        //     if (!string.IsNullOrEmpty(searchParam.factory))
        //     {
        //         pred = pred.And(x => x.factory_id.Trim() == searchParam.factory.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_no))
        //     {
        //         pred = pred.And(x => x.line_id.Trim() == searchParam.line_no.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_type))
        //     {
        //         pred = pred.And(x => x.line_type_id.Trim() == searchParam.line_type.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.model))
        //     {
        //         pred = pred.And(x => x.model_no.ToUpper().Trim() == searchParam.model.ToUpper().Trim());
        //     }
        //     var LayoutDesignOverall = _bLLayoutDesignOverallRepository.FindAll(pred);
        //     var Line = _bLLinesRepository.FindAll();
        //     var LineType = _bLLineTypeRepository.FindAll();
        //     var Model = _modelRepository.FindAll();
        //     var data = LayoutDesignOverall.GroupJoin(
        //         Line,
        //         x => new { x.factory_id, x.line_id },
        //         y => new { y.factory_id, y.line_id },
        //         (x, y) => new { LayoutDesignOverall = x, Line = y })
        //         .SelectMany(
        //             x => x.Line.DefaultIfEmpty(),
        //             (x, y) => new { x.LayoutDesignOverall, Line = y })
        //             .GroupJoin(
        //                 LineType,
        //                 x => new { x.LayoutDesignOverall.factory_id, x.LayoutDesignOverall.line_type_id },
        //                 y => new { y.factory_id, y.line_type_id },
        //                 (x, y) => new { x.LayoutDesignOverall, x.Line, LineType = y })
        //                 .SelectMany(
        //                 x => x.LineType.DefaultIfEmpty(),
        //                 (x, y) => new { x.LayoutDesignOverall, x.Line, LineType = y })
        //                 .GroupJoin(
        //                     Model,
        //                     x => new { x.LayoutDesignOverall.factory_id, x.LayoutDesignOverall.model_no },
        //                     y => new { y.factory_id, y.model_no },
        //                     (x, y) => new { x.LayoutDesignOverall, x.Line, x.LineType, Model = y })
        //                     .SelectMany(
        //                         x => x.Model.DefaultIfEmpty(),
        //                         (x, y) => new C2BOverallLayoutReportDetailDTO
        //                         {
        //                             factory_id = x.LayoutDesignOverall.factory_id,
        //                             line_name = x.Line.line_name,
        //                             line_type_name = x.LineType.line_type_name,
        //                             model_no = x.LayoutDesignOverall.model_no,
        //                             model_name = y.model_name,
        //                             line_id = x.LayoutDesignOverall.line_id,
        //                             line_type_id = x.LayoutDesignOverall.line_type_id,
        //                             no_of_process_before = x.LayoutDesignOverall.no_of_process_before,
        //                             no_of_process_after = x.LayoutDesignOverall.no_of_process_after,
        //                             tct_before = x.LayoutDesignOverall.tct_before,
        //                             tct_after = x.LayoutDesignOverall.tct_after,
        //                             cps_mp_before = x.LayoutDesignOverall.cps_mp_before,
        //                             cps_mp_after = x.LayoutDesignOverall.cps_mp_after,
        //                             assembly_mp_before = x.LayoutDesignOverall.assembly_mp_before,
        //                             assembly_mp_after = x.LayoutDesignOverall.assembly_mp_after,
        //                             eolr_before = x.LayoutDesignOverall.eolr_before,
        //                             eolr_after = x.LayoutDesignOverall.eolr_after,
        //                             ller_before_percent = x.LayoutDesignOverall.ller_before_percent,
        //                             ller_after_percent = x.LayoutDesignOverall.ller_after_percent,
        //                             tentative_pph_before = x.LayoutDesignOverall.tentative_pph_before,
        //                             tentative_pph_after = x.LayoutDesignOverall.tentative_pph_after,
        //                             tentative_efficiency_before_percent = x.LayoutDesignOverall.tentative_efficiency_before_percent,
        //                             tentative_efficiency_after_percent = x.LayoutDesignOverall.tentative_efficiency_after_percent,
        //                             c2b_overall_image = x.LayoutDesignOverall.c2b_overall_image,
        //                             c2b_overall_image_result = imageUrl + x.LayoutDesignOverall.c2b_overall_image,
        //                             update_by = x.LayoutDesignOverall.update_by,
        //                             update_time = x.LayoutDesignOverall.update_time,
        //                         });
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return await data.FirstOrDefaultAsync();
        // }

        // public async Task<PagedList<C2BOverallLayoutReportFilesDTO>> GetFilesDTO(PaginationParams pagination, C2BOverallLayoutReportParam searchParam)
        // {
        //     var pred = PredicateBuilder.New<BL_Attachments>(true);
        //     _configuration.GetSection("AppSettings:DataSeach").Value = searchParam.factory.Trim();
        //     if (!string.IsNullOrEmpty(searchParam.factory))
        //     {
        //         pred = pred.And(x => x.factory_id.Trim() == searchParam.factory.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_no))
        //     {
        //         pred = pred.And(x => x.line_id.Trim() == searchParam.line_no.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_type))
        //     {
        //         pred = pred.And(x => x.line_type_id.Trim() == searchParam.line_type.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.model))
        //     {
        //         pred = pred.And(x => x.model_no.ToUpper().Trim() == searchParam.model.ToUpper().Trim());
        //     }
        //     var Attachments = _blAttachmentsRepository.FindAll(pred);
        //     var Line = _bLLinesRepository.FindAll();
        //     var LineType = _bLLineTypeRepository.FindAll();
        //     var Model = _modelRepository.FindAll();
        //     var data = Attachments.GroupJoin(
        //         Line,
        //         x => new { x.factory_id, x.line_id },
        //         y => new { y.factory_id, y.line_id },
        //         (x, y) => new { Attachments = x, Line = y })
        //         .SelectMany(
        //             x => x.Line.DefaultIfEmpty(),
        //             (x, y) => new { x.Attachments, Line = y })
        //             .GroupJoin(
        //                 LineType,
        //                 x => new { x.Attachments.factory_id, x.Attachments.line_type_id },
        //                 y => new { y.factory_id, y.line_type_id },
        //                 (x, y) => new { x.Attachments, x.Line, LineType = y })
        //                 .SelectMany(
        //                 x => x.LineType.DefaultIfEmpty(),
        //                 (x, y) => new { x.Attachments, x.Line, LineType = y })
        //                 .GroupJoin(
        //                     Model,
        //                     x => new { x.Attachments.factory_id, x.Attachments.model_no },
        //                     y => new { y.factory_id, y.model_no },
        //                     (x, y) => new { x.Attachments, x.Line, x.LineType, Model = y })
        //                     .SelectMany(
        //                         x => x.Model.DefaultIfEmpty(),
        //                         (x, y) => new C2BOverallLayoutReportFilesDTO
        //                         {
        //                             factory_id = x.Attachments.factory_id,
        //                             line_id = x.Attachments.line_id,
        //                             line_name = x.Line.line_name,
        //                             line_type_id = x.Attachments.line_type_id,
        //                             line_type_name = x.LineType.line_type_name,
        //                             model_no = x.Attachments.model_no,
        //                             attachment_type_id = x.Attachments.attachment_type_id,
        //                             attachment_name = x.Attachments.attachment_name,
        //                             attachment_file_url = x.Attachments.attachment_file_url,
        //                             update_by = x.Attachments.update_by,
        //                             update_time = x.Attachments.update_time,
        //                         });
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return await PagedList<C2BOverallLayoutReportFilesDTO>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetLineNo(string factory_id)
        // {
        //     _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
        //     var data = await _bLLinesRepository.FindAll().OrderBy(x => x.sequence)
        //                                                 .Select(x => new KeyValuePair<string, string>(x.line_id.Trim(), x.line_name.Trim())).Distinct().ToListAsync();
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return data;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetLineType(string factory_id)
        // {
        //     _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
        //     var data = await _bLLineTypeRepository.FindAll().OrderBy(x => x.sequence)
        //                                                     .Select(x => new KeyValuePair<string, string>(x.line_type_id.Trim(), x.line_type_name.Trim())).Distinct().ToListAsync();
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return data;
        // }

        // public async Task<PagedList<C2BOverallLayoutReportDTO>> Search(PaginationParams pagination, C2BOverallLayoutReportParam searchParam)
        // {
        //     var pred = PredicateBuilder.New<BL_Layout_Design_Overall>(true);
        //     _configuration.GetSection("AppSettings:DataSeach").Value = searchParam.factory.Trim();
        //     if (!string.IsNullOrEmpty(searchParam.factory))
        //     {
        //         pred = pred.And(x => x.factory_id.Trim() == searchParam.factory.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_no))
        //     {
        //         pred = pred.And(x => x.line_id.Trim() == searchParam.line_no.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_type))
        //     {
        //         pred = pred.And(x => x.line_type_id.Trim() == searchParam.line_type.Trim());
        //     }
        //     var LayoutDesignOverall = _bLLayoutDesignOverallRepository.FindAll(pred);
        //     var Line = _bLLinesRepository.FindAll();
        //     var LineType = _bLLineTypeRepository.FindAll();
        //     var Model = _modelRepository.FindAll();
        //     var data = LayoutDesignOverall.GroupJoin(
        //         Line,
        //         x => new { x.factory_id, x.line_id },
        //         y => new { y.factory_id, y.line_id },
        //         (x, y) => new { LayoutDesignOverall = x, Line = y })
        //         .SelectMany(
        //             x => x.Line.DefaultIfEmpty(),
        //             (x, y) => new { x.LayoutDesignOverall, Line = y })
        //             .GroupJoin(
        //                 LineType,
        //                 x => new { x.LayoutDesignOverall.factory_id, x.LayoutDesignOverall.line_type_id },
        //                 y => new { y.factory_id, y.line_type_id },
        //                 (x, y) => new { x.LayoutDesignOverall, x.Line, LineType = y })
        //                 .SelectMany(
        //                 x => x.LineType.DefaultIfEmpty(),
        //                 (x, y) => new { x.LayoutDesignOverall, x.Line, LineType = y })
        //                 .GroupJoin(
        //                     Model,
        //                     x => new { x.LayoutDesignOverall.factory_id, x.LayoutDesignOverall.model_no },
        //                     y => new { y.factory_id, y.model_no },
        //                     (x, y) => new { x.LayoutDesignOverall, x.Line, x.LineType, Model = y })
        //                     .SelectMany(
        //                         x => x.Model.DefaultIfEmpty(),
        //                         (x, y) => new { x.LayoutDesignOverall, x.Line, x.LineType, Model = y })
        //                     .OrderBy(x => x.LayoutDesignOverall.line_id).ThenBy(x => x.LineType.sequence)
        //                     .Select(x => new C2BOverallLayoutReportDTO
        //                     {
        //                         factory = x.LayoutDesignOverall.factory_id,
        //                         line_id = x.Line.line_id,
        //                         line_name = x.Line.line_name,
        //                         line_type_id = x.LineType.line_type_id,
        //                         line_type_name = x.LineType.line_type_name,
        //                         model_no = x.LayoutDesignOverall.model_no,
        //                         model_name = x.Model.model_name,
        //                         update_by = x.LayoutDesignOverall.update_by,
        //                         update_time = x.LayoutDesignOverall.update_time
        //                     });
        //     if (!string.IsNullOrEmpty(searchParam.model))
        //     {
        //         data = data.Where(x => x.model_no.ToUpper().Trim().Contains(searchParam.model.ToUpper().Trim()) || x.model_name.Contains(searchParam.model.ToUpper().Trim()));
        //     }
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return await PagedList<C2BOverallLayoutReportDTO>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        // }


    }
}