using CodePulse.API.Models;
using CodePulse.API.Models.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace CodePulse.API.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BlogPostModel> BlogPosts { get; set; }

        public DbSet<CategoryModel> CategoryPosts { get; set; }

        public DbSet<BlogImage> BlogImages { get; set; }
    }
}
