using CodePulse.API.Models.DataBase;
using CodePulse.API.Models.Request;

namespace CodePulse.API.Models.Response
{
    public class BlogPostResponseModel
    {
        public Guid ID { get; set; }
        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Content { get; set; }

        public string UrlHandle { get; set; }

        public string FeaturedImageURL { get; set; }

        public DateTime DateCreated { get; set; }

        public string Author { get; set; }

        public bool IsVisible { get; set; }

        public List<CategoryModel> categoryResponse { get; set; } = new List<CategoryModel>();
    }
}
