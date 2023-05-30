namespace JunquillalUserSystem.Models
{
    public class TrabajadorModelo
    {
        public string ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string correo { get; set; }
        public string Puesto { get; set; }

        public TrabajadorModelo() { 
            ID = 0;
            Nombre = "";
            Apellido1 = "";
            Apellido2 = "";
            correo = "";
            Puesto = "";

        }

        public TrabajadorModelo obtenerCredenciales(IFormCollection form)
        {
            TrabajadorModelo trabajador = new TrabajadorModelo();
            trabajador.ID  = form["usuario"];


            return trabajador;

        }
        
    }
}
