using JunquillalUserSystem.Models;
using JunquillalUserSystemTest;
using System.Data;
using System.Data.SqlClient;
namespace JunquillalUserSystem.Handlers

/*Hechas por Sabrina Brenes Hernandez C01309*/
{
    public class HandlerPruebas : HandlerBase
    {
        public HandlerPruebas() { }

        public List<HospederoModelo> obtenerHospedero(string identificacion)
        {
            List<HospederoModelo> hospedero = new List<HospederoModelo>();
            string consulta = "SELECT * FROM Hospedero WHERE Hospedero.Identificacion = '" + identificacion + "';";
            System.Diagnostics.Debug.WriteLine(consulta);
            DataTable tablaDeHospederos = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaDeHospederos.Rows)
            {
                hospedero.Add(
                new HospederoModelo
                {
                    Nombre = Convert.ToString(columna["Nombre"]),
                    Apellido1= Convert.ToString(columna["Apellido1"]),
                    Apellido2 = Convert.ToString(columna["Apellido2"]),
                    Telefono = Convert.ToString(columna["Telefono"]),
                    Email = Convert.ToString(columna["Email"]),
                    Identificacion = Convert.ToString(columna["Identificacion"])
                });
            }
            return hospedero;
        }

        public List<ReservacionModeloPruebas> obtenerReservacion(string identificacion)
        {
            List<ReservacionModeloPruebas> reservacion = new List<ReservacionModeloPruebas>();
            string consulta = "SELECT * FROM Reservacion WHERE Reservacion.IdentificadorReserva = '" + identificacion + "';";
            System.Diagnostics.Debug.WriteLine(consulta);
            DataTable tablaDeReservaciones = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaDeReservaciones.Rows)
            {
                reservacion.Add(
                new ReservacionModeloPruebas
                {
                    Identificador = Convert.ToString(columna["IdentificadorReserva"]),
                    PrimerDia = Convert.ToString(columna["PrimerDia"]),
                    UltimoDia = Convert.ToString(columna["UltimoDia"]),
                    Motivo = Convert.ToString(columna["Motivo"]),
                    TipoActividad = Convert.ToString(columna["TipoActividad"]),
                    Estado = Convert.ToString(columna["Estado"]),
                    CantidadTotal = Convert.ToInt32(columna["CantidadTotalPersonas"])
                });
            } 
            return reservacion;
        }

        public List<NacionalidadPruebas> obtenerNacionalidad(string identificacion)
        {
            List<NacionalidadPruebas> nacionalidades = new List<NacionalidadPruebas>();
            string consulta = "SELECT * FROM TieneNacionalidad WHERE TieneNacionalidad.IdentificadorReserva = '" + identificacion + "';";
            System.Diagnostics.Debug.WriteLine(consulta);
            DataTable tablaDeNacionalidades = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaDeNacionalidades.Rows)
            {
                nacionalidades.Add(
                new NacionalidadPruebas
                {
                    Identificador = Convert.ToString(columna["IdentificadorReserva"]),
                    NombrePais = Convert.ToString(columna["NombrePais"]),
                    CantidadTotal = Convert.ToInt32(columna["Cantidad"])
                }) ;
            }
            return nacionalidades;
        }

        public List<ProvinciaPruebas> obtenerProvincia(string identificacion)
        {
            List<ProvinciaPruebas> provincias = new List<ProvinciaPruebas>();
            string consulta = "SELECT * FROM ProvinciaReserva WHERE ProvinciaReserva.IdentificadorReserva = '" + identificacion + "';";
            System.Diagnostics.Debug.WriteLine(consulta);
            DataTable tablaDeProvincias = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaDeProvincias.Rows)
            {
                provincias.Add(
                new ProvinciaPruebas
                {
                    Identificador = Convert.ToString(columna["IdentificadorReserva"]),
                    NombreProvincia = Convert.ToString(columna["NombreProvincia"]),
                    CantidadTotal = Convert.ToInt32(columna["Cantidad"])
                });
            }
            return provincias;
        }

        public List<PagoPruebas> obtenerPago(string comprobante)
        {
            List<PagoPruebas> pagos = new List<PagoPruebas>();
            string consulta = "SELECT * FROM Pago WHERE Pago.Comprobante = '" + comprobante + "';";
            System.Diagnostics.Debug.WriteLine(consulta);
            DataTable tablaDePagos = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaDePagos.Rows)
            {
                pagos.Add(
                new PagoPruebas
                {
                    Comprobante = Convert.ToString(columna["Comprobante"]),
                    FechaPago = Convert.ToString(columna["FechaPago"]),
                });
            }
            return pagos;
        }
    }
}

