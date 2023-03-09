// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc;
// using SmartTooling_API._Services.Interfaces.BestLine;
// using SmartTooling_API.Helpers;
// using SmartTooling_API.Helpers.Params;

// namespace SmartTooling_API.Controllers.BestLine
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class BLAuditReportController : ControllerBase
//     {
//         private readonly IBLAuditReportService _auditReportService;
//         private readonly IWebHostEnvironment _webHostEnvironment;

//         public BLAuditReportController(IBLAuditReportService auditReportService, IWebHostEnvironment webHostEnvironment)
//         {
//             _auditReportService = auditReportService;
//             _webHostEnvironment = webHostEnvironment;
//         }

//         [HttpGet("GetAllAudit")]
//         public async Task<IActionResult> GetAllAudit([FromQuery] BLSearchAuditReportParam search, [FromQuery] PaginationParams pagination)
//         {
//             var result = await _auditReportService.GetAllAudit(search, pagination);
//             Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
//             return Ok(result);
//         }

//         // Detail
//         [HttpGet("BestPracticeUrl")]
//         public async Task<IActionResult> BestPracticeUrl([FromQuery] BLSearchAuditReportParam param)
//         {
//             var result = await _auditReportService.BestPracticeUrl(param);
//             return Ok(result);
//         }

//         [HttpGet("LayoutLink")]
//         public async Task<IActionResult> LayoutLink([FromQuery] BLSearchAuditReportParam param)
//         {
//             var result = await _auditReportService.LayoutLink(param);
//             return Ok(result);
//         }

//         [HttpGet("GetAuditDetail")]
//         public async Task<IActionResult> GetAuditDetail([FromQuery] BLSearchAuditReportParam search, [FromQuery] PaginationParams pagination)
//         {
//             var result = await _auditReportService.GetAuditDetail(search, pagination);
//             Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
//             return Ok(result);
//         }

//         [HttpGet("GetLineNo")]
//         public async Task<List<KeyValuePair<string, string>>> GetLineNo([FromQuery] string factory) 
//         {
//             var result = await _auditReportService.GetLineNo(factory);
//             return result;
//         }
//     }
// }