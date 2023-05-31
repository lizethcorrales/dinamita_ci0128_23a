namespace JunquillalUserSystem.Models
{

    public class HospederoModelo
    {
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string identificacion { get; set; }
        public string nacionalidad { get; set; }
        public int estado { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }

        public string tipoIdentificacion { get; set; }

        public string motivo { get; set; }

        public HospederoModelo()
        {


        }


        /*
         * Llena el modelo hospedero con la informacion del form
         */
        public HospederoModelo LlenarHospedero(IFormCollection form)
        {
            HospederoModelo hospedero = new HospederoModelo();

            hospedero.nombre = form["nombre"];
            hospedero.apellido1 = form["primerApellido"];
            hospedero.apellido2 = form["segundoApellido"];
            hospedero.identificacion = form["identificacion"];
            hospedero.telefono = form["segundoApellido"];
            hospedero.email = form["email"];
            hospedero.tipoIdentificacion = form["nacionalidad"];
            hospedero.nacionalidad = form["pais"];
            hospedero.motivo = form["motivo"];
            hospedero.estado = 0;

            return hospedero;
        }

    }


}

