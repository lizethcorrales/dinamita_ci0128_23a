namespace JunquillalUserSystem.Models
{

    public class HospederoModelo
    {
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
        private string identificacion;
        public string Identificacion
        {
            get { return Identificacion; }
            set { Identificacion = value; }
        }
        private string nacionalidad;
        public string Nacionalidad
        {
            get { return nacionalidad; }
            set { nacionalidad = value; }
        }
        private int estado;
        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }
        private string telefono;
        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string tipoIdentificacion;
        public string TipoIdentificacion
        {
            get { return tipoIdentificacion; }
            set { tipoIdentificacion = value; }
        }

        private string motivo;
        public string Motivo
        {
            get { return motivo; }
            set { motivo = value; }
        }

        public HospederoModelo()
        {


        }


        /*
         * Llena el modelo hospedero con la informacion del form
         */
        public HospederoModelo LlenarHospedero(IFormCollection form)
        {
            HospederoModelo hospedero = new HospederoModelo();

            hospedero.Nombre = form["nombre"];
            hospedero.Apellido1 = form["primerApellido"];
            hospedero.Apellido2 = form["segundoApellido"];
            hospedero.Identificacion = form["identificacion"];
            hospedero.Telefono = form["segundoApellido"];
            hospedero.Email = form["email"];
            hospedero.TipoIdentificacion = form["nacionalidad"];
            hospedero.Nacionalidad = form["pais"];
            hospedero.Motivo = form["motivo"];
            hospedero.Estado = 0;

            return hospedero;
        }

    }


}

