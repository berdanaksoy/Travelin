using Travelin.Dtos.SiteSettingDtos;

namespace Travelin.Services.SiteSettingServices
{
    public interface ISiteSettingService
    {
        Task<ResultSiteSettingDto> GetSiteSettingAsync();
        Task UpdateSiteSettingAsync(UpdateSiteSettingDto updateSiteSettingDto);
    }
}