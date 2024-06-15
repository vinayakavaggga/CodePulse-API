using CodePulse.API.Models.DataBase;

namespace CodePulse.API.Repositories.IRepositories
{
    public interface IBlogPostRepository
    {
        public Task<BlogPostModel> AddBlogPost(BlogPostModel model);

        public Task<IEnumerable<BlogPostModel>> GetBlogPostModel();

        public Task<BlogPostModel?> GetBlogPostById(Guid id);

        public Task<BlogPostModel?> UpdateBlogPost(BlogPostModel model);
    }
}
