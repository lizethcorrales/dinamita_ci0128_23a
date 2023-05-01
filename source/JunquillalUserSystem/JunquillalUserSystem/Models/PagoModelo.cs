namespace JunquillalUserSystem.Models
{
    struct servicio
    {
        public string Tipo;
        public float precio;
    }

    public class PagoModelo
    {
        public int comprobante { get; set; }
        public DateOnly fechaPago { get; set; }

    }

    public class PagoServicios : PagoModelo 
    { 
        public List<servicio> listaServicio { get; set; }
        public float precioServicios { get; set; }
        public int cantidadServicios { get; set; }
 
    }

    public class PagoReservacion : PagoModelo
    {
        public int cantidadPersona { get; set; }
        public List<TarifaModelo> Tarifas { get; set; }
    }
}

// el pago se dividio en el pago de los servicios y las tarifas, no si se puede manejar asi o mejor una clase unica
