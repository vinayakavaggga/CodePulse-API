using CodePulse.API.Data;
using CodePulse.API.Models.DataBase;
using CodePulse.API.Repositories.IRepositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class ImageRepository : IImageBlogRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDBContext applicationDBContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor,
            ApplicationDBContext applicationDBContext )
        {
            this._webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.applicationDBContext = applicationDBContext;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        public async Task<ICollection<BlogImage>> GetAllImages()
        {
            return await applicationDBContext.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> UploadImage(IFormFile file, BlogImage blogImage)
        {
            //Save Files to image folder
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            //Update to database
            //URL Field: https://localhost:4200/Images//fileName.jpg

            var httpContext = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpContext.Scheme}://{httpContext.Host}{httpContext.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;
            await applicationDBContext.BlogImages.AddAsync(blogImage);
            await applicationDBContext.SaveChangesAsync();

            return blogImage;
        }
    }
}
