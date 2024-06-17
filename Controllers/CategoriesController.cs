using BitWiseBlog.API.Data;
using BitWiseBlog.API.Models.Domain;
using BitWiseBlog.API.Models.DTO;
using BitWiseBlog.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BitWiseBlog.API.Controllers
{
    //following will be the base route to the controller
    //https://localhost:xxxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;


        //creating constructor and injecting the DBContext class in here
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }


        //put action method
        //route to reach the post method
        //
        [HttpPost]//attribute
        //[Authorize(Roles = "Writer")]

        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            //Map DTO to domain model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

           await categoryRepository.CreateAsync(category);

            //for response as we never want to share our domain models with the user as per business logic we will map it 
            //Domain model to DTO again
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            //after this we want to return a response
            return Ok(response);//return the dt object
        }

        //path for the get method
        //GET: https://localhost:7007/api/Categories?query=html&sortBy=name&sortDirection=asc
        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] string? query,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize)
        {
            var categories = await categoryRepository.GetAllAsync(query,sortBy,sortDirection,pageNumber,pageSize);

            //map domain model to DTO

            var response = new List<CategoryDto>();
            foreach(var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle,
                });
            }

            return Ok(response);
        }

        //GetbyID
        //GET: https://localhost:7007/api/categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await categoryRepository.GetById(id);

            if(existingCategory is null) {
                return NotFound();
            }

            //map to DTO
            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle,
            };

            return Ok(response);
        }

        //PUT:https://localhost:7007/api/categories/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> EditCategory([FromRoute] Guid id,UpdateCategoryRequestDto request)
        {
            //map DTO to Domain model
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

          category =  await categoryRepository.UpdateAsync(category);

            if(category is null)
            {
                return NotFound();
            }

            //convert Domain model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);
        }

        //DELETE:https://localhost:7007/api/categories/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]


        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if(category is null)
            {
                return NotFound();
            }

            //Convert Domain model to DTO
            var response = new CategoryDto { 
                Id = category.Id, 
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);    
        }

        //GET:https://localhost:7007/api/categories/count
        [HttpGet]
        [Route("count")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetCategoriesTotal()
        {
            var count = await categoryRepository.GetCount();

            return Ok(count);
        }

    }
}
//DTO data transfer objects used to transfer data between different layers e.g transfer of data over the network or through layers of application