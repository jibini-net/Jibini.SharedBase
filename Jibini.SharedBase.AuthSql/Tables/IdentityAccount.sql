CREATE TABLE [dbo].[IdentityAccount] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [IdentityId] INT NOT NULL,
    [AccountId]  INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_IdentityAccount_ToAccount] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_IdentityAccount_ToIdentity] FOREIGN KEY ([IdentityId]) REFERENCES [dbo].[Identity] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_IdentityAccount_IdentityId]
    ON [dbo].[IdentityAccount]([IdentityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_IdentityAccount_AccountId]
    ON [dbo].[IdentityAccount]([AccountId] ASC);

