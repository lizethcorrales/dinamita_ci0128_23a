using System.ComponentModel.DataAnnotations;
namespace JunquillalUserSystem.Models
{
    public class ReportesModel
    {
        private string primerDia;
        public string PrimerDia
        {
            get { return primerDia; }
            set { primerDia = value; }
        }

        private string ultimoDia;
        public string UltimoDia
        {
            get { return ultimoDia; }
            set { ultimoDia = value; }
        }
    }
}

