CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

CREATE TABLE `ProdutosEmEstoque` (
    `Id` INTEGER NOT NULL,
    `Nome` TEXT NULL,
    `Quantidade` INTEGER NOT NULL,
    CONSTRAINT `PK_ProdutosEmEstoque` PRIMARY KEY (`Id`)
);

CREATE TABLE `Projetos` (
    `Id` INTEGER NOT NULL,
    `Nome` TEXT NOT NULL,
    `Status` TEXT NOT NULL,
    `DataInicio` TEXT NOT NULL,
    `DataEntrega` TEXT NOT NULL,
    `ProdutoUtilizadoId` INTEGER NOT NULL,
    `QuantidadeUtilizado` INTEGER NOT NULL,
    `Descricao` TEXT NULL,
    `Valor` REAL NOT NULL,
    `RowVersion` BLOB NULL,
    CONSTRAINT `PK_Projetos` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Projetos_ProdutosEmEstoque_ProdutoUtilizadoId` FOREIGN KEY (`ProdutoUtilizadoId`) REFERENCES `ProdutosEmEstoque` (`Id`)
);

CREATE INDEX `IX_Projetos_Nome` ON `Projetos` (`Nome`);

CREATE INDEX `IX_Projetos_ProdutoUtilizadoId` ON `Projetos` (`ProdutoUtilizadoId`);

CREATE INDEX `IX_Projetos_Status` ON `Projetos` (`Status`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230828121048_Realizado ajustes de migration', '7.0.9');

COMMIT;

