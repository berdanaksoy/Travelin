namespace Travelin.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendReservationApprovedEmailAsync(string toEmail, string customerName, string tourTitle, DateTime tourDate, int personCount);
        Task SendReservationCancelledEmailAsync(string toEmail, string customerName, string tourTitle, DateTime tourDate);
    }
}