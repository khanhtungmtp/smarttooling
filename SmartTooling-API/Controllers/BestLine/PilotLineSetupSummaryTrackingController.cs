
// using Aspose.Cells;
// using Microsoft.AspNetCore.Mvc;
// using SmartTooling_API._Services.Interfaces.BestLine;

// namespace SmartTooling_API.Controllers.BestLine
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class PilotLineSetupSummaryTrackingController : ControllerBase
//     {
//         private readonly IPilotLineSetupSummaryTrackingServices _services;
//         private readonly IWebHostEnvironment _webHostEnvironment;
//         public PilotLineSetupSummaryTrackingController(IPilotLineSetupSummaryTrackingServices services, IWebHostEnvironment webHostEnvironment)
//         {
//             _services = services;
//             _webHostEnvironment = webHostEnvironment;
//         }


//         [HttpGet("GetSeason")]
//         public async Task<IActionResult> GetSeason()
//         {
//             return Ok(await _services.GetProdSeasons());
//         }

//         [HttpGet("GetAllPilots")]
//         public async Task<IActionResult> GetAllPilots(string prodSeason)
//         {
//             var lists = await _services.GetAllPilots(prodSeason);
//             return Ok(lists);
//         }

//         [HttpGet("exportExcel")]
//         public async Task<IActionResult> ExportExcelAspose(string prodSeason)
//         {
//             var data = await _services.GetAllPilots(prodSeason);
//             var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources\\Template\\SmartTool\\Pilot_Line_Setup_Summary_Tracking_Report.xlsx");
//             WorkbookDesigner designer = new WorkbookDesigner();
//             designer.Workbook = new Workbook(path);

//             Worksheet ws = designer.Workbook.Worksheets[0];

//             int index = 3;
//             int firstRowSHC = 2;
//             int firstRowCB = firstRowSHC + data[0].Data.Count();
//             int firstRowTSH = firstRowCB + data[1].Data.Count();
//             int firstRowSPC = firstRowTSH + data[2].Data.Count();

//             foreach (var pilot in data)
//             {
//                 for (var i = 0; i < pilot.Data.Count; i++)
//                 {
//                     ws.Cells["A" + index].PutValue(pilot.Data[i].factory);
//                     ws.Cells["B" + index].PutValue(pilot.Data[i].prod_season);
//                     ws.Cells["C" + index].PutValue(pilot.Data[i].target_number_of_pilot_lines_setup.ToString(("0.##")));
//                     ws.Cells["D" + index].PutValue(pilot.Data[i].actual_number_of_pilot_lines_setup.ToString(("0.##")));
//                     ws.Cells["E" + index].PutValue(pilot.Data[i].pilot_lines_setup_percent.ToString(("0.##")) + " %");
//                     ws.Cells["F" + index].PutValue(pilot.Data[i].pilot_line);
//                     ws.Cells["G" + index].PutValue(pilot.Data[i].target_number_of_rollout_lines_setup_for_pilot_lines.ToString(("0.##")));
//                     ws.Cells["H" + index].PutValue(pilot.Data[i].actual_number_of_rollout_lines_setup_for_pilot_lines.ToString(("0.##")));
//                     ws.Cells["I" + index].PutValue(pilot.Data[i].rollout_lines_setup_percent_for_pilot_lines.ToString(("0.##")) + " %");
//                     index++;
//                 }
//             }
//             // Style
//             var style = designer.Workbook.CreateStyle();
//             style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
//             style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
//             style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
//             style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
//             style.HorizontalAlignment = TextAlignmentType.Center;
//             style.VerticalAlignment = TextAlignmentType.Center;
//             ws.Cells.CreateRange(2, 0, index - 3, 9).SetStyle(style);

//             // Merge cell SHC
//             ws.Cells.Merge(firstRowSHC, 0, data[0].Data.Count, 1);
//             ws.Cells.Merge(firstRowSHC, 1, data[0].Data.Count, 1);
//             ws.Cells.Merge(firstRowSHC, 2, data[0].Data.Count, 1);
//             ws.Cells.Merge(firstRowSHC, 3, data[0].Data.Count, 1);
//             ws.Cells.Merge(firstRowSHC, 4, data[0].Data.Count, 1);

//             // Merge cell CB
//             ws.Cells.Merge(firstRowCB, 0, data[1].Data.Count, 1);
//             ws.Cells.Merge(firstRowCB, 1, data[1].Data.Count, 1);
//             ws.Cells.Merge(firstRowCB, 2, data[1].Data.Count, 1);
//             ws.Cells.Merge(firstRowCB, 3, data[1].Data.Count, 1);
//             ws.Cells.Merge(firstRowCB, 4, data[1].Data.Count, 1);

//             // Merge cell TSH
//             ws.Cells.Merge(firstRowTSH, 0, data[2].Data.Count, 1);
//             ws.Cells.Merge(firstRowTSH, 1, data[2].Data.Count, 1);
//             ws.Cells.Merge(firstRowTSH, 2, data[2].Data.Count, 1);
//             ws.Cells.Merge(firstRowTSH, 3, data[2].Data.Count, 1);
//             ws.Cells.Merge(firstRowTSH, 4, data[2].Data.Count, 1);

//             // Merge cell SPC
//             ws.Cells.Merge(firstRowSPC, 0, data[3].Data.Count, 1);
//             ws.Cells.Merge(firstRowSPC, 1, data[3].Data.Count, 1);
//             ws.Cells.Merge(firstRowSPC, 2, data[3].Data.Count, 1);
//             ws.Cells.Merge(firstRowSPC, 3, data[3].Data.Count, 1);
//             ws.Cells.Merge(firstRowSPC, 4, data[3].Data.Count, 1);

//             MemoryStream stream = new MemoryStream();
//             designer.Workbook.Save(stream, SaveFormat.Xlsx);

//             byte[] result = stream.ToArray();

//             return File(result, "application/xlsx", ("Pilot_Line_Setup_Summary_Tracking_Report"
//                                                     + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")
//                                                     + ".xlsx"));
//         }
//     }
// }