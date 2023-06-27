Use Dinamita


--Este es un trigger que elimina las tuplas de Placa y PrecioReservacion que tengan como identificador
--el de la tupla de Reservacion que se trata de borra.
--Ademas la tupla de reservacion que se trata de borrar cambia a estado 2 que es cancelada o eliminada
GO
CREATE TRIGGER eliminar_Tupla_Reservacion
ON Reservacion iNSTEAD OF  DELETE
AS
BEGIN
    DELETE FROM Placa WHERE IdentificadorReserva IN (SELECT IdentificadorReserva FROM DELETED);  
    DELETE FROM PrecioReservacion WHERE IdentificadorReserva IN (SELECT IdentificadorReserva FROM DELETED);
	Update Reservacion Set Estado = 2 Where  IdentificadorReserva IN (SELECT IdentificadorReserva FROM DELETED);
END;

drop trigger eliminarTarifa;
GO
CREATE TRIGGER eliminarTarifa
ON Tarifa INSTEAD OF DELETE 
AS 
BEGIN 

	DECLARE @Nacionalidad AS VARCHAR(30);
	DECLARE @Poblacion AS VARCHAR(30);
	DECLARE @Actividad AS VARCHAR(30);

	SELECT @Nacionalidad = deleted.Nacionalidad, @Poblacion = deleted.Poblacion, @Actividad = deleted.Actividad
	FROM deleted;

	UPDATE Tarifa
	SET Tarifa.Esta_Vigente = 0
	WHERE Tarifa.Nacionalidad = @Nacionalidad AND Tarifa.Poblacion = @Poblacion AND Tarifa.Actividad = @Actividad;

END;

