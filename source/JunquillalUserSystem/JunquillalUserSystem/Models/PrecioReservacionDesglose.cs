namespace JunquillalUserSystem.Models
{
    public class PrecioReservacionDesglose
    {
        public string identificadorReserva { get; set; }

        public string nacionalidad { get; set; }

        public string poblacion { get; set; }

        public string actividad { get; set; }

        public int cantidad { get; set; }

        public double precioAlHacerReserva { get; set; }
    }
}
