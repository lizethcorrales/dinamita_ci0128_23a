using System;
using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;

//esta clase se encarga de manejar las interacciones con la base de datos cuando se hace una nueva reserva 
// y de finalizar los detalles de las nuevas reservas 


namespace JunquillalUserSystem.Handlers
{
    public class CampingHandler : ReservacionesHandlerBase
    {
      
        public CampingHandler()
        {
        }        


        /*método para insertar una nueva reserva a la base de datos
        recibe el identificador único de la reserva, las fechas del primer día y último día, el estado
        de la reserva (activa, cancelada, en curso) y la cantidad de personas totales de la reserva*/
        public void insertarReserva(ReservacionModelo reservacion, HospederoModelo hospedero, string estado)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertar_Reservacion";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificacion_entrante", reservacion.Identificador);
            comandoParaConsulta.Parameters.AddWithValue("@primerDia_entrante", reservacion.PrimerDia);
            comandoParaConsulta.Parameters.AddWithValue("@ultimoDia_entrante", reservacion.UltimoDia);
            comandoParaConsulta.Parameters.AddWithValue("@estado_entrante", estado);
            comandoParaConsulta.Parameters.AddWithValue("@cantidad_entrante", sacarCantidadPersonasTotal(reservacion));
            comandoParaConsulta.Parameters.AddWithValue("@motivo_entrante", hospedero.Motivo);
            comandoParaConsulta.Parameters.AddWithValue("@tipoActividad", reservacion.TipoActividad);
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

        //este método se encarga de encontrar los días en que no se puede hacer una reserva basado en la 
        //cantidad de personas que el usuario indica
        public string[] BuscarDiasNoDisponiblesReserva(ReservacionModelo reservacion)
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

        /*método para insertar todos los detalles de una reservación, usa los demás métodos de 
         insertar en base de datos, recibe dos modelos hospedero y reservación que cuentan con todos 
        los detalles que se necesitan meter a la base de datos*/
        public void InsertarEnBaseDatos(HospederoModelo hospedero , ReservacionModelo reservacion)
        {

            //llama al método para insertar un hospedero
            insertarHospedero(hospedero);   

            //llama al método para insertar una reserva
            insertarReserva(reservacion, hospedero, "0");

            insertarTieneNacionalidad(hospedero, reservacion);

            if(hospedero.Nacionalidad == "Costa Rica")
            {
                insertarProvincia(reservacion, hospedero);
            }
           
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

        public int transaccionReserva(HospederoModelo hospedero, ReservacionModelo reservacion)
        {
            // Initialize the return value to zero and create a StringWriter to display results.
            int returnValue = 0;
            System.IO.StringWriter writer = new System.IO.StringWriter();

            try
            {
                // Create the TransactionScope to execute the commands, guaranteeing
                // that both commands can commit or roll back as a single unit of work.
                using (TransactionScope scope = new TransactionScope())
                {
                    using (conexion)
                    {
                        // Opening the connection automatically enlists it in the
                        // TransactionScope as a lightweight transaction.
                        conexion.Open();

                        // Create the SqlCommand object and execute the first command.
                        InsertarEnBaseDatos(hospedero, reservacion);
                        writer.WriteLine("Rows to be affected by command1: {0}", returnValue);

                    }
                   
                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.
                    scope.Complete();
                    
                }
            }
            catch (TransactionAbortedException ex)
            {
                writer.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }

            // Display messages.
            Console.WriteLine(writer.ToString());

            return returnValue;
        }
    }

}
