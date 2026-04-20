using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Text;


public class EmailService
{
    public void Send(string toEmail, string subject, string body)
    {
        var fromAddress = new MailAddress("yourmail@gmail.com", "My App");
        const string password = "APP_PASSWORD";

        var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(fromAddress.Address, password),
            EnableSsl = true
        };

        var message = new MailMessage(fromAddress.Address, toEmail, subject, body);

        smtp.Send(message);
    }
}