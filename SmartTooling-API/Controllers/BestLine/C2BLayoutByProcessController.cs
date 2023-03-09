// using System;
// using System.IO;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using SmartTooling_API._Services.Interfaces.BestLine;
// using SmartTooling_API.DTO.BestLine;
// using SmartTooling_API.Helpers;
// using SmartTooling_API.Helpers.Params;
// using SmartTooling_API.Helpers.Params.SmartTool;

// namespace SmartTooling_API.Controllers.BestLine
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class C2BLayoutByProcessController : ControllerBase
//     {
//         private readonly IWebHostEnvironment _webHostEnvironment;
//         private readonly IC2BLayoutByProcessService _c2BLayoutByProcessService;
//         private readonly IConfiguration _configuration;
//         private string factory;
//         public C2BLayoutByProcessController(IWebHostEnvironment webHostEnvironment, IC2BLayoutByProcessService c2BLayoutByProcessService,
//                                 IConfiguration configuration)
//         {
//             _webHostEnvironment = webHostEnvironment;
//             _c2BLayoutByProcessService = c2BLayoutByProcessService;
//             _configuration = configuration;
//             factory = _configuration.GetSection("Appsettings:Factory").Value;
//         }

//         [HttpGet("GetLineID")]
//         public async Task<IActionResult> GetLineID()
//         {
//             var data = await _c2BLayoutByProcessService.GetLineID();
//             return Ok(data);
//         }

//         [HttpGet("GetLineTypeID")]
//         public async Task<IActionResult> GetLineTypeID(string lineID)
//         {
//             var data = await _c2BLayoutByProcessService.GetLineType(lineID);
//             return Ok(data);
//         }

//         [HttpGet("GetModelNo")]
//         public async Task<IActionResult> GetModelNo(string lineID, string lineTypeID)
//         {
//             var data = await _c2BLayoutByProcessService.GetModelNo(lineID, lineTypeID);
//             return Ok(data);
//         }

//         [HttpGet("Search")]
//         public async Task<IActionResult> Search(string lineID, string lineTypeID, string modelNo, [FromQuery] PaginationParams param)
//         {
//             var lists = await _c2BLayoutByProcessService.Search(lineID, lineTypeID, modelNo, param);
//             Response.AddPagination(lists.CurrentPage, lists.PageSize, lists.TotalCount, lists.TotalPages);
//             return Ok(lists);
//         }

//         [HttpGet("GetProcessType")]
//         public async Task<IActionResult> GetProcessType()
//         {
//             var data = await _c2BLayoutByProcessService.GetProcessType();
//             return Ok(data);
//         }


//         [HttpPost("Add")]
//         public async Task<IActionResult> Add(BL_Layout_Design_ProcessDTO lineID)
//         {
//             lineID.factory_id = factory;
//             lineID.create_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             lineID.line_id = lineID.line_id.Trim();
//             lineID.create_time = DateTime.Now;
//             lineID.update_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             lineID.update_time = DateTime.Now;
//             //Before
//             if (!string.IsNullOrEmpty(lineID.each_process_image_before))
//             {
//                 lineID.each_process_image_before = await GetNameFileUpload(lineID, lineID.each_process_image_before, "_Before.jpg");
//             }
//             else
//             {
//                 lineID.each_process_image_before = lineID.factory_id + "/no-image.jpg";
//             }

//             //After
//             if (!string.IsNullOrEmpty(lineID.each_process_image_after))
//             {
//                 lineID.each_process_image_after = await GetNameFileUpload(lineID, lineID.each_process_image_after, "_After.jpg");
//             }
//             else
//             {
//                 lineID.each_process_image_after = lineID.factory_id + "/no-image.jpg";
//             }

//             var data = await _c2BLayoutByProcessService.AddLayoutByProcess(lineID);
//             return Ok(data);
//         }

//         [HttpPut("Edit")]
//         public async Task<IActionResult> Edit(BL_Layout_Design_ProcessDTO lineID)
//         {
//             lineID.factory_id = factory;
//             lineID.update_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
//             lineID.update_time = DateTime.Now;

//             var result = await _c2BLayoutByProcessService.GetDataEdit(factory, lineID.line_id.Trim(), lineID.line_type_id, lineID.model_no, lineID.process_type_id);
//             //Before
//             string folder = _webHostEnvironment.WebRootPath + "/uploaded/";
//             string imgBefore = string.IsNullOrEmpty(result.each_process_image_before) ? "" : result.each_process_image_before;
//             string imgAfter = string.IsNullOrEmpty(result.each_process_image_after) ? "" : result.each_process_image_after;
//             if (string.IsNullOrEmpty(lineID.each_process_image_before))
//             {
//                 lineID.each_process_image_before = imgBefore;
//             }
//             else
//             {
//                 if (System.IO.File.Exists(folder + imgBefore) && imgBefore != lineID.factory_id + "/no-image.jpg")
//                     System.IO.File.Delete(folder + imgBefore);

//                 lineID.each_process_image_before = await GetNameFileUpload(lineID, lineID.each_process_image_before, "_Before.jpg");
//             }

//             //After
//             if (string.IsNullOrEmpty(lineID.each_process_image_after))
//             {
//                 lineID.each_process_image_after = imgAfter;
//             }
//             else
//             {
//                 if (System.IO.File.Exists(folder + imgAfter) && imgAfter != lineID.factory_id + "/no-image.jpg")
//                     System.IO.File.Delete(folder + imgAfter);

//                 lineID.each_process_image_after = await GetNameFileUpload(lineID, lineID.each_process_image_after, "_After.jpg");
//             }

//             var data = await _c2BLayoutByProcessService.EditLayoutByProcess(lineID);
//             return Ok(data);
//         }

//         private async Task<string> GetNameFileUpload(BL_Layout_Design_ProcessDTO lineID, string file, string fileExtension)
//         {
//             string folder = (_webHostEnvironment.WebRootPath + "/uploaded/" + lineID.factory_id + "/BestLine/"
//                             + lineID.line_id.Trim() + "/" + lineID.line_type_id + "/" + lineID.model_no + "/").Trim();
//             string base64 = file.Substring(file.IndexOf(',') + 1);

//             string fileName = (lineID.factory_id + "_" + lineID.line_id.Trim() + "_" + lineID.line_type_id + "_"
//                             + lineID.model_no + "_" + lineID.process_type_id + fileExtension).Trim();

//             byte[] fileData = Convert.FromBase64String(base64);
//             if (!Directory.Exists(folder))
//             {
//                 Directory.CreateDirectory(folder);
//             }
//             string filePath = Path.Combine(folder, fileName);
//             System.IO.File.WriteAllBytes(filePath, fileData);
//             // gán lại tên picture
//             file = lineID.factory_id + "/BestLine/" + lineID.line_id.Trim() + "/" + lineID.line_type_id + "/" + lineID.model_no + "/" + fileName;
//             return await Task.FromResult(file);
//         }

//         [HttpGet("itemEdit")]
//         public async Task<IActionResult> GetDataEdit(string lineID, string lineTypeID, string modelNo, string processNo)
//         {
//             var data = await _c2BLayoutByProcessService.GetDataEdit(factory, lineID, lineTypeID, modelNo, processNo);
//             return Ok(data);
//         }
//     }

// }