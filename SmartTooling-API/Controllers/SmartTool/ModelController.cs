using System.Net;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Aspose.Cells;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API.Controllers.SmartTool
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private string username;
        private string factory;

        public ModelController(IModelService modelService, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _modelService = modelService;
            _webHostEnvironment = webHostEnvironment;
            factory = configuration.GetSection("AppSettings:Factory").Value;
        }

        private string GetUserClaim()
        {
            return username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpPost("model-list")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams param, ModelParam modelParam)
        {
            var lists = await _modelService.SearchModel(param, modelParam);
            Response.AddPagination(lists.CurrentPage, lists.PageSize, lists.TotalCount, lists.TotalPages);
            return Ok(lists);
        }

        [HttpPost("createModel")]
        public async Task<IActionResult> CreateModel([FromBody] ModelDTO modelDto)
        {
            modelDto.update_by = GetUserClaim();
            modelDto.create_by = GetUserClaim();
            modelDto.factory_id = factory;
            string folder = _webHostEnvironment.WebRootPath + "\\uploaded\\" + factory + "\\Model\\";
            if (modelDto.model_picture == null || modelDto.model_picture == "")
            {
                var fileName = "no-image.jpg";
                modelDto.model_picture = factory + "/Model/" + fileName;
            }
            else
            {
                var source = modelDto.model_picture;
                string base64 = source.Substring(source.IndexOf(',') + 1);
                base64 = base64.Trim('\0');
                byte[] modelData = Convert.FromBase64String(base64);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var fileName = factory + "_" + modelDto.model_no + ".jpg";
                string filePathImages = Path.Combine(folder, fileName);
                System.IO.File.WriteAllBytes(filePathImages, modelData);
                modelDto.model_picture = factory + "/Model/" + fileName;
            }
            if (await _modelService.Add(modelDto))
            {
                return NoContent();
            }
            throw new Exception("Creating the Model failed on save");
        }


        [HttpGet("model-type")]
        public async Task<IActionResult> GetAllModelType()
        {
            var data = await _modelService.GetModelType();
            return Ok(data);
        }

        [HttpPost("updateModel")]
        public async Task<IActionResult> updateModel([FromBody] ModelDTO modelDto)
        {
            modelDto.update_by = GetUserClaim();
            modelDto.update_time = DateTime.Now;
            string folder = _webHostEnvironment.WebRootPath + "\\uploaded\\" + factory + "\\Model\\";
            if (modelDto.model_picture.Length > 100)
            {
                var source = modelDto.model_picture;
                string base64 = source.Substring(source.IndexOf(',') + 1);
                base64 = base64.Trim('\0');
                byte[] modelData = Convert.FromBase64String(base64);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var fileName = factory + "_" + modelDto.model_no + ".jpg";
                string filePathImages = Path.Combine(folder, fileName);
                // kiểm tra file cũ có chưa xóa đi
                if (System.IO.File.Exists(filePathImages))
                {
                    System.IO.File.Delete(filePathImages);
                }
                System.IO.File.WriteAllBytes(filePathImages, modelData);
                modelDto.model_picture = factory + "/Model/" + fileName;
            }

            if (await _modelService.Update(modelDto))
            {
                return NoContent();
            }
            throw new Exception("Creating the Model failed on save");
        }

        [HttpGet("edit/{modelNo}")]
        public async Task<ActionResult> GetByFactoryAndModelNo(string modelNo)
        {
            var modelRepo = await _modelService.GetByFactoryAndModelNo(factory, modelNo);
            if (modelRepo != null)
            {
                return Ok(modelRepo);
            }
            return NoContent();
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] PaginationParams param, [FromQuery] ModelParam modelParam)
        {
            var data = await _modelService.ExportExcel(param, modelParam);
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources\Template\SmartTool\Model_Report.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            designer.SetDataSource("result", data.Result);
            designer.Process();

            int index = 2;
            data.Result.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(item.model_picture))
                {
                    int i = ws.Hyperlinks.Add("L" + index, 1, 1, item.model_picture);
                    ws.Hyperlinks[i].TextToDisplay = "Click";
                }
                index++;
            });
            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;

            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] result = stream.ToArray();
            return File(result, "application/xlsx", "Model_Report.xlsx");
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel([FromForm] IFormFile file)
        {
            var user = GetUserClaim();
            var result = await _modelService.UploadExcel(file, user);
            return Ok(result);
        }
    }
}