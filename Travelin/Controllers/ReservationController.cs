using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Travelin.Dtos.ReservationDtos;
using Travelin.Models;
using Travelin.Services.ReservationServices;
using Travelin.Services.TourServices;

namespace Travelin.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ITourService _tourService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ReservationController(IReservationService reservationService, ITourService tourService)
        {
            _reservationService = reservationService;
            _tourService = tourService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateReservation(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("TourList", "Tour");

            var tour = await _tourService.GetTourByIdAsync(id);

            if (tour == null)
                return RedirectToAction("TourList", "Tour");

            if (tour.TourDate.Date < DateTime.Now.Date)
            {
                TempData["ReservationError"] = _localizer["TourExpiredError"].Value;
                return RedirectToAction("Detail", "Tour", new { id });
            }

            var approvedCount = await _reservationService.GetApprovedPersonCountByTourIdAsync(id);
            if (approvedCount >= tour.Capacity)
            {
                TempData["ReservationError"] = _localizer["TourFullError"].Value;
                return RedirectToAction("Detail", "Tour", new { id });
            }

            var model = new CreateReservationViewModel
            {
                Tour = tour,
                Reservation = new CreateReservationDto
                {
                    TourId = id,
                    PersonCount = 1,
                    ReservationDate = tour.TourDate
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Tour = await _tourService.GetTourByIdAsync(model.Reservation.TourId);
                return View(model);
            }

            var tour = await _tourService.GetTourByIdAsync(model.Reservation.TourId);

            if (tour == null)
                return RedirectToAction("TourList", "Tour");

            if (tour.TourDate.Date < DateTime.Now.Date)
            {
                TempData["ReservationError"] = _localizer["TourExpiredError"].Value;
                return RedirectToAction("Detail", "Tour", new { id = model.Reservation.TourId });
            }

            var approvedCount = await _reservationService.GetApprovedPersonCountByTourIdAsync(model.Reservation.TourId);
            if (approvedCount + model.Reservation.PersonCount > tour.Capacity)
            {
                TempData["ReservationError"] = _localizer["TourFullError"].Value;
                return RedirectToAction("Detail", "Tour", new { id = model.Reservation.TourId });
            }

            await _reservationService.CreateReservationAsync(model.Reservation);
            TempData["ReservationSuccess"] = true;
            return RedirectToAction("Detail", "Tour", new { id = model.Reservation.TourId });
        }
    }
}