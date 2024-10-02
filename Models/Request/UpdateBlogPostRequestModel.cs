namespace CodePulse.API.Models.Request
{
    public class UpdateBlogPostRequestModel
    {
        public required string Title { get; set; }

        public required string ShortDescription { get; set; }

        public required string Content { get; set; }

        public required string UrlHandle { get; set; }

        public string? FeaturedImageURL { get; set; }

        public required DateTime DateCreated { get; set; }

        public required string Author { get; set; }

        public Guid AuthorId { get; set; }

        public bool IsVisible { get; set; }

        public List<Guid> Category { get; set; } = new List<Guid>();
    }
}
