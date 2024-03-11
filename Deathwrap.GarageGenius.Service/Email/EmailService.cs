using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Deathwrap.GarageGenius.Service.Email;

public class EmailService: IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Send(string txt, string email)
    {
        var server = _configuration.GetSection("Email:Server").Get<string>();
        var port = _configuration.GetSection("Email.Port").Get<int>();
        var useSsl = _configuration.GetSection("Email:UseSSL").Get<bool>();
        var fromEmail = _configuration.GetSection("Email:Email").Get<string>();
        var user = _configuration.GetSection("Email:User").Get<string>();
        var frontUser = _configuration.GetSection("Email:FrontUser").Get<string>();
        var password = _configuration.GetSection("Email:Password").Get<string>();
        
        using (var smtp = new SmtpClient())
        {
            await smtp.ConnectAsync(server, port, useSsl);
            await smtp.AuthenticateAsync(user, password);

            var text = new BodyBuilder
            {
                TextBody = txt, HtmlBody = txt
            };
            
            var msg = new MimeMessage
            {
                Subject = "Подтверждение почты для CloudServer",
                Body = text.ToMessageBody()
            };
            
            msg.To.Add(MailboxAddress.Parse(email));
            msg.From.Add(new MailboxAddress(frontUser, fromEmail));

            await smtp.SendAsync(msg);
        }
    }
}