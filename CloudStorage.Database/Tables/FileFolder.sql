CREATE TABLE [dbo].[FileFolder] (
    [FileFolderId]   INT           IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (50) NOT NULL,
    [ParentFolderId] INT           NULL,
    CONSTRAINT [PK_FileFolder] PRIMARY KEY CLUSTERED ([FileFolderId] ASC),
    CONSTRAINT [FK_FileFolder_FileFolder1] FOREIGN KEY ([ParentFolderId]) REFERENCES [dbo].[FileFolder] ([FileFolderId])
);

