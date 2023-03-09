// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using SmartTooling_API._Services.Interfaces.BestLine;
// using SmartTooling_API.DTO.BestLine;
// using SmartTooling_API.Helpers;
// using SmartTooling_API.Helpers.Params;
// using SmartTooling_API.Helpers.Params.BestLine;

// namespace SmartTooling_API.Controllers.BestLine
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class CriticalProcessReportController : ControllerBase
//     {
//         private readonly ICriticalProcessReportService _service;
//         private readonly IConfiguration _configuration;

//         public CriticalProcessReportController(ICriticalProcessReportService service, IConfiguration configuration)
//         {
//             _service = service;
//             _configuration = configuration;
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

//         [HttpGet("GetStage")]
//         public async Task<IActionResult> GetStage([FromQuery] string factory_id)
//         {
//             var data = await _service.GetStage(factory_id);
//             return Ok(data);
//         }

//         [HttpGet("GetFactory")]
//         public async Task<IActionResult> GetFactory()
//         {
//             return Ok(await _service.GetAllFactory());
//         }

//         [HttpGet("Search")]
//         public async Task<IActionResult> Search([FromQuery] CriticalProcessReportParam searchParam, [FromQuery] PaginationParams pagination)
//         {

//             var result = await _service.Search(pagination, searchParam);
//             Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
//             return Ok(result);
//         }

//         [HttpPost("GetDetail")]
//         public async Task<IActionResult> Get([FromBody] CriticalProcessReportParam searchParam)
//         {
//             var data = await _service.GetDetail(searchParam);
//             return Ok(data);
//         }

//         [HttpPost("GetKaizen")]
//         public async Task<IActionResult> GetKaizen([FromBody] CriticalProcessReportParam searchParam, [FromQuery] PaginationParams pagination)
//         {
//             var result = await _service.GetKaizen(pagination, searchParam);
//             Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
//             return Ok(result);
//         }


//         [HttpGet("getKaizenDetail")]
//         public async Task<IActionResult> GetKaizenDetail(string factory_id, string model_no, string serial_no)
//         {
//             var data = await _service.GetKaizenDetail(factory_id, model_no, serial_no);
//             return Ok(data);
//         }
//     }
// }