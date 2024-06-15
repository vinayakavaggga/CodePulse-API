namespace CodePulse.API.Models.Request
{
    public class UpdateBlogPostRequestModel
    {
        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Content { get; set; }

        public string UrlHandle { get; set; }

        public string FeaturedImageURL { get; set; }

        public DateTime DateCreated { get; set; }

        public string Author { get; set; }

        public bool IsVisible { get; set; }

        public List<Guid> Category { get; set; } = new List<Guid>();
    }
}
