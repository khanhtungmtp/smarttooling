using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Aspose.Cells;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTooling_API._Services.Interfaces.ProductionBP;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;

namespace SmartTooling_API.Controllers.ProductionBP
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class PadPrintSettingController : ControllerBase
    {
        private readonly IPadPrintSettingService _padPrintSettingService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PadPrintSettingController(IPadPrintSettingService padPrintSettingService, IWebHostEnvironment webHostEnvironment)
        {
            _padPrintSettingService = padPrintSettingService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetAllModel")]
        public async Task<IActionResult> GetAllModel()
        {
            var data = await _padPrintSettingService.GetAllModel();
            return Ok(data);
        }

        [HttpGet("GetAllPadShape")]
        public async Task<IActionResult> GetAllPadShape()
        {
            var data = await _padPrintSettingService.GetAllPadShape();
            return Ok(data);
        }

        [HttpGet("GetAllMaterialType")]
        public async Task<IActionResult> GetAllMaterialType()
        {
            var data = await _padPrintSettingService.GetAllMaterialType();
            return Ok(data);
        }

        [HttpGet("GetAllMachineVendor")]
        public async Task<IActionResult> GetAllMachineVendor()
        {
            var data = await _padPrintSettingService.GetAllMachineVendor();
            return Ok(data);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] PadPrintSettingParam filterParam, [FromQuery] PaginationParams pagination)
        {
            var result = await _padPrintSettingService.Search(filterParam, pagination);
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PBP_Pad_Print_SettingDTO model)
        {
            model.create_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.update_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _padPrintSettingService.AddNew(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] PBP_Pad_Print_SettingDTO model)
        {
            model.update_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _padPrintSettingService.Update(model);
            return Ok(result);
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> Update([FromQuery] PBP_Pad_Print_Setting_DTO model)
        {
            var data = await _padPrintSettingService.GetDetail(model);
            return Ok(data);
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _padPrintSettingService.UploadExcel(file, user);
            return Ok(result);
        }
    }
}