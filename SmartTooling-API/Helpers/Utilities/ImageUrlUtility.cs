using Microsoft.Extensions.Options;

namespace SmartTooling_API.Helpers.Utilities
{
    public interface IImageUrlUtility {
        Task<string> GetImageUrlByFactory(string factory);
    }
    public class ImageUrlUtility : IImageUrlUtility
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<ImageUrlSetting> _imageUrlSetting;

        public ImageUrlUtility(IConfiguration configuration, IOptions<ImageUrlSetting> ImageUrlSetting){
            _configuration = configuration;
            _imageUrlSetting = ImageUrlSetting;
        }

        public async Task<string> GetImageUrlByFactory(string factory)
        {
            string area = _configuration.GetSection("AppSettings:Area").Value;
            string imageUrl = "";
            switch (factory)
            {
                case "SHC":
                    imageUrl = area == "T" ? _imageUrlSetting.Value.SHC_T : _imageUrlSetting.Value.SHC_O;
                    break;

                case "CB":
                    imageUrl = area == "T" ? _imageUrlSetting.Value.CB_T : _imageUrlSetting.Value.CB_O;
                    break;

                case "TSH":
                    imageUrl = area == "T" ? _imageUrlSetting.Value.TSH_T : _imageUrlSetting.Value.TSH_O;
                    break;

                case "SPC":
                    imageUrl = area == "T" ? _imageUrlSetting.Value.SPC_T : _imageUrlSetting.Value.SPC_O;
                    break;

                default:
                    imageUrl = "";
                    break;
            }
            return await Task.FromResult(imageUrl);
        }
    }
}