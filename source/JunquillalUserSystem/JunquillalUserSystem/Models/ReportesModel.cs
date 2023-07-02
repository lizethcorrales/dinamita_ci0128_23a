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

        private string nacionalidad;
        public string Nacionalidad
        {
            get { return nacionalidad; }
            set { nacionalidad = value; }
        }
        private string nombrePais;
        public string NombrePais
        {
            get { return nombrePais; }
            set { nombrePais = value; }
        }
        private string nombreProvincia;
        public string NombreProvincia
        {
            get { return nombreProvincia; }
            set { nombreProvincia = value; }
        }

        private string poblacion;
        public string Poblacion
        {
            get { return poblacion; }
            set { poblacion = value; }
        }

        private string actividad;
        public string Actividad
        {
            get { return actividad; }
            set { actividad = value; }
        }
        private int cantidad;
        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }
        private double ventasTotales;
        public double VentasTotales
        {
            get { return ventasTotales; }
            set { ventasTotales = value; }
        }

        private string fechaReporte;
        public string FechaReporte
        {
            get { return fechaReporte; }
            set { fechaReporte = value; }
        }

        private string motivo;
        public string Motivo
        {
            get { return motivo; }
            set { motivo = value; }
        }
    }
}

