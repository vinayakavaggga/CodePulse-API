using CodePulse.API.Models.DataBase;
using CodePulse.API.Models.Request;
using CodePulse.API.Models.Response;
using CodePulse.API.Repositories.IRepositories;
using jdk.nashorn.@internal.ir;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

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
                AuthorId = request.AuthorId,
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
                AuthorId = blogPost.AuthorId,
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
                    AuthorId = blogPost.AuthorId,
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
                AuthorId = existingid.AuthorId,
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
                AuthorId = request.AuthorId,
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
                AuthorId = blogPost.AuthorId,
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
                AuthorId = deletedBlogPost.AuthorId,
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
                AuthorId = blogpost.AuthorId,
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


        [HttpPost]
        [Route("AddAuthors")]
        public async Task<IActionResult> AddAuthors(AddAuthorRequestModel model)
        {
            var authors = new AuthorModel
            {
                AuthorName = model.AuthorName,
                AuthorEmail = model.AuthorEmail,
                RegisteredDate = model.RegisteredDate,
                Description = model.Description
            };

            authors = await blogPostRepository.AddAuthor(authors);

            var response = new AddAuthorResponseModel
            {
                AuthorName = authors.AuthorName,
                AuthorEmail = authors.AuthorEmail,
                RegisteredDate = authors.RegisteredDate,
                Description = authors.Description
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("GetAuthor")]
        public async Task<IActionResult> GetAuthors()
        {
            var response = await blogPostRepository.GetAuthor();

            var getAuthorList = new List<AddAuthorResponseModel>();

            foreach(var item in response)
            {
                getAuthorList.Add(new AddAuthorResponseModel
                {
                    ID = item.ID,
                    AuthorName = item.AuthorName,
                    AuthorEmail = item.AuthorEmail,
                    RegisteredDate = item.RegisteredDate,
                    Description = item.Description
                });
            }

            return Ok(getAuthorList);
        }

        [HttpGet]
        [Route("GetAuthor/{authorId:Guid}")]
        public async Task<IActionResult> GetAuthorById([FromRoute] Guid authorId)
        {
            var author = await blogPostRepository.GetAuthorById(authorId);

            if(author == null)
            {
                return NotFound();
            }

            var response = new AddAuthorResponseModel
            {
                ID = author.ID,
                AuthorName = author.AuthorName,
                AuthorEmail = author.AuthorEmail,
                RegisteredDate = author.RegisteredDate,
                Description = author.Description
            };

            return Ok(response);
        }
    }
}
