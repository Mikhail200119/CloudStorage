CREATE TABLE [dbo].[FileDescription] (
    [FileDescriptionId] INT             NOT NULL generated always as identity (increment by 1),
    [ProvidedName]      VARCHAR (50)    NOT NULL,
    [UniqueName]        VARCHAR (100)   NOT NULL,
    [ContentType]       VARCHAR (100)   NOT NULL,
    [SizeInBytes]       INT             NOT NULL,
    [CreatedDate]       DATETIME2 (7)   NOT NULL,
    [ContentHash]       VARCHAR (150)   NOT NULL,
    [UploadedBy]        VARCHAR (100)   NOT NULL,
    [Preview]           VARBINARY (MAX) NULL,
    CONSTRAINT [PK_FileDescription] PRIMARY KEY CLUSTERED ([FileDescriptionId] ASC)
);