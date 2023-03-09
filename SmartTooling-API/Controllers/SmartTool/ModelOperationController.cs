using System.Security.Claims;
using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API.Controllers.SmartTool
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelOperationController : ControllerBase
    {
        private readonly IModelOperationService _modelOperationService;
        private readonly IRFTService _rftService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string username;
        private string factory;
        public ModelOperationController(
            IModelOperationService modelOperationService,
            IRFTService rftService,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            _rftService = rftService;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _modelOperationService = modelOperationService;
            factory = _configuration.GetSection("AppSettings:Factory").Value;
        }

        private string GetUserClaim()
        {
            return username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpPost("All")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams param, [FromBody] ModelOperationParam modelParam)
        {
            var lists = await _modelOperationService.SearchModelOperation(param, modelParam);
            return Ok(lists);
        }

        [HttpGet("model-no")]
        public async Task<IActionResult> GetAllModel() => Ok(await _rftService.GetAllModel());

        [HttpGet("stage")]
        public async Task<IActionResult> GetAllStage() => Ok(await _rftService.GetAllStage());

        [HttpGet("process-type")]
        public async Task<IActionResult> GetAllProcessType() => Ok(await _modelOperationService.GetAllProcessType());

        [HttpPost("create-operation")]
        public async Task<IActionResult> CreateModelOperation([FromBody] ModelOperationDTO modelOperationDto)
        {
            modelOperationDto.update_by = GetUserClaim();
            modelOperationDto.create_by = GetUserClaim();
            modelOperationDto.create_time = DateTime.Now;
            modelOperationDto.factory_id = factory;
            if (await _modelOperationService.Add(modelOperationDto))
            {
                return NoContent();
            }
            throw new Exception("Creating the Model Operation failed on save");
        }

        [HttpPost("getModelOperation")]
        public async Task<IActionResult> GetModelOperation([FromBody] ModelOperationEditParam modelOperationEditParam)
        {
            var modelRepo = await _modelOperationService.GetModelOperation(modelOperationEditParam);
            if (modelRepo != null)
            {
                return Ok(modelRepo);
            }
            return NoContent();
        }

        [HttpPost("updateModelOperation")]
        public async Task<IActionResult> UpdateModelOperation(ModelOperationDTO modelOperationDTO)
        {
            modelOperationDTO.update_by = GetUserClaim();
            modelOperationDTO.update_time = DateTime.Now;
            if (await _modelOperationService.Update(modelOperationDTO))
                return NoContent();
            return BadRequest($"Updating Model Operation failed on save");
        }


        // [HttpPost("deleteModelOperation")]
        // public async Task<IActionResult> DeleteModelOperation(ModelOperationDTO operationDTO)
        // {
        //     if (await _modelOperationService.Delete(operationDTO))
        //         return NoContent();
        //     return BadRequest($"The Model Operation is already in use, it cannot be deleted");
        // }

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExecel([FromBody] ModelOperationParam param)
        {
            byte[] result = Array.Empty<byte>();
            var data = await _modelOperationService.GetExportExcelData(param);

            if (data.Count > 0)
            {
                var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources\Template\SmartTool\Model_Operation_Report.xlsx");
                var designer = new WorkbookDesigner();
                designer.Workbook = new Workbook(path);
                var ws = designer.Workbook.Worksheets[0];

                designer.SetDataSource("result", data);
                designer.Process();

                var stream = new MemoryStream();
                designer.Workbook.Save(stream, SaveFormat.Xlsx);
                result = stream.ToArray();
            }

            return File(result, "application/xlsx", "Model_Operation_Report_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel([FromForm] IFormFile file)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _modelOperationService.UploadExcel(file, user);
            return Ok(result);
        }
    }
}