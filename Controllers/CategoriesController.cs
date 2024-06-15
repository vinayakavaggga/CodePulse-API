using CodePulse.API.Data;
using CodePulse.API.Models.DataBase;
using CodePulse.API.Models.Request;
using CodePulse.API.Models.Response;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestModel catReq)
        {
            var categoryModel = new CategoryModel()
            {
                Name = catReq.Name,
                UrlHandle = catReq.UrlHandle,
            };

            await _categoryRepository.CreateAsync(categoryModel); 

            var categoryResponse = new CategoryResponseModel()
            {
                Name = categoryModel.Name,
                UrlHandle = categoryModel.UrlHandle,
            };

            return Ok(categoryResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories =  await _categoryRepository.GetAsync();
            var categoriesList = new List<CategoryResponseModel>();

            foreach(var category in categories)
            {
                categoriesList.Add(new CategoryResponseModel {
                    Id = category.Id,
                     Name = category.Name,
                     UrlHandle = category.UrlHandle,
                });
            }
            return Ok(categoriesList);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> getCategoryById([FromRoute]Guid id)
        {
            var response = await _categoryRepository.GetCatById(id);

            if(response == null)
            {
                return NotFound();
            }

            var category = new CategoryResponseModel
            {
                Id = response.Id,
                Name = response.Name,
                UrlHandle = response.UrlHandle,
            };
            return Ok(category);
        }

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, UpdateCategotyRequestModel requestModel)
        {
            var Category = new CategoryModel
            {
                Id = id,
                Name = requestModel.Name,
                UrlHandle = requestModel.UrlHandle,
            };

            Category = await _categoryRepository.UpdateById(Category);

            if(Category == null)
            {
                return NotFound();
            }

            var response = new CategoryResponseModel
            {
                Id = Category.Id,
                Name = Category.Name,
                UrlHandle = Category.UrlHandle,
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> DeleteCateory([FromRoute] Guid id)
        {
            var category = await _categoryRepository.DeleteCategory(id);

            if(category == null) 
            { 
                return NotFound(); 
            }

            var response = new CategoryResponseModel
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);
        }
    }
}
