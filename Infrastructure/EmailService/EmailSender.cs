using Domain.Abstractions.Contracts;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Web.Infrastructure.Options;

namespace Infrastructure.EmailService
{
    public class EmailSender(IOptions<EmailSettings> EmailSetting) : IEmailSender
    {
        private readonly EmailSettings _emailsettings = EmailSetting.Value;
        public async Task SendEmailAsync(string to, string subject ,string body)
        {
            var emailMessage = new MimeMessage();
            if (string.IsNullOrEmpty(_emailsettings.From))
            {
                throw new InvalidOperationException("Sender email address is not configured.");
            }
            emailMessage.From.Add(new MailboxAddress(_emailsettings.SenderName, _emailsettings.From));
            emailMessage.To.Add(new MailboxAddress("Receiver", "shehabmfathy@gmail.com"));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            await SendAsync(emailMessage);
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.CheckCertificateRevocation = false;
                    await client.ConnectAsync(_emailsettings.SMTPServer, _emailsettings.Port, SecureSocketOptions.StartTls);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailsettings.UserName, _emailsettings.Password);
                    await client.SendAsync(mailMessage);
                }
                catch (MailKit.Net.Smtp.SmtpCommandException smtpEx)
                {
                    throw new InvalidOperationException($"SMTP Authentication failed:" +
                        $" {smtpEx.Message}. Check your email credentials and SMTP settings.", smtpEx);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
