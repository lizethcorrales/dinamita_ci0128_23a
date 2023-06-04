using JunquillalUserSystem.Models.Builder_pattern;
using System.Text;

namespace JunquillalUserSystem.Models.Patron_Bridge
{
    // Clase de implementación concreta que implementa la interfaz IMensajeConfirmacionImplementacion
    // para generar un mensaje de confirmación en formato HTML
    public class MensajeConfirmacionImplementacionHTML : MensajeConfirmacionAbstraccion, IMensajeConfirmacionImplementacion
    {


        public override string CrearConfirmacionMensaje(ReservacionModelo reservacion, HospederoModelo hospedero, List<PrecioReservacionDesglose> desglose)
        {
            StringBuilder sb = new StringBuilder();

            // Agregar encabezado
            sb.Append(AgregarEncabezado(reservacion.TipoActividad));

            // Agregar datos del hospedero
            sb.Append(AgregarDatosHospedero(hospedero));

            // Agregar detalles de la reserva
            sb.Append(AgregarDetallesReserva(reservacion, desglose));

            return sb.ToString();
        }
        public string AgregarEncabezado(string tipoActividad)
        {
            // Agregar el encabezado según el tipo de actividad (picnic o camping)
            if (tipoActividad == "Picnic")
            {
                return "<h2 style='text-align:center;'>Confirmación de Reserva para picnic</h2><br><br>";
            }
            else
            {
                return "<h2 style='text-align:center;'>Confirmación de Reserva para Camping</h2><br><br>";
            }
        }

        public string AgregarDatosHospedero(HospederoModelo hospedero)
        {
            // Agregar los datos del hospedero al mensaje de confirmación en formato HTML
            StringBuilder sb = new StringBuilder();

            sb.Append("<h3>Datos del hospedero: </h3><br>");
            sb.Append("<h6>Identificación: " + hospedero.Identificacion + "</h6>");
            sb.Append("<h6>Nacionalidad: " + hospedero.Nacionalidad + "</h6>");
            sb.Append("<h6>Tipo de identificación: " + hospedero.TipoIdentificacion + "</h6>");
            sb.Append("</ul><br>");
            sb.Append("<h6>Motivo de la visita: " + hospedero.Motivo + "</h6>");
            sb.Append("<br><h6>" + hospedero.Nombre + " " +
                      hospedero.Apellido1 + " " + hospedero.Apellido2 + ", es un placer darles la bienvenida " +
                      "a Junquillal. Le deseamos un disfrute de su estadia,\n a continuación adjuntamos informacion de " +
                      "su reserva: </h6><br>");


            // Agregar más datos del hospedero si es necesario

            return sb.ToString();
        }

        public string AgregarDetallesReserva(ReservacionModelo reservacion, List<PrecioReservacionDesglose> desglose)
        {
            // Agregar los detalles de la reserva al mensaje de confirmación en formato HTML
            StringBuilder sb = new StringBuilder();

            sb.Append("<h3>Detalles de la reserva:</h3><br>");
            sb.Append("<h6>Tu código de reservación es: " + reservacion.Identificador + "</h6>");
            sb.Append("<h6>Fecha de ingreso: " + reservacion.PrimerDia + "</h6>");

            if (reservacion.TipoActividad == "Camping")
            {
                sb.Append("<h6>Fecha de salida: " + reservacion.UltimoDia + "</h6>");
            }

            sb.Append("<h6>Cantidad de personas: " + reservacion.cantTipoPersona.Sum() + "</h6>");
            sb.Append("<ul>");

            for (int i = 0; i < desglose.Count; i++)
            {
                if (desglose[i].poblacion == "Niño menor 6 años")
                {
                    sb.Append("<li> Niño menor de 6 años " + desglose[i].nacionalidad + ": " + desglose[i].cantidad + " Precio por persona: "
                   + desglose[i].precioAlHacerReserva + "</li>");
                }
                else if (desglose[i].poblacion == "Niño")
                {
                    sb.Append("<li> Niño mayor de 6 años " + desglose[i].nacionalidad + ": " + desglose[i].cantidad + " Precio por persona: "
                  + desglose[i].precioAlHacerReserva + "</li>");
                }
                else
                {
                    sb.Append("<li>" + desglose[i].poblacion + " " + desglose[i].nacionalidad + ": " + desglose[i].cantidad + " Precio por persona: "
                    + desglose[i].precioAlHacerReserva + "</li>");
                }
            }

            sb.Append("</ul><br>");
            sb.Append("<h6>Placas de vehículos:</h6>");
            sb.Append("<ul>");

            foreach (string placa in reservacion.placasVehiculos)
            {
                sb.Append("<li>Placa: " + placa + "</li>");
            }



            // Agregar más detalles de la reserva y desglose de precios si es necesario

            return sb.ToString();
        }
    }

}