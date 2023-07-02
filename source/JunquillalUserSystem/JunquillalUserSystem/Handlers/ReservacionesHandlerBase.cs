using System;
using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using NuGet.Protocol.Plugins;

//esta clase se encarga de manejar las interacciones con la base de datos cuando se hace una nueva reserva 
// y de finalizar los detalles de las nuevas reservas 


namespace JunquillalUserSystem.Handlers
{
    
    public class ReservacionesHandlerBase : HandlerBase
    {
        private static readonly Random _random = new Random();


        public ReservacionesHandlerBase()
        {
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
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                comandoParaConsulta.ExecuteNonQuery();
            }
            else
            {
                conexion.Open();
                comandoParaConsulta.ExecuteNonQuery();
                conexion.Close();
            }
            //ejecución del query



            return (double)costo.Value;
        }


        /*método para insertar a la base de datos las tuplas correspondientes a la cantidad de personas 
         de cada población (adulto nacional o extranjero, niño nacional o extranjero)
        recibe el identificador de la reserva y la cantidad de personas de cada población*/
        public void insertar_PrecioReservacion(ReservacionModelo reservacion)
        {
            foreach (var tarifa in reservacion.tarifas) {
                if (tarifa.Cantidad > 0)
                {
                    //se indica el procedimiento almacenado a ejecutarse
                    string consulta = "insertar_PrecioReservacionNuevo";
                    SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
                    comandoParaConsulta.CommandType = CommandType.StoredProcedure;
                    //se agregan los parámetros que recibe el procedimiento
                    comandoParaConsulta.Parameters.AddWithValue("@identificador_Reserva", reservacion.Identificador);
                    comandoParaConsulta.Parameters.AddWithValue("@cantidad", tarifa.Cantidad);
                    comandoParaConsulta.Parameters.AddWithValue("@poblacion", tarifa.Poblacion);
                    comandoParaConsulta.Parameters.AddWithValue("@nacionalidad", tarifa.Nacionalidad);
                    comandoParaConsulta.Parameters.AddWithValue("@tipoActividad", reservacion.TipoActividad);
                    if (conexion.State == System.Data.ConnectionState.Open)
                    {
                        comandoParaConsulta.ExecuteNonQuery();
                    }
                    else
                    {
                        conexion.Open();
                        comandoParaConsulta.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
            }
        }


        /*método para insertar un hospedero a la base de datos
       recibe el identificador único del hospedero (cedula), el email, nombre, los dos apellidos y un estado*/
        public void insertarHospedero(HospederoModelo hospedero)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertar_hospedero";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificacion_entrante", hospedero.Identificacion);
            comandoParaConsulta.Parameters.AddWithValue("@email_entrante", hospedero.Email);
            comandoParaConsulta.Parameters.AddWithValue("@nombre_entrante", hospedero.Nombre);
            comandoParaConsulta.Parameters.AddWithValue("@apellido1_entrante", hospedero.Apellido1);
            comandoParaConsulta.Parameters.AddWithValue("@apellido2_entrante", hospedero.Apellido2);
            comandoParaConsulta.Parameters.AddWithValue("@telefono_entrante", hospedero.Telefono);
            //ejecución del query
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                comandoParaConsulta.ExecuteNonQuery();
            }
            else
            {
                conexion.Open();
                comandoParaConsulta.ExecuteNonQuery();
                conexion.Close();
            }
          
        }

        public void insertarTieneNacionalidad(HospederoModelo hospedero, ReservacionModelo reservacion)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertarTieneNacionalidad";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@IdentificadorReserva", reservacion.Identificador);
            comandoParaConsulta.Parameters.AddWithValue("@NombrePais", hospedero.Nacionalidad);
            comandoParaConsulta.Parameters.AddWithValue("@cantidad", sacarCantidadPersonasTotal(reservacion));
            //ejecución del query
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                comandoParaConsulta.ExecuteNonQuery();
            }
            else
            {
                conexion.Open();
                comandoParaConsulta.ExecuteNonQuery();
                conexion.Close();
            }
        }

        public int sacarCantidadPersonasTotal(ReservacionModelo reservacion)
        {
            int cantidadPersonas = 0;
            for (int i = 0; i < reservacion.tarifas.Count(); i++)
            {
                cantidadPersonas += reservacion.tarifas[i].Cantidad;
            }
            return cantidadPersonas;
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
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                comandoParaConsulta.ExecuteNonQuery();
            }
            else
            {
                conexion.Open();
                comandoParaConsulta.ExecuteNonQuery();
                conexion.Close();
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
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                comandoParaConsulta.ExecuteNonQuery();
            }
            else
            {
                conexion.Open();
                comandoParaConsulta.ExecuteNonQuery();
                conexion.Close();
            }
        }

        public void insertarProvincia(ReservacionModelo reservacion, HospederoModelo hospedero)
        {
            string consulta = "InsertarProvincia";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@identificadorReserva", reservacion.Identificador);
            comandoParaConsulta.Parameters.AddWithValue("@nombreProvincia", hospedero.Provincia);
            comandoParaConsulta.Parameters.AddWithValue("@cantidad", sacarCantidadPersonasTotal(reservacion));
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                comandoParaConsulta.ExecuteNonQuery();
            }
            else
            {
                conexion.Open();
                comandoParaConsulta.ExecuteNonQuery();
                conexion.Close();
            }
        }

        public void insertarPlacasDelFormulario(ReservacionModelo reservacion)
        {
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
        }

        /*método para insertar una relación entre una reserva, un hopedero y un pago en la base de datos
         recibe el identificador único del hospedero y la reservación y el comprobante de pago*/
        public void insertarHospederoRealiza(HospederoModelo hospedero, ReservacionModelo reservacion,
            string identificador_pago)
        {
            string consulta = "insertar_HospederoRealiza";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@identificador_hospedero", hospedero.Identificacion);
            comandoParaConsulta.Parameters.AddWithValue("@identificador_reserva", reservacion.Identificador);
            comandoParaConsulta.Parameters.AddWithValue("@identificador_pago", identificador_pago);
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                comandoParaConsulta.ExecuteNonQuery();
            }
            else
            {
                conexion.Open();
                comandoParaConsulta.ExecuteNonQuery();
                conexion.Close();
            }
        }

        public List<PrecioReservacionDesglose> obtenerDesgloseReservaciones(string identificadorReserva)
        {
            List<PrecioReservacionDesglose> desglose = new List<PrecioReservacionDesglose>();
            string consultaBaseDatos = "SELECT * FROM PrecioReservacion WHERE PrecioReservacion.IdentificadorReserva = '"+identificadorReserva+"';";
            System.Diagnostics.Debug.WriteLine(consultaBaseDatos);
            DataTable tablaDeDesglose = CrearTablaConsulta(consultaBaseDatos);
            foreach (DataRow columna in tablaDeDesglose.Rows)
            {
                desglose.Add(
                new PrecioReservacionDesglose
                {
                    IdentificadorReserva = Convert.ToString(columna["IdentificadorReserva"]),
                    Nacionalidad = Convert.ToString(columna["Nacionalidad"]),
                    Poblacion = Convert.ToString(columna["Poblacion"]),
                    Actividad = Convert.ToString(columna["Actividad"]),
                    Cantidad = Convert.ToInt32(columna["Cantidad"]),
                    PrecioAlHacerReserva = Convert.ToDouble(columna["PrecioAlHacerReserva"])
                });
            }
            return desglose;
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

        public List<TarifaModelo> cargarTarifasCamping(string opcion)
        {
            List<TarifaModelo> tarifas = new List<TarifaModelo>();

            string consultaBaseDatos = "";

            if (opcion == "Camping")
            {
                consultaBaseDatos = "SELECT * FROM Tarifa where Actividad = 'Camping' AND Esta_Vigente = 1;";
            }
            else
            {
                consultaBaseDatos = "SELECT * FROM Tarifa where Actividad = 'Picnic' AND Esta_Vigente = 1;";
            }
            DataTable tablaDeTarifas = CrearTablaConsulta(consultaBaseDatos);
            foreach (DataRow columna in tablaDeTarifas.Rows)
            {
                tarifas.Add(
                new TarifaModelo
                {
                    Nacionalidad = Convert.ToString(columna["Nacionalidad"]),
                    Poblacion = Convert.ToString(columna["Poblacion"]),
                    Actividad = Convert.ToString(columna["Actividad"]),
                    Precio = Convert.ToDouble(columna["Precio"]),
                    Esta_Vigente = (Convert.ToBoolean(columna["Esta_Vigente"])) ? "Tarifa vigente" : "Tarifa no vigente"
                });
            }
            return tarifas;
        }

    }

}
