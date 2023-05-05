--PROCEDIMIENTOS DE LA BASE DE DATOS

CREATE PROCEDURE calcularCostoTotalReserva (
     @identificador_Reserva AS VARCHAR(10),
     @costo_Total AS DOUBLE PRECISION OUTPUT
) AS 
BEGIN
    SELECT @costo_Total = SUM(PrecioReservacion.Cantidad * PrecioReservacion.PrecioActual)
    FROM PrecioReservacion
    WHERE PrecioReservacion.IdentificadorReserva = @identificador_Reserva
    GROUP BY PrecioReservacion.IdentificadorReserva;
END;



CREATE PROCEDURE insertar_Hospedero (
	@identificacion_entrante AS CHAR(20),
	@email_entrante AS VARCHAR(60),
	@nombre_entrante AS VARCHAR(20),
	@apellido1_entrante AS VARCHAR(20),
	@apellido2_entrante AS VARCHAR(20),
	@estado_entrante AS BIT
) AS
BEGIN 
	SELECT Hospedero.Identificacion
	FROM Hospedero
	WHERE Hospedero.Identificacion = @identificacion_entrante;

	IF @@ROWCOUNT = 0 
		 INSERT INTO Hospedero
		 VALUES (@identificacion_entrante, @email_entrante, @nombre_entrante, @apellido1_entrante,
				 @apellido2_entrante, @estado_entrante);
END;



--Se elimina precio total de los atributos de la tabla reservacion
CREATE PROCEDURE insertar_Reservacion (
	@identificacion_entrante AS VARCHAR(10),
	@primerDia_entrante AS DATE,
	@ultimoDia_entrante AS DATE,
	@estado_entrante AS BIT
) AS
BEGIN
	INSERT INTO Reservacion
		VALUES (@identificacion_entrante, @primerDia_entrante, @ultimoDia_entrante, @estado_entrante)
END;



CREATE PROCEDURE insertar_PrecioReservacion(
	@identificador_Reserva AS VARCHAR(10),
	@adulto_nacional AS SMALLINT,
	@ninno_nacional AS SMALLINT,
	@adulto_extranjero AS SMALLINT,
	@ninno_extranjero AS SMALLINT
) AS
BEGIN
	DECLARE @precio AS DOUBLE PRECISION;

	IF (@adulto_nacional > 0)

		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Nacional' AND Tarifa.Poblacion = 'Adulto' AND Tarifa.Actividad = 'Camping';

		IF @adulto_nacional > 0
		INSERT INTO PrecioReservacion
		VALUES (@identificador_Reserva, 'Nacional', 'Adulto', 'Camping', @adulto_nacional, @precio);

	IF (@ninno_nacional > 0) 

		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Nacional' AND Tarifa.Poblacion = 'Niño' AND Tarifa.Actividad = 'Camping';

		IF (@ninno_nacional > 0) 
		INSERT INTO PrecioReservacion
		VALUES (@identificador_Reserva, 'Nacional', 'Niño', 'Camping', @ninno_nacional, @precio);

	IF (@adulto_extranjero > 0)

		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Extranjero' AND Tarifa.Poblacion = 'Adulto' AND Tarifa.Actividad = 'Camping';

		IF (@adulto_extranjero > 0)
		INSERT INTO PrecioReservacion
		VALUES (@identificador_Reserva, 'Extranjero', 'Adulto', 'Camping', @adulto_extranjero, @precio);

	IF (@ninno_extranjero >0) 

		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Extranjero' AND Tarifa.Poblacion = 'Niño' AND Tarifa.Actividad = 'Camping';

	IF (@ninno_extranjero >0) 
		INSERT INTO PrecioReservacion
		VALUES (@identificador_Reserva, 'Extranjero', 'Niño', 'Camping', @ninno_extranjero, @precio);

END;



CREATE PROCEDURE insertar_Placas (
	@identificador_reserva AS VARCHAR(10),
	@placa1 AS CHAR(10),
	@placa2 AS CHAR(10),
	@placa3 AS CHAR(10),
	@placa4 AS CHAR(10)
) AS
BEGIN 
	IF (@placa1 <> '')
		INSERT INTO Placa 
		VALUES (@identificador_reserva, @placa1);

	IF (@placa2 <> '')
		INSERT INTO Placa 
		VALUES (@identificador_reserva, @placa2);

	IF (@placa3 <> '')
		INSERT INTO Placa 
		VALUES (@identificador_reserva, @placa3);

	IF (@placa4 <> '')
		INSERT INTO Placa 
		VALUES (@identificador_reserva, @placa4);

END;


CREATE PROCEDURE insertar_Pago (
	@comprobante AS CHAR(6),
	@fecha_pago AS DATE
) AS
BEGIN 
	SELECT Pago.Comprobante
	FROM Pago
	WHERE PAGO.Comprobante = @comprobante;

	IF (@@ROWCOUNT = 0)
		INSERT INTO Pago 
		VALUES (@comprobante, @fecha_pago);
END;


CREATE PROCEDURE insertar_HospederoRealiza(
	@identificador_hospedero AS CHAR(20),
	@identificador_reserva AS VARCHAR(10),
	@identificador_pago AS CHAR(6)
) AS
BEGIN
	SELECT *
	FROM HospederoRealiza
	WHERE HospederoRealiza.IdentificadorReserva = @identificador_reserva AND 
			HospederoRealiza.IdentificacionHospedero  = @identificador_hospedero AND 
			HospederoRealiza.ComprobantePago = @identificador_pago;

	IF (@@ROWCOUNT <= 0)
		INSERT INTO HospederoRealiza
		VALUES (@identificador_hospedero, @identificador_reserva, @identificador_pago);
END;