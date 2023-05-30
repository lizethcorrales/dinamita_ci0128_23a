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


GO

