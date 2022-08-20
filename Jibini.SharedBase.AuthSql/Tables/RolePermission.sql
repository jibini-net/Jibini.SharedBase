CREATE TABLE [dbo].[RolePermission] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [RoleId]       INT NOT NULL,
    [PermissionId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RolePermission_ToPermission] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permission] ([Id]),
    CONSTRAINT [FK_RolePermission_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_RolePermission_RoleId]
    ON [dbo].[RolePermission]([RoleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RolePermission_PermissionId]
    ON [dbo].[RolePermission]([PermissionId] ASC);

