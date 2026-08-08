using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Travelin.Dtos.ContactDtos;
using Travelin.Settings;

namespace Travelin.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendReservationApprovedEmailAsync(string toEmail, string customerName, string tourTitle, DateTime tourDate, int personCount)
        {
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background: #16a34a; color: white; padding: 24px; text-align: center; border-radius: 8px 8px 0 0;'>
                        <h1 style='margin: 0; font-size: 22px;'>Rezervasyonunuz Onaylandı</h1>
                    </div>
                    <div style='padding: 24px; border: 1px solid #e2e8f0; border-top: none; border-radius: 0 0 8px 8px;'>
                        <p>Sayın <strong>{customerName}</strong>,</p>
                        <p>Aşağıdaki tur için rezervasyonunuz onaylanmıştır. Sizi aramızda görmekten mutluluk duyacağız!</p>
                        <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
                            <tr><td style='padding: 8px; color: #718096;'>Tur</td><td style='padding: 8px; font-weight: bold;'>{tourTitle}</td></tr>
                            <tr><td style='padding: 8px; color: #718096;'>Tarih</td><td style='padding: 8px; font-weight: bold;'>{tourDate:dd MMMM yyyy}</td></tr>
                            <tr><td style='padding: 8px; color: #718096;'>Kişi Sayısı</td><td style='padding: 8px; font-weight: bold;'>{personCount}</td></tr>
                        </table>
                        <p style='color: #718096; font-size: 13px;'>Bu e-posta otomatik olarak gönderilmiştir.</p>
                    </div>
                </div>";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Rezervasyonunuz Onaylandı - Travelin";

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.AppPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendReservationCancelledEmailAsync(string toEmail, string customerName, string tourTitle, DateTime tourDate)
        {
            var body = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <div style='background: #dc2626; color: white; padding: 24px; text-align: center; border-radius: 8px 8px 0 0;'>
                <h1 style='margin: 0; font-size: 22px;'>Rezervasyonunuz İptal Edildi</h1>
            </div>
            <div style='padding: 24px; border: 1px solid #e2e8f0; border-top: none; border-radius: 0 0 8px 8px;'>
                <p>Sayın <strong>{customerName}</strong>,</p>
                <p>Aşağıdaki tur için rezervasyonunuz iptal edilmiştir. Sorularınız için bizimle iletişime geçebilirsiniz.</p>
                <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
                    <tr><td style='padding: 8px; color: #718096;'>Tur</td><td style='padding: 8px; font-weight: bold;'>{tourTitle}</td></tr>
                    <tr><td style='padding: 8px; color: #718096;'>Tarih</td><td style='padding: 8px; font-weight: bold;'>{tourDate:dd MMMM yyyy}</td></tr>
                </table>
                <p style='color: #718096; font-size: 13px;'>Bu e-posta otomatik olarak gönderilmiştir.</p>
            </div>
        </div>";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Rezervasyonunuz İptal Edildi - Travelin";

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.AppPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendContactMessageAsync(ContactMessageDto message)
        {
            var body = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <div style='background: #17a2a2; color: white; padding: 24px; text-align: center; border-radius: 8px 8px 0 0;'>
                <h1 style='margin: 0; font-size: 22px;'>Yeni İletişim Mesajı</h1>
            </div>
            <div style='padding: 24px; border: 1px solid #e2e8f0; border-top: none; border-radius: 0 0 8px 8px;'>
                <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
                    <tr><td style='padding: 8px; color: #718096;'>Ad Soyad</td><td style='padding: 8px; font-weight: bold;'>{message.Name} {message.Surname}</td></tr>
                    <tr><td style='padding: 8px; color: #718096;'>E-posta</td><td style='padding: 8px; font-weight: bold;'>{message.Email}</td></tr>
                    <tr><td style='padding: 8px; color: #718096;'>Telefon</td><td style='padding: 8px; font-weight: bold;'>{message.Phone}</td></tr>
                </table>
                <p style='color: #718096;'><strong>Mesaj:</strong></p>
                <p style='padding: 12px; background: #f8f9fa; border-radius: 6px;'>{message.Message}</p>
            </div>
        </div>";

            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            mail.To.Add(MailboxAddress.Parse(_emailSettings.SenderEmail));
            mail.ReplyTo.Add(new MailboxAddress(message.Name, message.Email));
            mail.Subject = $"İletişim Formu: {message.Name} {message.Surname}";

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            mail.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.AppPassword);
            await client.SendAsync(mail);
            await client.DisconnectAsync(true);
        }
    }
}