// using System;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using SmartTooling_API._Services.Interfaces.BestLine;
// using SmartTooling_API.DTO.BestLine;
// using SmartTooling_API.Helpers.Params;

// namespace SmartTooling_API.Controllers.BestLine
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class RolloutAuditController : ControllerBase
//     {
//         private readonly IRolloutAuditService _rolloutAuditService;
//         private readonly IConfiguration _configuration;
//         private readonly IWebHostEnvironment _webHostEnvironment;

//         public RolloutAuditController(IRolloutAuditService rolloutAuditService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
//         {
//             _rolloutAuditService = rolloutAuditService;
//             _configuration = configuration;
//             _webHostEnvironment = webHostEnvironment;
//         }
//         [HttpGet("getrolloutlineno")]
//         public async Task<IActionResult> getLineNo()
//         {
//             var result = await _rolloutAuditService.GetLineNo();
//             return Ok(result);
//         }
//         [HttpGet("getlinetype")]
//         public async Task<IActionResult> getLineType([FromQuery] string line_type_id)
//         {
//             var result = await _rolloutAuditService.GetType(line_type_id);
//             return Ok(result);
//         }
//         [HttpGet("search")]
//         public async Task<IActionResult> Search(string rollout_line_id, string line_type_id, string text, [FromQuery] PaginationParams param)
//         {
//             var result = await _rolloutAuditService.Search(rollout_line_id, line_type_id, text, param);
//             return Ok(result);
//         }
//         //add=======================
//         [HttpGet("getmodelno")]
//         public async Task<IActionResult> GetModelNo([FromQuery] Params_Rollout_ProgressDTO parms_get)
//         {
//             var result = await _rolloutAuditService.GetModelNo(parms_get);
//             return Ok(result);
//         }
//         [HttpGet("getstage")]
//         public async Task<IActionResult> GetStage([FromQuery] Params_Rollout_ProgressDTO parms_get)
//         {
//             var result = await _rolloutAuditService.GetStage(parms_get);
//             return Ok(result);
//         }
//         [HttpGet("getoperaname")]
//         public async Task<IActionResult> GetOperaName([FromQuery] Params_Rollout_ProgressDTO parms_get)
//         {
//             var result = await _rolloutAuditService.GetOperaName(parms_get);
//             return Ok(result);
//         }
//         [HttpPost("auditcount")]
//         public async Task<IActionResult> GetAuditCount([FromBody] Params_Rollout_ProgressDTO model)
//         {
//             var result = await _rolloutAuditService.GetAuditCount(model);
//             return Ok(result);
//         }
//         [HttpPost("getremarks")]
//         public async Task<IActionResult> GetRemarks([FromBody] Params_Rollout_ProgressDTO model)
//         {
//             var result = await _rolloutAuditService.GetRemarks(model);
//             return Ok(result);
//         }
//         [HttpPost("add")]
//         public async Task<IActionResult> AddNew(BL_Rollout_AuditDTO model)
//         {
//             model.create_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             model.create_time = DateTime.Now;
//             model.update_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             model.update_time = DateTime.Now;
//             model.factory_id = _configuration.GetSection("AppSettings:Factory").Value;
//             var result = await _rolloutAuditService.AddNew(model);
//             return Ok(result);
//         }
//         [HttpPut("edit")]
//         public async Task<IActionResult> Update(BL_Rollout_AuditDTO model)
//         {
//             model.update_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             model.update_time = DateTime.Now;
//             model.factory_id = _configuration.GetSection("AppSettings:Factory").Value;
//             var result = await _rolloutAuditService.Edit(model);
//             return Ok(result);
//         }
//         [HttpPost("getaudit")]
//         public async Task<IActionResult> GetBL_Rollout_Audt([FromBody] Params_Rollout_ProgressDTO parms_get, [FromQuery] int audit_count)
//         {
//             var result = await _rolloutAuditService.GetBL_Rollout_Audt(parms_get, audit_count);
//             return Ok(result);
//         }
//     }
// }