CREATE TABLE [dbo].[Token] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [BlobHash]       NVARCHAR (500) NOT NULL,
    [BlobSalt]       NVARCHAR (50)  NOT NULL,
    [BlobDuration]   INT            NOT NULL,
    [Enabled]        BIT            DEFAULT ((1)) NOT NULL,
    [EnabledToggled] DATETIME       NULL,
    [Deleted]        DATETIME       NULL,
    [Created]        DATETIME       DEFAULT (getdate()) NOT NULL,
    [Updated]        DATETIME       DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Token_Blob]
    ON [dbo].[Token]([BlobHash] ASC);

