using MongoDB.Bson;
using MongoDB.Driver;
using Travelin.Entities;
using Travelin.Settings;

namespace Travelin.Seed
{
    public class DataSeeder
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMongoCollection<TourProgram> _tourProgramCollection;
        private readonly IMongoCollection<Comment> _commentCollection;
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<SiteSetting> _siteSettingCollection;

        public DataSeeder(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _tourProgramCollection = database.GetCollection<TourProgram>(databaseSettings.TourProgramCollectionName);
            _commentCollection = database.GetCollection<Comment>(databaseSettings.CommentCollectionName);
            _reservationCollection = database.GetCollection<Reservation>(databaseSettings.ReservationCollectionName);
            _siteSettingCollection = database.GetCollection<SiteSetting>(databaseSettings.SiteSettingCollectionName);
        }

        public async Task SeedAsync()
        {
            var categories = await SeedCategoriesAsync();
            var tours = await SeedToursAsync(categories);
            await SeedSiteSettingAsync();
            await SeedTourProgramsAsync(tours);
            await SeedCommentsAsync(tours);
            await SeedReservationsAsync(tours);
        }

        private async Task<List<Category>> SeedCategoriesAsync()
        {
            var existing = await _categoryCollection.CountDocumentsAsync(FilterDefinition<Category>.Empty);
            if (existing > 0)
                return await _categoryCollection.Find(FilterDefinition<Category>.Empty).ToListAsync();

            var categories = new List<Category>
            {
                new Category { CategoryName = "Kültür Turları", IconUrl = "https://cdn-icons-png.flaticon.com/512/3079/3079165.png", IsStatus = true },
                new Category { CategoryName = "Doğa ve Kamp", IconUrl = "https://cdn-icons-png.flaticon.com/512/2775/2775670.png", IsStatus = true },
                new Category { CategoryName = "Deniz ve Kum", IconUrl = "https://cdn-icons-png.flaticon.com/512/3199/3199845.png", IsStatus = true },
                new Category { CategoryName = "Şehir Turları", IconUrl = "https://cdn-icons-png.flaticon.com/512/1000/1000946.png", IsStatus = true },
                new Category { CategoryName = "Kış Turları", IconUrl = "https://cdn-icons-png.flaticon.com/512/2942/2942909.png", IsStatus = true },
                new Category { CategoryName = "Gastronomi", IconUrl = "https://cdn-icons-png.flaticon.com/512/706/706164.png", IsStatus = true }
            };

            await _categoryCollection.InsertManyAsync(categories);
            return categories;
        }

        private async Task<List<Tour>> SeedToursAsync(List<Category> categories)
        {
            var existing = await _tourCollection.CountDocumentsAsync(FilterDefinition<Tour>.Empty);
            if (existing > 0)
                return await _tourCollection.Find(FilterDefinition<Tour>.Empty).ToListAsync();

            var now = DateTime.Now;

            var tours = new List<Tour>
            {
                // KÜLTÜR TURLARI (categories[0])
                new Tour { Title = "Barselona Kültür ve Gaudí Turu", Country = "İspanya", City = "Barselona", Description = "Sagrada Familia, Park Güell ve Gaudí'nin eşsiz mimarisiyle Barselona'nın kültürel dokusunu keşfedin. Gotik Mahalle'de tarih, Las Ramblas'ta yaşam.", Capacity = 25, TourDate = now.AddDays(45), DayNight = "4 Gün / 3 Gece", ImageUrl = "https://images.unsplash.com/photo-1583422409516-2895a77efded?w=800", Price = 17000, LocationImageUrl = "https://images.unsplash.com/photo-1562883676-8c7feb83f09b?w=800", IsStatus = true, CategoryId = categories[0].CategoryId, VideoUrl = "" },
                new Tour { Title = "Roma Antik Şehir Turu", Country = "İtalya", City = "Roma", Description = "Kolezyum, Roma Forumu ve Vatikan ile antik dünyanın kalbine yolculuk. Trevi Çeşmesi'ne bozuk para atmayı unutmayın.", Capacity = 30, TourDate = now.AddDays(60), DayNight = "5 Gün / 4 Gece", ImageUrl = "https://images.unsplash.com/photo-1552832230-c0197dd311b5?w=800", Price = 19500, LocationImageUrl = "https://images.unsplash.com/photo-1515542622106-78bda8ba0e5b?w=800", IsStatus = true, CategoryId = categories[0].CategoryId, VideoUrl = "" },
                new Tour { Title = "Atina Mitoloji ve Akropolis Turu", Country = "Yunanistan", City = "Atina", Description = "Partenon, Akropolis ve antik Yunan medeniyetinin izleri. Plaka bölgesinde geleneksel taverna deneyimi.", Capacity = 28, TourDate = now.AddDays(38), DayNight = "4 Gün / 3 Gece", ImageUrl = "https://images.unsplash.com/photo-1555993539-1732b0258235?w=800", Price = 15500, LocationImageUrl = "https://images.unsplash.com/photo-1603565816030-6b389eeb23cb?w=800", IsStatus = true, CategoryId = categories[0].CategoryId, VideoUrl = "" },
                new Tour { Title = "Kahire Piramitler ve Firavunlar Turu", Country = "Mısır", City = "Kahire", Description = "Giza Piramitleri, Sfenks ve Mısır Müzesi. Nil Nehri'nde tekne turu ve antik firavun tarihine yolculuk.", Capacity = 22, TourDate = now.AddDays(52), DayNight = "6 Gün / 5 Gece", ImageUrl = "https://images.unsplash.com/photo-1539768942893-daf53e448371?w=800", Price = 21000, LocationImageUrl = "https://images.unsplash.com/photo-1572252009286-268acec5ca0a?w=800", IsStatus = true, CategoryId = categories[0].CategoryId, VideoUrl = "" },
                new Tour { Title = "Kudüs Kutsal Topraklar Turu", Country = "İsrail", City = "Kudüs", Description = "Üç semavi dinin buluştuğu kutsal şehir. Ağlama Duvarı, Kutsal Kabir Kilisesi ve tarihi eski şehir.", Capacity = 20, TourDate = now.AddDays(70), DayNight = "5 Gün / 4 Gece", ImageUrl = "https://images.unsplash.com/photo-1544971587-b4e40d6a1d78?w=800", Price = 18000, LocationImageUrl = "", IsStatus = false, CategoryId = categories[0].CategoryId, VideoUrl = "" },

                // DOĞA VE KAMP (categories[1])
                new Tour { Title = "Kapadokya Balon ve Vadi Turu", Country = "Türkiye", City = "Nevşehir", Description = "Peri bacaları, sıcak hava balonu ve yeraltı şehirleri. Güneşin doğuşunu gökyüzünden izleyin.", Capacity = 24, TourDate = now.AddDays(30), DayNight = "3 Gün / 2 Gece", ImageUrl = "https://images.unsplash.com/photo-1570939274717-7eda259b50ed?w=800", Price = 9500, LocationImageUrl = "https://images.unsplash.com/photo-1641128324972-af3212f0f6bd?w=800", IsStatus = true, CategoryId = categories[1].CategoryId, VideoUrl = "" },
                new Tour { Title = "İsviçre Alpleri Kamp Turu", Country = "İsviçre", City = "Interlaken", Description = "Alp dağları, buzul gölleri ve nefes kesen manzaralar. Doğayla iç içe kamp deneyimi ve yürüyüş parkurları.", Capacity = 16, TourDate = now.AddDays(85), DayNight = "5 Gün / 4 Gece", ImageUrl = "https://images.unsplash.com/photo-1531366936337-7c912a4589a7?w=800", Price = 24000, LocationImageUrl = "https://images.unsplash.com/photo-1508739773434-c26b3d09e071?w=800", IsStatus = true, CategoryId = categories[1].CategoryId, VideoUrl = "" },
                new Tour { Title = "Karadeniz Yaylaları Doğa Turu", Country = "Türkiye", City = "Rize", Description = "Ayder Yaylası, sis altında yeşil tepeler ve şelaleler. Geleneksel yayla evlerinde konaklama.", Capacity = 20, TourDate = now.AddDays(42), DayNight = "4 Gün / 3 Gece", ImageUrl = "https://images.unsplash.com/photo-1589553416260-f586c8f1514f?w=800", Price = 7500, LocationImageUrl = "", IsStatus = true, CategoryId = categories[1].CategoryId, VideoUrl = "" },
                new Tour { Title = "İzlanda Şelaleler ve Kuzey Işıkları", Country = "İzlanda", City = "Reykjavik", Description = "Buz mağaraları, gayzerler ve büyülü kuzey ışıkları. Doğanın en vahşi ve güzel halini yaşayın.", Capacity = 18, TourDate = now.AddDays(95), DayNight = "6 Gün / 5 Gece", ImageUrl = "https://images.unsplash.com/photo-1504829857797-ddff29c27927?w=800", Price = 28000, LocationImageUrl = "https://images.unsplash.com/photo-1531366936337-7c912a4589a7?w=800", IsStatus = true, CategoryId = categories[1].CategoryId, VideoUrl = "" },
                new Tour { Title = "Amazon Yağmur Ormanı Keşfi", Country = "Brezilya", City = "Manaus", Description = "Dünyanın akciğerleri Amazon'da nehir safarisi, egzotik canlılar ve yerli kabile kültürü.", Capacity = 14, TourDate = now.AddDays(110), DayNight = "7 Gün / 6 Gece", ImageUrl = "https://images.unsplash.com/photo-1516908205727-40afad9449a8?w=800", Price = 32000, LocationImageUrl = "", IsStatus = false, CategoryId = categories[1].CategoryId, VideoUrl = "" },

                // DENİZ VE KUM (categories[2])
                new Tour { Title = "Maldivler Cennet Adaları", Country = "Maldivler", City = "Male", Description = "Turkuaz sular, beyaz kum plajları ve su üstü bungalovlar. Dalış ve şnorkel ile mercan resiflerini keşfedin.", Capacity = 16, TourDate = now.AddDays(75), DayNight = "6 Gün / 5 Gece", ImageUrl = "https://images.unsplash.com/photo-1514282401047-d79a71a590e8?w=800", Price = 35000, LocationImageUrl = "https://images.unsplash.com/photo-1573843981267-be1999ff37cd?w=800", IsStatus = true, CategoryId = categories[2].CategoryId, VideoUrl = "" },
                new Tour { Title = "Antalya Tekne ve Koy Turu", Country = "Türkiye", City = "Antalya", Description = "Akdeniz'in saklı koyları, mavi yolculuk ve tarihi Kaleiçi. Güneş, deniz ve tarih bir arada.", Capacity = 30, TourDate = now.AddDays(35), DayNight = "4 Gün / 3 Gece", ImageUrl = "https://images.unsplash.com/photo-1590523278191-995cbcda646b?w=800", Price = 8500, LocationImageUrl = "", IsStatus = true, CategoryId = categories[2].CategoryId, VideoUrl = "" },
                new Tour { Title = "Bali Tropik Ada Turu", Country = "Endonezya", City = "Bali", Description = "Pirinç terasları, tapınaklar ve muhteşem plajlar. Ubud'da ruhani huzur, Kuta'da sörf keyfi.", Capacity = 22, TourDate = now.AddDays(88), DayNight = "7 Gün / 6 Gece", ImageUrl = "https://images.unsplash.com/photo-1537996194471-e657df975ab4?w=800", Price = 26000, LocationImageUrl = "https://images.unsplash.com/photo-1518548419970-58e3b4079ab2?w=800", IsStatus = true, CategoryId = categories[2].CategoryId, VideoUrl = "" },
                new Tour { Title = "Bodrum Mavi Yolculuk", Country = "Türkiye", City = "Muğla", Description = "Ege'nin berrak sularında yelkenli ile koydan koya. Gece yıldızlar altında tekne konaklaması.", Capacity = 12, TourDate = now.AddDays(48), DayNight = "5 Gün / 4 Gece", ImageUrl = "https://images.unsplash.com/photo-1567527970664-89d54d5cf7e5?w=800", Price = 12000, LocationImageUrl = "", IsStatus = true, CategoryId = categories[2].CategoryId, VideoUrl = "" },
                new Tour { Title = "Karayipler Kruvaziyer Turu", Country = "Bahamalar", City = "Nassau", Description = "Lüks kruvaziyer ile Karayip adaları. Beyaz plajlar, palmiyeler ve tropik cennet.", Capacity = 40, TourDate = now.AddDays(120), DayNight = "8 Gün / 7 Gece", ImageUrl = "https://images.unsplash.com/photo-1548574505-5e239809ee19?w=800", Price = 38000, LocationImageUrl = "", IsStatus = true, CategoryId = categories[2].CategoryId, VideoUrl = "" },

                // ŞEHİR TURLARI (categories[3])
                new Tour { Title = "Paris Işık Şehri Turu", Country = "Fransa", City = "Paris", Description = "Eyfel Kulesi, Louvre ve Şanzelize. Aşk şehrinde romantik bir kaçamak ve sanat dolu günler.", Capacity = 28, TourDate = now.AddDays(40), DayNight = "4 Gün / 3 Gece", ImageUrl = "https://images.unsplash.com/photo-1502602898657-3e91760cbb34?w=800", Price = 20000, LocationImageUrl = "https://images.unsplash.com/photo-1431274172761-fca41d930114?w=800", IsStatus = true, CategoryId = categories[3].CategoryId, VideoUrl = "" },
                new Tour { Title = "Prag Masalsı Şehir Turu", Country = "Çekya", City = "Prag", Description = "Orta Çağ'dan kalma köprüler, kaleler ve masalsı sokaklar. Astronomik saat ve Charles Köprüsü.", Capacity = 26, TourDate = now.AddDays(55), DayNight = "3 Gün / 2 Gece", ImageUrl = "https://images.unsplash.com/photo-1541849546-216549ae216d?w=800", Price = 14000, LocationImageUrl = "", IsStatus = true, CategoryId = categories[3].CategoryId, VideoUrl = "" },
                new Tour { Title = "Dubai Modern Şehir Turu", Country = "BAE", City = "Dubai", Description = "Burj Khalifa, çöl safari ve lüks alışveriş. Geleceğin şehrinde göz kamaştıran deneyimler.", Capacity = 32, TourDate = now.AddDays(50), DayNight = "5 Gün / 4 Gece", ImageUrl = "https://images.unsplash.com/photo-1512453979798-5ea266f8880c?w=800", Price = 23000, LocationImageUrl = "https://images.unsplash.com/photo-1518684079-3c830dcef090?w=800", IsStatus = true, CategoryId = categories[3].CategoryId, VideoUrl = "" },
                new Tour { Title = "Londra Klasik Turu", Country = "İngiltere", City = "Londra", Description = "Big Ben, Buckingham Sarayı ve Tower Bridge. Kızıl otobüsler ve İngiliz kültürü.", Capacity = 30, TourDate = now.AddDays(65), DayNight = "4 Gün / 3 Gece", ImageUrl = "https://images.unsplash.com/photo-1513635269975-59663e0ac1ad?w=800", Price = 22000, LocationImageUrl = "", IsStatus = true, CategoryId = categories[3].CategoryId, VideoUrl = "" },
                new Tour { Title = "New York Şehir Işıkları", Country = "ABD", City = "New York", Description = "Times Meydanı, Özgürlük Heykeli ve Central Park. Uyumayan şehirde unutulmaz anlar.", Capacity = 34, TourDate = now.AddDays(100), DayNight = "6 Gün / 5 Gece", ImageUrl = "https://images.unsplash.com/photo-1496442226666-8d4d0e62e6e9?w=800", Price = 30000, LocationImageUrl = "https://images.unsplash.com/photo-1522083165195-3424ed129620?w=800", IsStatus = true, CategoryId = categories[3].CategoryId, VideoUrl = "" },
                new Tour { Title = "Tokyo Geleneksel ve Modern", Country = "Japonya", City = "Tokyo", Description = "Tapınaklar, gökdelenler ve kiraz çiçekleri. Geleneğin ve teknolojinin buluştuğu büyüleyici şehir.", Capacity = 24, TourDate = now.AddDays(115), DayNight = "7 Gün / 6 Gece", ImageUrl = "https://images.unsplash.com/photo-1540959733332-eab4deabeeaf?w=800", Price = 33000, LocationImageUrl = "", IsStatus = true, CategoryId = categories[3].CategoryId, VideoUrl = "" },

                // KIŞ TURLARI (categories[4])
                new Tour { Title = "Uludağ Kayak Turu", Country = "Türkiye", City = "Bursa", Description = "Karlı pistler, kayak keyfi ve dağ manzarası. Kış sporları ve sıcak şömine başı.", Capacity = 26, TourDate = now.AddDays(-15), DayNight = "3 Gün / 2 Gece", ImageUrl = "https://images.unsplash.com/photo-1551698618-1dfe5d97d256?w=800", Price = 8000, LocationImageUrl = "", IsStatus = true, CategoryId = categories[4].CategoryId, VideoUrl = "" },
                new Tour { Title = "Alpler Kayak Merkezi Turu", Country = "Avusturya", City = "Innsbruck", Description = "Dünya çapında kayak pistleri, Alp köyleri ve kış masalı. Profesyonel kayak deneyimi.", Capacity = 20, TourDate = now.AddDays(-30), DayNight = "5 Gün / 4 Gece", ImageUrl = "https://images.unsplash.com/photo-1491555103944-7c647fd857e6?w=800", Price = 25000, LocationImageUrl = "https://images.unsplash.com/photo-1548777123-e216912df7d8?w=800", IsStatus = true, CategoryId = categories[4].CategoryId, VideoUrl = "" },
                new Tour { Title = "Laponya Kar ve Ren Geyiği Turu", Country = "Finlandiya", City = "Rovaniemi", Description = "Noel Baba'nın evi, ren geyiği kızağı ve kar altında büyülü bir dünya. Kuzey ışıkları eşliğinde.", Capacity = 18, TourDate = now.AddDays(80), DayNight = "6 Gün / 5 Gece", ImageUrl = "https://images.unsplash.com/photo-1517299321609-52687d1bc55a?w=800", Price = 29000, LocationImageUrl = "", IsStatus = true, CategoryId = categories[4].CategoryId, VideoUrl = "" },
                new Tour { Title = "Palandöken Kayak Tatili", Country = "Türkiye", City = "Erzurum", Description = "Türkiye'nin en uzun pistleri, tozlu kar ve kayak tutkusu. Kış sporları cenneti.", Capacity = 24, TourDate = now.AddDays(90), DayNight = "4 Gün / 3 Gece", ImageUrl = "https://images.unsplash.com/photo-1565992441121-4367c2967103?w=800", Price = 9000, LocationImageUrl = "", IsStatus = false, CategoryId = categories[4].CategoryId, VideoUrl = "" },

                // GASTRONOMİ (categories[5])
                new Tour { Title = "İtalya Gastronomi Turu", Country = "İtalya", City = "Bologna", Description = "Gerçek İtalyan mutfağı, şarap tadımı ve makarna atölyesi. Lezzetin başkentinde damak şöleni.", Capacity = 18, TourDate = now.AddDays(58), DayNight = "5 Gün / 4 Gece", ImageUrl = "https://images.unsplash.com/photo-1498579150354-977475b7ea0b?w=800", Price = 21000, LocationImageUrl = "https://images.unsplash.com/photo-1533777324565-a040eb52facd?w=800", IsStatus = true, CategoryId = categories[5].CategoryId, VideoUrl = "" },
                new Tour { Title = "Gaziantep Lezzet Turu", Country = "Türkiye", City = "Gaziantep", Description = "Baklava, kebap ve zengin Antep mutfağı. UNESCO gastronomi şehrinde eşsiz tatlar.", Capacity = 25, TourDate = now.AddDays(33), DayNight = "3 Gün / 2 Gece", ImageUrl = "https://images.unsplash.com/photo-1601050690597-df0568f70950?w=800", Price = 6500, LocationImageUrl = "", IsStatus = true, CategoryId = categories[5].CategoryId, VideoUrl = "" },
                new Tour { Title = "Fransa Şarap ve Peynir Rotası", Country = "Fransa", City = "Bordeaux", Description = "Bağ bahçeleri, şato şarapları ve Fransız peynirleri. Bordeaux'nun eşsiz gastronomi kültürü.", Capacity = 16, TourDate = now.AddDays(72), DayNight = "5 Gün / 4 Gece", ImageUrl = "https://images.unsplash.com/photo-1510812431401-41d2bd2722f3?w=800", Price = 24000, LocationImageUrl = "", IsStatus = true, CategoryId = categories[5].CategoryId, VideoUrl = "" },
                new Tour { Title = "Tayland Sokak Lezzetleri", Country = "Tayland", City = "Bangkok", Description = "Sokak mutfağı, baharatlı tatlar ve yüzen pazarlar. Asya'nın en renkli gastronomi deneyimi.", Capacity = 22, TourDate = now.AddDays(105), DayNight = "6 Gün / 5 Gece", ImageUrl = "https://images.unsplash.com/photo-1504674900247-0877df9cc836?w=800", Price = 27000, LocationImageUrl = "", IsStatus = true, CategoryId = categories[5].CategoryId, VideoUrl = "" },

                // Ek karışık turlar
                new Tour { Title = "Santorini Gün Batımı Turu", Country = "Yunanistan", City = "Santorini", Description = "Beyaz badanalı evler, mavi kubbeler ve dünyanın en güzel gün batımı. Ege'nin incisi.", Capacity = 20, TourDate = now.AddDays(62), DayNight = "4 Gün / 3 Gece", ImageUrl = "https://images.unsplash.com/photo-1570077188670-e3a8d69ac5ff?w=800", Price = 19000, LocationImageUrl = "https://images.unsplash.com/photo-1613395877344-13d4a8e0d49e?w=800", IsStatus = true, CategoryId = categories[2].CategoryId, VideoUrl = "" },
                new Tour { Title = "İstanbul Tarihi Yarımada Turu", Country = "Türkiye", City = "İstanbul", Description = "Ayasofya, Topkapı Sarayı ve Kapalıçarşı. İki kıtayı birleştiren şehrin binlerce yıllık tarihi.", Capacity = 35, TourDate = now.AddDays(28), DayNight = "3 Gün / 2 Gece", ImageUrl = "https://images.unsplash.com/photo-1541432901042-2d8bd64b4a9b?w=800", Price = 7000, LocationImageUrl = "https://images.unsplash.com/photo-1524231757912-21f4fe3a7200?w=800", IsStatus = true, CategoryId = categories[0].CategoryId, VideoUrl = "" }
            };

            await _tourCollection.InsertManyAsync(tours);
            return tours;
        }

        private async Task SeedSiteSettingAsync()
        {
            var existing = await _siteSettingCollection.CountDocumentsAsync(FilterDefinition<SiteSetting>.Empty);
            if (existing > 0)
                return;

            var siteSetting = new SiteSetting
            {
                VideoUrl = "https://www.youtube.com/embed/ysynaVNKr0I?controls=1&rel=0",
                Phone = "05551234567",
                Email = "berdan0227@gmail.com",
                Address = "Türkiye",
                FacebookUrl = "https://github.com/berdanaksoy",
                TwitterUrl = "https://x.com/twberdanaksoy",
                InstagramUrl = "https://www.instagram.com/berdanaksoy/",
                LinkedinUrl = "https://www.linkedin.com/in/berdanaksoy/?locale=en"
            };

            await _siteSettingCollection.InsertOneAsync(siteSetting);
        }

        private async Task SeedTourProgramsAsync(List<Tour> tours)
        {
            var existing = await _tourProgramCollection.CountDocumentsAsync(FilterDefinition<TourProgram>.Empty);
            if (existing > 0)
                return;

            var programs = new List<TourProgram>();

            var barcelona = tours.First(t => t.Title.Contains("Barselona"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = barcelona.TourId, DayNumber = 1, Title = "Varış ve Gotik Mahalle", Description = "Havalimanı karşılama ve otele yerleşme. Öğleden sonra Gotik Mahalle'de yürüyüş, Barselona Katedrali ziyareti ve Las Ramblas Bulvarı'nda serbest zaman." },
                new TourProgram { TourId = barcelona.TourId, DayNumber = 2, Title = "Gaudí Rotası", Description = "Sabah Sagrada Familia'nın büyüleyici mimarisi. Öğleden sonra Park Güell'in renkli mozaikleri ve Casa Batlló'nun eşsiz cephesi." },
                new TourProgram { TourId = barcelona.TourId, DayNumber = 3, Title = "Sanat ve Deniz", Description = "Picasso Müzesi ve Montjuïc Tepesi. Akşam Barceloneta Plajı boyunca yürüyüş ve deniz kenarında akşam yemeği." },
                new TourProgram { TourId = barcelona.TourId, DayNumber = 4, Title = "Serbest Gün ve Dönüş", Description = "Sabah alışveriş ve son gezinti için serbest zaman. Öğleden sonra havalimanına transfer ve dönüş." }
            });

            var roma = tours.First(t => t.Title.Contains("Roma"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = roma.TourId, DayNumber = 1, Title = "Varış ve Antik Merkez", Description = "Otele yerleşme sonrası Roma'nın kalbine ilk adım. Venedik Meydanı ve Kapitol Tepesi'nde akşam yürüyüşü." },
                new TourProgram { TourId = roma.TourId, DayNumber = 2, Title = "Kolezyum ve Roma Forumu", Description = "Antik dünyanın en görkemli arenası Kolezyum'un içinde tur. Ardından Roma Forumu ve Palatine Tepesi'nde imparatorluk kalıntıları." },
                new TourProgram { TourId = roma.TourId, DayNumber = 3, Title = "Vatikan Günü", Description = "Vatikan Müzeleri, Sistine Şapeli'nin tavan freskleri ve Aziz Petrus Bazilikası. Öğleden sonra Vatikan Meydanı'nda serbest zaman." },
                new TourProgram { TourId = roma.TourId, DayNumber = 4, Title = "Çeşmeler ve Meydanlar", Description = "Trevi Çeşmesi'ne dilek parası, İspanyol Merdivenleri ve Pantheon. Akşam Trastevere'de geleneksel İtalyan yemeği." },
                new TourProgram { TourId = roma.TourId, DayNumber = 5, Title = "Serbest Gün ve Dönüş", Description = "Son alışveriş ve kafede espresso keyfi için serbest sabah. Öğleden sonra havalimanı transferi ve dönüş." }
            });

            var atina = tours.First(t => t.Title.Contains("Atina"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = atina.TourId, DayNumber = 1, Title = "Varış ve Plaka", Description = "Otele yerleşme ve Atina'nın en eski mahallesi Plaka'da akşam yürüyüşü. Geleneksel taverna'da ilk akşam yemeği." },
                new TourProgram { TourId = atina.TourId, DayNumber = 2, Title = "Akropolis ve Partenon", Description = "Antik Yunan'ın simgesi Akropolis'e tırmanış, Partenon Tapınağı ve Akropolis Müzesi. Şehri tepeden izleyen eşsiz manzara." },
                new TourProgram { TourId = atina.TourId, DayNumber = 3, Title = "Antik Agora ve Müzeler", Description = "Antik Agora, Zeus Tapınağı ve Ulusal Arkeoloji Müzesi. Yunan mitolojisinin izinde dolu bir gün." },
                new TourProgram { TourId = atina.TourId, DayNumber = 4, Title = "Serbest Gün ve Dönüş", Description = "Monastiraki pazarında alışveriş için serbest zaman. Öğleden sonra havalimanına transfer ve dönüş." }
            });

            var kahire = tours.First(t => t.Title.Contains("Kahire"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = kahire.TourId, DayNumber = 1, Title = "Varış ve Nil Manzarası", Description = "Otele yerleşme ve Nil Nehri kıyısında akşam yürüyüşü. Şehrin hareketli atmosferine ilk bakış." },
                new TourProgram { TourId = kahire.TourId, DayNumber = 2, Title = "Giza Piramitleri", Description = "Dünyanın yedi harikasından Giza Piramitleri ve gizemli Sfenks. Çölde deve turu ve piramitlerin gölgesinde unutulmaz anlar." },
                new TourProgram { TourId = kahire.TourId, DayNumber = 3, Title = "Mısır Müzesi", Description = "Tutankamon hazineleri ve firavun mumyaları ile Mısır Müzesi. Antik uygarlığın büyüleyici eserleri." },
                new TourProgram { TourId = kahire.TourId, DayNumber = 4, Title = "Eski Kahire ve Çarşı", Description = "Han El-Halili çarşısında baharatlar ve el işleri. Tarihi camiler ve Kıpti Kahire'nin dar sokakları." },
                new TourProgram { TourId = kahire.TourId, DayNumber = 5, Title = "Nil Nehri Turu", Description = "Geleneksel felukka tekneleriyle Nil'de yelken. Akşam nehir üzerinde yemekli tekne turu ve gösteri." },
                new TourProgram { TourId = kahire.TourId, DayNumber = 6, Title = "Serbest Gün ve Dönüş", Description = "Son alışveriş için serbest sabah. Öğleden sonra havalimanına transfer ve dönüş." }
            });

            var istanbul = tours.First(t => t.Title.Contains("İstanbul"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = istanbul.TourId, DayNumber = 1, Title = "Sultanahmet Meydanı", Description = "Otele yerleşme sonrası Sultanahmet'te Ayasofya ve Sultanahmet Camii'nin ihtişamı. Akşam meydanda serbest zaman." },
                new TourProgram { TourId = istanbul.TourId, DayNumber = 2, Title = "Topkapı ve Kapalıçarşı", Description = "Osmanlı padişahlarının sarayı Topkapı ve hazine dairesi. Öğleden sonra Kapalıçarşı'nın labirent sokaklarında alışveriş." },
                new TourProgram { TourId = istanbul.TourId, DayNumber = 3, Title = "Boğaz ve Dönüş", Description = "Boğaz'da tekne turu, iki kıta arasında yolculuk. Öğleden sonra havalimanı transferi ve dönüş." }
            });

            var paris = tours.First(t => t.Title.Contains("Paris"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = paris.TourId, DayNumber = 1, Title = "Varış ve Eyfel", Description = "Otele yerleşme sonrası Eyfel Kulesi'ne çıkış ve Paris'i tepeden izleme. Akşam Seine Nehri kıyısında yürüyüş." },
                new TourProgram { TourId = paris.TourId, DayNumber = 2, Title = "Louvre ve Sanat", Description = "Mona Lisa'nın evi Louvre Müzesi'nde sanat şöleni. Öğleden sonra Tuileries Bahçeleri ve Şanzelize Bulvarı'nda gezinti." },
                new TourProgram { TourId = paris.TourId, DayNumber = 3, Title = "Montmartre ve Sacré-Cœur", Description = "Sanatçılar mahallesi Montmartre ve tepedeki Sacré-Cœur Bazilikası. Akşam Seine'de yemekli tekne turu." },
                new TourProgram { TourId = paris.TourId, DayNumber = 4, Title = "Serbest Gün ve Dönüş", Description = "Butiklerde alışveriş ve kafede kruvasan keyfi için serbest sabah. Öğleden sonra havalimanı transferi ve dönüş." }
            });

            var prag = tours.First(t => t.Title.Contains("Prag"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = prag.TourId, DayNumber = 1, Title = "Varış ve Eski Şehir", Description = "Otele yerleşme ve Eski Şehir Meydanı'nda akşam yürüyüşü. Ünlü Astronomik Saat'in gösterisini izleme." },
                new TourProgram { TourId = prag.TourId, DayNumber = 2, Title = "Prag Kalesi ve Charles Köprüsü", Description = "Tepedeki görkemli Prag Kalesi ve Aziz Vitus Katedrali. Öğleden sonra heykellerle süslü Charles Köprüsü'nde yürüyüş." },
                new TourProgram { TourId = prag.TourId, DayNumber = 3, Title = "Serbest Gün ve Dönüş", Description = "Geleneksel Çek biraevlerinde mola ve alışveriş. Öğleden sonra havalimanı transferi ve dönüş." }
            });

            var dubai = tours.First(t => t.Title.Contains("Dubai"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = dubai.TourId, DayNumber = 1, Title = "Varış ve Marina", Description = "Otele yerleşme sonrası Dubai Marina'da akşam yürüyüşü ve ışıltılı gökdelen manzarası." },
                new TourProgram { TourId = dubai.TourId, DayNumber = 2, Title = "Burj Khalifa", Description = "Dünyanın en yüksek binası Burj Khalifa'nın tepesinden manzara. Öğleden sonra Dubai Mall ve müzikli fıskiye gösterisi." },
                new TourProgram { TourId = dubai.TourId, DayNumber = 3, Title = "Çöl Safari", Description = "4x4 araçlarla kum tepelerinde safari, deve turu ve çöl kampında geleneksel akşam yemeği ile gösteriler." },
                new TourProgram { TourId = dubai.TourId, DayNumber = 4, Title = "Eski Dubai ve Altın Çarşı", Description = "Dubai'nin geleneksel yüzü: Al Fahidi tarihi mahallesi, abra tekneleriyle nehir geçişi ve Altın Çarşı." },
                new TourProgram { TourId = dubai.TourId, DayNumber = 5, Title = "Serbest Gün ve Dönüş", Description = "Lüks alışveriş merkezlerinde serbest zaman. Öğleden sonra havalimanı transferi ve dönüş." }
            });

            var londra = tours.First(t => t.Title.Contains("Londra"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = londra.TourId, DayNumber = 1, Title = "Varış ve Westminster", Description = "Otele yerleşme sonrası Big Ben, Parlamento Binası ve Westminster Köprüsü. Akşam Thames kıyısında yürüyüş." },
                new TourProgram { TourId = londra.TourId, DayNumber = 2, Title = "Saray ve Müzeler", Description = "Buckingham Sarayı'nda nöbet değişimi töreni. Öğleden sonra British Museum'un dünya hazineleri." },
                new TourProgram { TourId = londra.TourId, DayNumber = 3, Title = "Kule ve Köprü", Description = "Tower of London ve kraliyet mücevherleri, ikonik Tower Bridge. Akşam Soho'da tiyatro ve yemek." },
                new TourProgram { TourId = londra.TourId, DayNumber = 4, Title = "Serbest Gün ve Dönüş", Description = "Oxford Street'te alışveriş için serbest sabah. Öğleden sonra havalimanı transferi ve dönüş." }
            });

            var newyork = tours.First(t => t.Title.Contains("New York"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = newyork.TourId, DayNumber = 1, Title = "Varış ve Times Meydanı", Description = "Otele yerleşme sonrası ışıl ışıl Times Meydanı ve Broadway'in enerjisi. Şehrin nabzına ilk dokunuş." },
                new TourProgram { TourId = newyork.TourId, DayNumber = 2, Title = "Özgürlük Heykeli ve Manhattan", Description = "Feribotla Özgürlük Heykeli ve Ellis Adası. Öğleden sonra Wall Street ve One World gözlem terası." },
                new TourProgram { TourId = newyork.TourId, DayNumber = 3, Title = "Central Park ve Müzeler", Description = "Central Park'ta yürüyüş ve Metropolitan Sanat Müzesi. Fifth Avenue'da vitrin turu." },
                new TourProgram { TourId = newyork.TourId, DayNumber = 4, Title = "Empire State ve Brooklyn", Description = "Empire State Binası'nın tepesinden panorama. Brooklyn Köprüsü'nde yürüyüş ve DUMBO manzarası." },
                new TourProgram { TourId = newyork.TourId, DayNumber = 5, Title = "Müze ve Alışveriş", Description = "Modern Sanat Müzesi MoMA ve SoHo'da butik alışverişi. Akşam bir jazz kulübünde müzik." },
                new TourProgram { TourId = newyork.TourId, DayNumber = 6, Title = "Serbest Gün ve Dönüş", Description = "Son gezinti ve alışveriş için serbest zaman. Öğleden sonra havalimanı transferi ve dönüş." }
            });

            var tokyo = tours.First(t => t.Title.Contains("Tokyo"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = tokyo.TourId, DayNumber = 1, Title = "Varış ve Shibuya", Description = "Otele yerleşme sonrası dünyanın en yoğun yaya geçidi Shibuya ve rengarenk sokaklar." },
                new TourProgram { TourId = tokyo.TourId, DayNumber = 2, Title = "Tapınaklar ve Gelenek", Description = "Asakusa'daki Senso-ji Tapınağı ve geleneksel Nakamise çarşısı. İmparatorluk Sarayı bahçeleri." },
                new TourProgram { TourId = tokyo.TourId, DayNumber = 3, Title = "Modern Tokyo", Description = "Akihabara'nın teknoloji dünyası ve Tokyo Kulesi. Öğleden sonra Harajuku'nun sıra dışı modası." },
                new TourProgram { TourId = tokyo.TourId, DayNumber = 4, Title = "Balık Pazarı ve Bahçeler", Description = "Tsukiji dış pazarında taze suşi kahvaltısı. Shinjuku Gyoen bahçelerinde huzurlu bir yürüyüş." },
                new TourProgram { TourId = tokyo.TourId, DayNumber = 5, Title = "Manzara ve Kültür", Description = "Metropolitan Binası'ndan şehir manzarası ve Meiji Tapınağı'nın ormanlık huzuru." },
                new TourProgram { TourId = tokyo.TourId, DayNumber = 6, Title = "Alışveriş Günü", Description = "Ginza'nın lüks mağazaları ve Don Quijote'de hediyelik eşya avı." },
                new TourProgram { TourId = tokyo.TourId, DayNumber = 7, Title = "Serbest Gün ve Dönüş", Description = "Son keşifler için serbest sabah. Öğleden sonra havalimanı transferi ve dönüş." }
            });

            var maldivler = tours.First(t => t.Title.Contains("Maldivler"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = maldivler.TourId, DayNumber = 1, Title = "Varış ve Su Villası", Description = "Deniz uçağıyla adaya transfer ve su üstü villaya yerleşme. Turkuaz lagünde ilk yüzme ve gün batımı." },
                new TourProgram { TourId = maldivler.TourId, DayNumber = 2, Title = "Mercan Resifleri", Description = "Şnorkelle renkli mercan resifleri ve tropik balıklar arasında keşif. Öğleden sonra plajda dinlenme." },
                new TourProgram { TourId = maldivler.TourId, DayNumber = 3, Title = "Dalış Deneyimi", Description = "Uzman eşliğinde tüplü dalış ve derin mavinin canlıları. Akşam yıldızlar altında akşam yemeği." },
                new TourProgram { TourId = maldivler.TourId, DayNumber = 4, Title = "Ada Turu", Description = "Yerel bir adaya tekne turu ve Maldiv kültürü. Yunuslarla buluşma turu ve gün batımı yelkeni." },
                new TourProgram { TourId = maldivler.TourId, DayNumber = 5, Title = "Spa ve Dinlenme", Description = "Deniz üstü spa'da tropik masaj ve tam bir dinlenme günü. Serbest zaman ve plaj keyfi." },
                new TourProgram { TourId = maldivler.TourId, DayNumber = 6, Title = "Serbest Gün ve Dönüş", Description = "Son yüzme ve fotoğraf çekimi için serbest sabah. Deniz uçağıyla havalimanına transfer ve dönüş." }
            });

            var antalya = tours.First(t => t.Title.Contains("Antalya"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = antalya.TourId, DayNumber = 1, Title = "Varış ve Kaleiçi", Description = "Otele yerleşme sonrası tarihi Kaleiçi'nde yürüyüş, Hadrian Kapısı ve Yivli Minare. Yat limanında akşam." },
                new TourProgram { TourId = antalya.TourId, DayNumber = 2, Title = "Tekne Turu ve Koylar", Description = "Akdeniz'in saklı koylarında mavi yolculuk. Denize girme molaları ve tekne üzerinde öğle yemeği." },
                new TourProgram { TourId = antalya.TourId, DayNumber = 3, Title = "Şelaleler ve Antik Kentler", Description = "Düden Şelalesi ve antik Perge kalıntıları. Doğa ve tarih bir arada dolu bir gün." },
                new TourProgram { TourId = antalya.TourId, DayNumber = 4, Title = "Serbest Gün ve Dönüş", Description = "Plajda dinlenme ve çarşıda alışveriş için serbest zaman. Öğleden sonra havalimanı transferi ve dönüş." }
            });

            var bali = tours.First(t => t.Title.Contains("Bali"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = bali.TourId, DayNumber = 1, Title = "Varış ve Kuta", Description = "Otele yerleşme sonrası Kuta Plajı'nda gün batımı ve sahil boyunca akşam yürüyüşü." },
                new TourProgram { TourId = bali.TourId, DayNumber = 2, Title = "Ubud ve Pirinç Terasları", Description = "Tegallalang pirinç teraslarının yeşil basamakları ve Ubud maymun ormanı. Sanat köyleri ziyareti." },
                new TourProgram { TourId = bali.TourId, DayNumber = 3, Title = "Tapınaklar Rotası", Description = "Deniz üstündeki Tanah Lot Tapınağı ve Uluwatu'nun kayalık tapınağı. Geleneksel Kecak dans gösterisi." },
                new TourProgram { TourId = bali.TourId, DayNumber = 4, Title = "Volkan ve Sıcak Su", Description = "Kintamani'de Batur Yanardağı manzarası ve doğal kaplıcalarda dinlenme." },
                new TourProgram { TourId = bali.TourId, DayNumber = 5, Title = "Su Sporları", Description = "Nusa Dua'da şnorkel, jet ski ve deniz aktiviteleri. Öğleden sonra plajda serbest zaman." },
                new TourProgram { TourId = bali.TourId, DayNumber = 6, Title = "Spa ve Kültür", Description = "Geleneksel Bali masajı ve gümüş işçiliği atölyesi. Akşam yerel pazarında alışveriş." },
                new TourProgram { TourId = bali.TourId, DayNumber = 7, Title = "Serbest Gün ve Dönüş", Description = "Son plaj keyfi için serbest sabah. Öğleden sonra havalimanı transferi ve dönüş." }
            });

            var bodrum = tours.First(t => t.Title.Contains("Bodrum"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = bodrum.TourId, DayNumber = 1, Title = "Varış ve Marina", Description = "Yelkenliye yerleşme ve Bodrum Kalesi manzarası. Marina'da akşam yürüyüşü ve tekne konaklaması." },
                new TourProgram { TourId = bodrum.TourId, DayNumber = 2, Title = "Gökova Koyları", Description = "Gökova Körfezi'nin berrak koylarında yelken. İngiliz Limanı ve Sedir Adası'nda demirleme." },
                new TourProgram { TourId = bodrum.TourId, DayNumber = 3, Title = "Yedi Adalar", Description = "Yedi Adalar bölgesinde yüzme molaları ve şnorkel. Tekne üzerinde taze deniz ürünleri." },
                new TourProgram { TourId = bodrum.TourId, DayNumber = 4, Title = "Karaada ve Kaplıca", Description = "Karaada'nın şifalı çamur mağarası ve sıcak su kaynağı. Gün batımında yelken keyfi." },
                new TourProgram { TourId = bodrum.TourId, DayNumber = 5, Title = "Serbest Gün ve Dönüş", Description = "Bodrum çarşısında alışveriş için serbest zaman. Öğleden sonra transfer ve dönüş." }
            });

            var santorini = tours.First(t => t.Title.Contains("Santorini"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = santorini.TourId, DayNumber = 1, Title = "Varış ve Fira", Description = "Otele yerleşme sonrası başkent Fira'da yürüyüş ve kaldera manzarası. İlk gün batımı büyüsü." },
                new TourProgram { TourId = santorini.TourId, DayNumber = 2, Title = "Oia ve Gün Batımı", Description = "Beyaz evleri ve mavi kubbeleriyle ünlü Oia kasabası. Dünyanın en güzel gün batımına tanıklık." },
                new TourProgram { TourId = santorini.TourId, DayNumber = 3, Title = "Volkanik Adalar", Description = "Tekneyle volkanik ada turu ve sıcak su kaynaklarında yüzme. Kırmızı ve siyah kum plajları." },
                new TourProgram { TourId = santorini.TourId, DayNumber = 4, Title = "Serbest Gün ve Dönüş", Description = "Şarap tadımı ve butiklerde alışveriş için serbest zaman. Öğleden sonra transfer ve dönüş." }
            });

            var kapadokya = tours.First(t => t.Title.Contains("Kapadokya"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = kapadokya.TourId, DayNumber = 1, Title = "Varış ve Güvercinlik Vadisi", Description = "Otele yerleşme sonrası Güvercinlik Vadisi ve Uçhisar Kalesi manzarası. Gün batımında peri bacaları." },
                new TourProgram { TourId = kapadokya.TourId, DayNumber = 2, Title = "Balon ve Yeraltı Şehri", Description = "Gün doğumunda sıcak hava balonuyla vadilerin üzerinde eşsiz uçuş. Öğleden sonra Derinkuyu Yeraltı Şehri." },
                new TourProgram { TourId = kapadokya.TourId, DayNumber = 3, Title = "Göreme ve Dönüş", Description = "Göreme Açık Hava Müzesi'nin kaya kiliseleri ve çömlekçilik atölyesi. Öğleden sonra transfer ve dönüş." }
            });

            var isvicre = tours.First(t => t.Title.Contains("İsviçre"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = isvicre.TourId, DayNumber = 1, Title = "Varış ve Interlaken", Description = "Otele yerleşme ve iki göl arasındaki Interlaken'de yürüyüş. Alp manzarası eşliğinde ilk akşam." },
                new TourProgram { TourId = isvicre.TourId, DayNumber = 2, Title = "Jungfraujoch Zirvesi", Description = "Trenle Avrupa'nın en yüksek istasyonu Jungfraujoch'a çıkış. Buzul ve karlı zirvelerin nefes kesen manzarası." },
                new TourProgram { TourId = isvicre.TourId, DayNumber = 3, Title = "Göl ve Kamp", Description = "Brienz Gölü kıyısında doğa yürüyüşü ve göl kenarında kamp kurulumu. Yıldızlar altında ateş başı." },
                new TourProgram { TourId = isvicre.TourId, DayNumber = 4, Title = "Grindelwald Parkurları", Description = "Grindelwald köyünden başlayan dağ yürüyüş parkurları ve Alp çayırları. Şelaleler ve panoramik rotalar." },
                new TourProgram { TourId = isvicre.TourId, DayNumber = 5, Title = "Serbest Gün ve Dönüş", Description = "Alp köyünde dinlenme ve İsviçre çikolatası alışverişi. Öğleden sonra transfer ve dönüş." }
            });

            var karadeniz = tours.First(t => t.Title.Contains("Karadeniz"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = karadeniz.TourId, DayNumber = 1, Title = "Varış ve Ayder", Description = "Ayder Yaylası'na varış ve yayla evine yerleşme. Sisli yeşil tepeler arasında ilk akşam." },
                new TourProgram { TourId = karadeniz.TourId, DayNumber = 2, Title = "Şelaleler ve Yaylalar", Description = "Gelin Tülü Şelalesi ve yüksek yaylalarda doğa yürüyüşü. Bulutların üzerinde eşsiz manzaralar." },
                new TourProgram { TourId = karadeniz.TourId, DayNumber = 3, Title = "Fırtına Vadisi", Description = "Fırtına Deresi boyunca tarihi kemer köprüler ve Zilkale. Yemyeşil doğanın kalbinde bir gün." },
                new TourProgram { TourId = karadeniz.TourId, DayNumber = 4, Title = "Serbest Gün ve Dönüş", Description = "Yaylada dinlenme ve yöresel ürün alışverişi. Öğleden sonra transfer ve dönüş." }
            });

            var izlanda = tours.First(t => t.Title.Contains("İzlanda"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = izlanda.TourId, DayNumber = 1, Title = "Varış ve Reykjavik", Description = "Otele yerleşme ve başkent Reykjavik'te renkli evler, Hallgrimskirkja Kilisesi. Kuzey ışıkları için ilk gece." },
                new TourProgram { TourId = izlanda.TourId, DayNumber = 2, Title = "Altın Çember", Description = "Gullfoss Şelalesi, Geysir gayzerleri ve Thingvellir Milli Parkı. İzlanda'nın en ünlü doğa rotası." },
                new TourProgram { TourId = izlanda.TourId, DayNumber = 3, Title = "Buz Mağaraları", Description = "Vatnajökull buzulunda mavi buz mağaraları keşfi ve buzul yürüyüşü. Doğanın en saf hali." },
                new TourProgram { TourId = izlanda.TourId, DayNumber = 4, Title = "Şelaleler ve Kara Plaj", Description = "Seljalandsfoss ve Skogafoss şelaleleri, Reynisfjara kara kum plajı. Bazalt sütunların gizemi." },
                new TourProgram { TourId = izlanda.TourId, DayNumber = 5, Title = "Mavi Lagün", Description = "Ünlü Mavi Lagün jeotermal spa'da dinlenme ve şifalı sıcak sular. Kuzey ışıkları avı." },
                new TourProgram { TourId = izlanda.TourId, DayNumber = 6, Title = "Serbest Gün ve Dönüş", Description = "Reykjavik'te son alışveriş için serbest zaman. Öğleden sonra havalimanı transferi ve dönüş." }
            });

            var uludag = tours.First(t => t.Title.Contains("Uludağ"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = uludag.TourId, DayNumber = 1, Title = "Varış ve Pistler", Description = "Otele yerleşme ve kayak ekipmanı temini. Öğleden sonra başlangıç pistlerinde ilk kayak deneyimi." },
                new TourProgram { TourId = uludag.TourId, DayNumber = 2, Title = "Kayak ve Kar Keyfi", Description = "Profesyonel eğitmen eşliğinde kayak dersleri ve telesiyej turu. Karlı zirvelerde dolu bir gün." },
                new TourProgram { TourId = uludag.TourId, DayNumber = 3, Title = "Serbest Gün ve Dönüş", Description = "Son kayak keyfi ve şömine başında sıcak çikolata. Öğleden sonra transfer ve dönüş." }
            });

            var alpler = tours.First(t => t.Title.Contains("Alpler"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = alpler.TourId, DayNumber = 1, Title = "Varış ve Innsbruck", Description = "Otele yerleşme ve Innsbruck'un tarihi merkezi, Altın Çatı. Alp köyünün büyülü atmosferi." },
                new TourProgram { TourId = alpler.TourId, DayNumber = 2, Title = "Kayak Pistleri", Description = "Dünya çapında Nordkette pistlerinde kayak. Alp zirvelerinin ihtişamı eşliğinde kış sporları." },
                new TourProgram { TourId = alpler.TourId, DayNumber = 3, Title = "İleri Parkurlar", Description = "Deneyimli kayakçılar için zorlu parkurlar ve kızak pisti. Dağ restoranında geleneksel Avusturya yemeği." },
                new TourProgram { TourId = alpler.TourId, DayNumber = 4, Title = "Buz Sarayı ve Manzara", Description = "Teleferikle zirveye çıkış ve buzul manzarası. Öğleden sonra termal tesiste dinlenme." },
                new TourProgram { TourId = alpler.TourId, DayNumber = 5, Title = "Serbest Gün ve Dönüş", Description = "Alp köyünde alışveriş için serbest zaman. Öğleden sonra transfer ve dönüş." }
            });

            var laponya = tours.First(t => t.Title.Contains("Laponya"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = laponya.TourId, DayNumber = 1, Title = "Varış ve Rovaniemi", Description = "Otele yerleşme ve Noel Baba Köyü ziyareti. Kutup dairesi sınırında ilk kar deneyimi." },
                new TourProgram { TourId = laponya.TourId, DayNumber = 2, Title = "Ren Geyiği Çiftliği", Description = "Geleneksel ren geyiği kızağı turu ve Sami kültürü. Karlı ormanlarda büyülü bir yolculuk." },
                new TourProgram { TourId = laponya.TourId, DayNumber = 3, Title = "Husky Safari", Description = "Sibirya kurtlarının çektiği kızakla karlı ormanlarda safari. Öğleden sonra buz balıkçılığı deneyimi." },
                new TourProgram { TourId = laponya.TourId, DayNumber = 4, Title = "Kuzey Işıkları Avı", Description = "Kar motoruyla ışık kirliliğinden uzak noktalara yolculuk. Gökyüzünde dans eden auroraların büyüsü." },
                new TourProgram { TourId = laponya.TourId, DayNumber = 5, Title = "Buz Otel ve Kar Aktiviteleri", Description = "Ünlü buz otelini ziyaret ve kar heykelleri. Kar ayakkabısıyla doğa yürüyüşü." },
                new TourProgram { TourId = laponya.TourId, DayNumber = 6, Title = "Serbest Gün ve Dönüş", Description = "Hediyelik eşya alışverişi için serbest zaman. Öğleden sonra havalimanı transferi ve dönüş." }
            });

            var italyaGastro = tours.First(t => t.Title.Contains("İtalya Gastronomi"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = italyaGastro.TourId, DayNumber = 1, Title = "Varış ve Bologna", Description = "Otele yerleşme ve lezzetin başkenti Bologna'nın tarihi merkezi. İlk akşam geleneksel trattoria'da tadım." },
                new TourProgram { TourId = italyaGastro.TourId, DayNumber = 2, Title = "Makarna Atölyesi", Description = "Şef eşliğinde el yapımı taze makarna ve tortellini yapımı. Öğleden sonra yerel pazar turu." },
                new TourProgram { TourId = italyaGastro.TourId, DayNumber = 3, Title = "Parma ve Peynir", Description = "Parmigiano peyniri üretim çiftliği ve Parma jambonu tesisi. Gerçek İtalyan lezzetlerinin kaynağında." },
                new TourProgram { TourId = italyaGastro.TourId, DayNumber = 4, Title = "Şarap Bölgesi", Description = "Modena'da balzamik sirke üretimi ve bağ bahçelerinde şarap tadımı. Toskana lezzetleriyle akşam yemeği." },
                new TourProgram { TourId = italyaGastro.TourId, DayNumber = 5, Title = "Serbest Gün ve Dönüş", Description = "Yerel delikatesler ve alışveriş için serbest zaman. Öğleden sonra transfer ve dönüş." }
            });

            var gaziantep = tours.First(t => t.Title.Contains("Gaziantep"));
            programs.AddRange(new[]
            {
                new TourProgram { TourId = gaziantep.TourId, DayNumber = 1, Title = "Varış ve Bakırcılar Çarşısı", Description = "Otele yerleşme sonrası tarihi Bakırcılar Çarşısı ve el sanatları. İlk akşam meşhur Antep kebabı." },
                new TourProgram { TourId = gaziantep.TourId, DayNumber = 2, Title = "Baklava ve Mutfak", Description = "Geleneksel baklava atölyesi ve fıstık tarlaları. Öğleden sonra Zeugma Mozaik Müzesi'nin eşsiz eserleri." },
                new TourProgram { TourId = gaziantep.TourId, DayNumber = 3, Title = "Serbest Gün ve Dönüş", Description = "Yöresel baharatlar ve fıstık alışverişi için serbest zaman. Öğleden sonra transfer ve dönüş." }
            });

            await _tourProgramCollection.InsertManyAsync(programs);
        }

        private async Task SeedCommentsAsync(List<Tour> tours)
        {
            var existing = await _commentCollection.CountDocumentsAsync(FilterDefinition<Comment>.Empty);
            if (existing > 0)
                return;

            var now = DateTime.Now;
            var comments = new List<Comment>();

            var barcelona = tours.First(t => t.Title.Contains("Barselona"));
            comments.AddRange(new[]
            {
                new Comment { TourId = barcelona.TourId, NameSurname = "Ahmet Yılmaz", Email = "ahmet@example.com", Headline = "Muhteşem bir deneyim", CommentDetail = "Gaudí'nin eserlerini görmek hayatımın en güzel anılarından biri oldu. Rehberimiz çok bilgiliydi, her detayı anlattı. Kesinlikle tavsiye ederim.", Score = 5, CommentDate = now.AddDays(-40), IsStatus = true },
                new Comment { TourId = barcelona.TourId, NameSurname = "Zeynep Kaya", Email = "zeynep@example.com", Headline = "Harika organizasyon", CommentDetail = "Her şey planlandığı gibi ilerledi. Sagrada Familia nefes kesiciydi. Otel konumu da mükemmeldi, merkeze çok yakındı.", Score = 5, CommentDate = now.AddDays(-35), IsStatus = true },
                new Comment { TourId = barcelona.TourId, NameSurname = "Mehmet Demir", Email = "mehmet@example.com", Headline = "Güzeldi ama yoğundu", CommentDetail = "Program biraz sıkışıktı, dinlenmeye pek vakit kalmadı. Yine de gördüğümüz yerler çok değerliydi. Park Güell favorimdi.", Score = 4, CommentDate = now.AddDays(-28), IsStatus = true },
                new Comment { TourId = barcelona.TourId, NameSurname = "Elif Şahin", Email = "elif@example.com", Headline = "Tekrar giderim", CommentDetail = "Barselona'ya aşık oldum. Yemekler, mimari, deniz... Her şey harikaydı. Tur ekibine teşekkürler.", Score = 5, CommentDate = now.AddDays(-20), IsStatus = true },
                new Comment { TourId = barcelona.TourId, NameSurname = "Can Öztürk", Email = "can@example.com", Headline = "Beklentimi karşıladı", CommentDetail = "Fiyatına göre gayet iyi bir turdu. Gotik Mahalle turu çok keyifliydi. Sadece serbest zaman biraz daha olabilirdi.", Score = 4, CommentDate = now.AddDays(-12), IsStatus = true },
                new Comment { TourId = barcelona.TourId, NameSurname = "Deniz Arslan", Email = "deniz@example.com", Headline = "Yeni yorum", CommentDetail = "Turdan yeni döndüm, çok memnun kaldım. Detaylı yorumumu sonra yazacağım ama şimdiden herkese tavsiye ederim.", Score = 5, CommentDate = now.AddDays(-2), IsStatus = false }
            });

            var paris = tours.First(t => t.Title.Contains("Paris"));
            comments.AddRange(new[]
            {
                new Comment { TourId = paris.TourId, NameSurname = "Selin Yıldız", Email = "selin@example.com", Headline = "Aşkın şehri", CommentDetail = "Paris hayalimdi ve bu tur onu gerçeğe dönüştürdü. Eyfel Kulesi'nde gün batımı unutulmazdı. Herkese öneririm.", Score = 5, CommentDate = now.AddDays(-45), IsStatus = true },
                new Comment { TourId = paris.TourId, NameSurname = "Burak Çelik", Email = "burak@example.com", Headline = "Louvre muhteşem", CommentDetail = "Sanat sevenler için cennet. Louvre'da geçirdiğimiz süre yeterliydi. Rehberimiz Mona Lisa'nın hikayesini çok güzel anlattı.", Score = 5, CommentDate = now.AddDays(-38), IsStatus = true },
                new Comment { TourId = paris.TourId, NameSurname = "Gamze Aydın", Email = "gamze@example.com", Headline = "Romantik ve keyifli", CommentDetail = "Seine nehrinde tekne turu çok romantikti. Montmartre'ın sokakları büyüleyici. Küçük bir eksik otelin kahvaltısıydı.", Score = 4, CommentDate = now.AddDays(-25), IsStatus = true },
                new Comment { TourId = paris.TourId, NameSurname = "Onur Kılıç", Email = "onur@example.com", Headline = "Değerdi", CommentDetail = "Biraz pahalı ama Paris için değer. Şanzelize'de alışveriş, müzeler, her şey güzeldi. Ulaşım kolaydı.", Score = 4, CommentDate = now.AddDays(-15), IsStatus = true },
                new Comment { TourId = paris.TourId, NameSurname = "İrem Doğan", Email = "irem@example.com", Headline = "Onay bekliyor", CommentDetail = "Muhteşem bir tur, çok teşekkürler. Fotoğraflarımı düzenleyince tekrar geleceğim ama şimdilik 5 yıldız hak ediyor.", Score = 5, CommentDate = now.AddDays(-1), IsStatus = false }
            });

            var kapadokya = tours.First(t => t.Title.Contains("Kapadokya"));
            comments.AddRange(new[]
            {
                new Comment { TourId = kapadokya.TourId, NameSurname = "Fatma Yücel", Email = "fatma@example.com", Headline = "Balon turu inanılmazdı", CommentDetail = "Gün doğumunda balonla uçmak hayatımın en güzel deneyimiydi. Peri bacaları büyüleyici. Herkesin görmesi gereken bir yer.", Score = 4, CommentDate = now.AddDays(-33), IsStatus = true },
                new Comment { TourId = kapadokya.TourId, NameSurname = "Emre Aksoy", Email = "emre@example.com", Headline = "Güzel ama beklentim yüksekti", CommentDetail = "Yeraltı şehri ve Göreme güzeldi ama balon turu hava nedeniyle iptal oldu, bu beni üzdü. Otel iyiydi. Genel olarak ortalama bir deneyim.", Score = 3, CommentDate = now.AddDays(-22), IsStatus = true },
                new Comment { TourId = kapadokya.TourId, NameSurname = "Sıla Bulut", Email = "sila@example.com", Headline = "Güzel ama kısa", CommentDetail = "3 gün yetmedi, keşke daha uzun olsaydı. Yine de gördüğümüz her yer muhteşemdi. Rehberimiz çok ilgiliydi.", Score = 4, CommentDate = now.AddDays(-14), IsStatus = true },
                new Comment { TourId = kapadokya.TourId, NameSurname = "Kaan Erdoğan", Email = "kaan@example.com", Headline = "Değerlendirme", CommentDetail = "Genel olarak memnunum. Hava koşulları nedeniyle balon turu bir gün ertelendi ama ekip çok iyi yönetti durumu.", Score = 4, CommentDate = now.AddDays(-6), IsStatus = true }
            });

            var maldivler = tours.First(t => t.Title.Contains("Maldivler"));
            comments.AddRange(new[]
            {
                new Comment { TourId = maldivler.TourId, NameSurname = "Ayşe Korkmaz", Email = "ayse@example.com", Headline = "Cennet gibi", CommentDetail = "Su üstü villada uyanmak, turkuaz denizde yüzmek... Tam bir cennet. Balayı için mükemmel bir seçim oldu.", Score = 5, CommentDate = now.AddDays(-50), IsStatus = true },
                new Comment { TourId = maldivler.TourId, NameSurname = "Murat Şen", Email = "murats@example.com", Headline = "Dalış harikaydı", CommentDetail = "Mercan resifleri ve deniz canlıları inanılmazdı. Biraz pahalı bir tur ama sunduğu deneyim paha biçilemez. Kesinlikle tavsiye ederim.", Score = 5, CommentDate = now.AddDays(-30), IsStatus = true },
                new Comment { TourId = maldivler.TourId, NameSurname = "Pınar Güneş", Email = "pinar@example.com", Headline = "Lüks ve huzur", CommentDetail = "Spa, plaj, harika yemekler. Dinlenmek isteyenler için ideal. Sadece adaya ulaşım biraz uzun sürdü ama sorun değil.", Score = 4, CommentDate = now.AddDays(-18), IsStatus = true }
            });

            var roma = tours.First(t => t.Title.Contains("Roma"));
            comments.AddRange(new[]
            {
                new Comment { TourId = roma.TourId, NameSurname = "Serkan Yalçın", Email = "serkan@example.com", Headline = "Tarih dolu", CommentDetail = "Kolezyum'un içinde olmak tarihe dokunmak gibiydi. Vatikan muhteşemdi. Kalabalıktı ama beklendiği gibi.", Score = 5, CommentDate = now.AddDays(-42), IsStatus = true },
                new Comment { TourId = roma.TourId, NameSurname = "Büşra Aktaş", Email = "busra@example.com", Headline = "İyiydi", CommentDetail = "Roma çok güzel bir şehir. Program yoğundu, çok yürüdük. Rahat ayakkabı şart. Trevi Çeşmesi favorimdi.", Score = 4, CommentDate = now.AddDays(-24), IsStatus = true },
                new Comment { TourId = roma.TourId, NameSurname = "Tolga Kurt", Email = "tolga@example.com", Headline = "Ortalama", CommentDetail = "Gördüğümüz yerler güzeldi ama otel biraz merkezden uzaktı. Yemekler beklediğimden vasattı. Yine de fena değildi.", Score = 3, CommentDate = now.AddDays(-16), IsStatus = true }
            });

            var dubai = tours.First(t => t.Title.Contains("Dubai"));
            comments.AddRange(new[]
            {
                new Comment { TourId = dubai.TourId, NameSurname = "Ece Polat", Email = "ece@example.com", Headline = "Görkemli şehir", CommentDetail = "Burj Khalifa'dan manzara nefes kesiciydi. Çöl safarisi çok eğlenceliydi. Modern ve lüks bir deneyim.", Score = 5, CommentDate = now.AddDays(-36), IsStatus = true },
                new Comment { TourId = dubai.TourId, NameSurname = "Umut Taş", Email = "umut@example.com", Headline = "Çöl safarisi süperdi", CommentDetail = "Kum tepelerinde safari ve akşam kamp muhteşemdi. Alışveriş merkezleri devasa. Sıcak biraz bunaltıcıydı ama genel olarak harika.", Score = 4, CommentDate = now.AddDays(-21), IsStatus = true },
                new Comment { TourId = dubai.TourId, NameSurname = "Merve Çakır", Email = "merve@example.com", Headline = "Onay bekliyor", CommentDetail = "Dubai beklentilerimin ötesindeydi. Detaylı yorum yazacağım ama şimdilik çok memnun kaldığımı söyleyebilirim.", Score = 4, CommentDate = now.AddDays(-3), IsStatus = false }
            });

            var istanbul = tours.First(t => t.Title.Contains("İstanbul"));
            comments.AddRange(new[]
            {
                new Comment { TourId = istanbul.TourId, NameSurname = "Hakan Şimşek", Email = "hakan@example.com", Headline = "İki kıtanın şehri", CommentDetail = "Ayasofya ve Topkapı muhteşemdi. Boğaz turu şehri farklı bir açıdan gösterdi. Kısa ama dolu dolu bir turdu.", Score = 5, CommentDate = now.AddDays(-27), IsStatus = true },
                new Comment { TourId = istanbul.TourId, NameSurname = "Nur Aslan", Email = "nur@example.com", Headline = "Tarih ve lezzet", CommentDetail = "Kapalıçarşı'da alışveriş çok keyifliydi. Yemekler harikaydı. Trafik biraz yorucuydu ama İstanbul bu, normal.", Score = 4, CommentDate = now.AddDays(-11), IsStatus = true }
            });

            var prag = tours.First(t => t.Title.Contains("Prag"));
            comments.AddRange(new[]
            {
                new Comment { TourId = prag.TourId, NameSurname = "Cem Yavuz", Email = "cem@example.com", Headline = "Masal şehri", CommentDetail = "Prag gerçekten masalsı bir yer. Charles Köprüsü ve kale çok etkileyiciydi. Kısa ama güzel bir kaçamaktı.", Score = 5, CommentDate = now.AddDays(-31), IsStatus = true },
                new Comment { TourId = prag.TourId, NameSurname = "Dilan Acar", Email = "dilan@example.com", Headline = "Keyifliydi", CommentDetail = "Eski şehir meydanı ve astronomik saat çok güzeldi. Hava biraz soğuktu ama şehir buna değerdi.", Score = 4, CommentDate = now.AddDays(-13), IsStatus = true }
            });

            var bali = tours.First(t => t.Title.Contains("Bali"));
            comments.AddRange(new[]
            {
                new Comment { TourId = bali.TourId, NameSurname = "Yasemin Ünal", Email = "yasemin@example.com", Headline = "Tropik cennet", CommentDetail = "Pirinç terasları ve tapınaklar büyüleyiciydi. Bali'nin ruhani atmosferi çok huzur vericiydi. Uzun ama dolu dolu bir tur.", Score = 4, CommentDate = now.AddDays(-29), IsStatus = true },
                new Comment { TourId = bali.TourId, NameSurname = "Berk Şahin", Email = "berk@example.com", Headline = "Güzel deneyim", CommentDetail = "Plajlar, tapınaklar, doğa... Her şey vardı. Uçuş uzundu ama Bali için değer. Yerel yemekler çok lezzetliydi.", Score = 4, CommentDate = now.AddDays(-10), IsStatus = true }
            });

            var antalya = tours.First(t => t.Title.Contains("Antalya"));
            comments.AddRange(new[]
            {
                new Comment { TourId = antalya.TourId, NameSurname = "Gökhan Er", Email = "gokhan@example.com", Headline = "Deniz ve tarih", CommentDetail = "Kaleiçi çok şirin, tekne turu harikaydı. Akdeniz'in koyları muhteşem. Ailecek çok keyif aldık.", Score = 5, CommentDate = now.AddDays(-19), IsStatus = true }
            });

            var gaziantep = tours.First(t => t.Title.Contains("Gaziantep"));
            comments.AddRange(new[]
            {
                new Comment { TourId = gaziantep.TourId, NameSurname = "Sevgi Doğan", Email = "sevgi@example.com", Headline = "Lezzet cenneti", CommentDetail = "Baklava ve kebaplar inanılmazdı. Zeugma müzesi bonus oldu. Yemek sevenler için kaçırılmayacak bir tur.", Score = 5, CommentDate = now.AddDays(-17), IsStatus = true },
                new Comment { TourId = gaziantep.TourId, NameSurname = "Alp Kaya", Email = "alp@example.com", Headline = "Doydum ve mutlu döndüm", CommentDetail = "Antep mutfağı efsane. Her öğün ziyafet gibiydi. Çarşı gezisi de çok keyifliydi. Kesinlikle tavsiye ederim.", Score = 5, CommentDate = now.AddDays(-7), IsStatus = true }
            });

            var santorini = tours.First(t => t.Title.Contains("Santorini"));
            comments.AddRange(new[]
            {
                new Comment { TourId = santorini.TourId, NameSurname = "Melis Yıldırım", Email = "melis@example.com", Headline = "Gün batımı büyülü", CommentDetail = "Oia'da gün batımı hayatımda gördüğüm en güzel manzaraydı. Beyaz evler, mavi kubbeler... Rüya gibiydi.", Score = 5, CommentDate = now.AddDays(-23), IsStatus = true }
            });

            var isvicre = tours.First(t => t.Title.Contains("İsviçre"));
            comments.AddRange(new[]
            {
                new Comment { TourId = isvicre.TourId, NameSurname = "Barış Aydın", Email = "baris@example.com", Headline = "Doğa muhteşem", CommentDetail = "Alp dağları ve göller nefes kesiciydi. Jungfraujoch zirvesi unutulmazdı. Kamp deneyimi çok keyifliydi. Biraz pahalı ama değer.", Score = 5, CommentDate = now.AddDays(-26), IsStatus = true }
            });

            var laponya = tours.First(t => t.Title.Contains("Laponya"));
            comments.AddRange(new[]
            {
                new Comment { TourId = laponya.TourId, NameSurname = "Ceren Öz", Email = "ceren@example.com", Headline = "Kuzey ışıkları rüya gibi", CommentDetail = "Auroraları görmek hayalimin gerçekleşmesiydi. Husky safari ve ren geyiği kızağı çok eğlenceliydi. Soğuğa değdi.", Score = 5, CommentDate = now.AddDays(-9), IsStatus = true },
                new Comment { TourId = laponya.TourId, NameSurname = "Ozan Demir", Email = "ozan@example.com", Headline = "Onay bekliyor", CommentDetail = "Laponya bir kış masalı gibiydi. Detaylı yorumumu yazacağım ama şimdilik herkese şiddetle tavsiye ederim.", Score = 5, CommentDate = now.AddDays(-2), IsStatus = false }
            });

            await _commentCollection.InsertManyAsync(comments);
        }

        private async Task SeedReservationsAsync(List<Tour> tours)
        {
            var existing = await _reservationCollection.CountDocumentsAsync(FilterDefinition<Reservation>.Empty);
            if (existing > 0)
                return;

            var now = DateTime.Now;
            var random = new Random(42);

            var firstNames = new[] { "Ahmet", "Mehmet", "Ayşe", "Fatma", "Ali", "Zeynep", "Mustafa", "Emine", "Hüseyin", "Hatice", "İbrahim", "Elif", "Emre", "Merve", "Burak", "Selin", "Cem", "Deniz", "Onur", "Ece", "Kaan", "Gamze", "Serkan", "Büşra", "Tolga", "Pınar", "Barış", "Ceren", "Ozan", "Melis" };
            var lastNames = new[] { "Yılmaz", "Kaya", "Demir", "Şahin", "Çelik", "Yıldız", "Yıldırım", "Öztürk", "Aydın", "Özdemir", "Arslan", "Doğan", "Kılıç", "Aslan", "Çetin", "Kara", "Koç", "Kurt", "Özkan", "Şimşek", "Polat", "Korkmaz", "Aksoy", "Erdoğan", "Güneş" };

            var statuses = new[] { ReservationStatuses.Approved, ReservationStatuses.Approved, ReservationStatuses.Approved, ReservationStatuses.Pending, ReservationStatuses.Cancelled };

            var reservations = new List<Reservation>();

            foreach (var tour in tours)
            {
                if (!tour.IsStatus)
                    continue;

                int reservationCount = random.Next(2, 9);

                for (int i = 0; i < reservationCount; i++)
                {
                    var name = firstNames[random.Next(firstNames.Length)];
                    var surname = lastNames[random.Next(lastNames.Length)];
                    var personCount = random.Next(1, 6);
                    var status = statuses[random.Next(statuses.Length)];
                    var daysAgo = random.Next(1, 60);

                    reservations.Add(new Reservation
                    {
                        TourId = tour.TourId,
                        Name = name,
                        Surname = surname,
                        Email = name.ToLower() + "." + surname.ToLower() + "@example.com",
                        Phone = "05" + random.Next(300000000, 599999999).ToString(),
                        ReservationDate = tour.TourDate,
                        PersonCount = personCount,
                        Status = status,
                        CreatedDate = now.AddDays(-daysAgo)
                    });
                }
            }

            // Prag turunu kapasiteye kadar doldur (dolu tur testi için)
            var pragTour = tours.First(t => t.Title.Contains("Prag"));
            int pragApproved = reservations.Where(r => r.TourId == pragTour.TourId && r.Status == ReservationStatuses.Approved).Sum(r => r.PersonCount);
            int remaining = pragTour.Capacity - pragApproved;

            while (remaining > 0)
            {
                int pc = Math.Min(remaining, random.Next(1, 5));
                reservations.Add(new Reservation
                {
                    TourId = pragTour.TourId,
                    Name = firstNames[random.Next(firstNames.Length)],
                    Surname = lastNames[random.Next(lastNames.Length)],
                    Email = "dolu" + remaining + "@example.com",
                    Phone = "05" + random.Next(300000000, 599999999).ToString(),
                    ReservationDate = pragTour.TourDate,
                    PersonCount = pc,
                    Status = ReservationStatuses.Approved,
                    CreatedDate = now.AddDays(-random.Next(1, 30))
                });
                remaining -= pc;
            }

            await _reservationCollection.InsertManyAsync(reservations);
        }
    }
}