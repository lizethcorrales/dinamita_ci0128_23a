namespace JunquillalUserSystem.Models
{
    public class TarifaModelo
    {
        public string Nacionalidad { get; set; }
        public string Poblacion { get; set; }        
        public string Actividad { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public string? Esta_Vigente { get; set; }
    }
}
