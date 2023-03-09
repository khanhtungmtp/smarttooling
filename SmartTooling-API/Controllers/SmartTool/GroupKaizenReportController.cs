using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Aspose.Cells;
using Microsoft.AspNetCore.Hosting;
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
    public class GroupKaizenReportController : ControllerBase
    {
        private readonly IGroupKaizenReportService _service;
        private readonly IKaizenReportService _serviceKaizenReport;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public GroupKaizenReportController(IGroupKaizenReportService service,
                                            IKaizenReportService serviceKaizenReport,
                                            IWebHostEnvironment webHostEnvironment,
                                            IConfiguration configuration)
        {
            _service = service;
            _serviceKaizenReport = serviceKaizenReport;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [HttpGet("getAllFactory")]
        public async Task<IActionResult> GetAllFactory()
        {
            var data = await _service.GetAllFactory();
            return Ok(data);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams param, KaizenReportGroupParam filterParam)
        {
            var result = await _service.Search(param, filterParam);
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost("exportExcel")]
        public async Task<IActionResult> ExportExcel(KaizenReportGroupParam filterParam)
        {
            var data = await _service.GetModelKaizens(filterParam);
            WorkbookDesigner designer = new WorkbookDesigner();
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources\\Template\\KaizenReport.xlsx");
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            designer.SetDataSource("result", data);
            designer.Process();

            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);

            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "Excel" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");
        }


        [HttpGet("getSeason")]
        public async Task<IActionResult> GetSeasonByUpper(string factory_id, string upper_id)
        {
            var data = await _service.GetSeasonByUpper(factory_id, upper_id);
            return Ok(data);
        }

        [HttpGet("getEfficiencys")]
        public async Task<IActionResult> GetEfficiencys(string factory_id, string upper_id, string season)
        {
            var data = await _service.GetEfficiencys(factory_id, upper_id, season);
            return Ok(data);
        }

        [HttpGet("getKaiZens")]
        public async Task<IActionResult> GetKaiZens([FromQuery] PaginationParams param, string factory_id, string model_no)
        {
            var result = await _service.GetKaiZens(param, factory_id, model_no);
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost("updateClickTimes")]
        public async Task<IActionResult> UpdateClickTimes([FromBody] KaizenModelDetail model)
        {
            _configuration.GetSection("AppSettings:DataSeach").Value = model.factory_id.Trim();
            var result = await _serviceKaizenReport.UpdateClickTimes(model);
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return Ok(result);
        }

        [HttpGet("getKaizenDetail")]
        public async Task<IActionResult> GetKaizenDetail(string factory_id, string model_no, string serial_no)
        {
            var data = await _service.GetKaizenDetail(factory_id, model_no, serial_no);
            return Ok(data);
        }

        [HttpGet("getFactory")]
        public async Task<IActionResult> GetFactory()
        {
            var data = _configuration.GetSection("AppSettings:Factory").Value;
            var result = await Task.FromResult(data);
            return Ok(result);
        }
        [HttpPost("addCrossSiteSharing")]
        public async Task<IActionResult> AddCrossSiteSharing([FromBody] Kaizen_Benefits_Application_FormDTO model)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var factory = _configuration.GetSection("AppSettings:Factory").Value;
            _configuration.GetSection("AppSettings:DataSeach").Value = model.factory_id.Trim();
            model.to_factory_id = factory;
            model.update_by = username;
            model.create_by = username;
            var result = await _service.AddCross(model);
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return Ok(result);
        }
    }
}