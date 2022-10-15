CREATE TABLE [dbo].[FileContent] (
    [FileContentId]     INT             IDENTITY (1, 1) NOT NULL,
    [FileDescriptionId] INT             NOT NULL,
    [Content]           VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_FileContent] PRIMARY KEY CLUSTERED ([FileContentId] ASC),
    CONSTRAINT [FK_FileContent_FileDescription] FOREIGN KEY ([FileDescriptionId]) REFERENCES [dbo].[FileDescription] ([FileDescriptionId]) ON DELETE CASCADE ON UPDATE CASCADE
);

