CREATE DATABASE IC_Estoque;
USE IC_Estoque;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

CREATE TABLE `Produtos` (
    `Id` INTEGER NOT NULL,
    `Nome` NVARCHAR(150) NOT NULL,
    `Valor` REAL NOT NULL,
    `Quantidade` INTEGER NOT NULL,
    CONSTRAINT `PK_Produtos` PRIMARY KEY (`Id`)
);

CREATE INDEX `IX_Produtos_Nome` ON `Produtos`(`Nome`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230808164038_InitialMigration', '7.0.10');

COMMIT;

