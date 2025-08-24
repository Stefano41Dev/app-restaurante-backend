
use master
if DB_ID('db_restaurante') is not null

	ALTER DATABASE db_restaurante
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

    -- Eliminar la base de datos
    DROP DATABASE db_restaurante;
go

CREATE DATABASE db_restaurante
GO

USE db_restaurante
GO

--usuarios
CREATE TABLE usuarios
(
    id       int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    nombre   varchar(255) NOT NULL,
    apellido varchar(255) NOT NULL,
    correo   varchar(255) UNIQUE NOT NULL,
    clave    varchar(255) NOT NULL ,
    rol      varchar(255) CHECK (rol IN ('ADMINISTRADOR', 'CAJERO', 'CAMARERO')) NOT NULL,
    activo   bit
)
GO

--mesas
CREATE TABLE mesas
(
    id        smallint IDENTITY (1, 1) NOT NULL,
    numero    varchar(255),
    capacidad smallint,
    estado    varchar(255),
    activo    bit,
    CONSTRAINT pk_mesas PRIMARY KEY (id)
)
GO

--ordenes
CREATE TABLE ordenes
(
    id             bigint IDENTITY (1, 1) NOT NULL,
    codigo_orden   varchar(14),
    mesa_id        smallint               NOT NULL,
    estado         varchar(255),
    fecha_creacion date,
    hora_creacion  time,
    monto_subtotal float(53),
    monto_total    float(53),
    activo         bit,
    CONSTRAINT pk_ordenes PRIMARY KEY (id)
)
GO

ALTER TABLE ordenes
    ADD CONSTRAINT uc_ordenes_codigoorden UNIQUE (codigo_orden)
GO

ALTER TABLE ordenes
    ADD CONSTRAINT FK_ORDENES_ON_MESA FOREIGN KEY (mesa_id) REFERENCES mesas (id)
GO

--categoria
CREATE TABLE categorias_items
(
    id            smallint IDENTITY (1, 1) NOT NULL,
    nombre        varchar(255),
    descripcion   varchar(255),
    precio_minimo float(53),
    activo        bit,
    CONSTRAINT pk_categorias_items PRIMARY KEY (id)
)
GO

ALTER TABLE categorias_items
    ADD CONSTRAINT uc_categorias_items_nombre UNIQUE (nombre)
GO

--item_menu
CREATE TABLE items_menu
(
    id            int IDENTITY (1, 1) NOT NULL,
    nombre        varchar(255),
    descripcion   varchar(255),
    precio        float(53),
    enlace_imagen varchar(255),
    categoria_id  smallint            NOT NULL,
    estado        varchar(255),
    activo        bit,
    CONSTRAINT pk_items_menu PRIMARY KEY (id)
)
GO

ALTER TABLE items_menu
    ADD CONSTRAINT uc_items_menu_nombre UNIQUE (nombre)
GO

ALTER TABLE items_menu
    ADD CONSTRAINT FK_ITEMS_MENU_ON_CATEGORIA FOREIGN KEY (categoria_id) REFERENCES categorias_items (id)
GO

--detalle_ordenes
CREATE TABLE detalle_ordenes
(
    id              bigint IDENTITY (1, 1) NOT NULL,
    orden_id        bigint                 NOT NULL,
    plato_id        int                    NOT NULL,
    cantidad        int,
    precio_unitario float(53),
    igv             float(53),
    subtotal        float(53),
    total           float(53),
    activo          bit,
    CONSTRAINT pk_detalle_ordenes PRIMARY KEY (id)
)
GO

ALTER TABLE detalle_ordenes
    ADD CONSTRAINT FK_DETALLE_ORDENES_ON_ORDEN FOREIGN KEY (orden_id) REFERENCES ordenes (id)
GO

ALTER TABLE detalle_ordenes
    ADD CONSTRAINT FK_DETALLE_ORDENES_ON_PLATO FOREIGN KEY (plato_id) REFERENCES items_menu (id)
GO
