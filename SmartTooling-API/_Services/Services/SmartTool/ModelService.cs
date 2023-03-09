using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Aspose.Cells;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;
using SmartTooling_API.Helpers.Utilities;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Services.Services.SmartTool
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _repo;

        private readonly IModelTypeRepository _repoModelType;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IImageUrlUtility _imageUrlUtility;
        private readonly IWebHostEnvironment _webHostEnv;

        private string factory;
        public ModelService(IModelRepository repo,
                            IMapper mapper,
                            MapperConfiguration configMapper,
                            IModelTypeRepository repoModelType,
                            IConfiguration configuration, IWebHostEnvironment webHostEnv,
                            IImageUrlUtility imageUrlUtility)
        {
            _configMapper = configMapper;
            _mapper = mapper;
            _repo = repo;
            _repoModelType = repoModelType;
            _webHostEnv = webHostEnv;
            factory = configuration.GetSection("AppSettings:Factory").Value;
            _imageUrlUtility = imageUrlUtility;
        }

        public async Task<PagedList<ModelDTO>> SearchModel(PaginationParams param, ModelParam modelParam)
        {
            var pred_Model = PredicateBuilder.New<Model>(true);
            bool active = true;
            if (!String.IsNullOrEmpty(modelParam.active) && modelParam.active != "all")
            {
                if (modelParam.active == "0")
                {
                    active = false;
                }
                pred_Model.And(x => x.is_active == active);
            }
            if (!String.IsNullOrEmpty(modelParam.model_search))
            {
                pred_Model.And(x => x.model_no.Contains(modelParam.model_search) || x.model_name.Contains(modelParam.model_search));
            }
            var list = _repo.FindAll(pred_Model).ProjectTo<ModelDTO>(_configMapper).OrderByDescending(x => x.prod_season);
            return await PagedList<ModelDTO>.CreateAsync(list, param.PageNumber, param.PageSize);
        }
        public async Task<bool> Add(ModelDTO model)
        {
            var models = _mapper.Map<Model>(model);
            models.create_time = DateTime.Now;
            _repo.Add(models);
            return await _repo.SaveAll();
        }

        public async Task<object> GetModelType()
        {
            return await _repoModelType.FindAll(x => x.factory_id.Trim() == factory && x.is_active == true)
            .GroupBy(x => new { x.model_type_id, x.model_type_name })
            .Select(x => new { Id = x.Key.model_type_id, Name = x.Key.model_type_name }).ToListAsync();
        }

        public Task<bool> Delete(object id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ModelDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public ModelDTO GetById(object id)
        {
            var model = _mapper.Map<Model, ModelDTO>(_repo.FindById(id));
            return model;
        }

        public Task<PagedList<ModelDTO>> GetWithPaginations(PaginationParams param)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<ModelDTO>> Search(PaginationParams param, object text)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(ModelDTO model)
        {
            var modelUp = _mapper.Map<Model>(model);
            _repo.Update(modelUp);
            return await _repo.SaveAll();
        }

        public async Task<ModelDTO> GetByFactoryAndModelNo(string facID, string modelNo)
        {
            var model = _mapper.Map<Model, ModelDTO>(await _repo.GetByFactoryAndModelNo(facID, modelNo));
            return model;
        }

        public async Task<OperationResult> UploadExcel(IFormFile file, string username)
        {
            //Lưu file vào wwwroot
            if (file == null)
            {
                return new OperationResult(false, "File not found.");
            }
            var extension = Path.GetExtension(file.FileName).ToLower();
            var uploadFile = $"Sample_Model{extension}";
            string uploadPath = @"uploaded/excels";
            string folder = Path.Combine(_webHostEnv.WebRootPath, uploadPath);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string filePath = Path.Combine(folder, uploadFile);
            if (File.Exists(filePath))
                File.Delete(filePath);
            try
            {
                using (FileStream fs = File.Create(filePath))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

            //Đọc file
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(filePath);
            Worksheet ws = designer.Workbook.Worksheets[0];

            int rows = ws.Cells.MaxDataRow;
            if (rows < 1)
                return new OperationResult(false, "An empty excel file", "Error");
            var dtNow = DateTime.Now;
            for (int i = 1; i <= rows; i++)
            {
                var topModel = ws.Cells[i, 8].StringValue?.Trim();
                var pilotLine = ws.Cells[i, 9].StringValue.Trim();
                var volume = ws.Cells[i, 10].StringValue?.Trim();
                var volume_percent = ws.Cells[i, 11].StringValue?.Trim();
                var excelModel = new Model
                {
                    factory_id = ws.Cells[i, 0].StringValue?.Trim(),
                    model_no = ws.Cells[i, 1].StringValue?.Trim(),
                    model_name = ws.Cells[i, 2].StringValue?.Trim(),
                    model_type_id = ws.Cells[i, 3].StringValue?.Trim(),
                    model_family = ws.Cells[i, 4].StringValue?.Trim(),
                    upper_id = ws.Cells[i, 5].StringValue?.Trim(),
                    dev_season = ws.Cells[i, 6].StringValue?.Trim(),
                    prod_season = ws.Cells[i, 7].StringValue?.Trim(),
                    top_model = topModel == "YES" ? true : false,
                    pilot_line = pilotLine == "YES" ? true : false,
                    volume = !string.IsNullOrEmpty(volume) ? Convert.ToDecimal(volume) : null,
                    volume_percent = !string.IsNullOrEmpty(volume_percent) ? Convert.ToDecimal(volume_percent) : null,
                    is_active = true,
                    remarks = ws.Cells[i, 12].StringValue?.Trim(),
                    model_picture = factory + "/no-image.jpg",
                    create_by = username,
                    create_time = dtNow,
                    update_by = username,
                    update_time = dtNow
                };

                var model = _repo.FindSingle(x => x.factory_id.Trim() == excelModel.factory_id.Trim() && x.model_no.Trim() == excelModel.model_no.Trim());
                if (model != null)
                {
                    model.model_name = excelModel.model_name;
                    model.model_type_id = excelModel.model_type_id;
                    model.dev_season = excelModel.dev_season;
                    model.prod_season = excelModel.prod_season;
                    model.upper_id = excelModel.upper_id;
                    model.remarks = excelModel.remarks;
                    model.volume = excelModel.volume;
                    model.top_model = excelModel.top_model;
                    model.pilot_line = excelModel.pilot_line;
                    model.volume_percent = excelModel.volume_percent;
                    model.model_family = excelModel.model_family;
                    model.model_picture = excelModel.model_picture;
                    model.update_by = username;
                    model.update_time = dtNow;
                    _repo.Update(model);
                }
                else
                {
                    _repo.Add(excelModel);
                }
            }

            await _repo.SaveAll();
            return new OperationResult(true);
        }

        public async Task<PageListUtility<ModelExcelDto>> ExportExcel(PaginationParams param, ModelParam modelParam)
        {
            var pred_Model = PredicateBuilder.New<Model>(true);
            bool active = true;
            if (!String.IsNullOrEmpty(modelParam.active) && modelParam.active != "all")
            {
                if (modelParam.active == "0")
                {
                    active = false;
                }
                pred_Model.And(x => x.is_active == active);
            }
            if (!String.IsNullOrEmpty(modelParam.model_search))
            {
                pred_Model.And(x => x.model_no.Contains(modelParam.model_search) || x.model_name.Contains(modelParam.model_search));
            }

            var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(factory);

            var data = await _repo.FindAll(pred_Model).Join(_repoModelType.FindAll(),
                x => new { model_type_id = x.model_type_id.Trim(), factory_id = x.factory_id.Trim() },
                y => new { model_type_id = y.model_type_id.Trim(), factory_id = y.factory_id.Trim() },
                (x, y) => new { model = x, modelType = y }
            ).Select(x => new ModelExcelDto
            {
                factory_id = x.model.factory_id,
                model_family = x.model.model_family,
                model_no = x.model.model_no,
                model_name = x.model.model_name,
                upper_id = x.model.upper_id,
                model_type_name = x.modelType.model_type_name,
                dev_season = x.model.dev_season,
                prod_season = x.model.prod_season,
                volume = x.model.volume,
                volume_percent = x.model.volume_percent,
                remarks = x.model.remarks,
                model_picture = imageUrl + x.model.model_picture
            }).ToListAsync();

            return PageListUtility<ModelExcelDto>.PageList(data, param.PageNumber, param.PageSize, false);
        }
    }
}