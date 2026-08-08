using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Travelin.Dtos.SiteSettingDtos;
using Travelin.Services.SiteSettingServices;

namespace Travelin.Controllers
{
    public class AdminSiteSettingController : Controller
    {
        private readonly ISiteSettingService _siteSettingService;
        private readonly IMapper _mapper;

        public AdminSiteSettingController(ISiteSettingService siteSettingService, IMapper mapper)
        {
            _siteSettingService = siteSettingService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var value = await _siteSettingService.GetSiteSettingAsync();
            var updateDto = _mapper.Map<UpdateSiteSettingDto>(value);
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UpdateSiteSettingDto updateSiteSettingDto)
        {
            await _siteSettingService.UpdateSiteSettingAsync(updateSiteSettingDto);
            TempData["Success"] = "Site ayarları güncellendi.";
            return RedirectToAction("Index");
        }
    }
}