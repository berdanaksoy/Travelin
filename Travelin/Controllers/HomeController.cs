using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Travelin.Dtos.ContactDtos;
using Travelin.Models;
using Travelin.Services.CategoryServices;
using Travelin.Services.CommentServices;
using Travelin.Services.EmailServices;
using Travelin.Services.SiteSettingServices;
using Travelin.Services.TourServices;

namespace Travelin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITourService _tourService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;
        private readonly IEmailService _emailService;
        private readonly ISiteSettingService _siteSettingService;

        public HomeController(ITourService tourService, ICategoryService categoryService, ICommentService commentService, IEmailService emailService, ISiteSettingService siteSettingService)
        {
            _tourService = tourService;
            _categoryService = categoryService;
            _commentService = commentService;
            _emailService = emailService;
            _siteSettingService = siteSettingService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index()
        {
            var featuredTours = await _tourService.GetToursByPageAsync(1, 6);

            foreach (var tour in featuredTours)
            {
                var rating = await _commentService.GetTourRatingAsync(tour.TourId);
                tour.AverageRating = rating.average;
                tour.CommentCount = rating.count;
            }

            var model = new HomeViewModel
            {
                FeaturedTours = featuredTours,
                Categories = await _categoryService.GetActiveCategoriesAsync(),
                FeaturedCategories = await _categoryService.GetRandomActiveCategoriesAsync(6),
                TopComments = await _commentService.GetTopRatedCommentsAsync(6)
            };

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            var settings = await _siteSettingService.GetSiteSettingAsync();
            ViewBag.Address = settings?.Address;
            ViewBag.Phone = settings?.Phone;
            ViewBag.Email = settings?.Email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactMessageDto dto)
        {
            if (!ModelState.IsValid)
            {
                var settings = await _siteSettingService.GetSiteSettingAsync();
                ViewBag.Address = settings?.Address;
                ViewBag.Phone = settings?.Phone;
                ViewBag.Email = settings?.Email;
                return View(dto);
            }

            try
            {
                await _emailService.SendContactMessageAsync(dto);
                TempData["ContactSuccess"] = "Mesajınız başarıyla gönderildi. En kısa sürede size dönüş yapacağız.";
            }
            catch (Exception ex)
            {
                TempData["ContactError"] = "Mesaj gönderilirken bir hata oluştu.";
            }
            return RedirectToAction("Contact");
        }
    }
}