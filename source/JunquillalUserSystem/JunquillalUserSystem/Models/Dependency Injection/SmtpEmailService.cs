using System.Net.Mail;
using System.Net;

namespace JunquillalUserSystem.Models.Dependency_Injection
{
    // Implementación concreta de IEmailService utilizando SmtpClient para enviar correos electrónicos
    public class SmtpEmailService : IEmailService
    {
        public void EnviarEmail(string mensaje, string correo)
        {
            // Lógica para enviar el correo electrónico utilizando SmtpClient
            SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("dinamita_PI@outlook.com", "PI_JUNQUILLAL");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("dinamita_PI@outlook.com", "Confirmacion de reserva");
            mail.To.Add(new MailAddress(correo));
            mail.Subject = "Un gusto saludarle por parte de Junquillal";
            mail.IsBodyHtml = true;
            mail.Body = mensaje;

            smtp.Send(mail);
        }
    }
}
