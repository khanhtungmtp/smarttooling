using System;
using System.IO;
using System.Linq;
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
    public class KaizenReportController : ControllerBase
    {
        private readonly IKaizenReportService _serviceKaizenReport;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string factory;
        public KaizenReportController(IKaizenReportService serviceKaizenReport,
                                        IWebHostEnvironment webHostEnvironment,
                                        IConfiguration configuration)
        {
            _serviceKaizenReport = serviceKaizenReport;
            _webHostEnvironment = webHostEnvironment;
            factory = configuration.GetSection("AppSettings:Factory").Value;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams param, KaizenReportParam filter)
        {
            var result = await _serviceKaizenReport.Search(param, filter, factory);
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost("exportExcel")]
        public async Task<IActionResult> ExportExcel(KaizenReportParam filter)
        {
            var data = await _serviceKaizenReport.GetModelKaizens(factory, filter);
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


        [HttpGet("getSeason/{upper_id}")]
        public async Task<IActionResult> GetSeasonByUpper(string upper_id)
        {
            var data = await _serviceKaizenReport.GetSeasonByUpper(factory, upper_id);
            return Ok(data);
        }

        [HttpGet("getEfficiencys")]
        public async Task<IActionResult> GetEfficiencys(string upper_id, string season)
        {
            var data = await _serviceKaizenReport.GetEfficiencys(factory, upper_id, season);
            return Ok(data);
        }

        [HttpGet("getKaizens/{model_no}")]
        public async Task<IActionResult> GetKaizens([FromQuery] PaginationParams param, string model_no)
        {
            var result = await _serviceKaizenReport.GetKaiZens(param, factory, model_no);
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost("updateClickTimes")]
        public async Task<IActionResult> UpdateClickTimes([FromBody] KaizenModelDetail model)
        {
            var result = await _serviceKaizenReport.UpdateClickTimes(model);
            return Ok(result);
        }

        [HttpGet("getKaizenDetail")]
        public async Task<IActionResult> GetKaizenDetails(string model_no, string serial_no)
        {
            var data = await _serviceKaizenReport.GetKaizenDetail(factory, model_no, serial_no);
            return Ok(data);
        }

    }
}