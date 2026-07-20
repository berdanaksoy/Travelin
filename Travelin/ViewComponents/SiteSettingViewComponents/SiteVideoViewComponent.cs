using Microsoft.AspNetCore.Mvc;
using Travelin.Services.SiteSettingServices;

namespace Travelin.ViewComponents.SiteSettingViewComponents
{
    public class SiteVideoViewComponent : ViewComponent
    {
        private readonly ISiteSettingService _siteSettingService;

        public SiteVideoViewComponent(ISiteSettingService siteSettingService)
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