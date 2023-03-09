using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params;

namespace SmartTooling_API.Controllers.SmartTool
{
    [ApiController]
    [Route("api/[controller]")]
    public class KaizenController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IKaizenService _kaizenService;
        private string factory;
        public KaizenController(IWebHostEnvironment webHostEnvironment, IKaizenService kaizenService,
                                IConfiguration configuration)
        {
            _kaizenService = kaizenService;
            _webHostEnvironment = webHostEnvironment;
            factory = configuration.GetSection("AppSettings:Factory").Value;

        }

        [HttpGet("kaizen-list")]
        public async Task<IActionResult> Search([FromQuery] PaginationParams param, string modelNo)
        {
            var lists = await _kaizenService.Search(param, modelNo, factory);
            Response.AddPagination(lists.CurrentPage, lists.PageSize, lists.TotalCount, lists.TotalPages);
            return Ok(lists);
        }

        [HttpGet("getmodel")]
        public async Task<IActionResult> GetAllModelNo()
        {
            var data = await _kaizenService.GetModelNo(factory);
            return Ok(data);
        }

        [HttpGet("getprocess")]
        public async Task<IActionResult> GetAllProcess(string modelNO, string stage)
        {
            var data = await _kaizenService.Getprocess(modelNO, stage, factory);
            return Ok(data);
        }

        [HttpGet("getstage")]
        public async Task<IActionResult> GetAllStage()
        {
            var data = await _kaizenService.GetStage(factory);
            return Ok(data);
        }

        [HttpGet("getkaizenfrom")]
        public async Task<IActionResult> GetAllKaiZenFrom()
        {
            var data = await _kaizenService.GetKaizenFrom();
            return Ok(data);
        }

        [HttpGet("getopera")]
        public async Task<IActionResult> GetAllOpera(string modelNO, string stage, string process)
        {
            var data = await _kaizenService.Getopera(modelNO, stage, process, factory);
            return Ok(data);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] KaizenDTO kaizenDTO)
        {
            kaizenDTO.factory_id = factory;
            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            kaizenDTO.update_by = username;
            kaizenDTO.create_by = username;
            var data = await _kaizenService.GetbyID(kaizenDTO.model_no);
            if (data != null)
            {
                kaizenDTO.serial_no = data.serial_no + 1;
            }
            else
            {
                kaizenDTO.serial_no = 1;
            }
            if (kaizenDTO.before_media != null)
            {

                var source = kaizenDTO.before_media;
                string folder = _webHostEnvironment.WebRootPath + "\\uploaded\\" + factory + "\\Kaizen\\" + kaizenDTO.model_no + "\\Before\\";
                string base64 = "";
                var fileName = "";
                if (kaizenDTO.before_media.Contains("video"))
                {
                    base64 = source.Replace("data:video/mp4;base64,", "");
                    fileName = factory + "_" + kaizenDTO.model_no + "_" + kaizenDTO.serial_no + "_" + "Before" + ".mp4";
                }
                else
                {
                    base64 = source.Substring(source.IndexOf(',') + 1);
                    base64 = base64.Trim('\0');
                    fileName = factory + "_" + kaizenDTO.model_no + "_" + kaizenDTO.serial_no + "_" + "Before" + ".jpg";
                }

                byte[] KaizenData = Convert.FromBase64String(base64);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePathImages = Path.Combine(folder, fileName);
                System.IO.File.WriteAllBytes(filePathImages, KaizenData);
                kaizenDTO.before_media = factory + "/Kaizen/" + kaizenDTO.model_no + "/Before/" + fileName;
            }
            else
            {
                kaizenDTO.before_media = "";
            }
            if (kaizenDTO.after_media != null)
            {
                var source = kaizenDTO.after_media;
                string folder = _webHostEnvironment.WebRootPath + "\\uploaded\\" + factory + "\\Kaizen\\" + kaizenDTO.model_no + "\\After\\";
                string base64 = "";
                var fileName = "";
                if (kaizenDTO.after_media.Contains("video"))
                {
                    base64 = source.Replace("data:video/mp4;base64,", "");
                    fileName = factory + "_" + kaizenDTO.model_no + "_" + kaizenDTO.serial_no + "_" + "Ater" + ".mp4";
                }
                else
                {
                    base64 = source.Substring(source.IndexOf(',') + 1);
                    base64 = base64.Trim('\0');
                    fileName = factory + "_" + kaizenDTO.model_no + "_" + kaizenDTO.serial_no + "_" + "After" + ".jpg";
                }
                byte[] KaizenData = Convert.FromBase64String(base64);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePathImages = Path.Combine(folder, fileName);
                System.IO.File.WriteAllBytes(filePathImages, KaizenData);
                kaizenDTO.after_media = factory + "/Kaizen/" + kaizenDTO.model_no + "/After/" + fileName;
            }
            else
            {
                kaizenDTO.after_media = "";
            }

            var result = await _kaizenService.AddKaizen(kaizenDTO);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> update([FromBody] KaizenDTO kaizenDTO)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            kaizenDTO.update_by = username;
            if (kaizenDTO.before_media.Length > 200)
            {
                string folder = _webHostEnvironment.WebRootPath + "\\uploaded\\" + factory + "\\Kaizen\\" + kaizenDTO.model_no + "\\Before\\";
                var source = kaizenDTO.before_media;
                string base64 = "";
                var fileName = "";
                var fileName1 = "";
                if (kaizenDTO.before_media.Contains("video"))
                {
                    base64 = source.Replace("data:video/mp4;base64,", "");
                    fileName = factory + "_" + kaizenDTO.model_no + "_" + kaizenDTO.serial_no + "_" + "Before" + ".mp4";
                    fileName1 = factory + "_" + kaizenDTO.model_no + "_" + kaizenDTO.serial_no + "_" + "Before" + ".jpg";
                }
                else
                {
                    base64 = source.Substring(source.IndexOf(',') + 1);
                    base64 = base64.Trim('\0');
                    fileName = factory + "_" + kaizenDTO.model_no + "_" + kaizenDTO.serial_no + "_" + "Before" + ".jpg";
                    fileName1 = factory + "_" + kaizenDTO.model_no + "_" + kaizenDTO.serial_no + "_" + "Before" + ".mp4";
                }
                byte[] modelData = Convert.FromBase64String(base64);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePathImages = Path.Combine(folder, fileName);
                string filePathImages1 = Path.Combine(folder, fileName1);
                // kiểm tra file cũ có chưa xóa đi
                if (System.IO.File.Exists(filePathImages))
                {
                    System.IO.File.Delete(filePathImages);
                }
                if (System.IO.File.Exists(filePathImages1))
                {
                    System.IO.File.Delete(filePathImages1);
                }
                System.IO.File.WriteAllBytes(filePathImages, modelData);
                kaizenDTO.before_media = factory + "/Kaizen/" + kaizenDTO.model_no + "/Before/" + fileName;
            }

            if (kaizenDTO.after_media.Length > 200)
            {
                string folder = _webHostEnvironment.WebRootPath + "\\uploaded\\" + factory + "\\Kaizen\\" + kaizenDTO.model_no + "\\After\\";
                var source = kaizenDTO.after_media;
                string base64 = "";
                var fileName = "";
                var fileName1 = "";//nếu update thay thế hình =video or ngược lại xóa file
                if (kaizenDTO.after_media.Contains("video"))
                {
                    base64 = source.Replace("data:video/mp4;base64,", "");
                    fileName = factory + "_" + kaizenDTO.model_no + "_" + kaizenDTO.serial_no + "_" + "After" + ".mp4";
                    fileName1 = factory + "_" + kaizenDTO.model_no + "_" + kaizenDTO.serial_no + "_" + "After" + ".jpg";
                }
                else
                {
                    base64 = source.Substring(source.IndexOf(',') + 1);
                    base64 = base64.Trim('\0');
                    fileName = factory + "_" + kaizenDTO.model_no + "_" + kaizenDTO.serial_no + "_" + "After" + ".jpg";
                    fileName1 = factory + "_" + kaizenDTO.model_no + "_" + kaizenDTO.serial_no + "_" + "After" + ".mp4";
                }
                byte[] modelData = Convert.FromBase64String(base64);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filePathImages = Path.Combine(folder, fileName);
                string filePathImages1 = Path.Combine(folder, fileName1);
                // kiểm tra file cũ có chưa xóa đi
                if (System.IO.File.Exists(filePathImages))
                {
                    System.IO.File.Delete(filePathImages);
                }
                if (System.IO.File.Exists(filePathImages1))
                {
                    System.IO.File.Delete(filePathImages1);
                }
                System.IO.File.WriteAllBytes(filePathImages, modelData);
                kaizenDTO.after_media = factory + "/Kaizen/" + kaizenDTO.model_no + "/After/" + fileName;
            }

            var result = await _kaizenService.UpdateKaizen(kaizenDTO);
            return Ok(result);
        }

        [HttpGet("getKaizenEdit")]
        public async Task<IActionResult> GetKaizenEdit(string modelNO, string serialNo)
        {
            var data = await _kaizenService.GetKaizenEdit(modelNO, serialNo, factory);
            return Ok(data);
        }
    }
}