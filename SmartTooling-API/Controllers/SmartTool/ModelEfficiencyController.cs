using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API.Controllers.SmartTool
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelEfficiencyController : ControllerBase
    {
        private readonly IModelEfficiencyService _modelEfficiencyService;
        private string username;
        private string factory;

        public ModelEfficiencyController(IModelEfficiencyService modelEfficiencyService, IConfiguration configuration)
        {
            _modelEfficiencyService = modelEfficiencyService;
            factory = configuration.GetSection("AppSettings:Factory").Value;
        }

        private string GetUserClaim()
        {
            return username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet("upperId")]
        public async Task<IActionResult> GetAllProcessType() => Ok(await _modelEfficiencyService.GetAllUpperID());


        [HttpGet("modelName")]
        public async Task<IActionResult> GetModelName(string upperId) => Ok(await _modelEfficiencyService.GetModelName(upperId, factory));


        [HttpPost("getModelEfficiency")]
        public async Task<IActionResult> GetModelEfficiency(ModelEfficiencyEditParam modelEfficiencyEditParram)
        {
            modelEfficiencyEditParram.factory = factory;
            var listEfficiency = await _modelEfficiencyService.ModelEfficiencyEdit(modelEfficiencyEditParram);
            return Ok(listEfficiency);
        }



        [HttpPost("updateModelEfficiency")]
        public async Task<IActionResult> UpdateModelEfficiency(List<ModelEfficiencyDTO> modelEfficiencyDTO)
        {
            var usernameParam = GetUserClaim();
            if (await _modelEfficiencyService.UpdateOrInsert(modelEfficiencyDTO, factory, usernameParam))
                return NoContent();
            return BadRequest($"Updating Model Efficiency failed on save");
        }
    }
}