using AutoMapper;
using MongoDB.Driver;
using Travelin.Dtos.SiteSettingDtos;
using Travelin.Entities;
using Travelin.Settings;

namespace Travelin.Services.SiteSettingServices
{
    public class SiteSettingService : ISiteSettingService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<SiteSetting> _siteSettingCollection;

        public SiteSettingService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _siteSettingCollection = database.GetCollection<SiteSetting>(databaseSettings.SiteSettingCollectionName);

            _mapper = mapper;
        }

        public async Task<ResultSiteSettingDto> GetSiteSettingAsync()
        {
            var value = await _siteSettingCollection.Find(x => true).FirstOrDefaultAsync();
            return _mapper.Map<ResultSiteSettingDto>(value);
        }

        public async Task UpdateSiteSettingAsync(UpdateSiteSettingDto updateSiteSettingDto)
        {
            var value = _mapper.Map<SiteSetting>(updateSiteSettingDto);
            await _siteSettingCollection.FindOneAndReplaceAsync(x => x.SiteSettingId == updateSiteSettingDto.SiteSettingId, value);
        }
    }
}