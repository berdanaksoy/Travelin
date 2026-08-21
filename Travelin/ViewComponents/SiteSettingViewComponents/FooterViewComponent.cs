using Microsoft.AspNetCore.Mvc;
using Travelin.Services.SiteSettingServices;
using Travelin.Services.CategoryServices;

namespace Travelin.ViewComponents.SiteSettingViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ISiteSettingService _siteSettingService;
        private readonly ICategoryService _categoryService;

        public FooterViewComponent(ISiteSettingService siteSettingService, ICategoryService categoryService)
        {
            _siteSettingService = siteSettingService;
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var value = await _siteSettingService.GetSiteSettingAsync();
            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            return View(value);
        }
    }
}