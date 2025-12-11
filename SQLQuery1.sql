ALTER TABLE Reserva
ADD Codigo_unico NVARCHAR(6) NOT NULL DEFAULT '';

ALTER TABLE Reserva
ADD CONSTRAINT UQ_Reserva_Codigo_unico UNIQUE (Codigo_unico);
