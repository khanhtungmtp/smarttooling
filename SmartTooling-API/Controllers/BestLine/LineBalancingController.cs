// using System;
// using System.IO;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using Aspose.Cells;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using SmartTooling_API._Services.Interfaces.BestLine;
// using SmartTooling_API.Helpers.Params.BestLine;

// namespace SmartTooling_API.Controllers.BestLine
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class LineBalancingController : ControllerBase
//     {
//         private readonly IWebHostEnvironment _webHostEnvironment;
//         private readonly ILineBalancingService _lineBalancingService;
//         private readonly IConfiguration _configuration;
//         private string factory;
//         public LineBalancingController(
//             IWebHostEnvironment webHostEnvironment, ILineBalancingService lineBalancingService,
//             IConfiguration configuration
//         )
//         {
//             _webHostEnvironment = webHostEnvironment;
//             _lineBalancingService = lineBalancingService;
//             _configuration = configuration;
//             factory = _configuration.GetSection("Appsettings:Factory").Value;
//         }

//         [HttpGet("GetLineID")]
//         public async Task<IActionResult> GetLineID()
//         {
//             var data = await _lineBalancingService.GetLineID();
//             return Ok(data);
//         }

//         [HttpGet("GetLineTypeID")]
//         public async Task<IActionResult> GetLineTypeID(string lineID)
//         {
//             var data = await _lineBalancingService.GetLineType(lineID);
//             return Ok(data);
//         }

//         [HttpGet("GetModelNo")]
//         public async Task<IActionResult> GetModelNo(string lineID, string lineTypeID)
//         {
//             var data = await _lineBalancingService.GetModelNo(lineID, lineTypeID);
//             return Ok(data);
//         }

//         [HttpGet("GetProcessType")]
//         public async Task<IActionResult> GetProcessType(string lineID, string lineTypeID, string modelNo)
//         {
//             var data = await _lineBalancingService.GetProcessType(lineID, lineTypeID, modelNo);
//             return Ok(data);
//         }

//         [HttpGet("GetData")]
//         public async Task<IActionResult> GetData([FromQuery] BL_Layout_Design_Process_Data_Params_DTO line)
//         {
//             var data = await _lineBalancingService.GetData(line);
//             return Ok(data);
//         }

//         [HttpGet("GetGraphData")]
//         public async Task<IActionResult> GetLayoutDesignProcessData([FromQuery] BL_Layout_Design_Process_Data_Params_DTO line)
//         {
//             var data = await _lineBalancingService.GetLayoutDesignProcessData(line);
//             return Ok(data);
//         }

//         [HttpGet("exportExcelAspose")]
//         public async Task<IActionResult> ExportExcelAspose([FromQuery] BL_Layout_Design_Process_Data_Params_DTO param)
//         {
//             var data = await _lineBalancingService.GetLayoutDesignProcessData(param);
//             var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources\\Template\\BestLine\\LineBalancing.xlsx");
//             WorkbookDesigner designer = new WorkbookDesigner();
//             designer.Workbook = new Workbook(path);

//             Worksheet ws = designer.Workbook.Worksheets[0];
//             Style styleTextLeft = designer.Workbook.CreateStyle();
//             styleTextLeft = await GetStyle(styleTextLeft);
//             styleTextLeft.VerticalAlignment = TextAlignmentType.Left;

//             Style styleTextRight = designer.Workbook.CreateStyle();
//             styleTextRight = await GetStyle(styleTextRight);
//             styleTextRight.VerticalAlignment = TextAlignmentType.Right;

//             StyleFlag flg = new StyleFlag();
//             flg.All = true;

//             int index = 3;

//             Aspose.Cells.Range rangeNoNameBefore = ws.Cells.CreateRange(index - 1, 0, data.listDataBefore.Count, 1);
//             rangeNoNameBefore.ApplyStyle(styleTextLeft, flg);
//             Aspose.Cells.Range rangeBefore = ws.Cells.CreateRange(index - 1, 1, data.listDataBefore.Count, 3);
//             rangeBefore.ApplyStyle(styleTextRight, flg);
//             Aspose.Cells.Range rangeNoNameAfter = ws.Cells.CreateRange(index - 1, 4, data.listDataBefore.Count, 1);
//             rangeNoNameAfter.ApplyStyle(styleTextLeft, flg);
//             Aspose.Cells.Range rangeAfter = ws.Cells.CreateRange(index - 1, 5, data.listDataBefore.Count, 3);
//             rangeAfter.ApplyStyle(styleTextRight, flg);

//             for (var i = 0; i < data.listDataBefore.Count; i++)
//             {
//                 ws.Cells["A" + index].PutValue(data.listNodeNameBefore[i]);
//                 ws.Cells["B" + index].PutValue(data.listDataBefore[i]);
//                 ws.Cells["C" + index].PutValue(data.listTaktTimeBefore[i]);
//                 ws.Cells["D" + index].PutValue(data.listEmployeeBefore[i]);
//                 ws.Cells["E" + index].PutValue(data.listNodeNameAfter[i]);
//                 ws.Cells["F" + index].PutValue(data.listDataAfter[i]);
//                 ws.Cells["G" + index].PutValue(data.listTaktTimeAfter[i]);
//                 ws.Cells["H" + index].PutValue(data.listEmployeeAfter[i]);
//                 index++;
//             }
//             MemoryStream stream = new MemoryStream();
//             designer.Workbook.Save(stream, SaveFormat.Xlsx);

//             byte[] result = stream.ToArray();

//             return File(result, "application/xlsx", (factory + "_" + param.line_id.Trim() + "_"
//                                                     + param.line_type_id + "_" + param.model_no + "_"
//                                                     + param.process_type_id).Trim() + "_"
//                                                     + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")
//                                                     + ".xlsx");
//         }

//         private async Task<Style> GetStyle(Style style)
//         {
//             style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
//             style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
//             style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
//             style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
//             return await Task.FromResult(style);
//         }

//         [HttpPost("importExcel")]
//         public async Task<IActionResult> importExcel([FromForm] BL_Layout_Design_Process_Data_Params param, IFormFile file)
//         {
//             var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             string fileNameExtension = (file.FileName.Split("."))[(file.FileName.Split(".")).Length - 1];
//             string fileName = (factory + "_" + param.line_id.Trim() + "_" + param.line_type_id + "_"
//                             + param.model_no + "_" + param.process_type_id).Trim()
//                             + "." + fileNameExtension;

//             string folder = _webHostEnvironment.WebRootPath + "/uploaded/" + factory + "/BestLine/excels/"
//                             + param.line_id.Trim() + "/" + param.line_type_id + "/" + param.model_no + "/"
//                             + param.process_type_id + "/";
//             if (!Directory.Exists(folder))
//             {
//                 Directory.CreateDirectory(folder);
//             }
//             string filePath = Path.Combine(folder, fileName);
//             if (System.IO.File.Exists(filePath))
//                 System.IO.File.Delete(filePath);
//             using (FileStream fs = System.IO.File.Create(filePath))
//             {
//                 file.CopyTo(fs);
//                 fs.Flush();
//             }
//             var result = await _lineBalancingService.ImportData(param, user, filePath);
//             return Ok(result);
//         }
//     }
// }