using CodePulse.API.Models.DataBase;

namespace CodePulse.API.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        public Task<CategoryModel> CreateAsync(CategoryModel category);

        public Task<IEnumerable<CategoryModel>> GetAsync();

        public Task<CategoryModel?> GetCatById(Guid id);

        public Task<CategoryModel?> UpdateById(CategoryModel model);

        public Task<CategoryModel?> DeleteCategory(Guid id);
    }
}
