using CodePulse.API.Data;
using CodePulse.API.Models.DataBase;
using CodePulse.API.Repositories.IRepositories;
using CodePulse.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
         private readonly ApplicationDBContext _dbContext;

        public CategoryRepository(ApplicationDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        

        public async Task<CategoryModel> CreateAsync(CategoryModel category)
        {
            await _dbContext.CategoryPosts.AddAsync(category);
            await _dbContext.SaveChangesAsync();
           
            return category;
        }

        
        public async Task<IEnumerable<CategoryModel>> GetAsync()
        {
            return await _dbContext.CategoryPosts.ToListAsync();
        }

        public async Task<CategoryModel?> GetCatById(Guid id)
        {
            return await _dbContext.CategoryPosts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<CategoryModel?> UpdateById(CategoryModel model)
        {
            var response = await _dbContext.CategoryPosts.FirstOrDefaultAsync(x => x.Id == model.Id);

            if(response != null)
            {
                _dbContext.Entry(response).CurrentValues.SetValues(model);
                await _dbContext.SaveChangesAsync();
                return model;
            }

            return null;
        }

        public async Task<CategoryModel?> DeleteCategory(Guid id)
        {
            var response = await _dbContext.CategoryPosts.FirstOrDefaultAsync(x => x.Id == id);

            if(response != null )
            {
                _dbContext.CategoryPosts.Remove(response);
                await _dbContext.SaveChangesAsync();
                return response;
            }

            return null;
        }

    }
}
