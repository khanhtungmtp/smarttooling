// using System;
// using System.IO;
// using System.Security.Claims;
// using System.Threading.Tasks;
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
//     public class RolloutProgressController : ControllerBase
//     {
//         private readonly IRolloutProgressService _RolloutProgressService;
//         private readonly IConfiguration _configuration;
//         private readonly IWebHostEnvironment _webHostEnvironment;

//         public RolloutProgressController(IRolloutProgressService rolloutProgressService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
//         {
//             _RolloutProgressService = rolloutProgressService;
//             _configuration = configuration;
//             _webHostEnvironment = webHostEnvironment;
//         }

//         [HttpGet("getlineno")]
//         public async Task<IActionResult> getLineNo()
//         {
//             var result = await _RolloutProgressService.GetLineNo();
//             return Ok(result);
//         }
//         [HttpGet("getlinetype")]
//         public async Task<IActionResult> getLineType([FromQuery] string lineType)
//         {
//             var result = await _RolloutProgressService.GetType(lineType);
//             return Ok(result);
//         }
//         [HttpGet("search")]
//         public async Task<IActionResult> Search(string lineNo, string lineType, string model, [FromQuery] PaginationParams param)
//         {
//             var result = await _RolloutProgressService.Search(lineNo, lineType, model, param);
//             return Ok(result);
//         }
//         //add and edit
//         [HttpGet("getmodelno")]
//         public async Task<IActionResult> GetModelNo([FromQuery] Params_Rollout_ProgressDTO parms_get)
//         {
//             var result = await _RolloutProgressService.GetModelNo(parms_get);
//             return Ok(result);
//         }
//         [HttpGet("getstage")]
//         public async Task<IActionResult> GetStage([FromQuery] Params_Rollout_ProgressDTO parms_get)
//         {
//             var result = await _RolloutProgressService.GetStage(parms_get);
//             return Ok(result);
//         }
//         [HttpGet("getoperaname")]
//         public async Task<IActionResult> GetOperaName([FromQuery] Params_Rollout_ProgressDTO parms_get)
//         {
//             var result = await _RolloutProgressService.GetOperaName(parms_get);
//             return Ok(result);
//         }
//         [HttpGet("getrolloutline")]
//         public async Task<IActionResult> GetRolloutLine()
//         {
//             var result = await _RolloutProgressService.GetRolloutLine();
//             return Ok(result);
//         }
//         [HttpGet("getrolloutlinename")]
//         public async Task<IActionResult> GetRolloutLineName([FromQuery] string line_id)
//         {
//             var result = await _RolloutProgressService.GetRolloutLineNam(line_id);
//             return Ok(result);
//         }
//         [HttpPost("getctafter")]
//         public async Task<IActionResult> GetCTAfter([FromBody] Params_Rollout_ProgressDTO model)
//         {
//             var result = await _RolloutProgressService.GetCTAfter(model);
//             return Ok(result);
//         }
//         [HttpPost("gethourlypph")]
//         public async Task<IActionResult> GetHourlyPPH([FromBody] Params_Rollout_ProgressDTO model)
//         {
//             var result = await _RolloutProgressService.GetHourlyPPH(model);
//             return Ok(result);
//         }
//         [HttpPost("getblrollout")]
//         public async Task<IActionResult> GetBL_Rollout([FromBody] Params_Rollout_ProgressDTO parms_get)
//         {
//             var result = await _RolloutProgressService.GetBL_Rollout(parms_get);
//             return Ok(result);
//         }
//         [HttpPost("addnew")]
//         public async Task<IActionResult> AddNew(BL_Rollout_ProgressDTO model)
//         {
//             model.create_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             model.create_time = DateTime.Now;
//             model.update_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             model.update_time = DateTime.Now;
//             model.factory_id = _configuration.GetSection("AppSettings:Factory").Value;
//             //ANDFILE
//             var fc = new FunctionUtility();

//             string folder = model.factory_id + "\\BestLine\\" + model.line_id + "\\"
//                 + model.line_type_id + "\\" + model.model_no + "\\RolloutProgress\\";

//             if (string.IsNullOrEmpty(model.operation_video_url))
//             {
//                 var fileName = "/no-image.jpg";
//                 model.operation_video_url = model.factory_id + fileName;
//             }
//             else
//             {
//                 var nameNewVideo = await fc.UploadAsync(model.operation_video_url, "uploaded\\" + folder, "OpVideo_" + Guid.NewGuid().ToString());
//                 model.operation_video_url = folder + nameNewVideo;
//             }

//             if (string.IsNullOrEmpty(model.rollout_operation_layout))
//             {
//                 var fileName = "/no-image.jpg";
//                 model.rollout_operation_layout = model.factory_id + fileName;
//             }
//             else
//             {
//                 var nameNewImage = await fc.UploadAsync(model.rollout_operation_layout, "uploaded\\" + folder, "OpLayout_" + Guid.NewGuid().ToString());
//                 model.rollout_operation_layout = folder + nameNewImage;
//             }
//             //END
//             var result = await _RolloutProgressService.AddNew(model);
//             return Ok(result);
//         }
//         [HttpPut("update")]
//         public async Task<IActionResult> Update(BL_Rollout_ProgressDTO model)
//         {
//             model.update_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             model.update_time = DateTime.Now;
//             model.factory_id = _configuration.GetSection("AppSettings:Factory").Value;

//             //ANDFILE
//             var fc = new FunctionUtility();

//             string folder = model.factory_id + "\\BestLine\\" + model.line_id + "\\"
//                 + model.line_type_id + "\\" + model.model_no + "\\RolloutProgress\\";
//             //checkvideo
//             if (!string.IsNullOrEmpty(model.operation_video_url))
//             {
//                 var nameNewVideo = await fc.UploadAsync(model.operation_video_url, "uploaded\\" + folder, "OpVideo_" + Guid.NewGuid().ToString());
//                 model.operation_video_url = folder + nameNewVideo;
//             }
//             //checkImg
//             if (!string.IsNullOrEmpty(model.rollout_operation_layout))
//             {
//                 var nameNewImage = await fc.UploadAsync(model.rollout_operation_layout, "uploaded\\" + folder, "OpLayout_" + Guid.NewGuid().ToString());
//                 model.rollout_operation_layout = folder + nameNewImage;
//             }
//             //END
//             var result = await _RolloutProgressService.Update(model);
//             return Ok(result);
//         }
//     }
// }