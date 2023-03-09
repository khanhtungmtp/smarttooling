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
//     public class C2BLayoutByProcessReportController : ControllerBase
//     {
//         private readonly IC2BLayoutByProcessReportService _service;

//         public C2BLayoutByProcessReportController(IC2BLayoutByProcessReportService service)
//         {
//             _service = service;
//         }

//         [HttpGet("Search")]
//         public async Task<IActionResult> Search([FromQuery] C2BLayoutByProcessReportParam searchParam, [FromQuery] PaginationParams pagination)
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

//         [HttpGet("GetProcessType")]
//         public async Task<IActionResult> GetProcessType([FromQuery] string factory_id)
//         {
//             var data = await _service.GetProcessType(factory_id);
//             return Ok(data);
//         }

//         [HttpGet("GetLayoutDesignProcess")]
//         public async Task<IActionResult> GetLayoutDesignProcess([FromQuery] C2BLayoutByProcessReportParam searchParam)
//         {
//             var data = await _service.GetLayoutDesignProcess(searchParam);
//             return Ok(data);
//         }
//         [HttpGet("GetFactory")]
//         public async Task<IActionResult> GetFactory()
//         {
//             return Ok(await _service.GetAllFactory());
//         }

//         [HttpGet("GetGraphData")]
//         public async Task<IActionResult> GetLayoutDesignProcessData([FromQuery] C2BLayoutByProcessReportParam searchParam)
//         {
//             var data = await _service.GetLayoutDesignProcessData(searchParam);
//             return Ok(data);
//         }

//     }
// }