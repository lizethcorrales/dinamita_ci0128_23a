--PROCEDIMIENTOS DE LA BASE DE DATOS
-- Este procedimiento devuelve el costo total de la reserva a partir de la información contenida en la tabla PrecioReservacion.
CREATE PROCEDURE calcularCostoTotalReserva (
     @identificador_Reserva AS VARCHAR(10),
     @costo_Total AS DOUBLE PRECISION OUTPUT
) AS 
BEGIN
    -- Se obtiene el producto de la cantidad de personas (de cierta población:niño/adulto, nacional/extranjero) por su tarifa, posteriormente es sumado para obtener el precio total de la reserva.
    SELECT @costo_Total = SUM(PrecioReservacion.Cantidad * PrecioReservacion.PrecioActual)
    FROM PrecioReservacion
    WHERE PrecioReservacion.IdentificadorReserva = @identificador_Reserva
    GROUP BY PrecioReservacion.IdentificadorReserva;
END;


-- Este procedimiento agrega los datos del hospedero a la tabla Hospedero, según los datos recibidos por parámetro.
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



-- Este procedimiento agrega la información de una reserva a la tabla Reservacion
CREATE PROCEDURE insertar_Reservacion (
	@identificacion_entrante AS VARCHAR(10),
	@primerDia_entrante AS DATE,
	@ultimoDia_entrante AS DATE,
	@estado_entrante AS BIT,
	@cantidad_entrante AS SMALLINT
) AS
BEGIN
	INSERT INTO Reservacion
		VALUES (@identificacion_entrante, @primerDia_entrante, @ultimoDia_entrante, @estado_entrante, @cantidad_entrante)
END;


-- Este procedimiento agrega a la tabla PrecioReservacion, el precio por cada tipo de población registrada en la reservación.
CREATE PROCEDURE insertar_PrecioReservacion(
	@identificador_Reserva AS VARCHAR(10),
	@adulto_nacional AS SMALLINT,
	@ninno_nacional AS SMALLINT,
	@adulto_extranjero AS SMALLINT,
	@ninno_extranjero AS SMALLINT
) AS
BEGIN
	DECLARE @precio AS DOUBLE PRECISION;
	-- Por cada tipo de población se agrega nacionalidad y su correspondiente tarifa
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


-- Este procedimiento agrega las placas ingresadas por el reservante a la tabla Placa
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

-- Este procedimiento agrega la fecha de pago y su respectivo comprobante a la tabla Pago
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

-- Este procedimiento registra el pago de una reservación realizada por un hospedero 
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

-- Este procedimiento calcula la cantidad de reservas realizadas en un día específico
CREATE PROCEDURE ReservasTotales(
    @fecha AS DATE,
    @espaciosOcupados AS INT OUTPUT
)
AS
BEGIN
    SELECT @espaciosOcupados = SUM(all p.Cantidad)
    FROM Reservacion AS r
    JOIN PrecioReservacion AS p ON r.IdentificadorReserva = p.IdentificadorReserva
    WHERE r.PrimerDia <= @fecha AND r.UltimoDia >= @fecha
END;


--Este procedimiento encuentra los dias no disponibles al momento de elegir dias de entrada y salida en el calendario
-- se toma en cuenta la cantidad de personas que registradas en la reservación
CREATE PROCEDURE [dbo].[BuscarDiasNoDisponibles]
    @fechas VARCHAR(MAX),
    @cantidadPersonas INT,
    @result VARCHAR(MAX) OUTPUT
AS
BEGIN
    -- Convertir la cadena de fechas en una tabla temporal
    DECLARE @fechasTabla TABLE (RowNum INT, fecha DATE);
    DECLARE @xml XML = N'<root><fecha>' + REPLACE(@fechas, ',', '</fecha><fecha>') + '</fecha></root>';
    INSERT INTO @fechasTabla (RowNum, fecha)
    SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum, fecha.value('.', 'date') AS fecha
    FROM @xml.nodes('//root/fecha') AS T(fecha);

    -- Crear una tabla temporal para almacenar las fechas dentro del intervalo dado
    DECLARE @diasNoDisponibles TABLE (fecha DATE);

    -- Buscar los días no disponibles
    DECLARE @fecha DATE;
    DECLARE @contador INT = 1;
    WHILE @contador <= (SELECT COUNT(*) FROM @fechasTabla)
    BEGIN
        SET @fecha = (SELECT fecha FROM @fechasTabla WHERE RowNum = @contador);
        IF EXISTS (
            SELECT 1
            FROM Reservacion r
            
            WHERE r.PrimerDia <= @fecha AND r.UltimoDia >= @fecha
            GROUP BY r.PrimerDia , r.CantidadTotalPersonas
            HAVING SUM(r.CantidadTotalPersonas) + @cantidadPersonas > 15
                OR 15 - SUM(r.CantidadTotalPersonas) < @cantidadPersonas 
        )
        BEGIN
            INSERT INTO @diasNoDisponibles (fecha) VALUES (@fecha);
        END;
        SET @contador = @contador + 1;
    END;



    -- Devolver los días no disponibles como cadena de caracteres
    DECLARE @resultString VARCHAR(MAX) = '';
    SELECT @resultString = @resultString + CAST(fecha AS VARCHAR(10)) + ',' FROM @diasNoDisponibles;
    SET @result = LEFT(@resultString, LEN(@resultString) - 1);
END;