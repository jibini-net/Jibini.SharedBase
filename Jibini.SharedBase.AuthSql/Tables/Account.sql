CREATE TABLE [dbo].[Account] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]        NVARCHAR (50)  NOT NULL,
    [LastName]         NVARCHAR (50)  NULL,
    [Email]            NVARCHAR (50)  NOT NULL,
    [CellNumber]       NVARCHAR (50)  NULL,
    [HomeNumber]       NVARCHAR (50)  NULL,
    [PasswordHash]     NVARCHAR (200) NULL,
    [PasswordSalt]     NVARCHAR (50)  NULL,
    [PasswordSet]      DATETIME       NULL,
    [PasswordDuration] INT            NULL,
    [LastLogin]        DATETIME       NULL,
    [Enabled]          BIT            DEFAULT ((1)) NOT NULL,
    [EnabledToggled]   DATETIME       NULL,
    [Deleted]          DATETIME       NULL,
    [Created]          DATETIME       DEFAULT (getdate()) NOT NULL,
    [Updated]          DATETIME       DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Account_Email]
    ON [dbo].[Account]([Email] ASC);

