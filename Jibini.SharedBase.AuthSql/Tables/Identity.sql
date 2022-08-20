CREATE TABLE [dbo].[Identity] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [TenantId]       INT           NOT NULL,
    [Name]           NVARCHAR (50) NOT NULL,
    [Expiration]     DATETIME      NULL,
    [Enabled]        BIT           DEFAULT ((1)) NULL,
    [EnabledToggled] DATETIME      NULL,
    [Deleted]        DATETIME      NULL,
    [Created]        DATETIME      DEFAULT (getdate()) NOT NULL,
    [Updated]        DATETIME      DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Identity_ToTenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Identity_TenantId]
    ON [dbo].[Identity]([TenantId] ASC);

