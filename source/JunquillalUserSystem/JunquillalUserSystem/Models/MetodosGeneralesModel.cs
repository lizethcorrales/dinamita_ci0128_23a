using System;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace JunquillalUserSystem.Models
{
    public class MetodosGeneralesModel
    {
         private static readonly Random random = new Random();


        public MetodosGeneralesModel()
        {

        }

        /*
         * Crea un ID de tamaño "length"
         */
        public string crearIdentificador(int length)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }

            return new string(result);
        }

        /*
         * Envia el mensaje de la descripcion al correo del hospedero
         */
        public void EnviarEmail(string mensaje, string correo)
        {
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

        /*
         * Genera la descripcion de confirmacion de la reserva con la informacion
         * del modelo de reservacion y hospedero
         */
        public string CrearConfirmacionMensaje(ReservacionModelo reservacion, HospederoModelo hospedero)
        {
            StringBuilder sb = new StringBuilder();

            // Encabezado
            if (reservacion.tipo == "Picnic")
            {
                sb.Append("<h2 style='text-align:center;'>Confirmación de Reserva para picnic</h2><br><br>");
            } else
            {
				sb.Append("<h2 style='text-align:center;'>Confirmación de Reserva para Camping</h2><br><br>");
			}

            sb.Append("<h3>Datos del hospedero: </h3><br>");
            sb.Append("<h6>Identificación: " + hospedero.identificacion + "</h6>");
            sb.Append("<h6>nacionalidad: " + hospedero.nacionalidad + "</h6>");
            sb.Append("<h6>tipo de identificación: " + hospedero.tipoIdentificacion + "</h6>");
            sb.Append("<br><h6>" + hospedero.nombre + " " +
                      hospedero.apellido1 + " " + hospedero.apellido2 + ", es un placer darles la bienvenida " +
                      "a Junquillal. Le deseamos un disfrute de su estadia,\n a continuación adjuntamos informacion de " +
                      "su reserva: </h6><br>");
            sb.Append("<h6>" + "</h6><br>");
            sb.Append("<h3> Detalles de la reserva: </h3><br>");
            sb.Append("<h6>Tu código de reservación es: " + reservacion.identificador + "</h6>");
            sb.Append("<h6>Fecha de ingreso: " + reservacion.primerDia + "</h6>");

            if (reservacion.tipo == "Camping")
            {
                sb.Append("<h6>Fecha de salida: " + reservacion.ultimoDia + "</h6>");
            }
            sb.Append("<h6>Cantidad de personas: " + reservacion.cantTipoPersona.Sum() + "</h6>");
            sb.Append("<ul>");

            if (reservacion.cantTipoPersona[0] != 0)
            {
                sb.Append("<li>Adultos nacionales: " + reservacion.cantTipoPersona[0] + "</li>");
            }
            if (reservacion.cantTipoPersona[1] != 0)
            {
                sb.Append("<li>Niños nacionales: " + reservacion.cantTipoPersona[1] + "</li>");
            }
            if (reservacion.cantTipoPersona[2] != 0)
            {
                sb.Append("<li>Adultos extranjeros: " + reservacion.cantTipoPersona[2] + "</li>");
            }
            if (reservacion.cantTipoPersona[3] != 0)
            {
                sb.Append("<li>Niños extranjeros: " + reservacion.cantTipoPersona[3] + "</li>");
            }
            sb.Append("</ul><br>");
            sb.Append("<h6>Placas de vehículos:</h6>");
            sb.Append("<ul>");
            foreach (string placa in reservacion.placasVehiculos)
            {
                sb.Append("<li>Placa: " + placa + "</li>");
            }
            sb.Append("</ul><br>");
            sb.Append("<h6>motivo de la visita: " + hospedero.motivo + "</h6>");

            return sb.ToString();

        }
    }
}
