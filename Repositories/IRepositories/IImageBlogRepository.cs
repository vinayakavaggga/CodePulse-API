using CodePulse.API.Models.DataBase;

namespace CodePulse.API.Repositories.IRepositories
{
    public interface IImageBlogRepository
    {
        public Task<BlogImage> UploadImage(IFormFile file, BlogImage blogImage);
    }
}
