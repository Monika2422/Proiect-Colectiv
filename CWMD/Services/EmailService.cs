using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;
using System.Net.Mail;

namespace CWMD.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await SendMsg(message);

        }

        private async Task SendMsg(IdentityMessage message)
        {
            MailMessage msg = new MailMessage("cwmdcwmd@gmail.com", message.Destination);
            msg.Subject = message.Subject;
            msg.Body = message.Body;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential()
            {
                UserName = "cwmdcwmd@gmail.com",
                Password = "adminpass123"
            };

            client.EnableSsl = true;

            client.Send(msg);
        }
    }
}