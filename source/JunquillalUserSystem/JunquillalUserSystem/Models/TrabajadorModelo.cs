using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace JunquillalUserSystem.Models
{
    public class TrabajadorModelo
    {
        private string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        private string apellido1;
        public string Apellido1
        {
            get { return apellido1; }
            set { apellido1 = value; }
        }
        private string apellido2;
        public string Apellido2
        {
            get { return apellido2; }
            set { apellido2 = value; }
        }
        private string correo;
        public string Correo
        {
            get { return correo; }
            set { correo = value; }
        }
        private string puesto;
        public string Puesto
        {
            get { return puesto; }
            set { puesto = value; }
        }
        private string contrasena;
        public string Contrasena
        {
            get { return contrasena; }
            set { contrasena = value; }
        }
        private string sal;
        public string Sal
        {
            get { return sal; }
            set { sal = value; }
        }

        public TrabajadorModelo() { 
            id = "";
            nombre = "";
            apellido1 = "";
            apellido2 = "";
            correo = "";
            puesto = "";
            contrasena = "";
            sal = "";
        }

        public void crearSal()
        {
            string salt = DateTime.Now.ToString("MM-dd-yyyy");
            string salt2 = salt.Replace('-', '1');
            sal = salt2;
        }

        public string HashearContrasena(string contrasena)
        {
            String contrasenaHash = "";
            SHA256 hash = SHA256.Create();
            if (contrasena != "")
            {
                var contrasenaBytes = Encoding.Default.GetBytes(contrasena);
                var contrasenaHasheada = hash.ComputeHash(contrasenaBytes);

                contrasenaHash = Convert.ToHexString(contrasenaHasheada);

            }
            
            return contrasenaHash;
        }


        
    }
}
