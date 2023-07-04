namespace JunquillalUserSystem.Models.Patron_Bridge
{
    // Interfaz que define los métodos que deben ser implementados por las clases de implementación
    public interface IMensajeConfirmacionImplementacion
    {
        string AgregarEncabezado(string tipoActividad);
        string AgregarDatosHospedero(HospederoModelo hospedero);
        string AgregarDetallesReserva(ReservacionModelo reservacion, List<PrecioReservacionDesglose> desglose);
        void CrearConfirmacionMensaje(ReservacionModelo reservacionModelo, HospederoModelo hospederoModelo, List<PrecioReservacionDesglose> precioReservacionDesgloses);
    }
}
