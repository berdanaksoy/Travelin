using Microsoft.AspNetCore.Mvc;
using Travelin.Entities;
using Travelin.Services.EmailServices;
using Travelin.Services.ReservationServices;
using Travelin.Services.TourServices;

namespace Travelin.Controllers
{
    public class AdminReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ITourService _tourService;
        private readonly IEmailService _emailService;

        public AdminReservationController(IReservationService reservationService, ITourService tourService, IEmailService emailService)
        {
            _reservationService = reservationService;
            _tourService = tourService;
            _emailService = emailService;
        }

        public async Task<IActionResult> ReservationList(string status)
        {
            var reservations = await _reservationService.GetAllReservationAsync();

            if (!string.IsNullOrEmpty(status))
                reservations = reservations.Where(r => r.Status == status).ToList();

            var tours = await _tourService.GetAllTourAsync();
            var capacityInfo = new Dictionary<string, string>();

            foreach (var reservation in reservations)
            {
                var tour = tours.FirstOrDefault(t => t.TourId == reservation.TourId);
                reservation.TourTitle = tour?.Title ?? "-";

                if (tour != null && !capacityInfo.ContainsKey(tour.TourId))
                {
                    var approved = await _reservationService.GetApprovedPersonCountByTourIdAsync(tour.TourId);
                    capacityInfo[tour.TourId] = approved + " / " + tour.Capacity;
                }
            }

            ViewBag.CapacityInfo = capacityInfo;

            var allReservations = await _reservationService.GetAllReservationAsync();
            ViewBag.PendingCount = allReservations.Count(r => r.Status == ReservationStatuses.Pending);
            ViewBag.Status = status;

            return View(reservations);
        }

        public async Task<IActionResult> ApproveReservation(string id, string status)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            var tour = await _tourService.GetTourByIdAsync(reservation.TourId);

            var approvedCount = await _reservationService.GetApprovedPersonCountByTourIdAsync(reservation.TourId);

            if (approvedCount + reservation.PersonCount > tour.Capacity)
            {
                TempData["Error"] = $"Kapasite aşılıyor. Onaylı: {approvedCount}/{tour.Capacity}, bu talep: {reservation.PersonCount} kişi.";
                return RedirectToAction("ReservationList", new { status });
            }

            await _reservationService.ChangeReservationStatusAsync(id, ReservationStatuses.Approved);

            try
            {
                await _emailService.SendReservationApprovedEmailAsync(
                    reservation.Email,
                    reservation.Name + " " + reservation.Surname,
                    tour.Title,
                    reservation.ReservationDate,
                    reservation.PersonCount
                );
                TempData["Success"] = "Rezervasyon onaylandı ve müşteriye e-posta gönderildi.";
            }
            catch
            {
                TempData["Error"] = "Rezervasyon onaylandı ancak e-posta gönderilemedi. Müşteriye kendiniz ulaşmanız gerekebilir.";
            }

            return RedirectToAction("ReservationList", new { status });
        }

        public async Task<IActionResult> CancelReservation(string id, string status)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            var tour = await _tourService.GetTourByIdAsync(reservation.TourId);

            await _reservationService.ChangeReservationStatusAsync(id, ReservationStatuses.Cancelled);

            try
            {
                await _emailService.SendReservationCancelledEmailAsync(
                    reservation.Email,
                    reservation.Name + " " + reservation.Surname,
                    tour.Title,
                    reservation.ReservationDate
                );
                TempData["Success"] = "Rezervasyon iptal edildi ve müşteriye e-posta gönderildi.";
            }
            catch
            {
                TempData["Error"] = "Rezervasyon iptal edildi ancak e-posta gönderilemedi. Müşteriye kendiniz ulaşmanız gerekebilir.";
            }

            return RedirectToAction("ReservationList", new { status });
        }
    }
}