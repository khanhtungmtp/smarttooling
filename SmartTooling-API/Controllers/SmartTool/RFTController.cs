using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.Helpers;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;

namespace SmartTooling_API.Controllers.SmartTool
{
    [ApiController]
    [Route("api/[controller]")]
    public class RFTController : ControllerBase
    {
        private readonly IRFTService _iRFTService;
        private string username;
        private string factory;

        public RFTController(IRFTService iRFTService, IConfiguration configuration)
        {
            _iRFTService = iRFTService;
            factory = configuration.GetSection("AppSettings:Factory").Value;
        }

        private string GetUserClaim()
        {
            return username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMeasure([FromQuery] PaginationParams param, string modelNo, string stage)
        {
            var lists = await _iRFTService.Search(param, modelNo, stage);
            Response.AddPagination(lists.CurrentPage, lists.PageSize, lists.TotalCount, lists.TotalPages);
            return Ok(lists);
        }

        [HttpGet("getallstage")]
        public async Task<IActionResult> GetAllStage() => Ok(await _iRFTService.GetAllStage());


        [HttpGet("getallmodel")]
        public async Task<IActionResult> GetAllModel() => Ok(await _iRFTService.GetAllModel());

        [HttpGet("process-type")]
        public async Task<IActionResult> GetAllProcessType(string model_no, string stage_id) => Ok(await _iRFTService.GetAllProcessType(model_no, stage_id));

        [HttpGet("processnoperationforedit")]
        public async Task<IActionResult> GetProcessNOperationForEdit(string model_no, string stage_id, string operation_id)
        {
            return Ok(await _iRFTService.GetProcessNOperation(model_no, stage_id, operation_id));
        }

        [HttpGet("getalldefectreason")]
        public async Task<IActionResult> GetAllDefectReason() => Ok(await _iRFTService.GetAllDefectReason());

        [HttpGet("getoptionname")]
        public async Task<IActionResult> GetOptionName(string model_no, string stage_id, string process_id)
        {
            return Ok(await _iRFTService.GetOperationName(model_no, stage_id, process_id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRFT(Measurement_RFTDTO measurement_RFTDTO)
        {
            measurement_RFTDTO.factory_id = factory;
            measurement_RFTDTO.create_by = GetUserClaim();
            measurement_RFTDTO.create_time = DateTime.Now;
            measurement_RFTDTO.update_by = GetUserClaim();
            measurement_RFTDTO.update_time = DateTime.Now;
            if (await _iRFTService.Add(measurement_RFTDTO))
            {
                return NoContent();
            }
            throw new Exception("Creating the Measurement RFT failed on save");
        }

        [HttpPost("edit")]
        public async Task<IActionResult> UpdateRFT(Measurement_RFTDTO measurement_RFTDTO)
        {
            measurement_RFTDTO.update_by = GetUserClaim();
            measurement_RFTDTO.update_time = DateTime.Now;
            if (await _iRFTService.Update(measurement_RFTDTO))
            {
                return NoContent();
            }
            return BadRequest($"Updating Measurement RFT {measurement_RFTDTO.model_no} failed on save");
        }

    }
}