using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travelin.Dtos.CategoryDtos;
using Travelin.Services.CategoryServices;

namespace Travelin.Controllers
{
    public class AdminCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public AdminCategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> CategoryList()
        {
            var values = await _categoryService.GetAllCategoryAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
                return View(createCategoryDto);

            await _categoryService.CreateCategoryAsync(createCategoryDto);
            TempData["Success"] = "Kategori başarıyla eklendi.";
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(string id)
        {
            var value = await _categoryService.GetCategoryByIdAsync(id);

            if (value == null)
                return RedirectToAction("CategoryList");

            var model = _mapper.Map<UpdateCategoryDto>(value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
                return View(updateCategoryDto);

            await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            TempData["Success"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction("CategoryList");
        }

        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            TempData["Success"] = "Kategori başarıyla silindi.";
            return RedirectToAction("CategoryList");
        }
    }
}