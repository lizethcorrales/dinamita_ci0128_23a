use Dinamita

delete HospederoRealiza 
where HospederoRealiza.IdentificadorReserva = 'yxl4cO4lsr';

delete Placa 
where Placa.IdentificadorReserva = 'yxl4cO4lsr';

delete Hospedero 
where Hospedero.Identificacion = '208240373';

delete PrecioReservacion
where PrecioReservacion.IdentificadorReserva = 'yxl4cO4lsr';

delete TieneNacionalidad
where TieneNacionalidad.IdentificadorReserva = 'yxl4cO4lsr';

delete ProvinciaReserva
where ProvinciaReserva.IdentificadorReserva = 'yxl4cO4lsr';

delete Reservacion
where Reservacion.IdentificadorReserva = 'yxl4cO4lsr';

delete Pago
where Pago.Comprobante = '218EFL';