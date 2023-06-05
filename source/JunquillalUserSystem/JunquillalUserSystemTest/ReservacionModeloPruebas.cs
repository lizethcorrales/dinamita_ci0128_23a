using JunquillalUserSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunquillalUserSystemTest
{
    public class ReservacionModeloPruebas
    {

        public string PrimerDia { get; set; }
        public string TipoActividad { get; set; }
        public string UltimoDia { get; set; }

        public string Identificador { get; set; }

        public int CantidadTotal { get; set; }

        public string Estado { get; set; }
        public string Motivo { get; set; }

    }
}
