CREATE TABLE [dbo].[IdentityPermission] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [IdentityId]   INT NOT NULL,
    [PermissionId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_IdentityPermission_ToIdentity] FOREIGN KEY ([IdentityId]) REFERENCES [dbo].[Identity] ([Id]),
    CONSTRAINT [FK_IdentityPermission_ToPermission] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permission] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_IdentityPermission_IdentityId]
    ON [dbo].[IdentityPermission]([IdentityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_IdentityPermission_PermissionId]
    ON [dbo].[IdentityPermission]([PermissionId] ASC);

