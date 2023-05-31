namespace JunquillalUserSystem.Models

{
    public class ReservacionModelo
    {
        private MetodosGeneralesModel metodosGenerales;
        public string PrimerDia { get; set; }
		public string Tipo { get; set; }
		public string UltimoDia { get; set; }

        public string Identificador { get; set; }

        public List<int> cantTipoPersona { get; set; }
        public List<String> placasVehiculos { get; set; }
        public string actividad { get; set; }

        public Dictionary<string, Tuple<int, String>> tipoPersona;
		public HospederoModelo hospedero { get; set; }



        public ReservacionModelo()
        {
			tipoPersona = new Dictionary<string, Tuple<int,String>>();
			cantTipoPersona = new List<int>();
            placasVehiculos = new List<string>();
            metodosGenerales = new MetodosGeneralesModel();
            hospedero = new HospederoModelo();

        }

		public ReservacionModelo(string tipo)
		{
            this.Tipo = tipo;
			cantTipoPersona = new List<int>();
			placasVehiculos = new List<string>();
			metodosGenerales = new MetodosGeneralesModel();

		}

		/*
        * Obtiene la informacion importante del form y la guarda en el modelo reserva
        */
		public ReservacionModelo LlenarInformacionResarva(ReservacionModelo reservacion, IFormCollection form)
        {

            if (form["placa1"] != "")
            {
                reservacion.placasVehiculos.Add(form["placa1"]);

            }

            if (form["placa2"] != "")
            {
                reservacion.placasVehiculos.Add(form["placa2"]);

            }

            if (form["placa3"] != "")
            {
                reservacion.placasVehiculos.Add(form["placa3"]);

            }

            if (form["placa4"] != "")
            {
                reservacion.placasVehiculos.Add(form["placa4"]);

            }

            string identificador = metodosGenerales.crearIdentificador(10);
            reservacion.Identificador = identificador;

            return reservacion;
        }


        /*
         * Obtiene los datos de la cantidad de personas que se almacenaron
         *   en el form y se guardan en el modelo reservacion
         */
        public ReservacionModelo LlenarCantidadPersonas(ReservacionModelo reservacion, IFormCollection form)
        {

            reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Adultos_Nacional"]));
            reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Ninnos_Nacional_mayor6"]));
            reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Ninnos_Nacional_menor6"]));
            reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_adulto_mayor"]));
            reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Adultos_Extranjero"]));
            reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_ninnos_extranjero"]));
            reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_adultoMayor_extranjero"]));


            return reservacion;
        }

        /*
         * Obtiene las fechas del form y las guarda en el modelo reservacion
         */
        public ReservacionModelo LlenarFechas(ReservacionModelo reservacion, IFormCollection form)
        {

            reservacion.PrimerDia = form["fecha-entrada"];
            reservacion.UltimoDia = form["fecha-salida"];

            return reservacion;
        }

    }

}
