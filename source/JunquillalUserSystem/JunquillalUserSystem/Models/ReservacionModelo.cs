namespace JunquillalUserSystem.Models
 
{
    public class ReservacionModelo
    {
        public string PrimerDia { get; set; }
        public string UltimoDia { get; set; }

        public int Identificador { get; set; }

        public List<int> cantTipoPersona { get; set; }
        public List<String> placasVehiculos { get; set; }
        public string actividadVisita { get; set; }

        public ReservacionModelo()
        {
            cantTipoPersona = new List<int>();
        }
    }


}
