using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> ProgramTourList(string search)
        {
            var tours = await _tourService.GetAllTourAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                tours = tours.Where(t =>
                    (t.Title != null && t.Title.ToLower().Contains(search.ToLower())) ||
                    (t.City != null && t.City.ToLower().Contains(search.ToLower())) ||
                    (t.Country != null && t.Country.ToLower().Contains(search.ToLower()))
                ).ToList();
            }

            var programCounts = new Dictionary<string, int>();
            foreach (var tour in tours)
            {
                var programs = await _tourProgramService.GetTourProgramsByTourIdAsync(tour.TourId);
                programCounts[tour.TourId] = programs.Count;
            }

            ViewBag.ProgramCounts = programCounts;
            ViewBag.Search = search;
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
            return RedirectToAction("ManageProgram", new { id = updateTourProgramDto.TourId });
        }

        public async Task<IActionResult> DeleteProgram(string id, string tourId)
        {
            await _tourProgramService.DeleteTourProgramAsync(id);
            return RedirectToAction("ManageProgram", new { id = tourId });
        }
    }
}