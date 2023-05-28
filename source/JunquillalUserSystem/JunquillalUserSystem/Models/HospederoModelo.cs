namespace JunquillalUserSystem.Models
{

    public class HospederoModelo
    {
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Identificacion { get; set; }
        public string Nacionalidad { get; set; }
        public int Estado { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public string TipoIdentificacion { get; set; }

        public string Motivo { get; set; }

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
            hospedero.Telefono = form["telefono"];
            hospedero.Email = form["email"];
            hospedero.TipoIdentificacion = form["nacionalidad"];
            hospedero.Nacionalidad = form["pais"];
            hospedero.Motivo = form["motivo"];
            hospedero.Estado = 0;

            return hospedero;
        }

    }


}

