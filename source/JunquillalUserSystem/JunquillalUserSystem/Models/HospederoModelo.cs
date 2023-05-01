namespace JunquillalUserSystem.Models
{
    public class HospederoModelo
    {
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public int Identificacion { get; set; }
        public string Nacionalidad { get; set; }
        public string Estado { get; set; }

    }

    // En vez de este modelo se puede hacer un modelo persona y ver luego si hace herencia
}

