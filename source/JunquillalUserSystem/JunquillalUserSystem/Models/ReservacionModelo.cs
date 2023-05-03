namespace JunquillalUserSystem.Models
 
{
    public class ReservacionModelo
    {
        public DateTime PrimerDia { get; set; }
        public DateTime UltimoDia { get; set; }

        public int Identificador { get; set; }

        public List<String> tipoPersona { get; set; }
        public List<int> cantTipoPersona { get; set; }
        public List<String> placasVehiculos { get; set; }
        public string actividadVisita { get; set; }
    }
}
