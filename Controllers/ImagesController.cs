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

        public ImagesController(IImageBlogRepository imageBlogRepository)
        {
            this.imageBlogRepository = imageBlogRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if(ModelState.IsValid)
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
