

// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using SmartTooling_API._Services.Interfaces.BestLine;
// using SmartTooling_API.Helpers;
// using SmartTooling_API.Helpers.Params;
// using SmartTooling_API.Helpers.Params.BestLine;

// namespace SmartTooling_API.Controllers.BestLine
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class C2BOverallLayoutReportController : ControllerBase
//     {
//         private readonly IC2BOverallLayoutReportService _service;
//         public C2BOverallLayoutReportController(IC2BOverallLayoutReportService service)
//         {
//             _service = service;
//         }

//         [HttpGet("GetFactory")]
//         public async Task<IActionResult> GetFactory()
//         {
//             return Ok(await _service.GetAllFactory());
//         }

//         [HttpGet("Search")]
//         public async Task<IActionResult> Search([FromQuery] C2BOverallLayoutReportParam searchParam, [FromQuery] PaginationParams pagination)
//         {
//             var result = await _service.Search(pagination, searchParam);
//             Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
//             return Ok(result);
//         }

//         [HttpGet("GetLine")]
//         public async Task<IActionResult> GetLine([FromQuery] string factory_id)
//         {
//             var data = await _service.GetLineNo(factory_id);
//             return Ok(data);
//         }

//         [HttpGet("GetLineType")]
//         public async Task<IActionResult> GetLineType([FromQuery] string factory_id)
//         {
//             var data = await _service.GetLineType(factory_id);
//             return Ok(data);
//         }

//         [HttpGet("GetDetail")]
//         public async Task<IActionResult> GetDetail([FromQuery] C2BOverallLayoutReportParam searchParam)
//         {
//             var data = await _service.GetDetailDTO(searchParam);
//             return Ok(data);
//         }

//         [HttpGet("GetFiles")]
//         public async Task<IActionResult> GetFiles([FromQuery] C2BOverallLayoutReportParam searchParam, [FromQuery] PaginationParams pagination)
//         {
//             var result = await _service.GetFilesDTO(pagination, searchParam);
//             Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
//             return Ok(result);
//         }
//     }
// }