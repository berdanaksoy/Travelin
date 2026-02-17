using Microsoft.AspNetCore.Mvc;
using Travelin.Dtos.TourDtos;
using Travelin.Services.TourServices;

namespace Travelin.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        public IActionResult CreateTour()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(CreateTourDto createTourDto)
        {
            await _tourService.CreateTourAsync(createTourDto);

            return RedirectToAction("TourList");
        }

        public async Task<IActionResult> TourList()
        {
            var values = await _tourService.GetAllTourAsync();

            return View(values);
        }
    }
}
