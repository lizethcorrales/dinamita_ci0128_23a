namespace JunquillalUserSystem.Models
 
{
    public class ReservacionModelo
    {
        public DateOnly PrimerDia { get; set; }
        public DateOnly UltimoDia { get; set; }

        public ParcelaModelo Parcela { get; set; }

        public int identificador { get; set; }

        public List<String> tipoPersona { get; set; }
        public List<int> cantTipoPersona { get; set; }
        public List<String> placasVehiculos { get; set; }
        public PagoReservacion pagoReservacion { get; set; }
        public PagoServicios pagoServicios { get; set; }
        public string actividadVisita { get; set; }
    }
}
