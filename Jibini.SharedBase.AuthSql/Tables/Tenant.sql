CREATE TABLE [dbo].[Tenant] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (50) NOT NULL,
    [Enabled]        BIT           DEFAULT ((1)) NOT NULL,
    [EnabledToggled] DATETIME      NULL,
    [Deleted]        DATETIME      NULL,
    [Created]        DATETIME      DEFAULT (getdate()) NOT NULL,
    [Updated]        DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

