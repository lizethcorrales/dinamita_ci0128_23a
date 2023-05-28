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
Estado BIT, -- se borra en el sprint 2
NombrePais VARCHAR(30) NOT NULL, -- esto se borra en nuevas actualizaciones
PRIMARY KEY (Identificacion),
FOREIGN KEY(NombrePais) REFERENCES Pais (Nombre), -- esto se borra en nuevas actualizaciones
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
ColorBrazalete VARCHAR(20), -- se borra en sprint 2
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
FechaEntrada DATE NOT NULL, -- se le agrega porque es parte de la llave primaria de visita
Nacionalidad VARCHAR(30) NOT NULL,
Poblacion VARCHAR(30) NOT NULL,
Actividad VARCHAR(30) NOT NULL,
Cantidad SMALLINT,
PrecioActual DOUBLE PRECISION,
PRIMARY KEY(IdentificacionVisita, FechaEntrada, Nacionalidad, Poblacion, Actividad),
FOREIGN KEY (IdentificacionVisita, FechaEntrada) REFERENCES Visita (Identificacion, FechaEntrada),
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
FOREIGN KEY(TipoServicio) REFERENCES Servicios(Tipo),
FOREIGN KEY(IdentificacionHospedero) REFERENCES Hospedero(Identificacion),
FOREIGN KEY(ComprobantePago) REFERENCES Pago(Comprobante)
);

CREATE TABLE VisitaSolicita(
TipoServicio VARCHAR(50) NOT NULL,
IdentificacionVisita CHAR(20) NOT NULL,
FechaEntrada DATE NOT NULL, -- se le agrega porque es parte de la llave primaria de visita
ComprobantePago CHAR(6) NOT NULL,
Precio DOUBLE PRECISION,
Cantidad SMALLINT,
PRIMARY KEY(TipoServicio, IdentificacionVisita, Precio),
FOREIGN KEY(TipoServicio) REFERENCES Servicios(Tipo),
FOREIGN KEY(IdentificacionVisita, FechaEntrada) REFERENCES Visita(Identificacion, FechaEntrada),
FOREIGN KEY(ComprobantePago) REFERENCES Pago(Comprobante)
);

-- Insertar tuplas de prueba

INSERT INTO Pais 
VALUES ('Costa Rica'), ('Panama'), ('Alemania'), ('Estados Unidos'), ('Espa�a'), ('Inglaterra')

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


INSERT INTO Hospedero
VALUES ('206780989', 'hoquadoibizo-2894@yopmail.com','Mateo', 'Barrantes', 'Garc�a', '0', 'Costa Rica'),
		('906750989', 'kucrexawagreu-9178@yopmail.com','Allen', 'Quesada', 'Sanchez', '0', 'Estados Unidos'),
		('806550769', 'loinazudelu-1663@yopmail.com','Gwen', 'Rodriguez', 'Chac�n', '0', 'Estados Unidos'),
		('106950745', 'kudelayequau-3433@yopmail.com','Ana', 'Smith', 'Morales', '0', 'Costa Rica'),
		('307980965', 'lappiddebrouyau-4416@yopmail.com','Gerardo', 'Brenes', 'Suarez', '0', 'Alemania'),
		('404580125', 'criprauquijeura-2692@yopmail.com','Nicolle', 'Hern�ndez', 'Matarrita', '0', 'Costa Rica'),
		('503890135', 'valayouxeiga-6823@yopmail.com','Samuel', 'Zamora', 'Miranda', '0', 'Espa�a')


INSERT INTO Reservacion 
VALUES ('8865933684', '2023-06-01', '2023-06-03', '37290'),
		('9932098365', '2023-05-11', '2023-05-13', '126.56'),
		('2595141556', '2023-05-17', '2023-05-20', '74.58'),
		('3957409889', '2023-05-11', '2023-05-14', '20340'),
		('3463933048', '2023-05-17', '2023-05-20', '361.6'),
		('9149985005', '2023-05-24', '2023-05-26', '180800'),
		('5396857873', '2023-05-31', '2023-06-03', '18.08')


INSERT INTO Placa
VALUES ('8865933684', 'bgd-125'),
		('9932098365', 'rfd-454'),
		('2595141556', 'sdf-456'),
		('3957409889', 'gtr-455'),
		('3463933048', 'rtt-789'),
		('9149985005', 'ter-987'),
		('5396857873', 'fgd-478')

INSERT INTO Tarifa
VALUES ('Nacional', 'Adulto', 'Picnic', '2260', ''),
		('Nacional', 'Ni�o', 'Picnic', '1130', ''),
		('Extranjero', 'Adulto', 'Picnic', '13.56', ''),
		('Extranjero', 'Ni�o', 'Picnic', '5.65', ''),
		('Nacional', 'Adulto', 'Camping', '4520', ''),
		('Nacional', 'Ni�o', 'Camping', '3390', ''),
		('Extranjero', 'Adulto', 'Camping', '18.08', ''),
		('Extranjero', 'Ni�o', 'Camping', '10.17', '')

INSERT INTO PrecioReservacion
VALUES ('8865933684', 'Nacional', 'Adulto', 'Camping', '6', '4520'),
		('8865933684', 'Nacional', 'Ni�o', 'Camping', '3', '3390'),
		('9932098365', 'Extranjero', 'Adulto', 'Camping', '7', '18.08'),
		('2595141556', 'Extranjero', 'Adulto', 'Camping', '3', '18.08'),
		('2595141556', 'Extranjero', 'Ni�o', 'Camping', '2', '10.17'),
		('3957409889', 'Nacional', 'Adulto', 'Camping', '3', '4520'),
		('3957409889', 'Nacional', 'Ni�o', 'Camping', '2', '3390'),
		('3463933048', 'Extranjero', 'Adulto', 'Camping', '20', '18.08'),
		('9149985005', 'Nacional', 'Adulto', 'Camping', '40', '4520'),
		('5396857873', 'Extranjero', 'Adulto', 'Camping', '1', '18.08')

INSERT INTO Parcela
VALUES ('1', '10'),
		('2', '10'),
		('3', '20'),
		('4', '2'),
		('5', '10'),
		('6', '10'),
		('7', '10')

INSERT INTO CaracteristicasParcela
VALUES ('1', 'Frente a Playa', '121'),
		('2', 'Frente a Playa', '121'),
		('3', 'Frente a Playa', '181'),
		('4', 'Bajo el sol', '120'),
		('5', 'Bajo el sol', '121'),
		('6', 'Frente a Playa', '121'),
		('7', 'Bajo el sol', '121')

INSERT INTO Hospedaje 
VALUES ('8865933684', '1'),
		('9932098365', '2'),
		('2595141556', '5'),
		('3957409889', '6'),
		('3463933048', '3'),
		('9149985005', '3'),
		('9149985005', '7'),
		('9149985005', '2'),
		('5396857873', '4')

INSERT INTO Pago
VALUES ('908967', '2023-05-25'),
		('345689', '2023-05-05'),
		('122334', '2023-05-10'),
		('675498', '2023-05-03'),
		('211345', '2023-05-01'),
		('808765', '2023-05-12'),
		('544992', '2023-05-20')

INSERT INTO HospederoRealiza
VALUES ('206780989', '8865933684', '908967'),
		('906750989', '9932098365', '345689'),
		('806550769', '2595141556', '122334'),
		('106950745', '3957409889', '675498'),
		('307980965', '3463933048', '211345'),
		('404580125', '9149985005', '808765'),
		('503890135', '5396857873', '544992')

-- Modificaciones y mejoras a las tablas 
ALTER TABLE Reservacion 
ADD Estado INT CHECK (Estado >= 0 AND Estado <= 2); 


-- se elimina la llave foranea de nombrePais con Pais de la tabla Hospedero

-- Creacion de una nueva tabla para la cantidad de personas por pais en una reserva

CREATE TABLE TieneNacionalidad(
IdentificadorReserva VARCHAR(10) NOT NULL,
NombrePais VARCHAR(30) NOT NULL,
Cantidad SMALLINT,
PRIMARY KEY (IdentificadorReserva, NombrePais),
FOREIGN KEY (IdentificadorReserva) REFERENCES Reservacion(IdentificadorReserva),
FOREIGN KEY (NombrePais) REFERENCES Pais(Nombre)
);

--Se le agregan valores nuevos de prueba al nuevo atributo de la tabla Reservacion
UPDATE Reservacion
SET Estado = '0';

-- Se agregan datos de prueba a la nueva tabla de nacionalidades

INSERT INTO TieneNacionalidad
VALUES ('8865933684', 'Costa Rica', '9'), 
		('2595141556', 'Estados Unidos', '5'),
		('3463933048', 'Alemania', '20'),
		('3957409889', 'Costa Rica', '5'),
		('5396857873', 'Espa�a', '1'),
		('9149985005', 'Costa Rica', '40'),
		('9932098365', 'Inglaterra', '7')


-- Sprint 2

CREATE TABLE NacionalidadVisita  (
IdentificadorVisita CHAR(20) NOT NULL,
FechaEntrada DATE NOT NULL,
NombrePais VARCHAR(30) NOT NULL,
Cantidad SMALLINT,
PRIMARY KEY (IdentificadorVisita, FechaEntrada, NombrePais),
FOREIGN KEY (IdentificadorVisita, FechaEntrada) REFERENCES Visita(Identificacion, FechaEntrada),
FOREIGN KEY (NombrePais) REFERENCES Pais(Nombre)
);

ALTER TABLE Hospedero
ADD Telefono VARCHAR(20);

ALTER TABLE Reservacion
ADD Motivo VARCHAR(30);