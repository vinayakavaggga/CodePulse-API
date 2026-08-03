using CodePulse.API.Data;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("VVBloggersConnectionStrings"));
});

// Register CORS BEFORE builder.Build()
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFirebase", policy =>
    {
        policy
            .WithOrigins(
                "https://gk-bloggers-v.web.app",
                "https://gk-bloggers-v.firebaseapp.com",
                "http://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IImageBlogRepository, ImageRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Use the CORS policy
app.UseCors("AllowFirebase");

app.UseAuthorization();

var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images");

if (!Directory.Exists(imagePath))
{
    Directory.CreateDirectory(imagePath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagePath),
    RequestPath = "/Resources/Images"
});

app.MapControllers();

app.Run();