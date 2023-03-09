using AutoMapper;
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
    public class C2BLayoutByProcessReportService : IC2BLayoutByProcessReportService
    {
        private readonly IBLLinesRepository _bLLinesRepository;
        private readonly IBLLineTypeRepository _bLLineTypeRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IBLLayoutDesignProcessRepository _bLLayoutDesignProcessRepository;
        private readonly IBLLayoutDesignProcessDataRepository _bLLayoutDesignProcessDataRepository;
        private readonly IProcessTypeRepository _processTypeRepository;
        private readonly IMapper _mapper;
        private readonly IFactoryRepository _factoryRepository;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;
        private readonly IImageUrlUtility _imageUrlUtility;

        public C2BLayoutByProcessReportService(
            IBLLinesRepository BLLinesRepository,
            IBLLineTypeRepository BLLineTypeRepository,
            IModelRepository modelRepository,
            IBLLayoutDesignProcessRepository bLLayoutDesignProcessRepository,
            IBLLayoutDesignProcessDataRepository bLLayoutDesignProcessDataRepository,
            IProcessTypeRepository processTypeRepository,
            IMapper mapper,
            IFactoryRepository factoryRepository,
            MapperConfiguration configMapper,
            IConfiguration configuration,
            IImageUrlUtility imageUrlUtility)
        {
            _bLLinesRepository = BLLinesRepository;
            _bLLineTypeRepository = BLLineTypeRepository;
            _modelRepository = modelRepository;
            _bLLayoutDesignProcessRepository = bLLayoutDesignProcessRepository;
            _bLLayoutDesignProcessDataRepository = bLLayoutDesignProcessDataRepository;
            _processTypeRepository = processTypeRepository;
            _mapper = mapper;
            _factoryRepository = factoryRepository;
            _configMapper = configMapper;
            _configuration = configuration;
            _imageUrlUtility = imageUrlUtility;
            _configMapper = configMapper;
        }



        // public async Task<BL_Layout_Design_ProcessDTO> GetLayoutDesignProcess(C2BLayoutByProcessReportParam searchParam)
        // {
        //     var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(searchParam.factory.Trim());
        //     var predicate = PredicateBuilder.New<BL_Layout_Design_Process>(true);
        //     _configuration.GetSection("AppSettings:DataSeach").Value = searchParam.factory.Trim();
        //     if (!string.IsNullOrEmpty(searchParam.factory))
        //     {
        //         predicate = predicate.And(x => x.factory_id.Trim() == searchParam.factory.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_no))
        //     {
        //         predicate = predicate.And(x => x.line_id.Trim() == searchParam.line_no.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_type))
        //     {
        //         predicate = predicate.And(x => x.line_type_id.Trim() == searchParam.line_type.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.process_no))
        //     {
        //         predicate = predicate.And(x => x.process_type_id.Trim() == searchParam.process_no.Trim());
        //     }

        //     var data = await _bLLayoutDesignProcessRepository.FindAll(predicate).FirstOrDefaultAsync();
        //     var result = _mapper.Map<BL_Layout_Design_ProcessDTO>(data);
        //     result.each_process_image_after_result = imageUrl + result.each_process_image_after;
        //     result.each_process_image_before_result = imageUrl + result.each_process_image_before;

        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return result;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetAllFactory()
        // {
        //     var data = await _factoryRepository.FindAll().Select(x => new KeyValuePair<string, string>(x.factory_id.Trim(), x.factory_name.Trim())).Distinct().ToListAsync();
        //     return data;
        // }

        // public async Task<C2BLayoutByProcessDetailDTO> GetLayoutDesignProcessData(C2BLayoutByProcessReportParam searchParam)
        // {
        //     var predicate = PredicateBuilder.New<BL_Layout_Design_Process_Data>(true);
        //     _configuration.GetSection("AppSettings:DataSeach").Value = searchParam.factory.Trim();
        //     if (!string.IsNullOrEmpty(searchParam.factory))
        //     {
        //         predicate = predicate.And(x => x.factory_id.Trim() == searchParam.factory.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_no))
        //     {
        //         predicate = predicate.And(x => x.line_id.Trim() == searchParam.line_no.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_type))
        //     {
        //         predicate = predicate.And(x => x.line_type_id.Trim() == searchParam.line_type.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.process_no))
        //     {
        //         predicate = predicate.And(x => x.process_type_id.Trim() == searchParam.process_no.Trim());
        //     }
        //     var data = await _bLLayoutDesignProcessDataRepository.FindAll(predicate)
        //                                                         .OrderBy(x => x.before_or_after).ThenBy(x => x.sequence).ToListAsync();

        //     var listDataBefore = data.Where(x => x.before_or_after == "before").Select(x => x.cycle_time).ToList();
        //     var listTaktTimeBefore = data.Where(x => x.before_or_after == "before").Select(x => x.takt_time).ToList();
        //     var listEmployeeBefore = data.Where(x => x.before_or_after == "before").Select(x => x.employee_qty).ToList();
        //     var listNodeNameBefore = data.Where(x => x.before_or_after == "before").Select(x => x.node_name).ToList();

        //     var listDataAfter = data.Where(x => x.before_or_after == "after").Select(x => x.cycle_time).ToList();
        //     var listTaktTimeAfter = data.Where(x => x.before_or_after == "after").Select(x => x.takt_time).ToList();
        //     var listEmployeeAfter = data.Where(x => x.before_or_after == "after").Select(x => x.employee_qty).ToList();
        //     var listNodeNameAfter = data.Where(x => x.before_or_after == "after").Select(x => x.node_name).ToList();
        //     var typeName = data.Select(x => x.process_type_id).FirstOrDefault();
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return new C2BLayoutByProcessDetailDTO
        //     {
        //         listDataBefore = listDataBefore,
        //         listTaktTimeBefore = listTaktTimeBefore,
        //         listEmployeeBefore = listEmployeeBefore,
        //         listNodeNameBefore = listNodeNameBefore,
        //         listDataAfter = listDataAfter,
        //         listTaktTimeAfter = listTaktTimeAfter,
        //         listEmployeeAfter = listEmployeeAfter,
        //         listNodeNameAfter = listNodeNameAfter,
        //         typeName = typeName
        //     };
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

        // public async Task<List<KeyValuePair<string, string>>> GetProcessType(string factory_id)
        // {
        //     _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
        //     var data = await _processTypeRepository.FindAll(x => x.is_active == true).OrderBy(x => x.sequence)
        //                                                     .Select(x => new KeyValuePair<string, string>(x.process_type_id.Trim(), x.process_type_name_en.Trim())).Distinct().ToListAsync();
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return data;
        // }

        // public async Task<PagedList<C2BLayoutByProcessDTO>> Search(PaginationParams pagination, C2BLayoutByProcessReportParam searchParam)
        // {
        //     var pred = PredicateBuilder.New<BL_Layout_Design_Process>(true);
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
        //     if (!string.IsNullOrEmpty(searchParam.process_no))
        //     {
        //         pred = pred.And(x => x.process_type_id.Trim() == searchParam.process_no.Trim());
        //     }
        //     var Line = _bLLinesRepository.FindAll();
        //     var LineType = _bLLineTypeRepository.FindAll();
        //     var Model = _modelRepository.FindAll();
        //     var LayoutDesignProcess = _bLLayoutDesignProcessRepository.FindAll(pred);
        //     var ProcessType = _processTypeRepository.FindAll();
        //     var data = LayoutDesignProcess.GroupJoin(
        //         Line,
        //         x => new { x.factory_id, x.line_id },
        //         y => new { y.factory_id, y.line_id },
        //         (x, y) => new { LayoutDesignProcess = x, Line = y })
        //         .SelectMany(
        //             x => x.Line.DefaultIfEmpty(),
        //             (x, y) => new { x.LayoutDesignProcess, Line = y })
        //             .GroupJoin(
        //                 LineType,
        //                 x => new { x.LayoutDesignProcess.factory_id, x.LayoutDesignProcess.line_type_id },
        //                 y => new { y.factory_id, y.line_type_id },
        //                 (x, y) => new { x.LayoutDesignProcess, x.Line, LineType = y })
        //                 .SelectMany(
        //                     x => x.LineType.DefaultIfEmpty(),
        //                     (x, y) => new { x.LayoutDesignProcess, x.Line, LineType = y })
        //                     .GroupJoin(
        //                         Model,
        //                         x => new { x.LayoutDesignProcess.factory_id, x.LayoutDesignProcess.model_no },
        //                         y => new { y.factory_id, y.model_no },
        //                         (x, y) => new { x.LayoutDesignProcess, x.Line, x.LineType, Model = y })
        //                         .SelectMany(
        //                             x => x.Model.DefaultIfEmpty(),
        //                             (x, y) => new { x.LayoutDesignProcess, x.Line, x.LineType, Model = y })
        //                         .GroupJoin(
        //                             ProcessType,
        //                             x => new { x.LayoutDesignProcess.factory_id, x.LayoutDesignProcess.process_type_id },
        //                             y => new { y.factory_id, y.process_type_id },
        //                             (x, y) => new { x.LayoutDesignProcess, x.Line, x.LineType, x.Model, ProcessType = y })
        //                             .SelectMany(
        //                                 x => x.ProcessType.DefaultIfEmpty(),
        //                                 (x, y) => new C2BLayoutByProcessDTO
        //                                 {
        //                                     factory_id = x.LayoutDesignProcess.factory_id,
        //                                     line_id = x.Line.line_id,
        //                                     line_name = x.Line.line_name,
        //                                     line_type_id = x.LineType.line_type_id,
        //                                     line_type_name = x.LineType.line_type_name,
        //                                     model_no = x.Model.model_no,
        //                                     model_name = x.Model.model_name,
        //                                     process_type_id = y.process_type_id,
        //                                     process_type_name_en = y.process_type_name_en,
        //                                     update_by = x.LayoutDesignProcess.update_by,
        //                                     update_time = x.LayoutDesignProcess.update_time,
        //                                 });
        //     if (!string.IsNullOrEmpty(searchParam.model))
        //     {
        //         data = data.Where(x => x.model_no.ToUpper().Trim().Contains(searchParam.model.ToUpper().Trim()) || x.model_name.Contains(searchParam.model.ToUpper().Trim()));
        //     }
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return await PagedList<C2BLayoutByProcessDTO>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        // }

    }
}