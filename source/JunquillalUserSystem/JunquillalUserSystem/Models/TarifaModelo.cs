namespace JunquillalUserSystem.Models
{
    public class TarifaModelo
    {
        private string nacionalidad;
        public string Nacionalidad
        {
            get { return nacionalidad; }
            set { nacionalidad = value; }
        }
        private string grupoPoblacion;
        public string GrupoPoblacion
        {
            get { return grupoPoblacion; }
            set { grupoPoblacion = value; }
        }

        private float precio;
        public float Precio
        {
            get { return precio; }
            set { precio = value; }
        }
        public string colorBrazalete;
        private string ColorBrazalete
        {
            get { return colorBrazalete; }
            set { colorBrazalete = value; }
        }

    }
}
