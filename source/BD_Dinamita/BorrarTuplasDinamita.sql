use Dinamita

delete HospederoRealiza 
where HospederoRealiza.IdentificadorReserva = 'XDHpYSWAWB';

delete Placa 
where Placa.IdentificadorReserva = 'XDHpYSWAWB';

delete Hospedero 
where Hospedero.Identificacion = '208940234';

delete PrecioReservacion
where PrecioReservacion.IdentificadorReserva = 'XDHpYSWAWB';

delete Reservacion
where Reservacion.IdentificadorReserva = 'XDHpYSWAWB';

delete Pago
where Pago.Comprobante = 'NPKNeQ';