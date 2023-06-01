using System;
using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

//esta clase se encarga de manejar las interacciones con la base de datos cuando se hace una nueva reserva 
// y de finalizar los detalles de las nuevas reservas 


namespace JunquillalUserSystem.Handlers
{
    public class PicnicHandler : ReservacionesHandlerBase
    {
        public PicnicHandler()
        {
            
        }
        /*método para insertar una nueva reserva a la base de datos
        recibe el identificador único de la reserva, las fechas del primer día y último día, el estado
        de la reserva (activa, cancelada, en curso) y la cantidad de personas totales de la reserva*/
        public void insertarVisita(ReservacionModelo reservacion, HospederoModelo hospedero, string estado)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertar_Reservacion";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificacion_entrante", reservacion.Identificador);
            comandoParaConsulta.Parameters.AddWithValue("@primerDia_entrante", reservacion.PrimerDia);
            comandoParaConsulta.Parameters.AddWithValue("@ultimoDia_entrante", reservacion.PrimerDia);
            comandoParaConsulta.Parameters.AddWithValue("@estado_entrante", estado);
            comandoParaConsulta.Parameters.AddWithValue("@cantidad_entrante", sacarCantidadPersonasTotal(reservacion));
            comandoParaConsulta.Parameters.AddWithValue("@motivo_entrante", hospedero.Motivo);
            comandoParaConsulta.Parameters.AddWithValue("@tipoActividad", reservacion.TipoActividad);
            //ejecución del query
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }
        //este método se encarga de encontrar los días en que no se puede hacer una reserva basado en la 
        //cantidad de personas que el usuario indica
        public string[] BuscarDiasNoDisponiblesVisita(ReservacionModelo reservacion)
        {
            int cantidadPersonas = reservacion.cantTipoPersona.Sum();
            string fechas = ObtenerFechasEntreMeses();
            // Crear el comando para ejecutar el procedimiento almacenado
            using (SqlCommand command = new SqlCommand("BuscarDiasNoDisponiblesVisita", conexion))
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
        /*método para insertar todos los detalles de una reservación, usa los demás métodos de 
         insertar en base de datos, recibe dos modelos hospedero y reservación que cuentan con todos 
        los detalles que se necesitan meter a la base de datos*/
        public void InsertarEnBaseDatosVisita(HospederoModelo hospedero, ReservacionModelo reservacion)
        {
            //llama al método para insertar un hospedero
            insertarHospedero(hospedero);
            //obtiene la cantidad total de personas en la reserva 
            int cantidadPersonas = sacarCantidadPersonasTotal(reservacion);
            // int cantidadTotal = reservacion.cantTipoPersona[0] + reservacion.cantTipoPersona[1] +
            //reservacion.cantTipoPersona[2] + reservacion.cantTipoPersona[3];
            //llama al método para insertar una reserva
            insertarVisita(reservacion, hospedero, "0");

            insertarTieneNacionalidad(hospedero, reservacion);
            System.Diagnostics.Debug.WriteLine(hospedero.Provincia);
            insertarProvincia(reservacion, hospedero);

            //se encarga de llamar al método de insertar las placas de los vehículos dependiendo de la 
            //cantidad de placas que haya introducido el usuario
            insertarPlacasDelFormulario(reservacion);
            //llama al método para insertar las tuplas de la cantidad de personas por población
            insertar_PrecioReservacion(reservacion);

            //genera un identificador de pago
            string identificadorPago = crearIdentificador(6);
            DateOnly date = new DateOnly();

            //llama al método para insertar un opago
            insertarPago(identificadorPago, date.ToString());

            //llama al método que crea la relación entre una reserva, un hospedero y un pago en la base de datos
            insertarHospederoRealiza(hospedero, reservacion, identificadorPago);

        }
    }
}


