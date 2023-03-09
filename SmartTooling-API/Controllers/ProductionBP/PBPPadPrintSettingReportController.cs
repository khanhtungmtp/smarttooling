using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using SmartTooling_API._Services.Interfaces.ProductionBP;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;

namespace SmartTooling_API.Controllers.ProductionBP
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class PBPPadPrintSettingReportController : ControllerBase
    {
        private readonly IPBPPadPrintSettingReportService _iPBPPadPrintService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public PBPPadPrintSettingReportController(IPBPPadPrintSettingReportService iPBPPadPrintService,
        IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _iPBPPadPrintService = iPBPPadPrintService;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [HttpGet("GetFactory")]
        public async Task<IActionResult> GetFactory()
        {
            return Ok(await _iPBPPadPrintService.GetAllFactory());
        }

        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams pagination, PBPPadPrintSettingParams searchParam)
        {
            var data = await _iPBPPadPrintService.Search(pagination, searchParam);
            return Ok(data);
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] PaginationParams pagination,[FromQuery] PBPPadPrintSettingParams searchParam)
        {
            var data = await _iPBPPadPrintService.Search(pagination, searchParam, false);
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources\Template\ProductionBP\Pad_Print_Setting_Report.xlsx");

            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            designer.SetDataSource("result", data.Result);
            designer.Process();
            int index = 2;
            data.Result.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(item.component_photo_url))
                {
                    int i = ws.Hyperlinks.Add("U" + index, 1, 1, item.component_photo_url_result);
                    ws.Hyperlinks[i].TextToDisplay = "click";
                }
                if(!string.IsNullOrEmpty(item.operation_video_url)){
                    int i = ws.Hyperlinks.Add("V" + index, 1, 1, item.operation_video_url_result);
                    ws.Hyperlinks[i].TextToDisplay = "click";
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
            return File(result, "application/xlsx", "Pad_Print_Setting_Report.xlsx");
        }
    }
}