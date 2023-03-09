using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using SmartTooling_API._Services.Interfaces.ProductionBP;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;

namespace SmartTooling_API.Controllers.ProductionBP
{
    [ApiController]
    [Route("api/[controller]")]
    public class BondingReportController : ControllerBase
    {
        private readonly IBondingReportService _BondingReportService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BondingReportController(IBondingReportService bondingReportService, IWebHostEnvironment webHostEnvironment)
        {
            _BondingReportService = bondingReportService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetFactory")]
        public async Task<IActionResult> GetFactory()
        {
            return Ok(await _BondingReportService.GetAllFactory());
        }
        [HttpGet("GetProcessType")]
        public async Task<IActionResult> GetProcessType([FromQuery] string factory_id)
        {
            var data = await _BondingReportService.GetProcessType(factory_id);
            return Ok(data);
        }
        [HttpGet("GetAutoTech")]
        public async Task<IActionResult> GetAutoTech([FromQuery] string factory_id)
        {
            var data = await _BondingReportService.GetAutoTech(factory_id);
            return Ok(data);
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams pagination, [FromQuery] BondingReportParam searchParam)
        {
            var result = await _BondingReportService.Search(pagination, searchParam, true);
            return Ok(result);
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] PaginationParams pagination, [FromQuery] BondingReportParam searchParam)
        {
            var data = await _BondingReportService.Search(pagination, searchParam, false);
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources\Template\ProductionBP\Bonding_Program_Setting_Report.xlsx");

            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            designer.SetDataSource("result", data.Result);
            designer.Process();
            int index = 2;
            data.Result.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(item.shoes_photo_url))
                {
                    int i = ws.Hyperlinks.Add("Q" + index, 1, 1, item.shoes_photo_url_result);
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
            return File(result, "application/xlsx", "Bonding_Program_Setting_Report.xlsx");
        }
    }
}