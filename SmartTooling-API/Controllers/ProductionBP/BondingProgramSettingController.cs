using Aspose.Cells;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Services.Interfaces.ProductionBP;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.DTO.ProductionBP.BondingProgramSetting;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;
using SmartTooling_API.Helpers.Params.ProductionBP;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartTooling_API.Controllers.ProductionBP
{
    [Route("api/[controller]")]
    [ApiController]
    public class BondingProgramSettingController : ControllerBase
    {
        private readonly IPBPBondingProgramSettingService _service;
        private readonly string _factory;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BondingProgramSettingController(IPBPBondingProgramSettingService service, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            _factory = configuration.GetSection("AppSettings:Factory").Value;
        }
        private string GetUserClaim()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet("getAllChemicalProcessType")]
        public async Task<IActionResult> GetAllChemicalProcessType()
        {
            var data = await _service.GetAllChemicalProcessType();
            return Ok(data);
        }

        [HttpGet("getAllAutoTech")]
        public async Task<IActionResult> GetAllAutoTech()
        {
            var data = await _service.GetAllAutoTech();
            return Ok(data);
        }

        [HttpGet("getAllModelNo")]
        public async Task<IActionResult> GetAllModelNo()
        {
            var data = await _service.GetAllModelNo();
            return Ok(data);
        }

        [HttpGet("getAllAdoptionComponentType")]
        public async Task<IActionResult> GetAllAdoptionComponentType()
        {
            var data = await _service.GetAllAdoptionComponentType();
            return Ok(data);
        }

        [HttpGet("getAllProcessAdoptionScopeType")]
        public async Task<IActionResult> GetAllProcessAdoptionScopeType()
        {
            var data = await _service.GetAllProcessAdoptionScopeType();
            return Ok(data);
        }

        [HttpGet("getAllMainUpperMaterialType")]
        public async Task<IActionResult> GetAllMainUpperMaterialType()
        {
            var data = await _service.GetAllMainUpperMaterialType();
            return Ok(data);
        }

        [HttpGet("getAllMainBottomMaterialType")]
        public async Task<IActionResult> GetAllMainBottomMaterialType()
        {
            var data = await _service.GetAllMainBottomMaterialType();
            return Ok(data);
        }

        [HttpGet("getAllChemicalSupplier")]
        public async Task<IActionResult> GetAllChemicalSupplier()
        {
            var data = await _service.GetAllChemicalSupplier();
            return Ok(data);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams param, [FromQuery] BondingProgramSettingParam filterParam)
        {
            var lists = await _service.Search(param, filterParam);
            Response.AddPagination(lists.CurrentPage, lists.PageSize, lists.TotalCount, lists.TotalPages);
            return Ok(lists);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] PBP_Bonding_Program_SettingDTO model)
        {
            var timeNow = DateTime.Now;
            var userLogin = GetUserClaim();
            model.update_by = userLogin;
            model.create_by = userLogin;
            model.create_time = timeNow;
            model.update_time = timeNow;
            model.factory_id = _factory;

            var fn = new FunctionUtility();
            string folderPath = _factory + "/ProductionBP/BondingProgramSetting/" +  model.model_no;
            if (model.shoes_photo_url == null || model.shoes_photo_url == "")
            {
                model.shoes_photo_url = _factory + "/no-image.jpg";
            }
            else
            {
                var fileName = await fn.UploadAsync(model.shoes_photo_url, "uploaded/" + folderPath, Guid.NewGuid().ToString());
                model.shoes_photo_url = folderPath + "/" + fileName;
            }
            var save =  await _service.AddBondingProgramSetting(model);
            return Ok(save);

        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] PBP_Bonding_Program_SettingDTO model)
        {
            model.update_by = GetUserClaim();
            model.update_time = DateTime.Now;
            model.factory_id = _factory;

            var fn = new FunctionUtility();
            string folderPath = _factory + "/ProductionBP/BondingProgramSetting/" +  model.model_no;
            if (model.shoes_photo_url == null || model.shoes_photo_url == "")
            {
                model.shoes_photo_url = _factory + "/no-image.jpg";
            }
            if (!model.shoes_photo_url.Contains("http")) {
                var fileName = await fn.UploadAsync(model.shoes_photo_url, "uploaded/" + folderPath, Guid.NewGuid().ToString());
                model.shoes_photo_url = folderPath + "/" + fileName;
            }
            var save = await _service.UpdateBondingProgramSetting(model);
            return Ok(save);
        }

        [HttpPost("uploadExcel")]
        public async Task<IActionResult> UploadExcel([FromForm] IFormFile file)
        {
            var user = GetUserClaim();
            var result = await _service.UploadExcel(file, user);
            return Ok(result);
        }

    }
}
