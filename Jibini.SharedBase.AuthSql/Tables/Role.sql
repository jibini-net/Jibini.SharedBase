CREATE TABLE [dbo].[Role] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [TenantId]       INT           NOT NULL,
    [Name]           NVARCHAR (50) NOT NULL,
    [Identifier]     NVARCHAR (50) NOT NULL,
    [Enabled]        BIT           DEFAULT ((1)) NOT NULL,
    [EnabledToggled] DATETIME      NULL,
    [Deleted]        DATETIME      NULL,
    [Created]        DATETIME      DEFAULT (getdate()) NOT NULL,
    [Updated]        DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Role_ToTenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Role_TenantId]
    ON [dbo].[Role]([TenantId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Role_Identifier]
    ON [dbo].[Role]([Identifier] ASC);

