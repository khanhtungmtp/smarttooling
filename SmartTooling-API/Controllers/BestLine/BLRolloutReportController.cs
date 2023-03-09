// using System.Collections.Generic;
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
//     public class BLRolloutReportController : ControllerBase
//     {
//         private readonly IBLRolloutReportService _rolloutReportService;
//         private readonly IWebHostEnvironment _webHostEnvironment;

//         public BLRolloutReportController(IBLRolloutReportService rolloutReportService, IWebHostEnvironment webHostEnvironment)
//         {
//             _rolloutReportService = rolloutReportService;
//             _webHostEnvironment = webHostEnvironment;
//         }

//         [HttpGet("GetAllRollout")]
//         public async Task<IActionResult> GetAllRollout([FromQuery] BLSearchRolloutReportParam search, [FromQuery] PaginationParams pagination) {
//             var result = await _rolloutReportService.GetAllRollout(search, pagination);
//             Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
//             return Ok(result);
//         }

//         [HttpGet("GetFactory")]
//         public async Task<List<KeyValuePair<string, string>>> GetFactory() {
//             var result = await _rolloutReportService.GetFactory();
//             return result;
//         }

//         [HttpGet("GetLineNo")]
//         public async Task<List<KeyValuePair<string, string>>> GetLineNo([FromQuery] string factory) {
//             var result = await _rolloutReportService.GetLineNo(factory);
//             return result;
//         }

//         [HttpGet("GetLineType")]
//         public async Task<List<KeyValuePair<string, string>>> GetLineType([FromQuery] string factory) {
//             var result = await _rolloutReportService.GetLineType(factory);
//             return result;
//         }

//         [HttpGet("GetStage")]
//         public async Task<List<KeyValuePair<string, string>>> GetStage([FromQuery] string factory) {
//             var result = await _rolloutReportService.GetStage(factory);
//             return result;
//         }

//     }
// }