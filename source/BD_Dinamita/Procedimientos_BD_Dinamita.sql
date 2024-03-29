--PROCEDIMIENTOS DE LA BASE DE DATOS
-- Este procedimiento devuelve el costo total de la reserva a partir de la información contenida en la tabla PrecioReservacion.
Drop proc calcularCostoTotalReserva;
GO
CREATE PROCEDURE calcularCostoTotalReserva (
               @identificador_Reserva AS VARCHAR(10),
               @costo_Total AS DOUBLE PRECISION OUTPUT
) AS 
BEGIN 
			DECLARE @primerDia AS DATE;
			DECLARE @ultimoDia AS DATE;
			DECLARE @cantidadDias AS INT;
			DECLARE @cambioDolar AS DOUBLE PRECISION;
			SET @costo_Total = 0;

			SELECT @primerDia = Reservacion.PrimerDia, @ultimoDia = Reservacion.UltimoDia
			FROM Reservacion
			WHERE Reservacion.IdentificadorReserva = @identificador_Reserva;

			IF @primerDia = @ultimoDia
			BEGIN 
				SET @cantidadDias = 1;
			END;
			ELSE
			BEGIN
				SET @cantidadDias = DATEDIFF(DAY, @primerDia, @ultimoDia) + 1;
			END;

			SELECT @cambioDolar = CambioDolar.ValorDolar
			FROM CambioDolar;

			IF @@ROWCOUNT > 0
			BEGIN
				SELECT @costo_Total = SUM(PrecioReservacion.Cantidad * 
				(CASE 
					WHEN PrecioReservacion.Nacionalidad = 'Extranjero' THEN PrecioReservacion.PrecioAlHacerReserva * @cantidadDias * @cambioDolar
					ELSE PrecioReservacion.PrecioAlHacerReserva * @cantidadDias
					END))
				FROM PrecioReservacion JOIN Reservacion ON PrecioReservacion.IdentificadorReserva = Reservacion.IdentificadorReserva
				WHERE PrecioReservacion.IdentificadorReserva = @identificador_Reserva AND Reservacion.Estado != '2'
				GROUP BY PrecioReservacion.IdentificadorReserva;
			END;
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

GO
CREATE PROCEDURE insertar_PrecioReservacion(
	@identificador_Reserva AS VARCHAR(10),
	@cantidad AS SMALLINT ,
	@poblacion AS VARCHAR(25),
	@nacionalidad AS VARCHAR(15),
	@tipoActividad AS VARCHAR(10)
) AS
BEGIN

DECLARE @precio AS DOUBLE PRECISION;

SELECT @precio = Tarifa.precio
		FROM Tarifa
		WHERE Tarifa.Nacionalidad = @nacionalidad AND Tarifa.Poblacion = @poblacion AND Tarifa.Actividad = @tipoActividad;

		INSERT INTO PrecioReservacion
		VALUES (@identificador_Reserva, @nacionalidad ,  @poblacion ,@tipoActividad, @cantidad, @precio);
		
END;

GO
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
CREATE PROCEDURE BuscarDiasNoDisponiblesVisita
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
SELECT R.PrimerDia, P.Nacionalidad,PR.NombreProvincia, TN.NombrePais, P.Poblacion, P.Actividad, SUM(P.Cantidad) AS Cantidad_Total, SUM(P.Cantidad*P.PrecioAlHacerReserva) AS Ventas_Totales
FROM PrecioReservacion AS P LEFT JOIN Reservacion AS R ON P.IdentificadorReserva = R.IdentificadorReserva 
 LEFT JOIN ProvinciaReserva AS PR ON PR.IdentificadorReserva = R.IdentificadorReserva LEFT JOIN TieneNacionalidad AS TN 
ON TN.IdentificadorReserva = R.IdentificadorReserva
WHERE R.Estado != '2' AND R.PrimerDia >= primerDia AND R.UltimoDia <= ultimoDia AND P.Actividad = actividad
GROUP BY  R.IdentificadorReserva, P.Nacionalidad, P.Poblacion, P.Actividad, R.PrimerDia, PR.NombreProvincia, TN.NombrePais
ORDER BY R.PrimerDia;

SELECT *
FROM dbo.ObtenerCredencialesTrabajador(@Identificacion)

  -- Se crea procedimiento que agrega a un trabajador
  CREATE PROCEDURE insertar_Trabajador (
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


GO 
CREATE PROCEDURE insertarNuevaTarifa(
	@nacionalidad AS VARCHAR(30),
	@poblacion AS VARCHAR(30),
	@actividad AS VARCHAR(30),
	@precio AS DOUBLE PRECISION
) AS
BEGIN 
	SELECT Tarifa.Nacionalidad, Tarifa.Poblacion, Tarifa.Actividad, Tarifa.Precio
	FROM Tarifa
	WHERE Tarifa.Nacionalidad = @nacionalidad AND Tarifa.Poblacion = @poblacion AND Tarifa.Actividad = @actividad;

	IF @@ROWCOUNT <= 0
	BEGIN
		INSERT INTO Tarifa
		VALUES (@nacionalidad, @poblacion, @actividad, @precio, '1');
	END;
END;



GO 
CREATE PROCEDURE insertarHospedajeReservacion(
	@identificadorReserva AS VARCHAR(10),
	@numeroParcela AS INT
)AS
BEGIN 
	SELECT Parcela.NumeroParcela
	FROM Parcela
	WHERE Parcela.NumeroParcela = @numeroParcela;

	IF @@ROWCOUNT >= 1
	BEGIN 
		SELECT Hospedaje.IdentificadorReserva
		FROM Hospedaje
		WHERE Hospedaje.IdentificadorReserva = @identificadorReserva;

		IF @@ROWCOUNT <= 0 
		BEGIN 
			INSERT INTO Hospedaje
			VALUES (@identificadorReserva, @numeroParcela);
		END;
	END;
END;
go

select * from PrecioReservacion

 -- Se crea procedimiento que actualiza el valor del dólar
GO
CREATE PROCEDURE actualizarValorDolar(
	@nuevo_valor AS FLOAT
) AS
BEGIN
	SELECT CambioDolar.ValorDolar
		FROM CambioDolar

	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO CambioDolar 
			VALUES(@nuevo_valor)
		END;

	BEGIN 
	UPDATE CambioDolar
	SET ValorDolar = @nuevo_valor
	END;		
END;
