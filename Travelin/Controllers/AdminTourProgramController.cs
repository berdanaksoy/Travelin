using Microsoft.AspNetCore.Mvc;
using Travelin.Dtos.TourDtos;
using Travelin.Dtos.TourProgramDtos;
using Travelin.Services.TourProgramServices;
using Travelin.Services.TourServices;

namespace Travelin.Controllers
{
    public class AdminTourProgramController : Controller
    {
        private readonly ITourProgramService _tourProgramService;
        private readonly ITourService _tourService;

        public AdminTourProgramController(ITourProgramService tourProgramService, ITourService tourService)
        {
            _tourProgramService = tourProgramService;
            _tourService = tourService;
        }

        public async Task<IActionResult> ProgramTourList(string search, int page = 1)
        {
            var filter = new TourFilterDto
            {
                Search = search,
                Page = page < 1 ? 1 : page,
                PageSize = 10
            };

            var result = await _tourService.GetFilteredToursAsync(filter);
            var tours = result.Tours;

            var programCounts = new Dictionary<string, int>();
            foreach (var tour in tours)
            {
                var programs = await _tourProgramService.GetTourProgramsByTourIdAsync(tour.TourId);
                programCounts[tour.TourId] = programs.Count;
            }

            ViewBag.ProgramCounts = programCounts;
            ViewBag.Search = search;
            ViewBag.Filter = filter;
            ViewBag.CurrentPage = filter.Page;
            ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / (double)filter.PageSize);
            ViewBag.TotalCount = result.TotalCount;

            ViewBag.PaginationBaseUrl = Url.Action("ProgramTourList", "AdminTourProgram");
            ViewBag.PaginationParams = new Dictionary<string, string>
    {
        { "search", search }
    };

            return View(tours);
        }

        public async Task<IActionResult> ManageProgram(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("ProgramTourList");

            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
                return RedirectToAction("ProgramTourList");

            var programs = await _tourProgramService.GetTourProgramsByTourIdAsync(id);

            ViewBag.Tour = tour;
            return View(programs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProgram(CreateTourProgramDto createTourProgramDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                TempData["ProgramError"] = errors;
                return RedirectToAction("ManageProgram", new { id = createTourProgramDto.TourId });
            }

            var existing = await _tourProgramService.GetTourProgramsByTourIdAsync(createTourProgramDto.TourId);
            int expectedDay = existing.Count + 1;

            if (createTourProgramDto.DayNumber != expectedDay)
            {
                TempData["ProgramError"] = $"Sıradaki gün {expectedDay} olmalıdır. Günler sırayla eklenmelidir.";
                return RedirectToAction("ManageProgram", new { id = createTourProgramDto.TourId });
            }

            await _tourProgramService.CreateTourProgramAsync(createTourProgramDto);
            TempData["Success"] = "Program günü başarıyla eklendi.";
            return RedirectToAction("ManageProgram", new { id = createTourProgramDto.TourId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProgram(UpdateTourProgramDto updateTourProgramDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                TempData["ProgramError"] = errors;
                return RedirectToAction("ManageProgram", new { id = updateTourProgramDto.TourId });
            }

            var existing = await _tourProgramService.GetTourProgramsByTourIdAsync(updateTourProgramDto.TourId);
            if (existing.Any(p => p.DayNumber == updateTourProgramDto.DayNumber
                               && p.TourProgramId != updateTourProgramDto.TourProgramId))
            {
                TempData["ProgramError"] = updateTourProgramDto.DayNumber + ". gün zaten tanımlı.";
                return RedirectToAction("ManageProgram", new { id = updateTourProgramDto.TourId });
            }

            await _tourProgramService.UpdateTourProgramAsync(updateTourProgramDto);
            TempData["Success"] = "Program günü başarıyla güncellendi.";
            return RedirectToAction("ManageProgram", new { id = updateTourProgramDto.TourId });
        }

        public async Task<IActionResult> DeleteProgram(string id, string tourId)
        {
            await _tourProgramService.DeleteTourProgramAsync(id);
            TempData["Success"] = "Program günü başarıyla silindi.";
            return RedirectToAction("ManageProgram", new { id = tourId });
        }
    }
}