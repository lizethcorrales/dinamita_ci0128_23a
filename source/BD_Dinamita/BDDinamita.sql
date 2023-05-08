use Dinamita

CREATE TABLE Pais (
Nombre VARCHAR(30) NOT NULL,
PRIMARY KEY (Nombre)
);

CREATE TABLE Hospedero ( -- no ocupa consecutivo porque se pueden sacar de la info de las fechas 
Identificacion CHAR(20) NOT NULL,
Email VARCHAR(60) NOT NULL,
Nombre VARCHAR(20) NOT NULL,
Apellido1 VARCHAR(20) NOT NULL,
Apellido2 VARCHAR(20),
Estado BIT,
NombrePais VARCHAR(30) NOT NULL,
PRIMARY KEY (Identificacion),
FOREIGN KEY(NombrePais) REFERENCES Pais (Nombre),
UNIQUE (Email)
);

CREATE TABLE Reservacion (
IdentificadorReserva VARCHAR(10) NOT NULL,
PrimerDia DATE NOT NULL,
UltimoDia DATE NOT NULL,
PrecioTotal DOUBLE PRECISION,
PRIMARY KEY(IdentificadorReserva)
);

CREATE TABLE Placa (
IdentificadorReserva VARCHAR(10) NOT NULL,
Placa CHAR(10) NOT NULL,
PRIMARY KEY (IdentificadorReserva, Placa),
FOREIGN KEY (IdentificadorReserva) REFERENCES Reservacion (IdentificadorReserva)
);

CREATE TABLE Servicios (
Tipo VARCHAR(50) NOT NULL,
Precio DOUBLE PRECISION,
PRIMARY KEY (Tipo)
);

CREATE TABLE Visita (
Identificacion CHAR(20) NOT NULL,
FechaEntrada DATE NOT NULL,
PRIMARY KEY (Identificacion, FechaEntrada)
);

CREATE TABLE Parcela (
NumeroParcela SMALLINT NOT NULL,
Capacidad SMALLINT, 
PRIMARY KEY (NumeroParcela)
);

CREATE TABLE CaracteristicasParcela(
NumeroParcela SMALLINT NOT NULL,
Ubicacion VARCHAR(50),
Tamanno FLOAT, 
PRIMARY KEY (NumeroParcela, Ubicacion, Tamanno),
FOREIGN KEY (NumeroParcela) REFERENCES Parcela (NumeroParcela)
);

CREATE TABLE Tarifa (
Nacionalidad VARCHAR(30) NOT NULL,
Poblacion VARCHAR(30) NOT NULL,
Actividad VARCHAR(30) NOT NULL,
Precio DOUBLE PRECISION,
ColorBrazalete VARCHAR(20),
PRIMARY KEY (Nacionalidad, Poblacion, Actividad)
);

CREATE TABLE Pago (
Comprobante CHAR(6),
FechaPago DATE,
PRIMARY KEY(Comprobante)
--derivados CostoTotal, Factura
);

CREATE TABLE PrecioReservacion (
IdentificadorReserva VARCHAR(10) NOT NULL,
Nacionalidad VARCHAR(30) NOT NULL,
Poblacion VARCHAR(30) NOT NULL,
Actividad VARCHAR(30) NOT NULL,
Cantidad SMALLINT,
PrecioActual DOUBLE PRECISION,
PRIMARY KEY(IdentificadorReserva, Nacionalidad, Poblacion, Actividad),
FOREIGN KEY(IdentificadorReserva) REFERENCES Reservacion(IdentificadorReserva),
FOREIGN KEY(Nacionalidad, Poblacion, Actividad) REFERENCES Tarifa(Nacionalidad, Poblacion, Actividad)
);

CREATE TABLE PrecioVisita(
IdentificacionVisita CHAR(20) NOT NULL,
Nacionalidad VARCHAR(30) NOT NULL,
Poblacion VARCHAR(30) NOT NULL,
Actividad VARCHAR(30) NOT NULL,
Cantidad SMALLINT,
PrecioActual DOUBLE PRECISION,
PRIMARY KEY(IdentificacionVisita, Nacionalidad, Poblacion, Actividad),
FOREIGN KEY (IdentificacionVisita) REFERENCES Visita(Identificacion),
FOREIGN KEY(Nacionalidad, Poblacion, Actividad) REFERENCES Tarifa(Nacionalidad, Poblacion, Actividad)
);

CREATE TABLE Hospedaje(
IdentificadorReserva VARCHAR(10) NOT NULL,
NumeroParcela SMALLINT NOT NULL,
PRIMARY KEY(IdentificadorReserva, NumeroParcela),
FOREIGN KEY(IdentificadorReserva) REFERENCES Reservacion(IdentificadorReserva),
FOREIGN KEY(NumeroParcela) REFERENCES Parcela(NumeroParcela)
);

CREATE TABLE HospederoRealiza(
IdentificacionHospedero CHAR(20) NOT NULL,
IdentificadorReserva VARCHAR(10) NOT NULL,
ComprobantePago CHAR(6) NOT NULL,
PRIMARY KEY(IdentificacionHospedero, IdentificadorReserva, ComprobantePago),
FOREIGN KEY (IdentificacionHospedero) REFERENCES Hospedero(Identificacion),
FOREIGN KEY (IdentificadorReserva) REFERENCES Reservacion(IdentificadorReserva),
FOREIGN KEY (ComprobantePago) REFERENCES Pago(Comprobante)
);

CREATE TABLE HospederoSolicita(
TipoServicio VARCHAR(50) NOT NULL,
IdentificacionHospedero CHAR(20) NOT NULL,
ComprobantePago CHAR(6) NOT NULL,
Precio DOUBLE PRECISION,
Cantidad SMALLINT,
PRIMARY KEY(TipoServicio, IdentificacionHospedero, ComprobantePago),
FOREIGN KEY(TipoServicio) REFERENCES Servicio(Tipo),
FOREIGN KEY(IdentificacionHospedero) REFERENCES Hospedero(Identificacion),
FOREIGN KEY(ComprobantePago) REFERENCES Pago(Comprobante)
);

CREATE TABLE VisitaSolicita(
TipoServicio VARCHAR(50) NOT NULL,
IdentificacionVisita CHAR(20) NOT NULL,
ComprobantePago CHAR(6) NOT NULL,
Precio DOUBLE PRECISION,
Cantidad SMALLINT,
PRIMARY KEY(TipoServicio, IdentificacionVisita, Precio),
FOREIGN KEY(TipoServicio) REFERENCES Servicio(Tipo),
FOREIGN KEY(IdentificacionVisita) REFERENCES Visita(Identificacion),
FOREIGN KEY(ComprobantePago) REFERENCES Pago(Comprobante)
);

--Se agrega un atributo cantidad de personas total a la tabla de Reservacion

ALTER TABLE Reservacion
ADD CantidadTotalPersonas SMALLINT;


--Datos de prueba para el nuevo atributo
UPDATE Reservacion
SET CantidadTotalPersonas = '5'
WHERE Reservacion.IdentificadorReserva = '3957409889';

UPDATE Reservacion
SET CantidadTotalPersonas = '5'
WHERE Reservacion.IdentificadorReserva = '2595141556';


UPDATE Reservacion
SET CantidadTotalPersonas = '20'
WHERE Reservacion.IdentificadorReserva = '3463933048';

UPDATE Reservacion
SET CantidadTotalPersonas = '1'
WHERE Reservacion.IdentificadorReserva = '5396857873';

UPDATE Reservacion
SET CantidadTotalPersonas = '9'
WHERE Reservacion.IdentificadorReserva = '8865933684';

UPDATE Reservacion
SET CantidadTotalPersonas = '5'
WHERE Reservacion.IdentificadorReserva = '8BX2Wag6c7';

UPDATE Reservacion
SET CantidadTotalPersonas = '40'
WHERE Reservacion.IdentificadorReserva = '9149985005';

UPDATE Reservacion
SET CantidadTotalPersonas = '7'
WHERE Reservacion.IdentificadorReserva = '9932098365';

UPDATE Reservacion
SET CantidadTotalPersonas = '7'
WHERE Reservacion.IdentificadorReserva = 'XDHpYSWAWB';


