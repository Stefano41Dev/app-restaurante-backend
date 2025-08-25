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
    rol      varchar(255) CHECK (rol IN ('Administrador', 'Cajero', 'Camarero')) NOT NULL,
    activo   bit
)
GO

--mesas
CREATE TABLE mesas
(
    id        smallint IDENTITY (1, 1) NOT NULL,
    numero    varchar(255),
    capacidad smallint,
    estado    varchar(255) CHECK (estado IN ('LIBRE', 'OCUPADO')) NOT NULL DEFAULT 'LIBRE',
    activo    bit default 1,
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
    estado        varchar(255) CHECK (estado IN ('DISPONIBLE', 'DESHABILITADO')) NOT NULL,
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

-- USUARIO 'Piero', ROL 'Administrador', CLAVE: piero123
-- USUARIO 'Hawell', ROL 'Cajero', CLAVE: hawell123
-- USUARIO 'Stefano', ROL 'Camarero', CLAVE: stefano123
INSERT INTO usuarios (nombre, apellido, correo, clave, rol, activo) VALUES ('Piero', 'Juarez', 'piero@gmail.com', '8e8e0e7ad7ee46c4b9863e68c9e5e0df3ae370ba1e16a7bcdbfe2a79493c5685', 'Administrador', 1)
INSERT INTO usuarios (nombre, apellido, correo, clave, rol, activo) VALUES ('Hawell', 'Urbina', 'hawell@gmail.com', 'bc857eeb73c814aba1d1b5fe68148bea01e1507d058349b77e56a79d27302bd0', 'Cajero', 1)
INSERT INTO usuarios (nombre, apellido, correo, clave, rol, activo) VALUES ('Stefano', 'Gonzales', 'stefano@gmail.com', 'd78d06ebae2aee16bd445eacc7e0134af1eac372b177b67d876af91c454e13b1', 'Camarero', 1)
GO

INSERT INTO categorias_items (activo, descripcion, nombre, precio_minimo) VALUES (1, 'Delicosas hamburguesas jugosas', 'Hamburguesas',10.0)
INSERT INTO categorias_items (activo, descripcion, nombre, precio_minimo) VALUES (1, 'Refrescantes bebidas gaseosas de siempre', 'Gaseosas',3.0)
INSERT INTO categorias_items (activo, descripcion, nombre, precio_minimo) VALUES (1, 'Platillos del norte que te harán babear', 'Comida Norteña',10.0)
INSERT INTO categorias_items (activo, descripcion, nombre, precio_minimo) VALUES (1, 'Platos tipicos de la selva', 'Comida de la Selva',15.0)
INSERT INTO categorias_items (activo, descripcion, nombre, precio_minimo) VALUES (1, 'Aperitivos', 'Entradas',8.0)
INSERT INTO categorias_items (activo, descripcion, nombre, precio_minimo) VALUES (1, 'Platillos abundantes de fideos', 'Pastas',15.0)
INSERT INTO categorias_items (activo, descripcion, nombre, precio_minimo) VALUES (1, 'Dulces tradicionales para endulzar el paladar', 'Postres',10.0)
GO


INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 1, 14, 'Hamburguesa triple parrillera ahumada al carbón', 'Hamburguesa Parrillera', 'https://i.ibb.co/Pzt1Zb2V/3-DLCu5-Wt2-Nmyqbc-E5-1080-x.webp', 'DISPONIBLE')
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 2, 3.5, 'Gaseosa con sabor a hierba luisa', 'Inka Kola 1L', 'https://i.ibb.co/VckFj98Z/20111231.webp', 'DISPONIBLE')
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 2, 6, 'Gaseosa con sabor a hierba luisa', 'Inka Kola 2.5L', 'https://i.ibb.co/Y76CTBcH/20256774.webp', 'DESHABILITADO')
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 3, 26, 'Delicioso arroz con pollo', 'Arroz con pollo y crema huancaina', 'https://i.ibb.co/Wv7pK51f/hq720.jpg', 'DISPONIBLE')
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 3, 34, 'Fijoles con pollo a la olla y arroz', 'Frijoles con pollo a la olla', 'https://i.ibb.co/ns6stpNq/sddefault.jpg', 'DESHABILITADO')
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 3, 23, 'Pure de papa de alta calidad para desgustar', 'Seco de pollo con pure de papa', 'https://i.ibb.co/pF5ntFt/maxresdefault.jpg', 'DISPONIBLE')
GO

-- Hamburguesas (categoria_id = 1)
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 1, 18, 'Hamburguesa con huevo frito y plátano frito al estilo peruano', 'Hamburguesa Criolla', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756146355/tqtaxvd5lhzvglhgqn8v.jpg', 'DISPONIBLE');
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 1, 20, 'Hamburguesa con ají amarillo y salsa huancaína', 'Hamburguesa Huancaína', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756146538/k15djglpwf4rbqlsrh9c.jpg', 'DISPONIBLE');
-- Gaseosas (categoria_id = 2)
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 2, 4, 'Bebida de sabor a cola, clásica en Perú', 'Coca Cola 500ml', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756146765/nuvzcsjjb254xhma4o1r.jpg', 'DISPONIBLE');
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 2, 4, 'Bebida de sabor a cola, clásica en Perú', 'Coca Cola 3L', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756151250/zydbzt6dyyilvbasfww4.jpg', 'DISPONIBLE');
-- Comida Norte�a (categoria_id = 3)
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 3, 28, 'Ceviche norteño fresco con camote y choclo', 'Ceviche de Pescado', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756147059/yn8ll694rn8msgqqfikq.jpg', 'DISPONIBLE');
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 3, 30, 'Arroz con cabrito tierno y jugoso, típico del norte', 'Arroz con Cabrito', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756147287/l246oazbdhytoka1ubs2.jpg', 'DISPONIBLE');
-- Comida de la Selva (categoria_id = 4)
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 4, 22, 'Juane tradicional envuelto en hojas de bijao', 'Juane de Gallina', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756148112/q8pdujvpt8gq6ue3ekln.jpg', 'DISPONIBLE');
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 4, 25, 'Tacacho con cecina y chorizo amazónico', 'Tacacho con Cecina', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756148205/srshzhoxxskhtkyn0mpb.jpg', 'DISPONIBLE');
-- Entradas (categoria_id = 5)
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 5, 12, 'Papa bañada en salsa huancaína con huevo y aceituna', 'Papa a la Huancaína', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756148300/gnesdlj3terkuxtq5rnp.jpg', 'DISPONIBLE');
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 5, 14, 'Causa limeña con pollo y palta fresca', 'Causa Limeña de Pollo', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756148376/vbi0tlaaz7izcms1yucs.jpg', 'DISPONIBLE');
-- Pastas (categoria_id = 6)
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 6, 22, 'Tallarin verde al pesto peruano con bistec apanado', 'Tallarín Verde con Bistec', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756148469/von63srfdto8jplrrhco.jpg', 'DISPONIBLE');
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 6, 24, 'Tallarin rojo criollo con pollo guisado', 'Tallarín Rojo con Pollo', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756150619/wlm8rqkfk3dz2dx0pus1.jpg', 'DISPONIBLE');
-- Postres (categoria_id = 7)
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 7, 10, 'Postre limeño de manjar y merengue', 'Suspiro a la Limeña', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756150853/eq193w5m4ktjx6ztawli.png', 'DISPONIBLE');
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 7, 9, 'Postre a base de maíz morado con frutas', 'Mazamorra Morada', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756150944/tmrj1slqeyfiovjfpfx7.jpg', 'DISPONIBLE');
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 7, 15, 'Arroz con leche con canela y pasas', 'Arroz con Leche', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756151026/fxg2osvlewwuiykkbgfp.jpg', 'DISPONIBLE');
INSERT INTO items_menu (activo, categoria_id, precio, descripcion, nombre, enlace_imagen, estado) VALUES (1, 7, 12, 'Dulce tradicional peruano de yuca rallada', 'Picarones con Miel', 'https://res.cloudinary.com/dmqzrq0w7/image/upload/v1756151111/o54oauwvczdkpqqoqzus.jpg', 'DISPONIBLE');
GO

INSERT INTO mesas (capacidad, numero, estado) VALUES (4, '1', 'LIBRE')
INSERT INTO mesas (capacidad, numero, estado) VALUES (4, '2', 'LIBRE')
GO
--a�adiendo 6 mesas
INSERT INTO mesas (capacidad, numero, estado) VALUES (5, '3', 'LIBRE')
INSERT INTO mesas (capacidad, numero, estado) VALUES (10, '4', 'LIBRE')
INSERT INTO mesas (capacidad, numero, estado) VALUES (2, '5', 'LIBRE')
INSERT INTO mesas (capacidad, numero, estado) VALUES (4, '6', 'LIBRE')
INSERT INTO mesas (capacidad, numero, estado) VALUES (5, '7', 'LIBRE')
INSERT INTO mesas (capacidad, numero, estado) VALUES (10, '8', 'LIBRE')
GO

SELECT @@SERVERNAME;