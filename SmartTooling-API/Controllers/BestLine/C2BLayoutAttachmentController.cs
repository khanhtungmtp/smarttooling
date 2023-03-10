using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Repositories.BestLine;
using SmartTooling_API._Services.Interfaces.BestLine;
using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API.Controllers.BestLine
{
    [ApiController]
    [Route("api/[controller]")]
    public class C2BLayoutAttachmentController : ControllerBase
    {
        private readonly IC2BLayoutAttachmentService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private string username;
        private string factory;
        public C2BLayoutAttachmentController(IC2BLayoutAttachmentService service, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            factory = configuration.GetSection("AppSettings:Factory").Value;
        }
        private string GetUserClaim() => User.FindFirst(ClaimTypes.NameIdentifier).Value;

        [HttpGet("GetAllLineNo")]
        public async Task<IActionResult> GetAllLineNo() => Ok(await _service.GetAllLineNo());
        [HttpGet("GetAllLineType")]
        public async Task<IActionResult> GetAllLineType() => Ok(await _service.GetAllLineType());
        [HttpGet("GetAllProdSeason")]
        public async Task<IActionResult> GetAllProdSeason() => Ok(await _service.GetAllProdSeason());
        [HttpGet("GetAllAttachmentType")]
        public async Task<IActionResult> GetAllAttachmentType() => Ok(await _service.GetAllAttachmentType());
        [HttpGet("GetAllLineNoOfAdd")]
        public async Task<IActionResult> GetAllLineNoOfAdd() => Ok(await _service.GetAllLineNoOfAdd());
        [HttpGet("GetAllLineTypeOfAdd")]
        public async Task<IActionResult> GetAllLineTypeOfAdd(string line_id) => Ok(await _service.GetAllLineTypeOfAdd(line_id));
        [HttpGet("GetAllModelNoOfAdd")]
        public async Task<IActionResult> GetAllModelNoOfAdd(string line_id, string line_type_id) => Ok(await _service.GetAllModelNoOfAdd(line_id, line_type_id));
        [HttpGet("GetAllProdSeasonOfAdd")]
        public async Task<IActionResult> GetAllProdSeasonOfAdd(string line_id, string line_type_id, string model_no) => Ok(await _service.GetAllProdSeasonOfAdd(line_id, line_type_id, model_no));
        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams param, [FromQuery] C2BLayoutAttachmentParam filterParam)
        {
            var result = await _service.Search(param, filterParam);
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromQuery] BL_AttachmentsDTO model, IFormFile file)
        {
            model.update_by = GetUserClaim();
            model.update_time = DateTime.Now;
            model.factory_id = factory;

            if (file != null)
            {
                string fileNameExtension = (file.FileName.Split("."))[(file.FileName.Split(".")).Length - 1];
                string folder = _webHostEnvironment.WebRootPath + "\\uploaded\\" + factory + "\\BestLine\\" + "/" + model.line_id + "/" + model.line_type_id + "/" + model.model_no + "\\Attachment\\";

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, file.FileName);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                model.attachment_file_url = factory + "/BestLine/" + model.line_id + "/" + model.line_type_id + "/" + model.model_no + "/Attachment/" + file.FileName;
                model.attachment_name = file.FileName;
            }
            var resurl = await _service.Create(model);
            return Ok(resurl);

        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAttachment([FromQuery] BL_AttachmentsDTO model)
        {
            string folder = _webHostEnvironment.WebRootPath + "\\uploaded\\" + factory + "\\BestLine\\" + "/" + model.line_id + "/" + model.line_type_id + "/" + model.model_no + "\\Attachment\\";
            string filePathImages = Path.Combine(folder, model.attachment_name);
            if (System.IO.File.Exists(filePathImages))
            {
                System.IO.File.Delete(filePathImages);
            }
            var result = await _service.DeleteAttachment(model);
            if (result)
                return Ok(result);
            else
                throw new Exception("Delete Attachment Error");
        }
    }
}