using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API.Controllers.SmartTool
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CrossSiteSharingController : ControllerBase
    {
        private readonly ICrossSiteSharingService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public CrossSiteSharingController(ICrossSiteSharingService service,
                                            IWebHostEnvironment webHostEnvironment,
                                            IConfiguration configuration)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams param, CrossSiteSharingParam filterParam)
        {
            var result = await _service.Search(param, filterParam);
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }
        [HttpGet("getCrossSiteSharingEdit")]
        public async Task<IActionResult> GetCrossSiteSharingEdit(string factory, string modelNO, string serialNo)
        {
            var data = await _service.GetCrossSiteSharingEdit(factory, modelNO, serialNo);
            return Ok(data);
        }
        [HttpPost("getCrossSiteSharingPDF")]
        public async Task<IActionResult> GetCrossSiteSharingPDF([FromBody] List<CrossSiteSharingDTO> filterParam)
        {
            var data = await _service.GetCrossSiteSharingPDF(filterParam);
            return Ok(data);
        }
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Kaizen_Benefits_Application_FormDTO model)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.update_by = username;
            var result = await _service.UpdateCrossSiteSharing(model);
            return Ok(result);
        }
    }
}