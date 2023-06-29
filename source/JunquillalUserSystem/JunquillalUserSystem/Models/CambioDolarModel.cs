using System.ComponentModel.DataAnnotations;
namespace JunquillalUserSystem.Models
{
    public class CambioDolarModel
    {
        private double valorDolar;

        [Required(ErrorMessage = "Por favor, introduzca un valor numérico")]
        public double ValorDolar
        {
            get { return valorDolar; }
            set { valorDolar = value; }
        }
    }
}
