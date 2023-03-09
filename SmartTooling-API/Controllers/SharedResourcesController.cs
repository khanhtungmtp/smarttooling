using Microsoft.AspNetCore.Mvc;
using SmartTooling_API._Services.Interfaces;

namespace SmartTooling_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SharedResourcesController : ControllerBase
    {
        private readonly ISharedResourcesService _service;
        private readonly IConfiguration _configuration;

        public SharedResourcesController(ISharedResourcesService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpGet("getAllFactory")]
        public async Task<IActionResult> GetAllFactory()
        {
            var data = await _service.GetAllFactory();
            return Ok(data);
        }

        [HttpGet("GetAllLine")]
        public async Task<IActionResult> GetAllLine(string factory)
        {
            var data = await _service.GetAllLine(factory);
            return Ok(data);
        }

        [HttpGet("GetAllLineType")]
        public async Task<IActionResult> GetAllLineType(string factory)
        {
            var data = await _service.GetAllLineType(factory);
            return Ok(data);
        }

        [HttpGet("GetStage")]
        public async Task<IActionResult> GetStage(string factory)
        {
            var data = await _service.GetStage(factory);
            return Ok(data);
        }

        [HttpGet("GetAllModel")]
        public async Task<IActionResult> GetAllModel()
        {
            var data = await _service.GetAllModel();
            return Ok(data);
        }

        [HttpGet("ServerInfo")]
        public async Task<IActionResult> GetServerInfo()
        {
            var factory = _configuration.GetSection("Appsettings:Factory").Value;
            return Ok(await Task.FromResult(new { factory }));
        }
    }
}