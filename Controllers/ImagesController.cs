using CodePulse.API.Models.DataBase;
using CodePulse.API.Models.Response;
using CodePulse.API.Repositories.IRepositories;
using jdk.nashorn.@internal.ir;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ImagesController : ControllerBase
    {
        private readonly IImageBlogRepository imageBlogRepository;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(IImageBlogRepository imageBlogRepository, ILogger<ImagesController> logger)
        {
            this.imageBlogRepository = imageBlogRepository;
            this._logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            try
            {
                ValidateFileUpload(file);
                if (ModelState.IsValid)
                {
                    //Create a domain model

                    var blogImage = new BlogImage
                    {
                        FileExtension = Path.GetExtension(file.FileName),
                        FileName = fileName,
                        Title = title,
                        DateCreated = DateTime.Now
                    };

                    blogImage = await imageBlogRepository.UploadImage(file, blogImage);

                    var response = new ImageBlogResponseModel
                    {
                        Id = blogImage.Id,
                        FileName = blogImage.FileName,
                        Title = blogImage.Title,
                        DateCreated = blogImage.DateCreated,
                        FileExtension = blogImage.FileExtension,
                        Url = blogImage.Url
                    };

                    return Ok(response);
                }
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Failed");
            }
            
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await imageBlogRepository.GetAllImages();

            //Convert from domain to DTO response

            var response = new List<ImageBlogResponseModel>();
            foreach(var image in images)
            {
                response.Add(new ImageBlogResponseModel
                {
                    Id = image.Id,
                    FileName = image.FileName,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    Url = image.Url
                });
            }

            return Ok(response);
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteImage()
        //{

        //}
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtension = new string[]  { ".jpg", ".jpeg", ".png" };

            if(!allowedExtension.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported Formate");
            }

            if(file.Length > 10485768)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10Mb");
            }
        }
    }
}
