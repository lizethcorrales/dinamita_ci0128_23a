using System.ComponentModel.DataAnnotations;

namespace JunquillalUserSystem.Models
{
    public class CamposDisponiblesModel
    {
        private string fecha;
        public string Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        private int cantidadCampos;
        public int CantidadCampos
        {
            get { return cantidadCampos; }
            set { cantidadCampos = value; }
        }
    }
}