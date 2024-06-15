namespace CodePulse.API.Models.DataBase
{
    public class CategoryModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string UrlHandle { get; set; }

        public ICollection<BlogPostModel> BlogPost { get; set; }
    }
}
