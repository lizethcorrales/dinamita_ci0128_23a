--PROCEDIMIENTOS DE LA BASE DE DATOS
-- Este procedimiento devuelve el costo total de la reserva a partir de la información contenida en la tabla PrecioReservacion.
CREATE PROCEDURE calcularCostoTotalReserva (
               @identificador_Reserva AS VARCHAR(10),
               @costo_Total AS DOUBLE PRECISION OUTPUT
) AS 
BEGIN 
			SET @costo_Total = 0;
            SELECT @costo_Total = SUM(PrecioReservacion.Cantidad * PrecioReservacion.PrecioAlHacerReserva)
            FROM PrecioReservacion JOIN Reservacion ON PrecioReservacion.IdentificadorReserva = Reservacion.IdentificadorReserva
			WHERE PrecioReservacion.IdentificadorReserva = @identificador_Reserva AND Reservacion.Estado != '2'
            GROUP BY PrecioReservacion.IdentificadorReserva;

END;

-- Este procedimiento agrega los datos del hospedero a la tabla Hospedero, según los datos recibidos por parámetro.
GO
CREATE PROCEDURE insertar_Hospedero (
	@identificacion_entrante AS CHAR(20),
	@email_entrante AS VARCHAR(60),
	@nombre_entrante AS VARCHAR(20),
	@apellido1_entrante AS VARCHAR(20),
	@apellido2_entrante AS VARCHAR(20),
	@telefono_entrante AS VARCHAR(20)
) AS
BEGIN 
	SELECT Hospedero.Identificacion
	FROM Hospedero
	WHERE Hospedero.Identificacion = @identificacion_entrante;

	IF @@ROWCOUNT = 0 
		 INSERT INTO Hospedero
		 VALUES (@identificacion_entrante, @email_entrante, @nombre_entrante, @apellido1_entrante,
				 @apellido2_entrante, @telefono_entrante);
END;



-- Este procedimiento agrega la información de una reserva a la tabla Reservacion
GO
CREATE PROCEDURE insertar_Reservacion (
	@identificacion_entrante AS VARCHAR(10),
	@primerDia_entrante AS DATE,
	@ultimoDia_entrante AS DATE,
	@estado_entrante AS BIT,
	@cantidad_entrante AS SMALLINT,
	@motivo_entrante AS VARCHAR(30),
	@tipoActividad AS VARCHAR(10)
) AS
BEGIN
	INSERT INTO Reservacion
		VALUES (@identificacion_entrante, @primerDia_entrante, @ultimoDia_entrante, 
		@estado_entrante, @cantidad_entrante, @motivo_entrante, @tipoActividad);
END;

-- Este procedimiento agrega a la tabla PrecioReservacion, el precio por cada tipo de población registrada en la reservación.
GO
CREATE PROCEDURE insertar_PrecioReservacion(
	@identificador_Reserva AS VARCHAR(10),
	@adulto_nacional AS SMALLINT,
	@ninno_nacional AS SMALLINT,
	@adulto_extranjero AS SMALLINT,
	@ninno_extranjero AS SMALLINT,
	@adulto_mayor_extranjero AS SMALLINT,
	@tipoActividad AS VARCHAR(10)
) AS
BEGIN
	DECLARE @precio AS DOUBLE PRECISION;
	-- Por cada tipo de población se agrega nacionalidad y su correspondiente tarifa
	IF (@adulto_nacional > 0)

		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Nacional' AND Tarifa.Poblacion = 'Adulto' AND Tarifa.Actividad = @tipoActividad;

		IF @adulto_nacional > 0
		INSERT INTO PrecioReservacion
		VALUES (@identificador_Reserva, 'Nacional', 'Adulto',@tipoActividad, @adulto_nacional, @precio);
		END;

	IF (@ninno_nacional_menor6 > 0) 
		BEGIN
		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Nacional' AND Tarifa.Poblacion = 'Niño menor 6 años' AND Tarifa.Actividad = @tipoActividad;
 
		INSERT INTO PrecioReservacion
		VALUES (@identificador_Reserva, 'Nacional', 'Niño menor 6 años', @tipoActividad, @ninno_nacional_menor6, @precio);
		END;

	IF (@ninno_nacional_mayor6 > 0) 
		BEGIN
		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Nacional' AND Tarifa.Poblacion = 'Niño' AND Tarifa.Actividad = @tipoActividad;
 
		INSERT INTO PrecioReservacion
		VALUES (@identificador_Reserva, 'Nacional', 'Niño', @tipoActividad, @ninno_nacional_mayor6, @precio);
		END;

	IF (@adulto_mayor_nacional > 0)
		BEGIN
		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Nacional' AND Tarifa.Poblacion = 'Adulto Mayor' AND Tarifa.Actividad = @tipoActividad;

		INSERT INTO PrecioReservacion
		VALUES (@identificador_Reserva, 'Nacional', 'Adulto Mayor', @tipoActividad, @adulto_mayor_nacional, @precio);
		END;

	IF (@adulto_extranjero > 0)

		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Extranjero' AND Tarifa.Poblacion = 'Adulto' AND Tarifa.Actividad = @tipoActividad;

		IF (@adulto_extranjero > 0)
		INSERT INTO PrecioReservacion
		VALUES (@identificador_Reserva, 'Extranjero', 'Adulto', @tipoActividad, @adulto_extranjero, @precio);
		END;

	IF (@ninno_extranjero >0) 

		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Extranjero' AND Tarifa.Poblacion = 'Niño' AND Tarifa.Actividad = @tipoActividad;

	IF (@ninno_extranjero >0) 
		INSERT INTO PrecioReservacion
		VALUES (@identificador_Reserva, 'Extranjero', 'Niño', @tipoActividad, @ninno_extranjero, @precio);
		END;

	IF (@adulto_mayor_extranjero > 0)
		BEGIN
		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Nacional' AND Tarifa.Poblacion = 'Adulto Mayor' AND Tarifa.Actividad = @tipoActividad;

		INSERT INTO PrecioReservacion
		VALUES (@identificador_Reserva, 'Extranjero', 'Adulto Mayor', @tipoActividad, @adulto_mayor_extranjero, @precio);
		END;

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
    if @espaciosOcupados is null set @espaciosOcupados = 0
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
            
            WHERE r.PrimerDia <= @fecha AND r.UltimoDia >= @fecha AND R.TipoActividad = 'Camping'
			And Estado = '0'
            GROUP BY r.PrimerDia , r.CantidadTotalPersonas
            HAVING SUM(r.CantidadTotalPersonas) + @cantidadPersonas > 80
                OR 80 - SUM(r.CantidadTotalPersonas) < @cantidadPersonas 
        )
        BEGIN
            INSERT INTO @diasNoDisponibles (fecha) VALUES (@fecha);
        END;
        SET @contador = @contador + 1;
    END;

	
    -- Devolver los días no disponibles como cadena de caracteres
    DECLARE @resultString VARCHAR(MAX) = '';
    SELECT @resultString = @resultString + CAST(fecha AS VARCHAR(10)) + ',' FROM @diasNoDisponibles;
    IF LEN(@resultString) > 0
    SET @result = LEFT(@resultString, LEN(@resultString) - 1);
ELSE
    SET @result = '';

END;

GO
CREATE PROCEDURE insertarVisita(
	@identificacion AS CHAR(20),
	@fechaEntrada AS DATE
)AS
BEGIN
	SELECT Visita.Identificacion, Visita.FechaEntrada
	FROM Visita
	WHERE Visita.Identificacion = @identificacion AND Visita.FechaEntrada = @fechaEntrada;

	IF @@ROWCOUNT <= 0
		BEGIN
		INSERT INTO Visita
		VALUES (@identificacion, @fechaEntrada)
		END;

END;

GO
CREATE PROCEDURE insertarNacionalidadVisita (
	@identificacionVisita AS CHAR(20),
	@fechaEntrada AS DATE,
	@NombrePais AS VARCHAR(30),
	@cantidad AS SMALLINT
) AS 
BEGIN 
	SELECT Pais.Nombre
	FROM Pais
	WHERE Pais.Nombre = @NombrePais;

	IF @@ROWCOUNT <= 0
		BEGIN 
		INSERT INTO Pais
		VALUES(@NombrePais);
		END;

	INSERT INTO NacionalidadVisita
	VALUES (@identificacionVisita, @fechaEntrada, @NombrePais, @cantidad);
END;

GO
CREATE PROCEDURE insertar_PrecioVisita(
	@identificador_Visita AS VARCHAR(20),
	@fechaEntrada AS DATE,
	@adulto_nacional AS SMALLINT,
	@ninno_nacional_mayor6 AS SMALLINT,
	@ninno_nacional_menor6 AS SMALLINT,
	@adulto_mayor_nacional AS SMALLINT,
	@adulto_extranjero AS SMALLINT,
	@ninno_extranjero AS SMALLINT,
	@adulto_mayor_extranjero AS SMALLINT
) AS
BEGIN
	DECLARE @precio AS DOUBLE PRECISION;

	IF (@adulto_nacional > 0)
		BEGIN
		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Nacional' AND Tarifa.Poblacion = 'Adulto' AND Tarifa.Actividad = 'Picnic';

		INSERT INTO PrecioVisita
		VALUES (@identificador_Visita, @fechaEntrada, 'Nacional', 'Adulto', 'Picnic', @adulto_nacional, @precio);
		END;

	IF (@ninno_nacional_menor6 > 0) 
		BEGIN
		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Nacional' AND Tarifa.Poblacion = 'Niño menor 6 años' AND Tarifa.Actividad = 'Picnic';
 
		INSERT INTO PrecioVisita
		VALUES (@identificador_Visita, @fechaEntrada, 'Nacional', 'Niño menor 6 años', 'Picnic', @ninno_nacional_menor6, @precio);
		END;

	IF (@ninno_nacional_mayor6 > 0) 
		BEGIN
		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Nacional' AND Tarifa.Poblacion = 'Niño' AND Tarifa.Actividad = 'Picnic';
 
		INSERT INTO PrecioVisita
		VALUES (@identificador_Visita, @fechaEntrada, 'Nacional', 'Niño', 'Picnic', @ninno_nacional_mayor6, @precio);
		END;

	IF (@adulto_mayor_nacional > 0)
		BEGIN
		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Nacional' AND Tarifa.Poblacion = 'Adulto Mayor' AND Tarifa.Actividad = 'Picnic';

		INSERT INTO PrecioVisita
		VALUES (@identificador_Visita, @fechaEntrada, 'Nacional', 'Adulto Mayor', 'Picnic', @adulto_mayor_nacional, @precio);
		END;

	IF (@adulto_extranjero > 0)
		BEGIN
		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Extranjero' AND Tarifa.Poblacion = 'Adulto' AND Tarifa.Actividad = 'Picnic';

		INSERT INTO PrecioVisita
		VALUES (@identificador_Visita, @fechaEntrada, 'Extranjero', 'Adulto', 'Picnic', @adulto_extranjero, @precio);
		END;

	IF (@ninno_extranjero >0) 
		BEGIN
		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Extranjero' AND Tarifa.Poblacion = 'Niño' AND Tarifa.Actividad = 'Picnic';

		INSERT INTO PrecioVisita
		VALUES (@identificador_Visita, @fechaEntrada, 'Extranjero', 'Niño', 'Picnic', @ninno_extranjero, @precio);
		END;

	IF (@adulto_mayor_extranjero > 0)
		BEGIN
		SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = 'Nacional' AND Tarifa.Poblacion = 'Adulto Mayor' AND Tarifa.Actividad = 'Picnic';

		INSERT INTO PrecioVisita
		VALUES (@identificador_Visita, @fechaEntrada, 'Extranjero', 'Adulto Mayor', 'Picnic', @adulto_mayor_extranjero, @precio);
		END;

END;

GO
CREATE PROCEDURE actualizarPrecioTarifa (
	@Nacionalidad AS VARCHAR(30),
	@Poblacion AS VARCHAR(30),
	@Actividad AS VARCHAR(30),
	@Precio AS FLOAT
) AS
BEGIN 
	SELECT *
	FROM Tarifa
	WHERE Tarifa.Nacionalidad = @Nacionalidad AND Tarifa.Poblacion = @Poblacion AND Tarifa.Actividad = @Actividad;

	IF @@ROWCOUNT = 1
		BEGIN 
		UPDATE Tarifa
		SET Precio = @Precio
		WHERE Tarifa.Nacionalidad = @Nacionalidad AND Tarifa.Poblacion = @Poblacion AND Tarifa.Actividad = @Actividad;
		END;
END;

GO
CREATE PROCEDURE insertarTieneNacionalidad(
	@IdentificadorReserva AS VARCHAR(10),
	@NombrePais AS VARCHAR(30),
	@cantidad AS SMALLINT
)
AS 
BEGIN 
SELECT Pais.Nombre
	FROM Pais
	WHERE Pais.Nombre = @NombrePais;

	IF @@ROWCOUNT <= 0
		BEGIN 
		INSERT INTO Pais
		VALUES(@NombrePais);
		END;

	SELECT *
	FROM TieneNacionalidad
	WHERE TieneNacionalidad.IdentificadorReserva = @IdentificadorReserva AND TieneNacionalidad.NombrePais = @NombrePais

	IF @@ROWCOUNT <= 0 
	BEGIN
		INSERT INTO TieneNacionalidad
		VALUES (@IdentificadorReserva, @NombrePais, @cantidad);
	END
END

GO
CREATE PROCEDURE InsertarProvincia(
@identificadorReserva AS VARCHAR(10),
@nombreProvincia AS VARCHAR(20),
@cantidad AS SMALLINT
) AS 
BEGIN 
	SELECT *
	FROM Reservacion
	WHERE Reservacion.IdentificadorReserva = @identificadorReserva;

	IF @@ROWCOUNT > 0 
		BEGIN
		SELECT *
		FROM Provincia
		WHERE Provincia.NombreProvincia = @nombreProvincia;

		IF @@ROWCOUNT > 0 
			BEGIN 
			SELECT *
			FROM ProvinciaReserva
			WHERE ProvinciaReserva.IdentificadorReserva = @identificadorReserva AND 
			ProvinciaReserva.NombreProvincia = @nombreProvincia;

			IF @@ROWCOUNT <=0
				BEGIN
				INSERT INTO ProvinciaReserva
				VALUES(@identificadorReserva, @nombreProvincia, @cantidad);
				END;
			END;
		END;
END;

--Es procedimiento busca reservaciones por fecha , en donde las reservaciones retornadas van a ser las
-- se encuentren entre la fecha pasada por parametro

go
CREATE FUNCTION ObtenerReservacionesPorFecha
(
    @Fecha Date
)
RETURNS TABLE
AS
RETURN
(
    SELECT R.IdentificadorReserva ,PrimerDia, UltimoDia , Hospedero.Nombre,
	        Hospedero.Apellido1 , Hospedero.Apellido2 , Hospedero.Email , 
			Hospedero.Identificacion , TN.NombrePais ,  Hospedero.Telefono , R.Motivo , R.TipoActividad
    FROM Reservacion as R 
	join HospederoRealiza AS HR on R.IdentificadorReserva = HR.IdentificadorReserva
	join Hospedero on HR.IdentificacionHospedero = Hospedero.Identificacion
	left join TieneNacionalidad as TN on  R.IdentificadorReserva  = TN.IdentificadorReserva
    WHERE PrimerDia <= @Fecha And @Fecha <= UltimoDia AND R.Estado != 2
)
go


--Procedimiento que busca las reservaciones por identificador de reserva
go
CREATE FUNCTION ObtenerReservacionesPorIdentificador
(
    @Identificador Varchar(10)
)
RETURNS TABLE
AS
RETURN
(
    SELECT R.IdentificadorReserva ,PrimerDia, UltimoDia , Hospedero.Nombre,
	        Hospedero.Apellido1 , Hospedero.Apellido2 , Hospedero.Email , 
			Hospedero.Identificacion , TN.NombrePais , Hospedero.Telefono , R.Motivo , R.TipoActividad
    FROM Reservacion as R 
	join HospederoRealiza AS HR on R.IdentificadorReserva = HR.IdentificadorReserva
	join Hospedero on HR.IdentificacionHospedero = Hospedero.Identificacion
	left join TieneNacionalidad as TN on  R.IdentificadorReserva  = TN.IdentificadorReserva
    WHERE R.IdentificadorReserva = @Identificador AND R.Estado != 2
)
go

select * from TieneNacionalidad
--Procedimiento que retorna una lista de placas de acuerdo al identificador de
-- reservacion que se pasa por parametro
go
CREATE FUNCTION ObtenerPlacasReservacion
(
    @Identificador VARCHAR(10)
)
RETURNS TABLE
AS
RETURN
(
    SELECT Placa.Placa
    FROM Placa 
    WHERE Placa.IdentificadorReserva = @Identificador 
)
go

--Funcion almacenda que devuelve el tipo de poblacion , la cantidad 
-- y el total que se pago en la reservacion que coincida con el identificador por parametro
go
CREATE FUNCTION ObtenerCantidaTipoPersona
(
    @Identificador VARCHAR(10)
)
RETURNS TABLE
AS
RETURN
(
    SELECT  Poblacion , Nacionalidad , Cantidad , PrecioAlHacerReserva 
    FROM  PrecioReservacion
    WHERE PrecioReservacion.IdentificadorReserva = @Identificador 
)
go


SELECT *
FROM dbo.ObtenerReservacionesPorFecha(@Fecha)


go
--Este procedimiento encuentra los dias no disponibles al momento de elegir el dia d entrada  en el calendario
-- se toma en cuenta la cantidad de personas que registradas en una visita de Picnic
CREATE PROCEDURE [dbo].[BuscarDiasNoDisponiblesVisita]
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
            
            WHERE r.PrimerDia = @fecha  AND R.TipoActividad = 'Picnic'
			And Estado = '0'
            GROUP BY r.PrimerDia , r.CantidadTotalPersonas
            HAVING SUM(r.CantidadTotalPersonas) + @cantidadPersonas > 40
                OR 40 - SUM(r.CantidadTotalPersonas) < @cantidadPersonas 
        )
        BEGIN
            INSERT INTO @diasNoDisponibles (fecha) VALUES (@fecha);
        END;
        SET @contador = @contador + 1;
    END;

	
    -- Devolver los días no disponibles como cadena de caracteres
    DECLARE @resultString VARCHAR(MAX) = '';
    SELECT @resultString = @resultString + CAST(fecha AS VARCHAR(10)) + ',' FROM @diasNoDisponibles;
    IF LEN(@resultString) > 0
    SET @result = LEFT(@resultString, LEN(@resultString) - 1);
ELSE
    SET @result = '';

END;
go

--Esta funcion busca las credenciales de un empleado dentro de la tabla Trabajador, en donde las credenciales retornadas 
--van a ser las del empleado que sea encontrado con la identificacion pasada por parametro
go
CREATE FUNCTION ObtenerCredencialesTrabajador
(
    @Identificacion VARCHAR(10)
)
RETURNS TABLE
AS
RETURN
(
    SELECT T.Cedula, T.Nombre, T.Apellido1, T.Contrasena, T.Salt, T.Puesto
    FROM Trabajador as T
    WHERE T.Cedula = @Identificacion
)

DECLARE @Identificacion  VARCHAR(10) = '211118888'

--Para los reportes se usa el siguiente codigo

	SELECT P.Nacionalidad, P.Poblacion, P.Actividad, SUM(P.Cantidad) AS Cantidad_Total, SUM(P.Cantidad*P.PrecioAlHacerReserva) AS Ventas_Totales
	FROM PrecioReservacion AS P JOIN Reservacion AS R ON P.IdentificadorReserva = R.IdentificadorReserva
	WHERE R.Estado != '2' AND R.PrimerDia >= @primerDia AND R.UltimoDia <= @ultimoDia AND P.Actividad = @actividad
	GROUP BY  P.Nacionalidad, P.Poblacion, P.Actividad

SELECT *
FROM dbo.ObtenerCredencialesTrabajador(@Identificacion)


DECLARE @costoTotal AS DOUBLE PRECISION;
EXEC calcularCostoTotalReserva 'yxl4cO4lsr', @costo_Total = @costoTotal OUTPUT;
select @costoTotal;

SELECT * FROM Hospedero WHERE Hospedero.Identificacion = '308970567';
select * from Reservacion Where Reservacion.IdentificadorReserva = '13t2tdxZ6q';
delete Hospedero
where Hospedero.Identificacion = '308970567';

Delete Pais 
where Pais.Nombre = 'Francia';

delete TieneNacionalidad 
where TieneNacionalidad.IdentificadorReserva = '3463933048' AND TieneNacionalidad.NombrePais= 'Francia'

delete TieneNacionalidad 
where TieneNacionalidad.IdentificadorReserva = '2595141556' AND TieneNacionalidad.NombrePais= 'Estados Unidos'
delete TieneNacionalidad 
where TieneNacionalidad.IdentificadorReserva = '2595141556' AND TieneNacionalidad.NombrePais= 'Alemania';

delete Reservacion
where Reservacion.IdentificadorReserva = 'fURHS56lEV';

delete ProvinciaReserva
where ProvinciaReserva.IdentificadorReserva = '8865933684';

  delete Pago
  where Pago.Comprobante = 'f8JEHa';

  -- Se crea procedimiento que agrega a un trabajador
  CREATE PROCEDURE 
 [dbo].[insertar_Trabajador] (
	@Cedula_entrante AS VARCHAR(10),
	@Nombre_entrante AS VARCHAR(50),
	@Apellido1_entrante AS VARCHAR(50),
	@Apellido2_entrante AS VARCHAR(50),
	@Correo_entrante AS VARCHAR(80),
	@Puesto_entrante AS VARCHAR(20),
	@Contrasena_entrante AS VARCHAR(255),
	@Salt_entrante AS VARCHAR(255)
) AS
BEGIN 
	SELECT Trabajador.Cedula
	FROM Trabajador
	WHERE Trabajador.Cedula = @Cedula_entrante;

	IF @@ROWCOUNT = 0 
		 INSERT INTO Trabajador
		 VALUES (@Cedula_entrante, @Nombre_entrante, @Apellido1_entrante, @Apellido2_entrante,
				 @Correo_entrante, @Puesto_entrante, @Contrasena_entrante, @Salt_entrante);
END;

select * from PrecioReservacion