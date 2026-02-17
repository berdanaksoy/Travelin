using Microsoft.AspNetCore.Mvc;
using Travelin.Dtos.CategoryDtos;
using Travelin.Services.CategoryServices;

namespace Travelin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            createCategoryDto.IsStatus = true;
            await _categoryService.CreateCategoryAsync(createCategoryDto);

            return RedirectToAction("CategoryList");
        }
    }
}
