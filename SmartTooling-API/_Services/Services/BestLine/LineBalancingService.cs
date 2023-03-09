using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.BestLine;
using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;
using SmartTooling_API.Models.BestLine;

namespace SmartTooling_API._Services.Services.BestLine
{
    public class LineBalancingService : ILineBalancingService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IModelRepository _modelRepository;
        private readonly IBLLinesRepository _linesRepository;
        private readonly IBLLayoutDesignProcessRepository _layoutDesignProcessRepository;
        private readonly IBLLineTypeRepository _lineTypeRepository;
        private readonly IProcessTypeRepository _processTypeRepository;
        private readonly IBLLayoutDesignProcessDataRepository _layoutDesignProcessDataRepository;
        private readonly IConfiguration _configuration;
        private string factory;

        public LineBalancingService(
            IMapper mapper,
            MapperConfiguration configMapper,
            IModelRepository modelRepository,
            IBLLinesRepository linesRepository,
            IBLLayoutDesignProcessRepository layoutDesignProcessRepository,
            IBLLineTypeRepository lineTypeRepository,
            IProcessTypeRepository processTypeRepository,
            IBLLayoutDesignProcessDataRepository layoutDesignProcessDataRepository,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _modelRepository = modelRepository;
            _linesRepository = linesRepository;
            _layoutDesignProcessRepository = layoutDesignProcessRepository;
            _lineTypeRepository = lineTypeRepository;
            _processTypeRepository = processTypeRepository;
            _layoutDesignProcessDataRepository = layoutDesignProcessDataRepository;
            _configuration = configuration;
            factory = _configuration.GetSection("Appsettings:Factory").Value;
        }


        // public async Task<object> GetData(BL_Layout_Design_Process_Data_Params_DTO line)
        // {
        //     return await _layoutDesignProcessDataRepository.FindAll(x => x.line_id.Trim() == line.line_id.Trim()
        //                                                               && x.line_type_id.Trim() == line.line_type_id.Trim()
        //                                                               && x.model_no.Trim() == line.model_no.Trim()
        //                                                               && x.process_type_id.Trim() == line.process_type_id.Trim()
        //                                                               && x.before_or_after.Trim() == line.before_or_after.Trim())
        //                                                     .OrderBy(x => x.sequence).ToListAsync();
        // }

        // public async Task<C2BLayoutByProcessDetailDTO> GetLayoutDesignProcessData(BL_Layout_Design_Process_Data_Params_DTO line)
        // {
        //     var predicate = PredicateBuilder.New<BL_Layout_Design_Process_Data>(true);
        //     if (!string.IsNullOrEmpty(line.line_id))
        //     {
        //         predicate = predicate.And(x => x.line_id.Trim() == line.line_id.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(line.line_type_id))
        //     {
        //         predicate = predicate.And(x => x.line_type_id.Trim() == line.line_type_id.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(line.model_no))
        //     {
        //         predicate = predicate.And(x => x.model_no.Trim() == line.model_no.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(line.process_type_id))
        //     {
        //         predicate = predicate.And(x => x.process_type_id.Trim() == line.process_type_id.Trim());
        //     }
        //     var data = await _layoutDesignProcessDataRepository.FindAll(predicate)
        //                                                         .OrderBy(x => x.before_or_after).ThenBy(x => x.sequence).ToListAsync();

        //     var dataBefore = data.Where(x => x.before_or_after == "before").ToList();

        //     var dataAfter = data.Where(x => x.before_or_after == "after").ToList();
        //     return new C2BLayoutByProcessDetailDTO
        //     {
        //         listDataBefore = dataBefore.Select(x => x.cycle_time).ToList(),
        //         listEmployeeBefore = dataBefore.Select(x => x.employee_qty).ToList(),
        //         listNodeNameBefore = dataBefore.Select(x => x.node_name).ToList(),
        //         listTaktTimeBefore = dataBefore.Select(x => x.takt_time).ToList(),
        //         listDataAfter = dataAfter.Select(x => x.cycle_time).ToList(),
        //         listEmployeeAfter = dataAfter.Select(x => x.employee_qty).ToList(),
        //         listNodeNameAfter = dataAfter.Select(x => x.node_name).ToList(),
        //         listTaktTimeAfter = dataAfter.Select(x => x.takt_time).ToList(),
        //         typeName = line.process_type_id
        //     };
        // }

        // public async Task<object> GetLineID()
        // {
        //     var blLayout = _layoutDesignProcessRepository.FindAll();
        //     var blLine = _linesRepository.FindAll();
        //     var data = await (from a in blLayout
        //                       join b in blLine on new { factory = a.factory_id, lineId = a.line_id }
        //                       equals new { factory = b.factory_id, lineId = b.line_id }
        //                       select new BL_LinesDTO()
        //                       {
        //                           line_id = a.line_id.Trim(),
        //                           line_name = b.line_name.Trim(),
        //                           sequence = b.sequence
        //                       }).Distinct().OrderBy(x => x.sequence).ToListAsync();
        //     return data.Select(x => new { id = x.line_id, text = x.line_name }).ToList();
        // }

        // public async Task<object> GetLineType(string lineID)
        // {
        //     var blLayoutPred = PredicateBuilder.New<BL_Layout_Design_Process>(true);
        //     if (!string.IsNullOrEmpty(lineID))
        //     {
        //         blLayoutPred = blLayoutPred.And(x => x.line_id == lineID);
        //     }
        //     var blLayout = _layoutDesignProcessRepository.FindAll(blLayoutPred);
        //     var blLineType = _lineTypeRepository.FindAll();
        //     var data = await (from a in blLayout
        //                       join b in blLineType on new { factory = a.factory_id, lineTypeID = a.line_type_id }
        //                       equals new { factory = b.factory_id, lineTypeID = b.line_type_id }
        //                       select new BL_Line_TypeDTO()
        //                       {
        //                           line_type_id = a.line_type_id.Trim(),
        //                           line_type_name = b.line_type_name.Trim(),
        //                           sequence = b.sequence
        //                       }).Distinct().OrderBy(x => x.sequence).ToListAsync();
        //     return data.Select(x => new { id = x.line_type_id, text = x.line_type_name }).ToList();
        // }

        // public async Task<object> GetModelNo(string lineID, string lineTypeID)
        // {
        //     var blLayoutPred = PredicateBuilder.New<BL_Layout_Design_Process>(true);
        //     if (!string.IsNullOrEmpty(lineID))
        //     {
        //         blLayoutPred = blLayoutPred.And(x => x.line_id == lineID);
        //     }
        //     if (!string.IsNullOrEmpty(lineTypeID))
        //     {
        //         blLayoutPred = blLayoutPred.And(x => x.line_type_id == lineTypeID);
        //     }
        //     var blLayout = _layoutDesignProcessRepository.FindAll(blLayoutPred);
        //     var model = _modelRepository.FindAll();
        //     var data = await blLayout.GroupJoin(model, x => new { x.factory_id, x.model_no },
        //     y => new { y.factory_id, y.model_no }, (x, y) => new { layout = x, modelno = y })
        //     .SelectMany(x => x.modelno.DefaultIfEmpty(), (x, y) => new
        //     {
        //         id = x.layout.model_no.Trim(),
        //         text = y.model_name.Trim(),
        //     }).Distinct().OrderBy(x => x.id).ToListAsync();
        //     return data;
        // }

        // public async Task<object> GetProcessType(string lineID, string lineTypeID, string modelNo)
        // {
        //     var blLayoutPred = PredicateBuilder.New<BL_Layout_Design_Process>(true);
        //     if (!string.IsNullOrEmpty(lineID))
        //     {
        //         blLayoutPred = blLayoutPred.And(x => x.line_id == lineID);
        //     }
        //     if (!string.IsNullOrEmpty(lineTypeID))
        //     {
        //         blLayoutPred = blLayoutPred.And(x => x.line_type_id == lineTypeID);
        //     }
        //     if (!string.IsNullOrEmpty(modelNo))
        //     {
        //         blLayoutPred = blLayoutPred.And(x => x.model_no == modelNo);
        //     }
        //     var blLayout = _layoutDesignProcessRepository.FindAll(blLayoutPred);
        //     var process = _processTypeRepository.FindAll();
        //     var data = await (from a in blLayout
        //                       join b in process on new { factory = a.factory_id, processTypeID = a.process_type_id }
        //                       equals new { factory = b.factory_id, processTypeID = b.process_type_id }
        //                       select new BL_Layout_Design_Process_Params_DTO()
        //                       {
        //                           process_type_id = a.process_type_id.Trim(),
        //                           process_type_name_en = b.process_type_name_en.Trim(),
        //                           sequence = b.sequence
        //                       }).Distinct().OrderBy(x => x.sequence).ToListAsync();
        //     return data.Select(x => new { id = x.process_type_id, text = x.process_type_name_en }).ToList();
        // }

        // public async Task<bool> ImportData(BL_Layout_Design_Process_Data_Params param, string user, string file)
        // {
        //     var result = await _layoutDesignProcessDataRepository
        //         .FindAll(x => x.line_id == param.line_id
        //                    && x.line_type_id == param.line_type_id
        //                    && x.model_no == param.model_no
        //                    && x.process_type_id == param.process_type_id
        //                    && x.factory_id == factory).ToListAsync();
        //     if (result.Any())
        //     {
        //         _layoutDesignProcessDataRepository.RemoveMultiple(result);
        //     }
        //     DateTime time = DateTime.Now;
        //     List<BL_Layout_Design_Process_DataDTO> listData = new List<BL_Layout_Design_Process_DataDTO>();
        //     using (var package = new ExcelPackage(new FileInfo(file)))
        //     {
        //         ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
        //         int totalRows = workSheet.Dimension.Rows;

        //         int index = 1;
        //         for (int i = 3; i <= totalRows; i++)
        //         {
        //             BL_Layout_Design_Process_DataDTO dataBefore = new BL_Layout_Design_Process_DataDTO();
        //             BL_Layout_Design_Process_DataDTO dataAfter = new BL_Layout_Design_Process_DataDTO();

        //             dataBefore.line_id = dataAfter.line_id = param.line_id;
        //             dataBefore.line_type_id = dataAfter.line_type_id = param.line_type_id;
        //             dataBefore.model_no = dataAfter.model_no = param.model_no;
        //             dataBefore.factory_id = dataAfter.factory_id = factory;
        //             dataBefore.process_type_id = dataAfter.process_type_id = param.process_type_id;
        //             dataBefore.update_by = dataAfter.update_by = user;
        //             dataBefore.sequence = dataAfter.sequence = index;
        //             dataBefore.update_time = dataAfter.update_time = time;

        //             // Before
        //             dataBefore.before_or_after = "before";
        //             dataBefore.node_name = workSheet.Cells[i, 1].Value.ToSafetyString();
        //             dataBefore.cycle_time = workSheet.Cells[i, 2].Value.ToDecimal();
        //             dataBefore.takt_time = workSheet.Cells[i, 3].Value.ToDecimal();
        //             dataBefore.employee_qty = workSheet.Cells[i, 4].Value.ToDecimal();
        //             // After
        //             dataAfter.before_or_after = "after";
        //             dataAfter.node_name = workSheet.Cells[i, 5].Value.ToSafetyString();
        //             dataAfter.cycle_time = workSheet.Cells[i, 6].Value.ToDecimal();
        //             dataAfter.takt_time = workSheet.Cells[i, 7].Value.ToDecimal();
        //             dataAfter.employee_qty = workSheet.Cells[i, 8].Value.ToDecimal();
        //             index++;

        //             // Kiểm tra nếu như có dữ liệu thì Add
        //             if (!string.IsNullOrEmpty(dataBefore.node_name))
        //                 listData.Add(dataBefore);

        //             if (!string.IsNullOrEmpty(dataAfter.node_name))
        //                 listData.Add(dataAfter);
        //         }
        //     }
        //     var listProcessData = _mapper.Map<List<BL_Layout_Design_Process_Data>>(listData);
        //     _layoutDesignProcessDataRepository.AddMultiple(listProcessData);
        //     try
        //     {
        //         return await _layoutDesignProcessDataRepository.SaveAll();
        //     }
        //     catch (System.Exception)
        //     {
        //         return false;
        //     }
        // }

    }
}