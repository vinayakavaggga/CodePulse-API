using CodePulse.API.Data;
using CodePulse.API.Models.DataBase;
using CodePulse.API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository: IBlogPostRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public BlogPostRepository(ApplicationDBContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<BlogPostModel> AddBlogPost(BlogPostModel model)
        {
            await _dbContext.BlogPosts.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<BlogPostModel>> GetBlogPostModel()
        {
            return await _dbContext.BlogPosts.Include(x => x.Category).ToListAsync();
        }

        public async Task<BlogPostModel?> GetBlogPostById(Guid id)
        {
           return await _dbContext.BlogPosts.Include(x => x.Category).FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<BlogPostModel?> UpdateBlogPost(BlogPostModel blogPostmodel)
        {
            var existingBlogPost = await _dbContext.BlogPosts.Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.ID == blogPostmodel.ID);

            if (existingBlogPost == null)
            {
                return null;
            }

            //update blogpost
            _dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPostmodel);

            //update categories
            existingBlogPost.Category = blogPostmodel.Category;

            await _dbContext.SaveChangesAsync();

            return blogPostmodel;
        }

        public async Task<BlogPostModel?> DeleteBlogPost(Guid id)
        {
            var existingBlogPost = await _dbContext.BlogPosts.FirstOrDefaultAsync(x => x.ID == id);

            if(existingBlogPost == null)
            {
                return null;
            }

            _dbContext.BlogPosts.Remove(existingBlogPost);
            await _dbContext.SaveChangesAsync();

            return existingBlogPost;
        }
    }
}
