IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [OTP] (
    [Id] int NOT NULL IDENTITY,
    [EmailAddress] nvarchar(max) NULL,
    [OTPCode] nvarchar(max) NULL,
    [RetryCount] int NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_OTP] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231029103657_InitialMigration', N'7.0.13');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231029133006_UpdatedSnapshot', N'7.0.13');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[OTP].[CreatedDate]', N'CreatedDates', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231116104135_migration1', N'7.0.13');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[OTP].[CreatedDates]', N'CreatedDate', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231116104508_migration2', N'7.0.13');
GO

COMMIT;
GO

