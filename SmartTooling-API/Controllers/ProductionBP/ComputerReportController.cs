using System.Threading.Tasks;
using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using SmartTooling_API._Services.Interfaces.ProductionBP;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;

namespace SmartTooling_API.Controllers.ProductionBP
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComputerReportController : ControllerBase
    {
        private readonly IComputerReportService _computerReportService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ComputerReportController(IComputerReportService computerReportService, IWebHostEnvironment webHostEnvironment)
        {
            _computerReportService = computerReportService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("GetFactory")]
        public async Task<IActionResult> GetFactory()
        {
            return Ok(await _computerReportService.GetAllFactory());
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams pagination, [FromQuery] ComputerReportParam searchParam)
        {
            var result = await _computerReportService.Search(pagination, searchParam, true);
            return Ok(result);
        }
        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] ComputerReportParam search, [FromQuery] PaginationParams pagination)
        {
            var data = await _computerReportService.Search(pagination, search, false);
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources\Template\ProductionBP\Computer_Stitching_Setting_Report.xlsx");

            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            designer.SetDataSource("result", data.Result);
            designer.Process();

            int index = 2;
            data.Result.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(item.jig_photo_url))
                {
                    int i = ws.Hyperlinks.Add("T" + index, 1, 1, item.jig_photo_urlResult);
                    ws.Hyperlinks[i].TextToDisplay = "click";
                }
                if (!string.IsNullOrEmpty(item.cs_video_url))
                {
                    int j = ws.Hyperlinks.Add("U" + index, 1, 1, item.cs_video_urlResult);
                    ws.Hyperlinks[j].TextToDisplay = "click";
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
            return File(result, "application/xlsx", "Computer_Stitching_Setting_Report.xlsx");
        }
    }
}