using System;
using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

//esta clase se encarga de manejar las interacciones con la base de datos cuando se hace una nueva reserva 
// y de finalizar los detalles de las nuevas reservas 


namespace JunquillalUserSystem.Handlers
{
    public class ReservacionesHandlerBase : HandlerBase
    {
        protected MetodosGeneralesModel metodosGenerales = new MetodosGeneralesModel();
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
            //ejecución del query
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();



            return (double)costo.Value;
        }


        /*método para insertar a la base de datos las tuplas correspondientes a la cantidad de personas 
         de cada población (adulto nacional o extranjero, niño nacional o extranjero)
        recibe el identificador de la reserva y la cantidad de personas de cada población*/
        public void insertar_PrecioReservacion(ReservacionModelo reservacion)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertar_PrecioReservacion";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificador_Reserva", reservacion.Identificador);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_nacional", reservacion.cantTipoPersona[0]);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_nacional_mayor6", reservacion.cantTipoPersona[1]);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_nacional_menor6", reservacion.cantTipoPersona[2]);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_mayor_nacional", reservacion.cantTipoPersona[3]);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_extranjero", reservacion.cantTipoPersona[4]);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_extranjero", reservacion.cantTipoPersona[5]);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_mayor_extranjero", reservacion.cantTipoPersona[6]);
            comandoParaConsulta.Parameters.AddWithValue("@tipoActividad", reservacion.TipoActividad);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
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
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
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
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }

        public int sacarCantidadPersonasTotal(ReservacionModelo reservacion)
        {
            int cantidadPersonas = 0;
            for (int i = 0; i < reservacion.cantTipoPersona.Count(); i++)
            {
                cantidadPersonas += reservacion.cantTipoPersona[i];
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
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
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

        public void insertarProvincia(ReservacionModelo reservacion, HospederoModelo hospedero)
        {
            string consulta = "InsertarProvincia";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@identificadorReserva", reservacion.Identificador);
            comandoParaConsulta.Parameters.AddWithValue("@nombreProvincia", hospedero.Provincia);
            comandoParaConsulta.Parameters.AddWithValue("@cantidad", sacarCantidadPersonasTotal(reservacion));
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
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
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
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
                    identificadorReserva = Convert.ToString(columna["IdentificadorReserva"]),
                    nacionalidad = Convert.ToString(columna["Nacionalidad"]),
                    poblacion = Convert.ToString(columna["Poblacion"]),
                    actividad = Convert.ToString(columna["Actividad"]),
                    cantidad = Convert.ToInt32(columna["Cantidad"]),
                    precioAlHacerReserva = Convert.ToDouble(columna["PrecioAlHacerReserva"])
                });
            }
            return desglose;
        }

    }

}
