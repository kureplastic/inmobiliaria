-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 18-04-2023 a las 05:27:20
-- Versión del servidor: 10.4.25-MariaDB
-- Versión de PHP: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `Id` int(8) UNSIGNED ZEROFILL NOT NULL,
  `InquilinoId` int(8) UNSIGNED ZEROFILL NOT NULL,
  `PropietarioId` int(8) UNSIGNED ZEROFILL NOT NULL,
  `InmuebleId` int(8) NOT NULL,
  `fechaInicio` datetime NOT NULL,
  `fechaFin` datetime NOT NULL,
  `MontoMensual` decimal(8,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`Id`, `InquilinoId`, `PropietarioId`, `InmuebleId`, `fechaInicio`, `fechaFin`, `MontoMensual`) VALUES
(00000001, 00000004, 00000001, 1, '2023-04-16 23:22:19', '2023-04-21 23:22:19', '30000');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `Id` int(8) NOT NULL,
  `Direccion` varchar(255) NOT NULL,
  `Tipo` varchar(255) NOT NULL,
  `Ambientes` int(8) UNSIGNED NOT NULL,
  `Precio` decimal(8,0) NOT NULL,
  `Estado` tinyint(1) NOT NULL,
  `PropietarioId` int(8) UNSIGNED ZEROFILL NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`Id`, `Direccion`, `Tipo`, `Ambientes`, `Precio`, `Estado`, `PropietarioId`) VALUES
(1, 'Las Heras 840', 'Departamento', 2, '20000', 1, 00000001),
(2, 'Otra Dire', 'Casa', 3, '25500', 0, 00000001),
(3, 'Direccion actualizada 2', 'Casa', 1, '25500', 1, 00000002);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `Id` int(8) UNSIGNED ZEROFILL NOT NULL,
  `dni` varchar(255) NOT NULL,
  `Nombre` varchar(255) NOT NULL,
  `Apellido` varchar(255) NOT NULL,
  `LugarTrabajo` varchar(255) DEFAULT '',
  `Telefono` varchar(255) NOT NULL,
  `Email` varchar(255) DEFAULT '',
  `NombreGarante` varchar(255) NOT NULL,
  `DniGarante` varchar(255) NOT NULL,
  `TelefonoGarante` varchar(255) NOT NULL,
  `EmailGarante` varchar(255) DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`Id`, `dni`, `Nombre`, `Apellido`, `LugarTrabajo`, `Telefono`, `Email`, `NombreGarante`, `DniGarante`, `TelefonoGarante`, `EmailGarante`) VALUES
(00000004, '41727953', 'Valu', 'Gonzalez', 'Poder Judicial', '4141231', 'uncorreo@gmail.com', 'Franco Gonzalez', '37811667', '255232323', 'kureplastic@gmail.com'),
(00000007, '123456345', 'PEPE', 'PEREZ', '', '02665017100', 'kureplastic@gmail.com', 'Franco Gonzalez', '37811667', '02665017100', 'kurefg@outlook.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `Id` int(8) UNSIGNED ZEROFILL NOT NULL,
  `ContratoId` int(8) UNSIGNED ZEROFILL NOT NULL,
  `numPago` int(8) NOT NULL,
  `fechaPago` datetime NOT NULL,
  `importe` decimal(8,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id` int(8) UNSIGNED ZEROFILL NOT NULL,
  `dni` varchar(255) NOT NULL,
  `apellido` varchar(255) NOT NULL,
  `nombre` varchar(255) NOT NULL,
  `Telefono` varchar(255) DEFAULT NULL,
  `email` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id`, `dni`, `apellido`, `nombre`, `Telefono`, `email`) VALUES
(00000001, '37811667', 'GONZALEZ', 'FRANCO', '2665017100', 'frangesgonzalez@gmail.com'),
(00000002, '37811668', 'Gonzalez', 'Vale', '02665017100', 'kureplastic@gmail.com');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `foranea_propietario` (`PropietarioId`),
  ADD KEY `foranea_inquilino` (`InquilinoId`),
  ADD KEY `foranea_inmueble` (`InmuebleId`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `foranea_propietario` (`PropietarioId`) USING BTREE;

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `dni unico` (`dni`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `foranea_contrato` (`ContratoId`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email unico` (`email`),
  ADD UNIQUE KEY `dni unico` (`dni`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `Id` int(8) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `Id` int(8) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id` int(8) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `Id` int(8) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id` int(8) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `foranea_inmueble` FOREIGN KEY (`InmuebleId`) REFERENCES `inmuebles` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `foranea_inquilino` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilinos` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `foranea_propietario` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `foranea` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`id`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `foranea_contrato` FOREIGN KEY (`ContratoId`) REFERENCES `contratos` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
