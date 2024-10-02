using CodePulse.API.Data;
using CodePulse.API.Models.DataBase;
using CodePulse.API.Repositories.IRepositories;
using com.sun.xml.@internal.bind.v2.model.core;
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

        public async Task<BlogPostModel?> GetBlogPostByURL(string url)
        {
            return await _dbContext.BlogPosts.Include(x => x.Category).FirstOrDefaultAsync(x => x.UrlHandle == url);
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

        public async Task<AuthorModel> AddAuthor(AuthorModel model)
        {
            await _dbContext.Authors.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<AuthorModel>> GetAuthor()
        {
            return await _dbContext.Authors.ToListAsync();
        }

        public async Task<AuthorModel?> GetAuthorById(Guid authorId)
        {
            return await _dbContext.Authors.FirstOrDefaultAsync(x => x.ID == authorId);
        }
    }
}
