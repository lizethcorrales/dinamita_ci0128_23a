namespace JunquillalUserSystem.Models
{
    public class PrecioReservacionDesglose
    {
        private string identificadorReserva;
        public string IdentificadorReserva {
            get { return identificadorReserva; }
            set { identificadorReserva = value; } 
        }

        private string nacionalidad;
        public string Nacionalidad
        {
            get { return nacionalidad; }
            set { nacionalidad = value; }
        }
        private string poblacion;
        public string Poblacion
        {
            get { return poblacion; }
            set { poblacion = value; }
        }

        private string actividad;
        public string Actividad
        {
            get { return actividad; }   
            set { actividad = value; }
        }
        private int cantidad;
        public int Cantidad
        {
            get { return cantidad; }    
            set { cantidad = value; }
        }
        private double precioAlHacerReserva; 
        public double PrecioAlHacerReserva
        {
           get { return precioAlHacerReserva; }
           set { precioAlHacerReserva = value; }
        }
        
    }
}
