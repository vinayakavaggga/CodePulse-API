using CodePulse.API.Models.DataBase;
using CodePulse.API.Models.Request;
using CodePulse.API.Models.Response;
using CodePulse.API.Repositories.IRepositories;
using jdk.nashorn.@internal.ir;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostController(IBlogPostRepository blogPostRepository,
            ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }

        public IBlogPostRepository BlogPostRepository { get; }

        [HttpPost]
        public async Task<IActionResult> AddBlogPost(BlogPostRequestModel request)
        {
            var blogPost = new BlogPostModel
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                UrlHandle = request.UrlHandle,
                FeaturedImageURL = request.FeaturedImageURL,
                DateCreated = request.DateCreated,
                Author = request.Author,
                IsVisible = request.IsVisible,
                Category = new List<CategoryModel>()
            };

            foreach (var categories in request.Category)
            {
                var exisitingCategory = await categoryRepository.GetCatById(categories);
                if (exisitingCategory != null)
                {
                    blogPost.Category.Add(exisitingCategory);
                }
            }

            blogPost = await blogPostRepository.AddBlogPost(blogPost);

            var response = new BlogPostResponseModel
            {
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                FeaturedImageURL = blogPost.FeaturedImageURL,
                DateCreated = blogPost.DateCreated,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                categoryResponse = blogPost.Category.Select(x => new CategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogPost()
        {
            var response = await blogPostRepository.GetBlogPostModel();
            var blogpostList = new List<BlogPostResponseModel>();

            foreach(var blogPost in response)
            {
                blogpostList.Add(new BlogPostResponseModel
                {
                    ID = blogPost.ID,
                    Title = blogPost.Title,
                    ShortDescription = blogPost.ShortDescription,
                    Content = blogPost.Content,
                    UrlHandle = blogPost.UrlHandle,
                    FeaturedImageURL = blogPost.FeaturedImageURL,
                    DateCreated= blogPost.DateCreated,
                    Author = blogPost.Author,
                    IsVisible = blogPost.IsVisible,
                    categoryResponse = blogPost.Category.Select(x => new CategoryModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,
                    }).ToList()
                });
            }

            return Ok(blogpostList);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute]Guid id)
        {
            var existingid = await blogPostRepository.GetBlogPostById(id);

            if(existingid == null)
            {
                return NotFound();
            }

            //Convert from model to DTO
            var response = new BlogPostResponseModel
            {
                ID = existingid.ID,
                Title = existingid.Title,
                ShortDescription = existingid.ShortDescription,
                Content = existingid.Content,
                UrlHandle = existingid.UrlHandle,
                FeaturedImageURL = existingid.FeaturedImageURL,
                DateCreated = existingid.DateCreated,
                Author = existingid.Author,
                IsVisible = existingid.IsVisible,
                categoryResponse = existingid.Category.Select(x => new CategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestModel request)
        {
            //convert from DTO to Domain

            var blogPost = new BlogPostModel
            {
                ID = id,
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                UrlHandle = request.UrlHandle,
                FeaturedImageURL = request.FeaturedImageURL,
                DateCreated = request.DateCreated,
                Author = request.Author,
                IsVisible = request.IsVisible,
                Category = new List<CategoryModel>()
            };

            foreach(var category in request.Category)
            {
                var existingCategory = await categoryRepository.GetCatById(category);
                if(existingCategory != null)
                {
                    blogPost.Category.Add(existingCategory);
                }
            }

            //call repository and update database

            var updatedBlgPost = await blogPostRepository.UpdateBlogPost(blogPost);

            if(updatedBlgPost == null)
            {
                return NotFound();
            }

            // Convert from domain to DTO

            var response = new BlogPostResponseModel
            {
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                FeaturedImageURL = blogPost.FeaturedImageURL,
                DateCreated = blogPost.DateCreated,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                categoryResponse = blogPost.Category.Select(x => new CategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            //Call repository
            var deletedBlogPost = await blogPostRepository.DeleteBlogPost(id);

            if(deletedBlogPost == null)
            {
                return NotFound();
            }
            
            //convert from Domain to DTO

            var response = new BlogPostResponseModel
            {
                Title = deletedBlogPost.Title,
                ShortDescription = deletedBlogPost.ShortDescription,
                Content = deletedBlogPost.Content,
                UrlHandle = deletedBlogPost.UrlHandle,
                FeaturedImageURL = deletedBlogPost.FeaturedImageURL,
                DateCreated = deletedBlogPost.DateCreated,
                Author = deletedBlogPost.Author,
                IsVisible = deletedBlogPost.IsVisible,
                
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{url}")]
        public async Task<IActionResult> GetBlogPostByURL([FromRoute] string url)
        {
            var blogpost = await blogPostRepository.GetBlogPostByURL(url);

            if (blogpost == null)
            {
                return NotFound();
            }

            //Convert from model to DTO
            var response = new BlogPostResponseModel
            {
                ID = blogpost.ID,
                Title = blogpost.Title,
                ShortDescription = blogpost.ShortDescription,
                Content = blogpost.Content,
                UrlHandle = blogpost.UrlHandle,
                FeaturedImageURL = blogpost.FeaturedImageURL,
                DateCreated = blogpost.DateCreated,
                Author = blogpost.Author,
                IsVisible = blogpost.IsVisible,
                categoryResponse = blogpost.Category.Select(x => new CategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };

            return Ok(response);
        }
    }
}
