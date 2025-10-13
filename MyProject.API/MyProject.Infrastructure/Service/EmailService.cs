using Microsoft.Extensions.Configuration;
using MyProject.Application.Interface;
using System.Net;
using System.Net.Mail;

namespace MyProject.Infrastructure.Service
{
    public class EmailService(IConfiguration configuration,ITokenService tokenService) : IEmailService
    {
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpServer = configuration["Email:SmtpServer"];
                var smtpPort = int.Parse(configuration["Email:SmtpPort"] ?? "25");
                var smtpUser = configuration["Email:User"];
                var smtpPass = configuration["Email:Password"];

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true,
                    UseDefaultCredentials = false
                };

                var message = new MailMessage(smtpUser, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email send failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendVerifyEmail(string email)
        {
            try
            {
                var emailToken = tokenService.GenerateValidateEmailToken(email);
                var subject = "Verify your email";
                var verifyLink = $"http://172.16.161.5:8080/api/User/verify-email/{emailToken}";

                var body = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                      <meta charset='UTF-8'>
                      <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                      <title>Verify your email</title>
                      <style>
                        body {{
                          font-family: 'Segoe UI', Arial, sans-serif;
                          background-color: #f4f4f4;
                          margin: 0;
                          padding: 0;
                          color: #333;
                        }}
                        .container {{
                          max-width: 600px;
                          margin: 40px auto;
                          background: #ffffff;
                          border-radius: 12px;
                          box-shadow: 0 4px 12px rgba(0,0,0,0.1);
                          overflow: hidden;
                        }}
                        .header {{
                          background: linear-gradient(135deg, #4f46e5, #6366f1);
                          color: white;
                          text-align: center;
                          padding: 30px 20px;
                        }}
                        .header h1 {{
                          margin: 0;
                          font-size: 24px;
                          font-weight: 600;
                        }}
                        .content {{
                          padding: 30px 40px;
                          line-height: 1.6;
                        }}
                        .button {{
                          display: inline-block;
                          background: #4f46e5;
                          color: white;
                          padding: 14px 28px;
                          border-radius: 8px;
                          text-decoration: none;
                          font-weight: 600;
                          margin-top: 20px;
                        }}
                        .button:hover {{
                          background: #4338ca;
                        }}
                        .footer {{
                          text-align: center;
                          font-size: 13px;
                          color: #999;
                          padding: 20px;
                          border-top: 1px solid #eee;
                        }}
                      </style>
                    </head>
                    <body>
                      <div class='container'>
                        <div class='header'>
                          <h1>Email Verification</h1>
                        </div>
                        <div class='content'>
                          <p>Hi there 👋,</p>
                          <p>Thank you for registering an account with <strong>MyProject</strong>.</p>
                          <p>Please confirm that <strong>{email}</strong> is your email address by clicking the button below:</p>

                          <p style='text-align:center;'>
                            <a style="" color: white "" href='{verifyLink}' class='button'>Verify My Email</a>
                          </p>

                          <p>If you didn’t request this email, you can safely ignore it.</p>
                          <p>For security reasons, this link will expire in 15 minutes.</p>
                          <p>— The MyProject Team 💜</p>
                        </div>
                        <div class='footer'>
                          <p>© {DateTime.UtcNow.Year} MyProject. All rights reserved.</p>
                        </div>
                      </div>
                    </body>
                    </html>";

                return await SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email send failed: {ex.Message}");
                return false;
            }
        }
    }
}
