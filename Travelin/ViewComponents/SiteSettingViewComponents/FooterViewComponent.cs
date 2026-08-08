using Microsoft.AspNetCore.Mvc;
using Travelin.Services.SiteSettingServices;

namespace Travelin.ViewComponents.SiteSettingViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ISiteSettingService _siteSettingService;

        public FooterViewComponent(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var value = await _siteSettingService.GetSiteSettingAsync();
            return View(value);
        }
    }
}