CREATE TABLE [dbo].[IdentityToken] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [IdentityId] INT NOT NULL,
    [TokenId]    INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_IdentityToken_ToIdentity] FOREIGN KEY ([IdentityId]) REFERENCES [dbo].[Identity] ([Id]),
    CONSTRAINT [FK_IdentityToken_ToToken] FOREIGN KEY ([TokenId]) REFERENCES [dbo].[Token] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_IdentityToken_IdentityId]
    ON [dbo].[IdentityToken]([IdentityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_IdentityToken_TokenId]
    ON [dbo].[IdentityToken]([TokenId] ASC);

