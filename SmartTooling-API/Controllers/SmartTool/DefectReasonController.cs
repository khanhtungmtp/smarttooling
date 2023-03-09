using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class DefectReasonController : ControllerBase
    {
        private readonly IDefectReasonService _defectreason;

        private string username;
        private string factory;
        public DefectReasonController(IDefectReasonService defectReason, IConfiguration configuration)
        {
            _defectreason = defectReason;
            factory = configuration.GetSection("AppSettings:Factory").Value;
        }

        private string GetUserClaim()
        {
            return username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet(Name = "GetDefectReasons")]
        public async Task<IActionResult> GetDefectReasons([FromQuery] PaginationParams param)
        {
            var defectreasons = await _defectreason.GetWithPaginations(param);
            Response.AddPagination(defectreasons.CurrentPage, defectreasons.PageSize, defectreasons.TotalCount, defectreasons.TotalPages);
            return Ok(defectreasons);
        }

        [HttpGet("all", Name = "GetAllDefectReasons")]
        public async Task<IActionResult> GetAll()
        {
            var defectreasons = await _defectreason.GetAllAsync();
            return Ok(defectreasons);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams param, DefectReasonParam filter)
        {
            var lists = await _defectreason.SearchDefectReason(param, filter);
            Response.AddPagination(lists.CurrentPage, lists.PageSize, lists.TotalCount, lists.TotalPages);
            return Ok(lists);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDefectReason(DefectReasonDTO defectReasonDTO)
        {
            defectReasonDTO.factory_id = factory;
            // 10/2 havent check exists
            if (await _defectreason.CheckDefectReasonExists(defectReasonDTO.defect_reason_id))
            {
                return BadRequest("Defect Reason ID already exists!");
            }
            defectReasonDTO.update_by = GetUserClaim();
            defectReasonDTO.update_time = DateTime.Now;
            if (await _defectreason.Add(defectReasonDTO))
            {
                return NoContent();
                // return CreatedAtRoute("GetDefectReasons", new { });
            }

            throw new Exception("Creating the Defect Reason failed on save");
        }

        [HttpPost("edit")]
        public async Task<IActionResult> UpdateDefectReason(DefectReasonDTO defectReasonDTO)
        {
            defectReasonDTO.update_by = GetUserClaim();
            defectReasonDTO.update_time = DateTime.Now;
            if (await _defectreason.Update(defectReasonDTO))
            {
                return NoContent();
            }
            return BadRequest($"Updating Defect Reason {defectReasonDTO.defect_reason_id} failed on save");
        }
    }
}