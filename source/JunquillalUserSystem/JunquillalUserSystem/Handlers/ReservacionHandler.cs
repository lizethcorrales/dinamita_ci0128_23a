using System;
using System.Collections.Generic;
using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Text;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics;

//esta clase se encarga de manejar las interacciones con la base de datos cuando se hace una nueva reserva 
// y de finalizar los detalles de las nuevas reservas 


namespace JunquillalUserSystem.Handlers
{
    public class ReservacionHandler
    {
        private SqlConnection conexion;
        private string rutaConexion;
        private static readonly Random _random = new Random();
        public ReservacionHandler()
        {
            var builder = WebApplication.CreateBuilder();
            rutaConexion =
            builder.Configuration.GetConnectionString("ContextoJunquillal");
            conexion = new SqlConnection(rutaConexion);
        }

        //método para llenar una tabla a partir de la información obtenida de una consulta a la base de datos
        private DataTable CrearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta,
            conexion);
            SqlDataAdapter adaptadorParaTabla = new
            SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            conexion.Close();
            return consultaFormatoTabla;
        }

        // este método calcula el costo total de la reserva cuando se indica un indentificador de reserva válido
        public double CostoTotal(string identificadorReserva)
        {
            //PROCEDIMIENTO

            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "calcularCostoTotalReserva";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificador_Reserva", identificadorReserva);
            SqlParameter costo = new SqlParameter("@costo_total", SqlDbType.Float);
            //si hay un parámetro de tipo output se indica de esta forma
            costo.Direction = ParameterDirection.Output;
            comandoParaConsulta.Parameters.Add(costo);
            //ejecución del query
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();

            //CONSULTA NORMAL

            //System.Diagnostics.Debug.Write(costo.Value);
            //string consulta = "SELECT * FROM Hospedero";
            // DataTable tablaResultado = CrearTablaConsulta(consulta);
            //foreach (DataRow columna in tablaResultado.Rows)
            //{
            //System.Diagnostics.Debug.Write(columna["Nombre"]);
            //
            return (double)costo.Value;
        }


        /*método para insertar a la base de datos las tuplas correspondientes a la cantidad de personas 
         de cada población (adulto nacional o extranjero, niño nacional o extranjero)
        recibe el identificador de la reserva y la cantidad de personas de cada población*/
        public void insertar_PrecioReservacion(string indentificador, string adulto_nacional,
            string ninno_nacional, string adulto_extranjero, string ninno_extranjero)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertar_PrecioReservacion";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificador_Reserva", indentificador);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_nacional", adulto_nacional);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_nacional", ninno_nacional);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_extranjero", adulto_extranjero);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_extranjero", ninno_extranjero);
            //ejecución del query
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }

        /*método para insertar una nueva reserva a la base de datos
        recibe el identificador único de la reserva, las fechas del primer día y último día, el estado
        de la reserva (activa, cancelada, en curso) y la cantidad de personas totales de la reserva*/
        public void insertarReserva(string identificador, string primerDia, string ultimoDia, string estado,
            string cantidad)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertar_Reservacion";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificacion_entrante", identificador);
            comandoParaConsulta.Parameters.AddWithValue("@primerDia_entrante", primerDia);
            comandoParaConsulta.Parameters.AddWithValue("@ultimoDia_entrante", ultimoDia);
            comandoParaConsulta.Parameters.AddWithValue("@estado_entrante", estado);
            comandoParaConsulta.Parameters.AddWithValue("@cantidad_entrante", cantidad);
            //ejecución del query
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }


        /*método para insertar un hospedero a la base de datos
       recibe el identificador único del hospedero (cedula), el email, nombre, los dos apellidos y un estado*/
        public void insertarHospedero(string identificacion, string email, string nombre, string apellido1,
            string apellido2, bool estado)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertar_hospedero";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificacion_entrante", identificacion);
            comandoParaConsulta.Parameters.AddWithValue("@email_entrante", email);
            comandoParaConsulta.Parameters.AddWithValue("@nombre_entrante", nombre);
            comandoParaConsulta.Parameters.AddWithValue("@apellido1_entrante", apellido1);
            comandoParaConsulta.Parameters.AddWithValue("@apellido2_entrante", apellido2);
            comandoParaConsulta.Parameters.AddWithValue("@estado_entrante", estado);
            //ejecución del query
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }

        /*método para insertar las placas de los vehículos que van a llevar el día de la reserva
       recibe el identificador único de la reserva y las 4 placas que se pueden tener por ahora, las placas 
        pueden estar vacías en cuyo caso solo se insertan cuando la variable trae algo diferente a un vacío*/
        public void insertarPlacas(string identificador, string placa1, string placa2, string placa3, string placa4)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertar_Placas";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificador_reserva", identificador);
            comandoParaConsulta.Parameters.AddWithValue("@placa1", placa1);
            comandoParaConsulta.Parameters.AddWithValue("@placa2", placa2);
            comandoParaConsulta.Parameters.AddWithValue("@placa3", placa3);
            comandoParaConsulta.Parameters.AddWithValue("@placa4", placa4);
            //ejecución del query
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }


        //este método se encarga de encontrar los días en que no se puede hacer una reserva basado en la 
        //cantidad de personas que el usuario indica
        public string [] BuscarDiasNoDisponibles(ReservacionModelo reservacion)
        {
            int cantidadPersonas = reservacion.cantTipoPersona.Sum();
            string fechas = ObtenerFechasEntreMeses();
                // Crear el comando para ejecutar el procedimiento almacenado
                using (SqlCommand command = new SqlCommand("BuscarDiasNoDisponibles", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros al comando
                    command.Parameters.AddWithValue("@fechas", fechas);
                    command.Parameters.AddWithValue("@cantidadPersonas", cantidadPersonas);

                    // Agregar el parámetro de salida al comando
                    SqlParameter resultParam = new SqlParameter("@result", SqlDbType.VarChar, -1);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);

                    // Abrir la conexión y ejecutar el comando
                    conexion.Open();
                    command.ExecuteNonQuery();
                    conexion.Close();

                // Obtener el resultado del parámetro de salida
                string result = (string)command.Parameters["@result"].Value;

                   // Retornar el resultado
                 string[] vectorFechas = result.Split(',');
                 return vectorFechas;
                }
            
        }

        /*
         * Obtienes las fechas en un intervalo que va desde el dia actual
         * hasta el ultimo dia del siguiente mes
         */ 
        public string ObtenerFechasEntreMeses()
        {
            DateTime hoy = DateTime.Today;
            DateTime primerDiaMesActual = new DateTime(hoy.Year, hoy.Month, hoy.Day);
            DateTime ultimoDiaMesSiguiente = primerDiaMesActual.AddMonths(2).AddDays(-1);
            int totalDias = (ultimoDiaMesSiguiente - primerDiaMesActual).Days + 1;
            string[] fechas = new string[totalDias];
            for (int i = 0; i < totalDias; i++)
            {
                fechas[i] = primerDiaMesActual.AddDays(i).ToString("yyyy-MM-dd");
            }


            Debug.WriteLine($"Fechas {string.Join(",", fechas)}");
            return string.Join(",", fechas);

        }

        /*método para insertar un pago realizado cuando se confirma la reserva 
         rescibe el comprobante único del pago y la fecha en que se hizo*/
        public void insertarPago(string comprobante, string fechaPago)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertar_Pago";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@comprobante", comprobante);
            comandoParaConsulta.Parameters.AddWithValue("@fecha_pago", fechaPago);
            //ejecución del query
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }

        /*método para insertar todos los detalles de una reservación, usa los demás métodos de 
         insertar en base de datos, recibe dos modelos hospedero y reservación que cuentan con todos 
        los detalles que se necesitan meter a la base de datos*/
        public void InsertarEnBaseDatos(HospederoModelo hospedero , ReservacionModelo reservacion)
        {
            //llama al método para insertar un hospedero
            insertarHospedero(hospedero.Identificacion, hospedero.Email , hospedero.Nombre , hospedero.Apellido1
                ,hospedero.Apellido2,false);
            //obtiene la cantidad total de personas en la reserva 
            int cantidadTotal = reservacion.cantTipoPersona[0] + reservacion.cantTipoPersona[1] +
                reservacion.cantTipoPersona[2] + reservacion.cantTipoPersona[3];
            //llama al método para insertar una reserva
            insertarReserva(reservacion.Identificador,reservacion.PrimerDia,reservacion.UltimoDia,"0", 
                cantidadTotal.ToString());

            //se encarga de llamar al método de insertar las placas de los vehículos dependiendo de la 
            //cantidad de placas que haya introducido el usuario
            switch (reservacion.placasVehiculos.Count)
            {
                case 0:
                    insertarPlacas(reservacion.Identificador, "", "", "", "");
                    break;
                case 1:
                    insertarPlacas(reservacion.Identificador, reservacion.placasVehiculos[0], "", "", "");
                    break;
                case 2:
                    insertarPlacas(reservacion.Identificador, reservacion.placasVehiculos[0], reservacion.placasVehiculos[1], "", "");
                    break;
                case 3:
                    insertarPlacas(reservacion.Identificador, reservacion.placasVehiculos[0], reservacion.placasVehiculos[1],
                        reservacion.placasVehiculos[2], "");
                    break;
                case 4:
                    insertarPlacas(reservacion.Identificador, reservacion.placasVehiculos[0], reservacion.placasVehiculos[1],
                       reservacion.placasVehiculos[2], reservacion.placasVehiculos[3]);
                    break;

            }
            //llama al método para insertar las tuplas de la cantidad de personas por población
            insertar_PrecioReservacion(reservacion.Identificador , reservacion.cantTipoPersona[0].ToString(),
            reservacion.cantTipoPersona[1].ToString(), reservacion.cantTipoPersona[2].ToString(), 
            reservacion.cantTipoPersona[3].ToString());

            //genera un identificador de pago
            string identificadorPago = crearIdentificador(6);
            DateOnly date = new DateOnly();

            //llama al método para insertar un opago
            insertarPago(identificadorPago, date.ToString());

            //llama al método que crea la relación entre una reserva, un hospedero y un pago en la base de datos
            insertarHospederoRealiza(hospedero.Identificacion, reservacion.Identificador,
            identificadorPago);


        }

        /*método para insertar una relación entre una reserva, un hopedero y un pago en la base de datos
         recibe el identificador único del hospedero y la reservación y el comprobante de pago*/
        public void insertarHospederoRealiza(string identificador_hospedero, string identificador_Reserva, 
            string identificador_pago)
        {
            string consulta = "insertar_HospederoRealiza";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@identificador_hospedero", identificador_hospedero);
            comandoParaConsulta.Parameters.AddWithValue("@identificador_reserva", identificador_Reserva);
            comandoParaConsulta.Parameters.AddWithValue("@identificador_pago", identificador_pago);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }

        /*
         * Obtiene los datos de la cantidad de personas que se almacenaron
         * en el form y se guardan en el modelo reservacion
         */ 
        public ReservacionModelo LlenarCantidadPersonas(ReservacionModelo reservacion, IFormCollection form)
        {

             reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Adultos_Nacional"]));
             reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Ninnos_Nacional"]));
             reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Adultos_Extranjero"]));
             reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_ninnos_extranjero"]));
          


            return reservacion;
        }

        /*
         * Obtiene las fechas del form y las guarda en el modelo reservacion
         */ 
        public ReservacionModelo LlenarFechas(ReservacionModelo reservacion, IFormCollection form)
        {

            reservacion.PrimerDia = form["fecha-entrada"];
            reservacion.UltimoDia = form["fecha-salida"];

            return reservacion;
        }

        /*
         * Obtiene la informacion importante del form y la guarda en el modelo reserva
         */ 
        public ReservacionModelo LlenarInformacionResarva(ReservacionModelo reservacion, IFormCollection form)
        {

            if (form["placa1"] != "")
            {
                reservacion.placasVehiculos.Add(form["placa1"]);

            }

            if (form["placa2"] != "")
            {
                reservacion.placasVehiculos.Add(form["placa2"]);

            }

            if (form["placa3"] != "")
            {
                reservacion.placasVehiculos.Add(form["placa1"]);

            }

            if (form["placa4"] != "")
            {
                reservacion.placasVehiculos.Add(form["placa1"]);

            }

            string identificador = crearIdentificador(10);
            reservacion.Identificador = identificador;

            return reservacion;
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
                result[i] = allowedChars[_random.Next(0, allowedChars.Length)];
            }

            return new string(result);
        }

        /*
         * Llena el modelo hospedero con la informacion del form
         */ 
        public HospederoModelo LlenarHospedero(IFormCollection form)
        {
            HospederoModelo hospedero = new HospederoModelo();

            hospedero.Nombre = form["nombre"];
            hospedero.Apellido1 = form["primerApellido"];
            hospedero.Apellido2 = form["segundoApellido"];
            hospedero.Identificacion = form["identificacion"];
            hospedero.Telefono = form["segundoApellido"];
            hospedero.Email = form["email"];
            hospedero.TipoIdentificacion = form["nacionalidad"];
            hospedero.Nacionalidad = form["pais"];
            hospedero.Motivo = form["motivo"];
            hospedero.Estado = 0;

            return hospedero;
        }

        /*
         * Genera la descripcion de confirmacion de la reserva con la informacion
         * del modelo de reservacion y hospedero
         */ 
        public string CrearConfirmacionMensaje(ReservacionModelo reservacion, HospederoModelo hospedero)
        {
            StringBuilder sb = new StringBuilder();

            // Encabezado
            sb.Append("<h2 style='text-align:center;'>Confirmación de Reserva</h2><br><br>");

            sb.Append("<h3>Datos del hospedero: </h3><br>");
            sb.Append("<h6>Identificación: " + hospedero.Identificacion + "</h6>");
            sb.Append("<h6>Nacionalidad: " + hospedero.Nacionalidad + "</h6>");
            sb.Append("<h6>Tipo de identificación: " + hospedero.TipoIdentificacion + "</h6>");
            sb.Append("<br><h6>" + hospedero.Nombre + " " +
                      hospedero.Apellido1 + " " + hospedero.Apellido2 + ", es un placer darles la bienvenida " +
                      "a Junquillal. Le deseamos un disfrute de su estadia,\n a continuación adjuntamos informacion de " +
                      "su reserva: </h6><br>");
            sb.Append("<h6>" + "</h6><br>");
            sb.Append("<h3> Detalles de la reserva: </h3><br>");
            sb.Append("<h6>Tu código de reservación es: " + reservacion.Identificador + "</h6>");
            sb.Append("<h6>Primer día: " + reservacion.PrimerDia + "</h6>");
            sb.Append("<h6>Último día: " + reservacion.UltimoDia + "</h6>");
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
            sb.Append("<h6>Motivo de la visita: " + hospedero.Motivo + "</h6>");

            return sb.ToString();

        }


        /*
         * Envia el mensaje de la descripcion al correo del hospedero
         */ 
        public void EnviarEmail(string mensaje , string correo)
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
    }

}
