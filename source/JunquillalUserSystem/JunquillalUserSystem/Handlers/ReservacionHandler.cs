using System;
using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

//esta clase se encarga de manejar las interacciones con la base de datos cuando se hace una nueva reserva 
// y de finalizar los detalles de las nuevas reservas 


namespace JunquillalUserSystem.Handlers
{
    public class ReservacionHandler
    {
        private MetodosGeneralesModel metodosGenerales = new MetodosGeneralesModel();
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



            return (double)costo.Value;
        }


        /*método para insertar a la base de datos las tuplas correspondientes a la cantidad de personas 
         de cada población (adulto nacional o extranjero, niño nacional o extranjero)
        recibe el identificador de la reserva y la cantidad de personas de cada población*/
        public void insertar_PrecioReservacion(string indentificador, string adulto_nacional,
            string ninno_nacional_mayor6, string ninno_nacional_menor6, string adulto_mayor_nacional,
            string adulto_extranjero, string ninno_extranjero, string adulto_mayor_extranjero)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertar_PrecioReservacion";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificador_Reserva", indentificador);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_nacional", adulto_nacional);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_nacional_mayor6", ninno_nacional_mayor6);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_nacional_menor6", ninno_nacional_menor6);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_mayor_nacional", adulto_mayor_nacional);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_extranjero", adulto_extranjero);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_extranjero", ninno_extranjero);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_mayor_extranjero", adulto_mayor_extranjero);

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
            string apellido2)
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
                ,hospedero.Apellido2);
            //obtiene la cantidad total de personas en la reserva 
            int cantidadPersonas = 0;
            for (int i = 0; i < reservacion.cantTipoPersona.Count(); i++)
            {
                cantidadPersonas += reservacion.cantTipoPersona[i];
            }
           // int cantidadTotal = reservacion.cantTipoPersona[0] + reservacion.cantTipoPersona[1] +
                //reservacion.cantTipoPersona[2] + reservacion.cantTipoPersona[3];
            //llama al método para insertar una reserva
            insertarReserva(reservacion.Identificador,reservacion.PrimerDia,reservacion.UltimoDia,"0", 
                cantidadPersonas.ToString());

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
            reservacion.cantTipoPersona[3].ToString(), reservacion.cantTipoPersona[4].ToString(), reservacion.cantTipoPersona[5].ToString(),
            reservacion.cantTipoPersona[6].ToString());

            //genera un identificador de pago
            string identificadorPago = metodosGenerales.crearIdentificador(6);
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
    }

}
