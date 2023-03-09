using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Aspose.Cells;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Services.Interfaces.ProductionBP;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;

namespace SmartTooling_API.Controllers.ProductionBP
{
    [ApiController]
    [Route("api/[controller]")]
    public class BPComputerStitchingSettingController : ControllerBase
    {
        private readonly IBPComputerStitchingSettingService _computerService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public BPComputerStitchingSettingController(IBPComputerStitchingSettingService computerService, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _computerService = computerService;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [HttpGet("GetAllComputerStitchingSetting")]
        public async Task<IActionResult> GetAllComputerStitchingSetting([FromQuery] BPSearchComputerStitchingSetting search, [FromQuery] PaginationParams pagination) {
            var result = await _computerService.GetAllComputerStitchingSetting(search, pagination);
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost("CreateComputerStitchingSetting")]
        public async Task<IActionResult> CreateComputerStitchingSetting([FromBody] ComputerStitchingSettingViewDTO model) {
            model.factory_id = _configuration.GetSection("AppSettings:Factory").Value;
            model.create_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.create_time = DateTime.Now;
            model.update_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.update_time = DateTime.Now;

            var result = await _computerService.CreateComputerStitchingSetting(model);
            return Ok(result);
        }

        [HttpPut("UpdateComputerStitchingSetting")]
        public async Task<IActionResult> UpdateComputerStitchingSetting([FromBody] ComputerStitchingSettingViewDTO model) {
            model.update_by = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.update_time = DateTime.Now;

            var result = await _computerService.UpdateComputerStitchingSetting(model);
            return Ok(result);
        }

        [HttpGet("GetModel")]
        public async Task<List<KeyValuePair<string, string>>> GetModel() {
            var result = await _computerService.GetModel();
            return result;
        }

        [HttpGet("GetStage")]
        public async Task<List<KeyValuePair<string, string>>> GetStage() {
            var result = await _computerService.GetStage();
            return result;
        }

        [HttpGet("GetCSOperation")]
        public async Task<IActionResult> GetCSOperation([FromQuery] string model_no, [FromQuery] string stage_id) {
            var result = await _computerService.GetCSOperation(model_no, stage_id);
            return Ok(result);
        }

        [HttpGet("GetCSType")]
        public async Task<List<KeyValuePair<string, string>>> GetCSType() {
            var result = await _computerService.GetCSType();
            return result;
        }

        [HttpGet("GetCSMachineType")]
        public async Task<List<KeyValuePair<string, string>>> GetCSMachineType() {
            var result = await _computerService.GetCSMachineType();
            return result;
        }

        [HttpGet("GetMainUpperMaterialType")]
        public async Task<List<KeyValuePair<string, string>>> GetMainUpperMaterialType() {
            var result = await _computerService.GetMainUpperMaterialType();
            return result;
        }

        [HttpGet("GetJigDesign")]
        public async Task<List<KeyValuePair<string, string>>> GetJigDesign() {
            var result = await _computerService.GetJigDesign();
            return result;
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile file) {
            var factory = _configuration.GetSection("AppSettings:Factory").Value;
            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _computerService.UploadExcel(file, username, factory);
            return Ok(result);
        }
    }
}