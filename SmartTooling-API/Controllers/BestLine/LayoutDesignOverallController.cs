using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SmartTooling_API._Services.Interfaces.BestLine;
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
        private string GetUserClaim() => User.FindFirst(ClaimTypes.NameIdentifier).Value;

        [HttpGet("getLineNoOfMain")]
        public async Task<IActionResult> GetLineNoOfMain() => Ok(await _service.GetLineNoOfMain());

        [HttpGet("getLineTypeOfMain")]

        public async Task<IActionResult> GetLineTypeOfMain() => Ok(await _service.GetLineTypeOfMain());
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
            model.prod_season = model.prod_season.ToUpper();
            string folder = _webHostEnvironment.WebRootPath + "\\uploaded\\" + factory + "\\Polit_Line\\BL_Layout_Design_Overall\\";
            if (model.c2b_overall_image == null || model.c2b_overall_image == "")
                model.c2b_overall_image = factory + "/no-image.jpg";
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
                var fileName = Guid.NewGuid() + ".jpg";
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
            string pathRoot = _webHostEnvironment.WebRootPath + "\\uploaded\\";
            string folder = pathRoot + factory + "\\Polit_Line\\BL_Layout_Design_Overall\\";
            if (string.IsNullOrEmpty(model.c2b_overall_image))
                model.c2b_overall_image = result.c2b_overall_image;
            else
            {
                var source = model.c2b_overall_image;
                string base64 = source.Substring(source.IndexOf(',') + 1);
                base64 = base64.Trim('\0');
                byte[] modelData = Convert.FromBase64String(base64);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                var fileNameOld = result.c2b_overall_image;
                string filePathImages = Path.Combine(pathRoot, fileNameOld);
                // ki???m tra file c?? c?? ch??a x??a ??i
                if (System.IO.File.Exists(filePathImages))
                    System.IO.File.Delete(filePathImages);
                string fileUpdate = factory + "/Polit_Line/BL_Layout_Design_Overall/" + Guid.NewGuid() + ".jpg";
                string pathUpdate = pathRoot + fileUpdate;
                System.IO.File.WriteAllBytes(pathUpdate, modelData);
                model.c2b_overall_image = fileUpdate;
            }

            return Ok(await _service.UpdateLayoutDesignOverall(model));
            throw new Exception("Update the Model failed on save");
        }

        [HttpGet("edit")]
        public async Task<ActionResult> GetParamsEdit(string line_id, string line_type_id, string model_no) =>
             Ok(await _service.GetParamsEdit(factory, line_id, line_type_id, model_no));

    }
}