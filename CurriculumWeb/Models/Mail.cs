using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using static System.Net.Mime.MediaTypeNames;

namespace CurriculumWeb.Models
{
    public interface IMailer
    {
        Task SendEmailAsync(string email, string subject, string body);
        string GenerateBodyMessage(Contacto contacto);
    }
    public class Mail : IMailer
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IWebHostEnvironment _env;

        public Mail(IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment env)
        {
            _smtpSettings = smtpSettings.Value;
            _env = env;
        }
      
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.Mail_From, _smtpSettings.Mail_From));
                message.To.Add(new MailboxAddress(email));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = body
                };

                using (var Client = new SmtpClient())
                {
                    Client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    if(_env.IsDevelopment())
                    {
                        await Client.ConnectAsync(_smtpSettings.SMTP_Client, _smtpSettings.SMTP_Port);
                    }
                    else
                    {
                        await Client.ConnectAsync(_smtpSettings.SMTP_Client);
                    }

                    await Client.AuthenticateAsync(_smtpSettings.Mail_From, _smtpSettings.Mail_Password);
                    await Client.SendAsync(message);
                    await Client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {

                throw new InvalidOperationException(e.Message);
            }
        }
        public  string GenerateBodyMessage(Contacto contacto)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Mensaje de Curriculum Web");
            sb.Append(@"<br/>");
            sb.Append("Nombre: " + contacto.Nombre);
            sb.Append(@"<br/>");
            sb.Append("Apellido: " + contacto.Apellido);
            sb.Append(@"<br/>");
            sb.Append("Email: " + contacto.Email);
            sb.Append(@"<br/>");
            sb.Append("Mensaje: " + contacto.Mensaje);
            return sb.ToString();
        }
    }
}
