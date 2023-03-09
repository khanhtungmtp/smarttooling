using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Cells;
using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.ProductionBP;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.ProductionBP;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;
using SmartTooling_API.Helpers.Params.ProductionBP;
using SmartTooling_API.Models.ProductionBP;
using SmartTooling_API.Models.SmartTool;
using System.Text.RegularExpressions;

namespace SmartTooling_API._Services.Services.ProductionBP
{
    public class PadPrintSettingService : IPadPrintSettingService
    {
        private readonly IPBPPadPrintSettingRepository _padPrintSettingRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IPBPPadShapeTypeRepository _padShapeTypeRepository;
        private readonly IPBPMachineVendorTypeRepository _machineVendorTypeRepository;
        private readonly IPBPMaterialTypeRepository _materialTypeRepository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IWebHostEnvironment _webhostEnvironment;
        private readonly IConfiguration _configuration;

        public PadPrintSettingService(
            IPBPPadPrintSettingRepository padPrintSettingRepository,
            IModelRepository modelRepository,
            IPBPPadShapeTypeRepository padShapeTypeRepository,
            IPBPMachineVendorTypeRepository machineVendorTypeRepository,
            IPBPMaterialTypeRepository materialTypeRepository,
            IMapper mapper,
            MapperConfiguration mapperConfiguration,
            IWebHostEnvironment webhostEnvironment,
            IConfiguration configuration
            )
        {
            _padPrintSettingRepository = padPrintSettingRepository;
            _modelRepository = modelRepository;
            _padShapeTypeRepository = padShapeTypeRepository;
            _machineVendorTypeRepository = machineVendorTypeRepository;
            _materialTypeRepository = materialTypeRepository;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
            _webhostEnvironment = webhostEnvironment;
            _configuration = configuration;
        }

        public async Task<OperationResult> AddNew(PBP_Pad_Print_SettingDTO model)
        {
            model.factory_id = _configuration.GetSection("AppSettings:Factory").Value;
            if (await FindPadPrintSetting(model))
            {
                var fu = new FunctionUtility();
                var data = _mapper.Map<PBP_Pad_Print_Setting>(model);
                if (data.article_no_remarks == "")
                    data.article_no_remarks = null;
                data.create_time = DateTime.Now;
                data.update_time = DateTime.Now;
                //img & video
                var filePath = data.factory_id + "/" + "ProductionBP" + "/" + "PadPrintSetting" + "/" + data.model_no;
                var fileName = Guid.NewGuid().ToString();


                if (string.IsNullOrEmpty(model.component_photo_url_file))
                {
                    data.component_photo_url = data.factory_id + "/" + "no-image.jpg";
                }
                else
                {
                    var componentPhotoUrl = await fu.UploadAsync(model.component_photo_url_file, "uploaded/" + filePath, fileName);
                    data.component_photo_url = filePath + "/" + componentPhotoUrl;
                }
                if (string.IsNullOrEmpty(model.operation_video_url_file))
                {
                    data.operation_video_url = data.factory_id + "/" + "no-image.jpg";
                }
                else
                {
                    var operationVideoUrl = await fu.UploadAsync(model.operation_video_url_file, "uploaded/" + filePath, fileName);
                    data.operation_video_url = filePath + "/" + operationVideoUrl;
                }
                _padPrintSettingRepository.Add(data);

                if (await _padPrintSettingRepository.SaveAll())
                    return new OperationResult { Caption = "Success", Message = "Add Successfully", Success = true };

                return new OperationResult { Caption = "Adding failed on save", Success = false };



            }
            else
            {
                return new OperationResult { Caption = "Fail", Message = "Adding failed on save, Pad Print Setting already exists", Success = false };
            }
        }

        public async Task<bool> FindPadPrintSetting(PBP_Pad_Print_SettingDTO model)
        {
            var data = await _padPrintSettingRepository.FindAll(x => x.factory_id.Trim() == model.factory_id.Trim()
                                                                && x.model_no.Trim().ToUpper() == model.model_no.Trim().ToUpper()
                                                                && x.component_name.Trim().ToUpper() == model.component_name.Trim().ToUpper()
                                                                && x.material_type_id.Trim() == model.material_type_id.Trim()
                                                                && x.chemical_ink.Trim() == model.chemical_ink.Trim()).AsNoTracking().FirstOrDefaultAsync();
            if (data == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllMachineVendor()
        {
            var data = await _machineVendorTypeRepository.FindAll(x => x.is_active == true).OrderBy(x => x.sequence).Select(x => new KeyValuePair<string, string>(x.machine_vendor_id.Trim(), x.machine_vendor_name.Trim())).Distinct().ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllMaterialType()
        {
            var data = await _materialTypeRepository.FindAll(x => x.is_active == true).OrderBy(x => x.sequence).Select(x => new KeyValuePair<string, string>(x.material_type_id.Trim(), x.material_type_name.Trim())).Distinct().ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllModel()
        {
            var data = await _modelRepository.FindAll(x => x.is_active == true).OrderBy(x => x.model_no).Select(x => new KeyValuePair<string, string>(x.model_no.Trim(), x.model_name.Trim())).Distinct().ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllPadShape()
        {
            var data = await _padShapeTypeRepository.FindAll(x => x.is_active == true).OrderBy(x => x.sequence).Select(x => new KeyValuePair<string, string>(x.pad_shape_id.Trim(), x.pad_shape_name.Trim())).Distinct().ToListAsync();
            return data;
        }

        public async Task<PBP_Pad_Print_SettingDTO> GetDetail(PBP_Pad_Print_Setting_DTO model)
        {
            var PadPrintSetting = _padPrintSettingRepository.FindAll(x => x.factory_id.Trim() == model.factory_id.Trim()
                                                                && x.model_no.Trim().ToUpper() == model.model_no.Trim().ToUpper()
                                                                && x.component_name.Trim().ToUpper() == model.component_name.Trim().ToUpper()
                                                                && x.material_type_id.Trim() == model.material_type_id.Trim()
                                                                && x.chemical_ink.Trim() == model.chemical_ink.Trim()).AsNoTracking();
            var MaterialType = _materialTypeRepository.FindAll();
            var models = _modelRepository.FindAll();
            var data = await PadPrintSetting.GroupJoin(
                MaterialType,
                x => new { x.factory_id, x.material_type_id },
                y => new { y.factory_id, y.material_type_id },
                (x, y) => new { PadPrintSetting = x, MaterialType = y })
                .SelectMany(
                    x => x.MaterialType.DefaultIfEmpty(),
                    (x, y) => new { x.PadPrintSetting, MaterialType = y })
                    .GroupJoin(models,
                    x => new { x.PadPrintSetting.factory_id, x.PadPrintSetting.model_no },
                    y => new { y.factory_id, y.model_no },
                    (x, y) => new { x.PadPrintSetting, x.MaterialType, Models = y })
                    .SelectMany(x => x.Models.DefaultIfEmpty(),
                    (x, y) => new PBP_Pad_Print_SettingDTO
                    {
                        factory_id = x.PadPrintSetting.factory_id,
                        model_no = x.PadPrintSetting.model_no,
                        component_name = x.PadPrintSetting.component_name,
                        material_type_id = x.PadPrintSetting.material_type_id,
                        chemical_ink = x.PadPrintSetting.chemical_ink,
                        material_type_name = x.MaterialType.material_type_name,
                        dev_season = y.dev_season,
                        production_season = y.prod_season,
                        material_description = x.PadPrintSetting.material_description,
                        pad_shape_id = x.PadPrintSetting.pad_shape_id,
                        number_of_pad_hits = x.PadPrintSetting.number_of_pad_hits,
                        machine_vendor_id = x.PadPrintSetting.machine_vendor_id,
                        machine_model = x.PadPrintSetting.machine_model,
                        is_rotary_table_used = x.PadPrintSetting.is_rotary_table_used,
                        chemical_hardener = x.PadPrintSetting.chemical_hardener,
                        chemical_additive = x.PadPrintSetting.chemical_additive,
                        chemical_primer = x.PadPrintSetting.chemical_primer,
                        chemical_others = x.PadPrintSetting.chemical_others,
                        article_no_is_general = x.PadPrintSetting.article_no_is_general,
                        article_no_remarks = x.PadPrintSetting.article_no_remarks,
                        component_photo_url = x.PadPrintSetting.component_photo_url,
                        operation_video_url = x.PadPrintSetting.operation_video_url,
                        create_by = x.PadPrintSetting.create_by,
                        create_time = x.PadPrintSetting.create_time,
                        update_by = x.PadPrintSetting.update_by,
                        update_time = x.PadPrintSetting.update_time,
                    }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<PagedList<PBP_Pad_Print_Setting_DTO>> Search(PadPrintSettingParam filterParam, PaginationParams pagination)
        {
            var predModel = PredicateBuilder.New<Model>(true);
            if (!string.IsNullOrEmpty(filterParam.dev_season))
            {
                predModel = predModel.And(x => x.dev_season.Trim().ToUpper().Contains(filterParam.dev_season.Trim().ToUpper()));
            }
            if (!string.IsNullOrEmpty(filterParam.production_season))
            {
                predModel = predModel.And(x => x.prod_season.Trim().ToUpper().Contains(filterParam.production_season.ToUpper().Trim()));
            }

            var PadShapeType = _padShapeTypeRepository.FindAll();
            var Model = _modelRepository.FindAll(predModel);
            var PadPrintSetting = _padPrintSettingRepository.FindAll();
            var data = PadPrintSetting.Join(
                Model,
                x => new { x.factory_id, x.model_no },
                y => new { y.factory_id, y.model_no },
                (x, y) => new { PadPrintSetting = x, Model = y })
                    .GroupJoin(
                        PadShapeType,
                        x => new { x.PadPrintSetting.factory_id, x.PadPrintSetting.pad_shape_id },
                        y => new { y.factory_id, y.pad_shape_id },
                        (x, y) => new { x.PadPrintSetting, x.Model, PadShapeType = y })
                        .SelectMany(
                            x => x.PadShapeType.DefaultIfEmpty(),
                            (x, y) => new PBP_Pad_Print_Setting_DTO
                            {
                                factory_id = x.PadPrintSetting.factory_id,
                                model_no = x.PadPrintSetting.model_no,
                                component_name = x.PadPrintSetting.component_name,
                                material_type_id = x.PadPrintSetting.material_type_id,
                                chemical_ink = x.PadPrintSetting.chemical_ink,
                                dev_season = x.Model.dev_season,
                                production_season = x.Model.prod_season,
                                model_name = x.Model.model_name,
                                article_no_remarks = x.PadPrintSetting.article_no_remarks,
                                material_description = x.PadPrintSetting.material_description,
                                pad_shape_name = y.pad_shape_name,
                                number_of_pad_hits = x.PadPrintSetting.number_of_pad_hits
                            });
            if (!string.IsNullOrEmpty(filterParam.model_no))
            {
                data = data.Where(x => x.model_no.ToUpper().Trim().Contains(filterParam.model_no.ToUpper().Trim()) || x.model_name.Contains(filterParam.model_no.ToUpper().Trim()));
            }
            return await PagedList<PBP_Pad_Print_Setting_DTO>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<OperationResult> Update(PBP_Pad_Print_SettingDTO model)
        {
            if (await FindPadPrintSetting(model))
            {
                return new OperationResult { Caption = "Fail", Success = false, Message = "Data Not Found!" };
            }
            var fu = new FunctionUtility();
            var oldModel = await _padPrintSettingRepository.FindAll(x => x.factory_id.Trim() == model.factory_id.Trim()
                                                                && x.model_no.Trim().ToUpper() == model.model_no.Trim().ToUpper()
                                                                && x.component_name.Trim().ToUpper() == model.component_name.Trim().ToUpper()
                                                                && x.material_type_id.Trim() == model.material_type_id.Trim()
                                                                && x.chemical_ink.Trim() == model.chemical_ink.Trim()).AsNoTracking().FirstOrDefaultAsync();

            var filePath = model.factory_id + "/" + "ProductionBP" + "/" + "PadPrintSetting" + "/" + model.model_no;
            var fileName = Guid.NewGuid().ToString();

            if (!string.IsNullOrEmpty(model.component_photo_url))
            {
                if (!string.IsNullOrEmpty(oldModel.component_photo_url))
                {
                    var currentComponentPhotoUrl = Path.Combine(_webhostEnvironment.WebRootPath, "uploaded/", oldModel.component_photo_url);
                    if (!oldModel.component_photo_url.Contains("no-image"))
                        fu.DeleteFile(currentComponentPhotoUrl);
                }
                var componentPhotoUrl = await fu.UploadAsync(model.component_photo_url_file, "uploaded/" + filePath, fileName);
                model.component_photo_url = filePath + "/" + componentPhotoUrl;
            }
            else
            {
                model.component_photo_url = oldModel.component_photo_url;
            }

            if (!string.IsNullOrEmpty(model.operation_video_url))
            {
                if (!string.IsNullOrEmpty(oldModel.operation_video_url))
                {
                    var currentOperationVideoUrl = Path.Combine(_webhostEnvironment.WebRootPath, "uploaded/", oldModel.operation_video_url);
                    if (!oldModel.operation_video_url.Contains("no-image"))
                        fu.DeleteFile(currentOperationVideoUrl);
                }
                var operationVideoUrl = await fu.UploadAsync(model.operation_video_url_file, "uploaded/" + filePath, fileName);
                model.operation_video_url = filePath + "/" + operationVideoUrl;
            }
            else
            {
                model.operation_video_url = oldModel.operation_video_url;
            }
            var newModel = _mapper.Map<PBP_Pad_Print_Setting>(model);
            newModel.update_time = DateTime.Now;
            _padPrintSettingRepository.Update(newModel);

            if (await _padPrintSettingRepository.SaveAll())
                return new OperationResult { Success = true, Message = "Update Successfully", Caption = "Success" };

            return new OperationResult { Success = false, Caption = "Updating failed on save." };

        }

        public async Task<OperationResult> UploadExcel(IFormFile file, string user)
        {
            if (file == null)
                return new OperationResult(false, "File not found.");

            var uploadFile = $"Sample_PadPrintSetting";
            var uploadPath = @"uploaded\excels";
            var fileName = await new FunctionUtility().UploadAsync(file, uploadPath, uploadFile);
            var filePath = Path.Combine(_webhostEnvironment.WebRootPath, uploadPath, fileName);

            // Đọc file
            var designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(filePath);
            var ws = designer.Workbook.Worksheets[0];
            int rows = ws.Cells.MaxDataRow;
            var dtNow = DateTime.Now;

            if (rows < 1)
                return new OperationResult(false, "An empty excel file", "Error");

            var listPadShape = await GetAllPadShape();
            var listMaterialType = await GetAllMaterialType();
            var listModel = await GetAllModel();
            var listMachineVendor = await GetAllMachineVendor();
            List<PBP_Pad_Print_Setting> listDataUpload = new List<PBP_Pad_Print_Setting>();

            for (var i = 1; i <= rows; i++)
            {
                PBP_Pad_Print_SettingDTO data = new PBP_Pad_Print_SettingDTO();
                data.factory_id = _configuration.GetSection("AppSettings:Factory").Value;
                data.model_no = ws.Cells[i, 0].StringValue?.Trim();
                if (!(_modelRepository.FindAll(x => x.is_active == true && x.model_no.Trim() == data.model_no).AsNoTracking().Any()))
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    return new OperationResult(false, "", "NotModel", data.model_no);
                }

                data.component_name = ws.Cells[i, 1].StringValue?.Trim();
                data.material_type_id = ws.Cells[i, 2].StringValue?.Trim();
                data.chemical_ink = ws.Cells[i, 3].StringValue?.Trim();
                data.material_description = ws.Cells[i, 4].StringValue?.Trim();
                data.pad_shape_id = ws.Cells[i, 5].StringValue?.Trim();
                data.machine_vendor_id = ws.Cells[i, 7].StringValue?.Trim();
                data.machine_model = ws.Cells[i, 10].StringValue?.Trim();
                data.chemical_hardener = ws.Cells[i, 12].StringValue?.Trim();
                data.chemical_additive = ws.Cells[i, 13].StringValue?.Trim();
                data.chemical_primer = ws.Cells[i, 14].StringValue?.Trim();
                data.chemical_others = ws.Cells[i, 15].StringValue?.Trim();
                data.article_no_is_general = ws.Cells[i, 8].StringValue?.Trim() == "Yes" ? true : false;
                data.is_rotary_table_used = ws.Cells[i, 11].StringValue?.Trim() == "Yes" ? true : false;

                var isNumber = int.TryParse(ws.Cells[i, 6].StringValue?.Trim(), out int number);
                if (isNumber)
                {
                    data.number_of_pad_hits = number;
                }
                else
                {
                    continue;
                }

                if (data.article_no_is_general)
                {
                    data.article_no_remarks = null;
                }
                else
                {
                    data.article_no_remarks = ws.Cells[i, 9].StringValue?.Trim();
                    if (string.IsNullOrEmpty(data.article_no_remarks))
                    {
                        continue;
                    }
                }



                if (await FindPadPrintSetting(data))
                {
                    Regex regex = new Regex(@"^[A-Za-z ]+$");
                    if
                    (regex.IsMatch(data.component_name) &&
                        regex.IsMatch(data.chemical_ink) &&
                        !string.IsNullOrEmpty(data.component_name) &&
                        !string.IsNullOrEmpty(data.chemical_ink) &&
                        listModel.Any(x => x.Key == data.model_no) &&
                        listMaterialType.Any(x => x.Key == data.material_type_id) &&
                        !string.IsNullOrEmpty(data.material_description) &&
                        listPadShape.Any(x => x.Key == data.pad_shape_id) &&
                        listMachineVendor.Any(x => x.Key == data.machine_vendor_id) &&
                        !string.IsNullOrEmpty(data.machine_model) &&
                        !string.IsNullOrEmpty(data.chemical_hardener) &&
                        !string.IsNullOrEmpty(data.chemical_additive) &&
                        !string.IsNullOrEmpty(data.chemical_primer) &&
                        !string.IsNullOrEmpty(data.chemical_others)
                    )
                    {
                        data.create_by = user;
                        data.create_time = dtNow;
                        data.update_by = user;
                        data.update_time = dtNow;
                        data.component_photo_url = data.factory_id + "/" + "no-image.jpg";
                        data.operation_video_url = data.factory_id + "/" + "no-image.jpg";
                        var item = _mapper.Map<PBP_Pad_Print_Setting>(data);
                        listDataUpload.Add(item);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
            _padPrintSettingRepository.AddMultiple(listDataUpload);
            if (await _padPrintSettingRepository.SaveAll())
            {
                return new OperationResult { Success = true, Message = "Upload Successfully", Caption = "Success" };
            }
            return new OperationResult { Success = false, Message = "Upload failed on save. Please check the excel data again.", Caption = "Error" };
        }
    }
}