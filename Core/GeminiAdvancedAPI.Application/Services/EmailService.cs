using GeminiAdvancedAPI.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace GeminiAdvancedAPI.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;


        public EmailService(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var fromEmail = smtpSettings["SenderEmail"];
            var password = smtpSettings["Password"]; // Şifre!
            var host = smtpSettings["Host"];
            var port = int.Parse(smtpSettings["Port"]);


            using (var client = new SmtpClient(host, port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(fromEmail, password);
                client.EnableSsl = true; // Genellikle gereklidir

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // HTML formatında e-posta göndermek için
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
        }
        // Yeni metot:
        public async Task SendResetPasswordEmailAsync(string toEmail, string resetPasswordUrl)
        {

            //Şablonun bulunduğu dizin
            var templatePath = Path.Combine(_env.ContentRootPath, "Templates", "ResetPasswordEmail.html");
            string emailBody;
            using (var reader = new StreamReader(templatePath))
            {
                emailBody = await reader.ReadToEndAsync();
            }

            emailBody = emailBody.Replace("{ResetPasswordUrl}", resetPasswordUrl);

            await SendEmailAsync(toEmail, "Şifre Sıfırlama", emailBody);
        }
    }
}
