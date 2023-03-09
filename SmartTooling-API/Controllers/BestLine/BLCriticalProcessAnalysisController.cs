// using System.Security.Claims;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using SmartTooling_API._Services.Interfaces.BestLine;
// using SmartTooling_API.DTO.BestLine;
// using SmartTooling_API.Helpers.Params;
// using SmartTooling_API.Helpers.Params.BestLine;

// namespace SmartTooling_API.Controllers.BestLine
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     // [Authorize]
//     public class BLCriticalProcessAnalysisController : ControllerBase
//     {
//         private readonly IBLCriticalProcessAnalysisService _iBLCriticalProcessAnalysisService;
//         private readonly IWebHostEnvironment _webHostEnvironment;
//         private readonly IConfiguration _configuration;

//         public BLCriticalProcessAnalysisController(IBLCriticalProcessAnalysisService iBLCriticalProcessAnalysisService,
//                                             IWebHostEnvironment webHostEnvironment,
//                                             IConfiguration configuration)
//         {
//             _iBLCriticalProcessAnalysisService = iBLCriticalProcessAnalysisService;
//             _webHostEnvironment = webHostEnvironment;
//             _configuration = configuration;
//         }

//         [HttpGet("GetLineNo")]
//         public async Task<IActionResult> GetLineNo()
//         {
//             var data = await _iBLCriticalProcessAnalysisService.GetLineNo();
//             return Ok(data);
//         }
//         [HttpGet("GetStage")]
//         public async Task<IActionResult> GetStage()
//         {
//             var data = await _iBLCriticalProcessAnalysisService.GetStage();
//             return Ok(data);
//         }
//         [HttpGet("GetModel")]
//         public async Task<IActionResult> GetModel()
//         {
//             var data = await _iBLCriticalProcessAnalysisService.GetModelNo();
//             return Ok(data);
//         }
//         [HttpGet("GetOperationName")]
//         public async Task<IActionResult> GetOperationName(string model_no, string stage)
//         {
//             var data = await _iBLCriticalProcessAnalysisService.GetOperationName(model_no, stage);
//             return Ok(data);
//         }
//         [HttpGet("GetLineType")]
//         public async Task<IActionResult> GetLineType([FromQuery] string line_id)
//         {
//             var data = await _iBLCriticalProcessAnalysisService.GetLineType(line_id);
//             return Ok(data);
//         }
//         [HttpPost("GetAll")]
//         public async Task<IActionResult> GetAll(SearchBLCriticalParam search, [FromQuery] PaginationParams pagination)
//         {
//             var data = await _iBLCriticalProcessAnalysisService.GetAll(search, pagination);
//             return Ok(data);
//         }

//         [HttpGet("GetData")]
//         public async Task<IActionResult> GetData([FromQuery] CriticalParam param)
//         {
//             var result = await _iBLCriticalProcessAnalysisService.GetData(param);
//             return Ok(result);
//         }

//         [HttpPost("Create")]
//         public async Task<IActionResult> Create([FromBody] BL_Critical_Process_AnalysisDTO BLCriticalDto)
//         {
//             BLCriticalDto.factory_id = _configuration.GetSection("Appsettings:Factory").Value;
//             BLCriticalDto.update_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             BLCriticalDto.create_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             var result = await _iBLCriticalProcessAnalysisService.Create(BLCriticalDto);
//             return Ok(result);
//         }
//         [HttpPut("Update")]
//         public async Task<IActionResult> Update([FromBody] BL_Critical_Process_AnalysisDTO BLCriticalDto)
//         {
//             BLCriticalDto.factory_id = _configuration.GetSection("Appsettings:Factory").Value;
//             BLCriticalDto.update_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             var result = await _iBLCriticalProcessAnalysisService.Update(BLCriticalDto);
//             return Ok(result);
//         }
//     }
// }