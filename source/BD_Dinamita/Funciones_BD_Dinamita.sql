create function ReservasTotales(
    @fecha date
)
returns int
as
begin
    declare @espaciosOcupados int
    select @espaciosOcupados = sum(all p.Cantidad)
    from Reservacion as r
    join PrecioReservacion as p on r.IdentificadorReserva = p.IdentificadorReserva
    where r.PrimerDia <= @fecha and r.UltimoDia >= @fecha
    return @espaciosOcupados;
end
