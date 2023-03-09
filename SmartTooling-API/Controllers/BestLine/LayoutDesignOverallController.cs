using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Services.Interfaces.BestLine;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;

namespace SmartTooling_API.Controllers.BestLine
{
    [ApiController]
    [Route("api/[controller]")]
    public class LayoutDesignOverallController : ControllerBase
    {
        private readonly ILayoutDesignOverallService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string username;
        private string factory;
        public LayoutDesignOverallController(ILayoutDesignOverallService service, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            factory = configuration.GetSection("AppSettings:Factory").Value;
        }
        private string GetUserClaim()
        {
            return username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        [HttpGet("getAllLineNo")]
        public async Task<IActionResult> GetAllLineNo() => Ok(await _service.GetAllLineNo());
        [HttpGet("getAllLineType")]
        public async Task<IActionResult> GetAllLineType() => Ok(await _service.GetAllLineType());
        [HttpGet("getAllModelNo")]
        public async Task<IActionResult> GetAllModelNo() => Ok(await _service.GetAllModelNo());
        [HttpGet("getAllProdSeason")]
        public async Task<IActionResult> GetAllProdSeason() => Ok(await _service.GetAllProdSeason());

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams param, [FromQuery] LayoutDesignOverallParam filterParam)
        {
            var lists = await _service.Search(param, filterParam);
            Response.AddPagination(lists.CurrentPage, lists.PageSize, lists.TotalCount, lists.TotalPages);
            return Ok(lists);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] BL_Layout_Design_OverallDTO model)
        {
            model.update_by = GetUserClaim();
            model.create_by = GetUserClaim();
            model.create_time = DateTime.Now;
            model.update_time = DateTime.Now;
            model.factory_id = factory;
            string folder = _webHostEnvironment.WebRootPath + "\\uploaded\\" + factory + "\\Polit_Line\\BL_Layout_Design_Overall\\";
            if (model.c2b_overall_image == null || model.c2b_overall_image == "")
            {
                var fileName = "/no-image.jpg";
                model.c2b_overall_image = factory + fileName;
            }
            else
            {
                var source = model.c2b_overall_image;
                string base64 = source.Substring(source.IndexOf(',') + 1);
                base64 = base64.Trim('\0');
                byte[] modelData = Convert.FromBase64String(base64);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var fileName = model.guid + ".jpg";
                string filePathImages = Path.Combine(folder, fileName);
                System.IO.File.WriteAllBytes(filePathImages, modelData);
                model.c2b_overall_image = factory + "/Polit_Line/BL_Layout_Design_Overall/" + fileName;
            }
            return Ok(await _service.AddLayoutDesignOverall(model));
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] BL_Layout_Design_OverallDTO model)
        {
            model.update_by = GetUserClaim();
            model.update_time = DateTime.Now;
            model.factory_id = factory;
            var result = await _service.GetParamsEdit(factory, model.line_id, model.line_type_id, model.model_no);
            string folder = _webHostEnvironment.WebRootPath + "\\uploaded\\" + factory + "\\Polit_Line\\BL_Layout_Design_Overall\\";
            if (string.IsNullOrEmpty(model.c2b_overall_image))
            {
                model.c2b_overall_image = result.c2b_overall_image;
            }
            else
            {
                var source = model.c2b_overall_image;
                string base64 = source.Substring(source.IndexOf(',') + 1);
                base64 = base64.Trim('\0');
                byte[] modelData = Convert.FromBase64String(base64);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var fileName = model.guid + ".jpg";
                string filePathImages = Path.Combine(folder, fileName);
                // kiểm tra file cũ có chưa xóa đi
                if (System.IO.File.Exists(filePathImages))
                {
                    System.IO.File.Delete(filePathImages);
                }
                System.IO.File.WriteAllBytes(filePathImages, modelData);
                model.c2b_overall_image = factory + "/Polit_Line/BL_Layout_Design_Overall/" + fileName;
            }

            if (await _service.UpdateLayoutDesignOverall(model))
            {
                return NoContent();
            }
            throw new Exception("Update the Model failed on save");
        }
        [HttpGet("edit")]
        public async Task<ActionResult> GetParamsEdit(string line_id, string line_type_id, string model_no)
        {
            var data = await _service.GetParamsEdit(factory, line_id, line_type_id, model_no);
            return Ok(data);
        }

    }
}