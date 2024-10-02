using CodePulse.API.Models.DataBase;
using CodePulse.API.Models.Request;

namespace CodePulse.API.Models.Response
{
    public class BlogPostResponseModel
    {
        public Guid ID { get; set; }
        public required string Title { get; set; }

        public required string ShortDescription { get; set; }

        public required string Content { get; set; }

        public required string UrlHandle { get; set; }

        public string? FeaturedImageURL { get; set; }

        public required DateTime DateCreated { get; set; }

        public required string Author { get; set; }

        public Guid AuthorId { get; set; }

        public bool IsVisible { get; set; }

        public List<CategoryModel> categoryResponse { get; set; } = new List<CategoryModel>();
    }
}
