using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Travelin.Services.ReservationServices;
using Travelin.Services.TourServices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Travelin.Controllers
{
    public class AdminCustomerController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ITourService _tourService;

        public AdminCustomerController(IReservationService reservationService, ITourService tourService)
        {
            _reservationService = reservationService;
            _tourService = tourService;
        }

        public async Task<IActionResult> TourCustomers(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("TourList", "AdminTour");

            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
                return RedirectToAction("TourList", "AdminTour");

            var reservations = await _reservationService.GetApprovedReservationsByTourIdAsync(id);

            ViewBag.Tour = tour;
            return View(reservations);
        }

        public async Task<IActionResult> ExportExcel(string id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            var reservations = await _reservationService.GetApprovedReservationsByTourIdAsync(id);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Müşteriler");

            worksheet.Cell(1, 1).Value = "Ad Soyad";
            worksheet.Cell(1, 2).Value = "E-posta";
            worksheet.Cell(1, 3).Value = "Telefon";
            worksheet.Cell(1, 4).Value = "Kişi Sayısı";

            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#16a34a");
            headerRow.Style.Font.FontColor = XLColor.White;

            int row = 2;
            foreach (var reservation in reservations)
            {
                worksheet.Cell(row, 1).Value = reservation.Name + " " + reservation.Surname;
                worksheet.Cell(row, 2).Value = reservation.Email;
                worksheet.Cell(row, 3).Value = reservation.Phone;
                worksheet.Cell(row, 4).Value = reservation.PersonCount;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"{tour.Title}_musteriler.xlsx";

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public async Task<IActionResult> ExportPdf(string id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            var reservations = await _reservationService.GetApprovedReservationsByTourIdAsync(id);

            int totalPeople = reservations.Sum(r => r.PersonCount);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Tura Kayıtlı Müşteriler").FontSize(20).Bold().FontColor("#16a34a");
                        col.Item().Text(tour.Title).FontSize(13).FontColor("#4a5568");
                        col.Item().Text($"Tarih: {tour.TourDate:dd MMMM yyyy}  •  Toplam: {reservations.Count} kayıt / {totalPeople} kişi")
                            .FontSize(10).FontColor("#718096");
                    });

                    page.Content().PaddingVertical(15).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background("#16a34a").Padding(6).AlignCenter().Text("✓").FontColor("#ffffff").Bold();
                            header.Cell().Background("#16a34a").Padding(6).Text("Ad Soyad").FontColor("#ffffff").Bold();
                            header.Cell().Background("#16a34a").Padding(6).Text("E-posta").FontColor("#ffffff").Bold();
                            header.Cell().Background("#16a34a").Padding(6).Text("Telefon").FontColor("#ffffff").Bold();
                            header.Cell().Background("#16a34a").Padding(6).Text("Kişi").FontColor("#ffffff").Bold();
                        });

                        foreach (var r in reservations)
                        {
                            table.Cell().BorderBottom(1).BorderColor("#e2e8f0").Padding(6).AlignCenter().Element(e => e.Width(14).Height(14).Border(1).BorderColor("#4a5568"));
                            table.Cell().BorderBottom(1).BorderColor("#e2e8f0").Padding(6).Text(r.Name + " " + r.Surname);
                            table.Cell().BorderBottom(1).BorderColor("#e2e8f0").Padding(6).Text(r.Email);
                            table.Cell().BorderBottom(1).BorderColor("#e2e8f0").Padding(6).Text(r.Phone);
                            table.Cell().BorderBottom(1).BorderColor("#e2e8f0").Padding(6).Text(r.PersonCount.ToString());
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Travelin — ").FontColor("#a0aec0").FontSize(9);
                        x.Span($"{DateTime.Now:dd.MM.yyyy HH:mm}").FontColor("#a0aec0").FontSize(9);
                    });
                });
            });

            var pdfBytes = document.GeneratePdf();
            var fileName = $"{tour.Title}_musteriler.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}