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

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
VALUES (N'1', N'2c467309-7e89-4730-8c6c-e5bfeed66ad7', N'User', N'USER');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
VALUES (N'2', N'67ae3d67-9521-494e-b2b6-0529f2179eb3', N'Admin', N'ADMIN');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] OFF;
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250607130604_AddIdentity', N'6.0.36');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserTokens]') AND [c].[name] = N'Name');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [AspNetUserTokens] ALTER COLUMN [Name] nvarchar(450) NOT NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserTokens]') AND [c].[name] = N'LoginProvider');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [AspNetUserTokens] ALTER COLUMN [LoginProvider] nvarchar(450) NOT NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserLogins]') AND [c].[name] = N'ProviderKey');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [AspNetUserLogins] ALTER COLUMN [ProviderKey] nvarchar(450) NOT NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserLogins]') AND [c].[name] = N'LoginProvider');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [AspNetUserLogins] ALTER COLUMN [LoginProvider] nvarchar(450) NOT NULL;
GO

CREATE TABLE [AspNetRole] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(max) NULL,
    [NormalizedName] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRole] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUser] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(max) NULL,
    [NormalizedUserName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [NormalizedEmail] nvarchar(max) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUser] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Image] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [StatusCat] bit NULL,
    [ParentId] int NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_categories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_categories_categories_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [categories] ([Id])
);
GO

CREATE TABLE [discounts] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [DiscountPercentage] decimal(18,2) NOT NULL,
    [Status] tinyint NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    CONSTRAINT [PK_discounts] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaim] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaim] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaim_AspNetRole_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRole] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetRoleAspNetUser] (
    [RolesId] nvarchar(450) NOT NULL,
    [UsersId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetRoleAspNetUser] PRIMARY KEY ([RolesId], [UsersId]),
    CONSTRAINT [FK_AspNetRoleAspNetUser_AspNetRole_RolesId] FOREIGN KEY ([RolesId]) REFERENCES [AspNetRole] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetRoleAspNetUser_AspNetUser_UsersId] FOREIGN KEY ([UsersId]) REFERENCES [AspNetUser] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaim] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaim] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaim_AspNetUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUser] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogin] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogin] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogin_AspNetUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUser] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserToken] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [FK_AspNetUserToken_AspNetUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUser] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [carts] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [Status] tinyint NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_carts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_carts_AspNetUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUser] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [toys] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Image] nvarchar(max) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Discount] decimal(18,2) NULL,
    [Description] nvarchar(max) NULL,
    [Status] bit NULL,
    [CategoryId] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_toys] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_toys_categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [categories] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [orders] (
    [Id] int NOT NULL IDENTITY,
    [TotalAmount] decimal(18,2) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    [DiscountId] int NULL,
    [ShippingAddress] nvarchar(max) NOT NULL,
    [PaymentMethod] tinyint NOT NULL,
    [Status] tinyint NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_orders_AspNetUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUser] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_orders_discounts_DiscountId] FOREIGN KEY ([DiscountId]) REFERENCES [discounts] ([Id])
);
GO

CREATE TABLE [cart_Details] (
    [Id] int NOT NULL IDENTITY,
    [ToyId] int NOT NULL,
    [CartId] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Quentity] int NOT NULL,
    [Discount] int NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_cart_Details] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_cart_Details_carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [carts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_cart_Details_toys_ToyId] FOREIGN KEY ([ToyId]) REFERENCES [toys] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [comments] (
    [Id] int NOT NULL IDENTITY,
    [ToyComment] nvarchar(max) NOT NULL,
    [ToyId] int NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    [Status] tinyint NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_comments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_comments_AspNetUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUser] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_comments_toys_ToyId] FOREIGN KEY ([ToyId]) REFERENCES [toys] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [order_Details] (
    [Id] int NOT NULL IDENTITY,
    [Price] decimal(18,2) NOT NULL,
    [TotalPrice] decimal(18,2) NOT NULL,
    [ToyId] int NOT NULL,
    [OrderId] int NOT NULL,
    [Quantity] int NOT NULL,
    [Discount] int NULL,
    [Status] tinyint NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_order_Details] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_order_Details_orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [orders] ([Id]) ON DELETE CASCADE
);
GO

UPDATE [AspNetRoles] SET [ConcurrencyStamp] = N'ae4a3f3f-889a-4bd1-afff-9e4ccf9cf7d7'
WHERE [Id] = N'1';
SELECT @@ROWCOUNT;

GO

UPDATE [AspNetRoles] SET [ConcurrencyStamp] = N'dc405251-0711-40bf-b8fe-c662f9b6fb21'
WHERE [Id] = N'2';
SELECT @@ROWCOUNT;

GO

CREATE INDEX [IX_AspNetRoleAspNetUser_UsersId] ON [AspNetRoleAspNetUser] ([UsersId]);
GO

CREATE INDEX [IX_AspNetRoleClaim_RoleId] ON [AspNetRoleClaim] ([RoleId]);
GO

CREATE INDEX [IX_AspNetUserClaim_UserId] ON [AspNetUserClaim] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogin_UserId] ON [AspNetUserLogin] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserToken_UserId] ON [AspNetUserToken] ([UserId]);
GO

CREATE INDEX [IX_cart_Details_CartId] ON [cart_Details] ([CartId]);
GO

CREATE INDEX [IX_cart_Details_ToyId] ON [cart_Details] ([ToyId]);
GO

CREATE INDEX [IX_carts_UserId] ON [carts] ([UserId]);
GO

CREATE INDEX [IX_categories_ParentId] ON [categories] ([ParentId]);
GO

CREATE INDEX [IX_comments_ToyId] ON [comments] ([ToyId]);
GO

CREATE INDEX [IX_comments_UserId] ON [comments] ([UserId]);
GO

CREATE INDEX [IX_order_Details_OrderId] ON [order_Details] ([OrderId]);
GO

CREATE INDEX [IX_orders_DiscountId] ON [orders] ([DiscountId]);
GO

CREATE INDEX [IX_orders_UserId] ON [orders] ([UserId]);
GO

CREATE INDEX [IX_toys_CategoryId] ON [toys] ([CategoryId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260228223212_AddProjectTables', N'6.0.36');
GO

COMMIT;
GO

