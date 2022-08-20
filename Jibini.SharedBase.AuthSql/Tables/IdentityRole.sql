CREATE TABLE [dbo].[IdentityRole] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [IdentityId] INT NOT NULL,
    [RoleId]     INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_IdentityRole_ToIdentity] FOREIGN KEY ([IdentityId]) REFERENCES [dbo].[Identity] ([Id]),
    CONSTRAINT [FK_IdentityRole_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_IdentityRole_IdentityId]
    ON [dbo].[IdentityRole]([IdentityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_IdentityRole_RoleId]
    ON [dbo].[IdentityRole]([RoleId] ASC);

