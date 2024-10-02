using CodePulse.API.Models.DataBase;

namespace CodePulse.API.Repositories.IRepositories
{
    public interface IBlogPostRepository
    {
        public Task<BlogPostModel> AddBlogPost(BlogPostModel model);

        public Task<IEnumerable<BlogPostModel>> GetBlogPostModel();

        public Task<BlogPostModel?> GetBlogPostById(Guid id);

        public Task<BlogPostModel?> GetBlogPostByURL(string url);

        public Task<BlogPostModel?> UpdateBlogPost(BlogPostModel model);

        public Task<BlogPostModel?> DeleteBlogPost(Guid id);

        public Task<AuthorModel> AddAuthor(AuthorModel model);

        public Task<IEnumerable<AuthorModel>> GetAuthor();

        public Task<AuthorModel?> GetAuthorById(Guid authorId);
    }
}
