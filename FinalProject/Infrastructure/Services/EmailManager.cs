using System.Net;
using System.Net.Mail;
using Application.Common.Interfaces;
using Application.Common.Models.Email;

namespace Infrastructure.Services
{
    public class EmailManager : IEmailService
    {
        private readonly string _wwwrootPath;

        public EmailManager(string wwwrootPath)
        {
            _wwwrootPath = wwwrootPath;
        }

        public void SendEmailInformation(SendEmailInformationDto sendEmailInformationDto)
        {

            string htmlContent;
                
                htmlContent = File.ReadAllText("../WebApi/wwwrooth/email_templates/email_information.html");
           
            
                htmlContent = htmlContent.Replace("{{name}}", sendEmailInformationDto.Name);
                string statusMessage = sendEmailInformationDto.OrderStatus.GetDescription();
                htmlContent = htmlContent.Replace("{{orderStatus}}", statusMessage);

                Send(new SendEmailDto(sendEmailInformationDto.Email, htmlContent, "CrawlerApp Information"));
            
        }

        private void Send(SendEmailDto sendEmailDto)
        {
            MailMessage message = new MailMessage();

            message.To.Add(sendEmailDto.EmailAddress);
            message.From = new MailAddress("demetrius11@ethereal.email"); 
            message.Subject = sendEmailDto.Subject;
            message.IsBodyHtml = true;
            message.Body = sendEmailDto.Content;
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.ethereal.email",
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("demetrius11@ethereal.email", "6nNdHRn2KrtqcJZb9B"),
            };

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
